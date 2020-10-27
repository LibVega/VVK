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
	// Writes the processed spec results out to C# source files
	public static class APIGenerator
	{
		// Top-level writing task
		public static bool GenerateResults(ProcessResults res)
		{
			// Generate vendor directories
			try {
				foreach (var vendor in res.Vendors) {
					var dirpath = Path.Combine(ArgParse.OutputPath, vendor.Key);
					if (!Directory.Exists(dirpath)) {
						Directory.CreateDirectory(dirpath);
					}
				}
			}
			catch (Exception ex) {
				Program.PrintError($"Failed to generate vendor folders - {ex.Message}");
				return false;
			}

			// Write the API constants and functions
			if (!GenerateConstants(res)) {
				return false;
			}
			if (!GenerateCommands(res)) {
				return false;
			}

			// Write the vendor contents
			foreach (var vendor in res.Vendors) {
				Console.WriteLine($"Generating files for {vendor.Value.DisplayName} ...");
				if (!GenerateEnums(vendor.Value)) {
					return false;
				}
				if (!GenerateStructs(vendor.Value, res.Constants)) {
					return false;
				}
				if (!GenerateHandles(vendor.Value)) {
					return false;
				}
			}

			return true;
		}

		// API constant generation
		private static bool GenerateConstants(ProcessResults res)
		{
			Console.WriteLine("Generating API constants...");

			try {
				// Constants file context and class context
				using var file = new SourceFile("Vk.Constants.cs", "Vk");
				using var constClass = file.PushBlock("public static class Constants");

				// Write each constant
				foreach (var constSpec in res.Constants.Values) {
					constClass.WriteLine($"public const {constSpec.TypeName} {constSpec.Name} = {constSpec.Value};");
				}
			}
			catch (Exception ex) {
				Program.PrintError($"Failed to generate API constants - {ex.Message}");
				return false;
			}

			return true;
		}

		// Command generation
		private static bool GenerateCommands(ProcessResults res)
		{
			Console.WriteLine("Generating API functions...");

			try {
				// File context
				using var file = new SourceFile("Vk.Commands.cs", "Vk");

				// Loop over the global and instance functions
				using (var block = file.PushBlock("public unsafe sealed partial class InstanceFunctionTable")) {
					// Global functions
					block.WriteLine("/* Global Functions */");
					foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Global)) {
						block.WriteLine($"public static readonly {cmd.PtrPrototype} {cmd.Name} = null;");
					}
					block.WriteLine();

					// Instance functions
					block.WriteLine("/* Instance Functions */");
					foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Instance)) {
						block.WriteLine($"public readonly {cmd.PtrPrototype} {cmd.Name} = null;");
					}
					block.WriteLine();

					// Write default constructor
					block.WriteLine("/// <summary>Creates a new function table with all null pointers.</summary>");
					block.WriteLine("public InstanceFunctionTable() { }");
					block.WriteLine();

					// Write the loading constructor
					block.WriteLine("/// <summary>Creates a new function table and loads the functions.</summary>");
					block.WriteLine("/// <param name=\"inst\">The instance to load the functions for.</param>");
					block.WriteLine("/// <param name=\"version\">The core API version that the instance was created with.</param>");
					using (var ctor = block.PushBlock("public InstanceFunctionTable(Vk.Handle<Vk.Instance> inst, Vk.Version version)")) {
						ctor.WriteLine("void* addr = null;");
						ctor.WriteLine("CoreVersion = version;");
						ctor.WriteLine("Vk.Version V10 = new(1, 0, 0);");
						ctor.WriteLine("Vk.Version V11 = new(1, 1, 0);");
						ctor.WriteLine("Vk.Version V12 = new(1, 2, 0);");
						ctor.WriteLine();

						// Loop over the loadable instance functions
						foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Instance)) {
							if (cmd.IsAlias) {
								ctor.WriteLine($"{cmd.Name} = {cmd.Alias!.Name};");
								ctor.WriteLine($"if (({cmd.Name} == null) && TryLoadFunc(inst, \"{cmd.Name}\", out addr)) {{");
								ctor.WriteLine($"\t{cmd.Name} =");
								ctor.WriteLine($"\t\t({cmd.PtrPrototype})addr;");
								ctor.WriteLine($"}}");
							}
							else if (cmd.IsCore) { // Throw exception when we cant load core functions
								ctor.WriteLine($"if (version >= V{cmd.Spec.FeatureVersion!.Value}) {{");
								ctor.WriteLine($"\t{cmd.Name} =");
								ctor.WriteLine($"\t\t({cmd.PtrPrototype})LoadFunc(inst, \"{cmd.Name}\");");
								ctor.WriteLine("}");
							}
							else { // Don't throw exception for vendor functions
								ctor.WriteLine($"if (TryLoadFunc(inst, \"{cmd.Name}\", out addr)) {{");
								ctor.WriteLine($"\t{cmd.Name} =");
								ctor.WriteLine($"\t\t({cmd.PtrPrototype})addr;");
								ctor.WriteLine($"}}");
							}
						}
					}

					// Write the static constructor
					using (var ctor = block.PushBlock("static InstanceFunctionTable()")) {
						foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Global)) {
							ctor.WriteLine($"{cmd.Name} =");
							ctor.WriteLine($"\t({cmd.PtrPrototype})VVK.VulkanLibrary.GetExport(\"{cmd.Name}\").ToPointer();");
						}
					}
				}

				// Loop over the device functions
				using (var block = file.PushBlock("public unsafe sealed partial class DeviceFunctionTable")) {
					// Write the fields
					foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Device)) {
						block.WriteLine($"public readonly {cmd.PtrPrototype} {cmd.Name} = null;");
					}
					block.WriteLine();

					// Write default constructor
					block.WriteLine("/// <summary>Creates a new function table with all null pointers.</summary>");
					block.WriteLine("public DeviceFunctionTable() { }");
					block.WriteLine();

					// Write the constructor
					block.WriteLine("/// <summary>");
					block.WriteLine("/// Creates a new function table and loads the functions.");
					block.WriteLine("/// </summary>");
					block.WriteLine("/// <param name=\"dev\">The device to load the functions for.</param>");
					block.WriteLine("/// <param name=\"version\">The core API version that the device was created with.</param>");
					using (var ctor = block.PushBlock("public DeviceFunctionTable(Vk.Handle<Vk.Device> dev, Vk.Version version)")) {
						ctor.WriteLine("void* addr = null;");
						ctor.WriteLine("CoreVersion = version;");
						ctor.WriteLine("Vk.Version V10 = new(1, 0, 0);");
						ctor.WriteLine("Vk.Version V11 = new(1, 1, 0);");
						ctor.WriteLine("Vk.Version V12 = new(1, 2, 0);");
						ctor.WriteLine();

						// Loop over the loadable instance functions
						foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Device)) {
							if (cmd.IsAlias) {
								ctor.WriteLine($"{cmd.Name} = {cmd.Alias!.Name};");
								ctor.WriteLine($"if (({cmd.Name} == null) && TryLoadFunc(dev, \"{cmd.Name}\", out addr)) {{");
								ctor.WriteLine($"\t{cmd.Name} =");
								ctor.WriteLine($"\t\t({cmd.PtrPrototype})addr;");
								ctor.WriteLine($"}}");
							}
							else if (cmd.IsCore) { // Throw exception when we cant load core functions
								ctor.WriteLine($"if (version >= V{cmd.Spec.FeatureVersion!.Value}) {{");
								ctor.WriteLine($"\t{cmd.Name} =");
								ctor.WriteLine($"\t\t({cmd.PtrPrototype})LoadFunc(dev, \"{cmd.Name}\");");
								ctor.WriteLine("}");
							}
							else { // Don't throw exception for vendor functions
								ctor.WriteLine($"if (TryLoadFunc(dev, \"{cmd.Name}\", out addr)) {{");
								ctor.WriteLine($"\t{cmd.Name} =");
								ctor.WriteLine($"\t\t({cmd.PtrPrototype})addr;");
								ctor.WriteLine($"}}");
							}
						}
					}
				}
			}
			catch (Exception ex) {
				Program.PrintError($"Failed to generate API commands - {ex.Message}");
				return false;
			}

			try {
				// File context
				using var file = new SourceFile("Vk.Commands.Functions.cs", "Vk");

				// Write the global/instance functions
				using (var block = file.PushBlock("public unsafe sealed partial class InstanceFunctionTable")) {
					// Loop over global functions
					foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Global)) {
						_WriteCommand(block, cmd);
					}

					// Loop over instance functions
					foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Instance)) {
						_WriteCommand(block, cmd);
					}
				}

				// Write the device functions
				using (var block = file.PushBlock("public unsafe sealed partial class DeviceFunctionTable")) {
					foreach (var cmd in res.Commands.Values.Where(c => c.CommandScope == CommandScope.Device)) {
						_WriteCommand(block, cmd);
					}
				}
			}
			catch (Exception ex) {
				Program.PrintError($"Failed to generate API function tables - {ex.Message}");
				return false;
			}

			return true;

			static void _WriteCommand(SourceBlock block, CommandOut cmd)
			{
				bool glob = cmd.CommandScope == CommandScope.Global;

				// Raw primary function
				var argStr = String.Join(", ", cmd.Arguments.Select(arg => $"{arg.Type} {arg.Name}"));
				var callStr = String.Join(", ", cmd.Arguments.Select(arg => arg.Name));
				var typeStr = String.Join(", ", cmd.Arguments.Select(arg => $"<c>{arg.Type}</c>"));
				block.WriteLine($"/// <summary>{cmd.Name}({typeStr})</summary>");
				block.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				block.WriteLine($"public {(glob ? "static " : "")}{cmd.ReturnType} {cmd.Name.Substring("Vk".Length)}({argStr})");
				block.WriteLine($"\t=> {cmd.Name}({callStr});");
				block.WriteLine();

				// Alternate function
				if (cmd.AlternateArgs is not null) {
					var altArgStr = String.Join(", ", cmd.AlternateArgs.Select(arg => $"{arg.Type} {arg.Name}"));
					var altCallStr = String.Join(", ", cmd.AlternateArgs.Select(arg => arg.UseStr));
					block.WriteLine($"/// <summary>{cmd.Name}({typeStr})</summary>");
					block.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using (var func = block.PushBlock($"public {(glob ? "static " : "")}{cmd.ReturnType} {cmd.Name.Substring("Vk".Length)}({altArgStr})")) {
						foreach (var arg in cmd.AlternateArgs.Where(a => a.UseStr.EndsWith("FIXED"))) {
							if (arg.Type.Contains("Span<")) {
								var tstr = arg.Type.Substring(arg.Type.IndexOf('<') + 1);
								tstr = tstr.Substring(0, tstr.Length - 1);
								func.WriteLine($"fixed ({tstr}* {arg.Name}FIXED = {arg.Name})");
							}
							else {
								var tstr = arg.Type.Substring(arg.Type.IndexOf(' ') + 1);
								func.WriteLine($"fixed ({tstr}* {arg.Name}FIXED = &{arg.Name})");
							}
						}
						func.WriteLine($"{((cmd.ReturnType != "void") ? "return " : "")}{cmd.Name}({altCallStr});");
					}
				}
			}
		}

		// Enum generation
		private static bool GenerateEnums(Vendor vendor)
		{
			if (vendor.Enums.Count == 0) {
				return true;
			}

			Program.PrintVerbose($"\tGenerating enums for {vendor.DisplayName}");

			try {
				// The file context
				using var file = new SourceFile(vendor.GetSourceFilename("Enums"), vendor.NamespaceName);

				// Visit each enum
				foreach (var enumSpec in vendor.Enums.Values) {
					if (enumSpec.IsBitmask) {
						file.WriteLine("[Flags]");
					}
					using var enumBlock = 
						file.PushBlock($"public enum {enumSpec.Name} : {(enumSpec.IsBitmask ? "uint" : "int")}");

					// Visit each value
					foreach (var value in enumSpec.Values) {
						enumBlock.WriteLine($"{value.Name} = {value.Value},");
					}
				}
			}
			catch (Exception ex) {
				Program.PrintError($"Failed to generate enums for {vendor.DisplayName} - {ex.Message}");
				return false;
			}

			return true;
		}

		// Struct generation
		private static bool GenerateStructs(Vendor vendor, Dictionary<string, ConstantOut> consts)
		{
			if (vendor.Structs.Count == 0) {
				return true;
			}

			Program.PrintVerbose($"\tGenerating structs for {vendor.DisplayName}");

			try {
				// File context
				using var file = new SourceFile(vendor.GetSourceFilename("Structs"), vendor.NamespaceName);

				// Visit each struct
				foreach (var structSpec in vendor.Structs.Values) {
					// Write the struct header
					if (structSpec.IsUnion) {
						file.WriteLine("[StructLayout(LayoutKind.Explicit)]");
					}
					else {
						file.WriteLine("[StructLayout(LayoutKind.Sequential)]");
					}
					using var structBlock = file.PushBlock($"public unsafe partial struct {structSpec.Name} : IEquatable<{structSpec.Name}>");

					// Write the struct type literal, if needed
					if (structSpec.HasSType) {
						structBlock.WriteLine($"public const Vk.StructureType TYPE = {structSpec.Fields[0].Value};");
						structBlock.WriteLine();
					}

					// Visit the fields
					var fprefix = structSpec.IsUnion ? "[FieldOffset(0)] " : "";
					foreach (var field in structSpec.Fields) {
						if (field.SizeLiteral is not null) {
							if (field.IsFixed) {
								structBlock.WriteLine(
									$"{fprefix}public fixed {field.Type} {field.Name}[{field.SizeLiteral}];");
							}
							else {
								var literal = Char.IsDigit(field.SizeLiteral[0]) 
									? field.SizeLiteral 
									: consts[field.SizeLiteral.Substring(field.SizeLiteral.LastIndexOf('.') + 1)].Value;
								var count = Int32.Parse(literal);
								for (int i = 0; i < count; ++i) {
									structBlock.WriteLine($"{fprefix}public {field.Type} {field.Name}_{i};");
								}
							}
						}
						else {
							structBlock.WriteLine($"{fprefix}public {field.Type} {field.Name};");
						}
					}

					// Generate overrides
					structBlock.WriteLine();
					structBlock.WriteLine($"public readonly override bool Equals(object? obj) => (obj is {structSpec.Name} o) && (this == o);");
					structBlock.WriteLine($"readonly bool IEquatable<{structSpec.Name}>.Equals({structSpec.Name} obj) => (this == obj);");
					using (var hashBlock = structBlock.PushBlock($"public readonly override int GetHashCode()")) {
						var field0 = structSpec.Fields[0];
						string fixStr =
							(field0.SizeLiteral is not null && field0.IsFixed) ? $"{field0.Name}[0]" :
							(field0.SizeLiteral is not null && !field0.IsFixed) ? $"{field0.Name}_0" :
							field0.Name;
						hashBlock.WriteLine($"fixed ({structSpec.Fields[0].Type}* ptr = &{fixStr}) {{");
						hashBlock.WriteLine($"\treturn VVK.Hasher.HashBytes(ptr, (uint)Unsafe.SizeOf<{structSpec.Name}>());");
						hashBlock.WriteLine("}");
					}

					// Generate the equality operators
					structBlock.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using (var eqBlock = structBlock.PushBlock($"public static bool operator == (in {structSpec.Name} l, in {structSpec.Name} r)")) {
						eqBlock.WriteLine($"fixed ({structSpec.Name}* lp = &l, rp = &r) {{");
						eqBlock.WriteLine($"\tReadOnlySpan<byte> lb = new((byte*)lp, Unsafe.SizeOf<{structSpec.Name}>());");
						eqBlock.WriteLine($"\tReadOnlySpan<byte> rb = new((byte*)rp, Unsafe.SizeOf<{structSpec.Name}>());");
						eqBlock.WriteLine("\treturn lb.SequenceCompareTo(rb) == 0;");
						eqBlock.WriteLine("}");
					}
					structBlock.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using (var neqBlock = structBlock.PushBlock($"public static bool operator != (in {structSpec.Name} l, in {structSpec.Name} r)")) {
						neqBlock.WriteLine($"fixed ({structSpec.Name}* lp = &l, rp = &r) {{");
						neqBlock.WriteLine($"\tReadOnlySpan<byte> lb = new((byte*)lp, Unsafe.SizeOf<{structSpec.Name}>());");
						neqBlock.WriteLine($"\tReadOnlySpan<byte> rb = new((byte*)rp, Unsafe.SizeOf<{structSpec.Name}>());");
						neqBlock.WriteLine("\treturn lb.SequenceCompareTo(rb) != 0;");
						neqBlock.WriteLine("}");
					}

					// Generate the initialization functions for typed structs
					if (structSpec.HasSType) {
						structBlock.WriteLine();
						structBlock.WriteLine(
							$"/// <summary>Creates a new {structSpec.Name} value with the correct type field.</summary>");
						structBlock.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
						structBlock.WriteLine(
							$"public static void New(out {structSpec.Name} value) => value = new() {{ sType = TYPE }};");
						structBlock.WriteLine(
							$"/// <summary>Initializes the sType and pNext fields to the correct default values.</summary>");
						structBlock.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
						structBlock.WriteLine(
							$"public static void Init(ref {structSpec.Name} value) {{ value.sType = TYPE; value.pNext = null; }}");
					}
				}
			}
			catch (Exception ex) {
				Program.PrintError($"Failed to generate structs for {vendor.DisplayName} - {ex.Message}");
				return false;
			}

			return true;
		}

		// Handle generation
		private static bool GenerateHandles(Vendor vendor)
		{
			if (vendor.Handles.Count == 0) {
				return true;
			}

			Program.PrintVerbose($"\tGenerating handles for {vendor.DisplayName}");

			try {
				// File context
				using var file = new SourceFile(vendor.GetSourceFilename("Handles"), vendor.NamespaceName);

				// Visit each handle
				foreach (var handleSpec in vendor.Handles.Values) {
					// Write the header
					using var handleBlock = file.PushBlock($"public unsafe partial struct {handleSpec.Name} : IHandleType<{handleSpec.Name}>");

					// Get some info
					bool instScope = handleSpec.Parent?.Name switch {
						null => true,
						"Instance" => true,
						"PhysicalDevice" => true,
						"Display" => true,
						"Surface" => true,
						_ => false
					};

					// Write the fields
					handleBlock.WriteLine($"public static readonly {handleSpec.Name} Null = new();");
					handleBlock.WriteLine();
					if (handleSpec.Parent is not null) {
						handleBlock.WriteLine($"public readonly {handleSpec.Parent.ProcessedName} Parent;");
					}
					handleBlock.WriteLine($"public readonly {((instScope && handleSpec.Name != "Device") ? "Vk.InstanceFunctionTable" : "Vk.DeviceFunctionTable")} Functions;");
					if (handleSpec.Name != "Instance") {
						handleBlock.WriteLine("public readonly Vk.Instance Instance;");
					}
					if (!instScope && (handleSpec.Name != "Device")) {
						handleBlock.WriteLine("public readonly Vk.Device Device;");
					}
					handleBlock.WriteLine($"private readonly Handle<{handleSpec.Name}> _handle;");
					handleBlock.WriteLine($"readonly Handle<{handleSpec.Name}> IHandleType<{handleSpec.Name}>.Handle => _handle;");
					handleBlock.WriteLine( "public readonly bool IsValid => _handle.IsValid;");
					handleBlock.WriteLine();

					// Write the ctor
					var ctorargs = (handleSpec.Parent is not null)
						? $"in {handleSpec.Parent.ProcessedName} parent, Vk.Handle<{handleSpec.Name}> handle"
						: $"Vk.Handle<{handleSpec.Name}> handle";
					if ((handleSpec.Name == "Instance") || (handleSpec.Name == "Device")) {
						ctorargs += ", Vk.Version apiVersion";
					}
					using (var ctor = handleBlock.PushBlock($"public {handleSpec.Name}({ctorargs})")) {
						// Parent
						if (handleSpec.Parent is not null) {
							ctor.WriteLine("Parent = parent;");
						}
						// Function table, Instance/Device
						if ((handleSpec.Name == "Instance")) {
							ctor.WriteLine("Functions = new(handle, apiVersion);");
						}
						else if (handleSpec.Name == "Device") {
							ctor.WriteLine("Functions = new(handle, apiVersion);");
							ctor.WriteLine("Instance = parent.Instance;");
						}
						else {
							ctor.WriteLine( "Functions = parent.Functions;");
							ctor.WriteLine($"Instance = {((handleSpec.Parent?.Name == "Instance") ? "parent" : "parent.Instance")};");
							if (!instScope) {
								ctor.WriteLine($"Device = {((handleSpec.Parent?.Name == "Device") ? "parent" : "parent.Device")};");
							}
						}
						// Handle
						ctor.WriteLine("_handle = handle;");
					}

					// Write the overrides
					handleBlock.WriteLine("public override readonly int GetHashCode() => _handle.GetHashCode();");
					handleBlock.WriteLine($"public override readonly string? ToString() => $\"[{handleSpec.Name} 0x{{(ulong)_handle:X16}}]\";");
					handleBlock.WriteLine($"public override readonly bool Equals(object? o) => (o is {handleSpec.Name} t) && (t._handle == _handle);");
					handleBlock.WriteLine($"readonly bool IEquatable<{handleSpec.Name}>.Equals({handleSpec.Name} other) => other._handle == _handle;");
					handleBlock.WriteLine();

					// Write the operators
					handleBlock.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					handleBlock.WriteLine($"public static implicit operator Vk.Handle<{handleSpec.Name}> (in {handleSpec.Name} handle) => handle._handle;");
					handleBlock.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					handleBlock.WriteLine($"public static bool operator == ({handleSpec.Name} l, {handleSpec.Name} r) => l._handle == r._handle;");
					handleBlock.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					handleBlock.WriteLine($"public static bool operator != ({handleSpec.Name} l, {handleSpec.Name} r) => l._handle != r._handle;");
					handleBlock.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					handleBlock.WriteLine($"public static implicit operator bool ({handleSpec.Name} handle) => handle._handle.IsValid;");
				}
			}
			catch (Exception ex) {
				Program.PrintError($"Failed to generate handles for {vendor.DisplayName} - {ex.Message}");
				return false;
			}

			return true;
		}
	}
}
