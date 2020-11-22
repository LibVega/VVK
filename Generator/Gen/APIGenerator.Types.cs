/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gen
{
	// Performs the type generation functions
	public static partial class APIGenerator
	{
		// Generate bitmask types
		private static void GenerateBitmasks(Vendor vendor)
		{
			if (vendor.Bitmasks.Count == 0) return;

			// Make path and file context
			var vpath = vendor.IsCore ? "Bitmasks.cs" : Path.Combine(vendor.Tag, "Bitmasks.cs");
			using var file = new SourceFile(vpath);

			// For each bitmask
			foreach (var type in vendor.Bitmasks.Values) {
				// Open block
				file.WriteLine("[Flags]");
				using var block = file.PushBlock($"public enum {type.Name} : uint");

				// Write values
				foreach (var entry in type.Entries) {
					block.WriteLine($"{entry.Name} = {entry.Value},");
				}
			}
		}

		// Generate handle types
		private static void GenerateHandles(Vendor vendor)
		{
			if (vendor.Handles.Count == 0) return;

			// Make path and file context
			var vpath = vendor.IsCore ? "Handles.cs" : Path.Combine(vendor.Tag, "Handles.cs");
			using var file = new SourceFile(vpath);

			// For each handle
			foreach (var handle in vendor.Handles.Values) {
				// Open block
				using var block = file.PushBlock($"public unsafe sealed partial class {handle.Name} : IVulkanHandle<{handle.Name}>");

				// Get parent info
				var tableType = (handle.FunctionParent == "VkInstance") ? "InstanceFunctionTable" : "DeviceFunctionTable";

				// Write fields
				block.WriteLine($"public readonly VulkanHandle<{handle.Name}> Handle;");
				if (handle.Parent is not null) {
					block.WriteLine($"public readonly {handle.Parent.Name} Parent;");
				}
				block.WriteLine($"public readonly {tableType} Functions;");
				block.WriteLine("public bool IsValid => Handle.IsValid;");
				block.WriteLine();

				// Write ctor
				if (handle.Name == "VkInstance") {
					using var ctor = block.PushBlock("public VkInstance(VulkanHandle<VkInstance> handle, VkVersion apiVersion)");
					ctor.WriteLine("Handle = handle;");
					ctor.WriteLine("Functions = handle ? new(handle, apiVersion) : new();");
				}
				else if (handle.Name == "VkDevice") {
					using var ctor = block.PushBlock("public VkDevice(VulkanHandle<VkDevice> handle, VkPhysicalDevice parent, VkVersion apiVersion)");
					ctor.WriteLine("Handle = handle;");
					ctor.WriteLine("Parent = parent;");
					ctor.WriteLine("Functions = handle ? new(handle, apiVersion) : new();");
				}
				else {
					using var ctor = block.PushBlock($"public {handle.Name}(VulkanHandle<{handle.Name}> handle, {handle.Parent!.Name} parent)");
					ctor.WriteLine("Handle = handle;");
					ctor.WriteLine("Parent = parent;");
					ctor.WriteLine("Functions = parent.Functions;");
				}

				// Write overrides
				block.WriteLine($"public override int GetHashCode() => Handle.GetHashCode();");
				block.WriteLine($"public override string? ToString() => Handle.ToString();");
				block.WriteLine($"public override bool Equals(object? o) => (o is {handle.Name} h) && (h.Handle == Handle);");
				block.WriteLine($"bool IEquatable<{handle.Name}>.Equals({handle.Name}? other) => other?.Handle == Handle;");
				block.WriteLine();

				// Equality
				block.WriteLine($"public static bool operator == ({handle.Name}? l, {handle.Name}? r) => l?.Handle == r?.Handle;");
				block.WriteLine($"public static bool operator != ({handle.Name}? l, {handle.Name}? r) => l?.Handle != r?.Handle;");
				block.WriteLine();

				// Casting
				block.WriteLine($"public static implicit operator VulkanHandle<{handle.Name}> ({handle.Name}? h) => h?.Handle ?? VulkanHandle<{handle.Name}>.Null;");
				block.WriteLine($"public static implicit operator bool ({handle.Name}? h) => h?.IsValid ?? false;");
				block.WriteLine();

				// Handle functions
				foreach (var cmd in handle.Commands) {
					var hasRet = cmd.ReturnType != "void";
					var retStr = hasRet ? "return " : String.Empty;

					foreach (var pset in cmd.ParamSets) {
						// If this is a (parent, handle, ...) function
						var parentArg = (pset.Count > 1) && (pset[1].Type == $"VulkanHandle<{handle.Name}>");
						var anyFix = pset.Any(par => par.NeedsFix);

						var fnname = cmd.Name.Substring("vk".Length);
						block.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
						if (cmd.Scope == CommandType.CommandScope.Global) {
							var protostr = String.Join(", ", pset.Select(par => $"{par.Type} {par.Name}"));
							var callstr = String.Join(", ", pset.Select(par => par.NeedsFix ? (par.Name + "FIXED") : par.Name));
							if (anyFix) {
								using var func = block.PushBlock($"public static {cmd.ReturnType} {fnname}({protostr})");
								foreach (var par in pset.Where(par => par.NeedsFix)) {
									func.WriteLine($"fixed ({par.Type.Substring(par.Type.IndexOf(' ') + 1)}* {par.Name}FIXED = &{par.Name})");
								}
								func.WriteLine($"{retStr}InstanceFunctionTable.{cmd.Name}({callstr});");
							}
							else {
								block.WriteLine($"public static {cmd.ReturnType} {fnname}({protostr})");
								block.WriteLine($"\t=> InstanceFunctionTable.{cmd.Name}({callstr});");
								block.WriteLine();
							}
						}
						else {
							var protostr = String.Join(", ", pset.Skip(parentArg ? 2 : 1).Select(par => $"{par.Type} {par.Name}"));
							var callstr = String.Join(", ", pset.Skip(parentArg ? 2 : 1).Select(par => par.NeedsFix ? (par.Name + "FIXED") : par.Name));
							if (anyFix) {
								using var func = block.PushBlock($"public {cmd.ReturnType} {fnname}({protostr})");
								foreach (var par in pset.Where(par => par.NeedsFix)) {
									func.WriteLine($"fixed ({par.Type.Substring(par.Type.IndexOf(' ') + 1)}* {par.Name}FIXED = &{par.Name})");
								}
								if (parentArg) {
									func.WriteLine($"{retStr}Functions.{cmd.Name}(Parent, Handle{(callstr.Length > 0 ? ", " : "")}{callstr});");
								}
								else {
									func.WriteLine($"{retStr}Functions.{cmd.Name}(Handle{(callstr.Length > 0 ? ", " : "")}{callstr});");
								}
							}
							else {
								block.WriteLine($"public {cmd.ReturnType} {fnname}({protostr})");
								if (parentArg) {
									block.WriteLine($"\t=> Functions.{cmd.Name}(Parent, Handle{(callstr.Length > 0 ? ", " : "")}{callstr});");
								}
								else {
									block.WriteLine($"\t=> Functions.{cmd.Name}(Handle{(callstr.Length > 0 ? ", " : "")}{callstr});");
								}
								block.WriteLine();
							}
						}
					}
				}
			}
		}

		// Generate enum types
		private static void GenerateEnums(Vendor vendor)
		{
			if (vendor.Enums.Count == 0) return;

			// Make path and file context
			var vpath = vendor.IsCore ? "Enums.cs" : Path.Combine(vendor.Tag, "Enums.cs");
			using var file = new SourceFile(vpath);

			// For each enum
			foreach (var type in vendor.Enums.Values) {
				// Open block
				using var block = file.PushBlock($"public enum {type.Name} : int");

				// Write values
				foreach (var entry in type.Entries) {
					block.WriteLine($"{entry.Name} = {entry.Value},");
				}
			}
		}

		// Generate struct types
		private static void GenerateStructs(Vendor vendor)
		{
			if (vendor.Structs.Count == 0) return;

			// Make path and file context
			var vpath = vendor.IsCore ? "Structs.cs" : Path.Combine(vendor.Tag, "Structs.cs");
			using var file = new SourceFile(vpath);

			// For each struct
			List<string> ctorArgs = new();
			List<string> ctorAssigns = new();
			List<string> hashes = new();
			List<string> comparisons = new();
			foreach (var @struct in vendor.Structs.Values) {
				// Open block
				file.WriteLine($"[StructLayout(LayoutKind.{(@struct.IsUnion ? "Explicit" : "Sequential")})]");
				using var block = file.PushBlock($"public unsafe partial struct {@struct.Name} : IEquatable<{@struct.Name}>");

				// Typed structs
				if (@struct.IsTyped) {
					block.WriteLine($"public const VkStructureType TYPE = VkStructureType.{@struct.Type!};");
					block.WriteLine();
				}

				// Fields (and ctor arguments)
				ctorArgs.Clear();
				ctorAssigns.Clear();
				hashes.Clear();
				comparisons.Clear();
				foreach (var field in @struct.Fields) {
					var lower = NameHelper.GetSafeArgName(field.Name);
					if (field.ArraySize is not null) {
						if (field.IsFixed) {
							if (@struct.IsUnion) {
								block.WriteLine("[FieldOffset(0)]");
							}
							block.WriteLine($"public fixed {field.Type} {field.Name}[{field.ArraySize}];");
							for (uint i = 0; i < field.ArraySize; ++i) {
								ctorArgs.Add($"{field.Type} {lower}_{i} = default");
								ctorAssigns.Add($"{field.Name}[{i}] = {lower}_{i};");
								hashes.Add($"{field.Name}[{i}].GetHashCode()");
								comparisons.Add($"(l.{field.Name}[{i}] == r.{field.Name}[{i}])");
							}
						}
						else {
							for (uint i = 0; i < field.ArraySize; ++i) {
								block.WriteLine($"public {field.Type} {field.Name}_{i};");
								ctorArgs.Add($"{field.Type} {lower}_{i} = default");
								ctorAssigns.Add($"{field.Name}_{i} = {lower}_{i};");
								hashes.Add($"{field.Name}_{i}.GetHashCode()");
								comparisons.Add($"(l.{field.Name}_{i} == r.{field.Name}_{i})");
							}
						}
					}
					else {
						if (@struct.IsUnion) {
							block.WriteLine("[FieldOffset(0)]");
						}
						block.WriteLine($"public {field.Type} {field.Name};");
						if (field.Name == "sType") {
							ctorAssigns.Add("sType = TYPE;");
						}
						else if (field.Name == "pNext") {
							ctorAssigns.Add("pNext = null;");
						}
						else {
							ctorArgs.Add($"{field.Type} {lower} = default");
							ctorAssigns.Add($"{field.Name} = {lower};"); 
						}
						var isPtr = field.Type.Contains('*');
						hashes.Add(isPtr ? $"((ulong){field.Name}).GetHashCode()" : $"{field.Name}.GetHashCode()");
						comparisons.Add($"(l.{field.Name} == r.{field.Name})");
					}
				}
				block.WriteLine();

				// Ctor
				if (!@struct.IsUnion && (!@struct.IsTyped || (@struct.Fields.Count > 2))) {
					block.WriteLine($"public {@struct.Name}(");
					for (int i = 0; i < ctorArgs.Count; ++i) {
						block.WriteLine($"\t{ctorArgs[i]}{((i == ctorArgs.Count - 1) ? "" : ",")}");
					}
					block.WriteLine(") {");
					foreach (var line in ctorAssigns) {
						block.WriteLine('\t' + line);
					}
					block.WriteLine("}");
					block.WriteLine(); 
				}

				// Equality
				block.WriteLine($"public readonly override bool Equals(object? o) => (o is {@struct.Name} s) && (this == s);");
				block.WriteLine($"readonly bool IEquatable<{@struct.Name}>.Equals({@struct.Name} o) => o == this;");
				block.WriteLine();

				// Hash code
				block.WriteLine("[MethodImpl(MethodImplOptions.AggressiveOptimization)]");
				using (var hash = block.PushBlock("public readonly override int GetHashCode()")) {
					hash.WriteLine("return");
					for (int i = 0; i < hashes.Count; i += 4) {
						hash.WriteLine('\t' + (i != 0 ? "^ " : "") + String.Join(" ^ ", hashes.Skip(i).Take(4)));
					}
					hash.WriteLine("\t;");
				}

				// Equality operators
				block.WriteLine("[MethodImpl(MethodImplOptions.AggressiveOptimization)]");
				using (var op = block.PushBlock($"public static bool operator == (in {@struct.Name} l, in {@struct.Name} r)")) {
					op.WriteLine("return");
					for (int i = 0; i < comparisons.Count; i += 4) {
						op.WriteLine('\t' + (i != 0 ? "&& " : "") + String.Join(" && ", comparisons.Skip(i).Take(4)));
					}
					op.WriteLine("\t;");
				}
				block.WriteLine("[MethodImpl(MethodImplOptions.AggressiveOptimization)]");
				using (var op = block.PushBlock($"public static bool operator != (in {@struct.Name} l, in {@struct.Name} r)")) {
					op.WriteLine("return");
					for (int i = 0; i < comparisons.Count; i += 4) {
						op.WriteLine('\t' + (i != 0 ? "|| " : "") + String.Join(" || ", 
							comparisons.Skip(i).Take(4).Select(cmp => cmp.Replace("==", "!="))));
					}
					op.WriteLine("\t;");
				}

				// New function
				block.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				if (@struct.IsTyped) {
					block.WriteLine($"public static void New(out {@struct.Name} s) => s = new() {{ sType = TYPE }};");
				}
				else {
					block.WriteLine($"public static void New(out {@struct.Name} s) => s = new();");
				}
			}
		}
	}
}
