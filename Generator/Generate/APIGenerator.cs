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
				using var file = new SourceFile("Vk.Constants.cs", "VVK.Vk");
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
				using var file = new SourceFile("Vk.Commands.cs", "VVK.Vk");

				// Loop over the global and instance functions
				using (var block = file.PushBlock("public unsafe sealed partial class InstanceFunctionTable")) {
					// Global functions
					block.WriteLine("/* Global Functions */");
					foreach (var cmd in res.Commands.Values.Where(c => c.Scope == CommandScope.Global)) {
						block.WriteLine($"public static readonly {cmd.Prototype} {cmd.Name} = null;");
					}
					block.WriteLine();

					// Instance functions
					block.WriteLine("/* Instance Functions */");
					foreach (var cmd in res.Commands.Values.Where(c => c.Scope == CommandScope.Instance)) {
						block.WriteLine($"public readonly {cmd.Prototype} {cmd.Name} = null;");
					}
					block.WriteLine();

					// Write default constructor
					block.WriteLine("/// <summary>Creates a new function table with all null pointers.</summary>");
					block.WriteLine("public InstanceFunctionTable() { }");
					block.WriteLine();

					// Write the loading constructor
					block.WriteLine("/// <summary>Creates a new function table and loads the functions.</summary>");
					block.WriteLine("/// <param name=\"inst\">The instance to load the functions for.</param>");
					using (var ctor = block.PushBlock("public InstanceFunctionTable(Vk.Instance inst)")) {
						ctor.WriteLine("void* addr = (void*)0;");
						ctor.WriteLine();

						// Loop over the loadable instance functions
						foreach (var cmd in res.Commands.Values.Where(c => c.Scope == CommandScope.Instance)) {
							if (cmd.IsAlias) {
								ctor.WriteLine($"{cmd.Name} = {cmd.Alias!.Name};");
							}
							else if (cmd.IsCore) { // Throw exception when we cant load core functions
								ctor.WriteLine($"{cmd.Name} =");
								ctor.WriteLine($"\t({cmd.Prototype})LoadFunc(inst, \"{cmd.Name}\");");
							}
							else { // Don't throw exception for vendor functions
								ctor.WriteLine($"if (TryLoadFunc(inst, \"{cmd.Name}\", out addr)) {{");
								ctor.WriteLine($"\t{cmd.Name} =");
								ctor.WriteLine($"\t\t({cmd.Prototype})addr;");
								ctor.WriteLine($"}}");
							}
						}
					}

					// Write the static constructor
					using (var ctor = block.PushBlock("static InstanceFunctionTable()")) {
						foreach (var cmd in res.Commands.Values.Where(c => c.Scope == CommandScope.Global)) {
							ctor.WriteLine($"{cmd.Name} =");
							ctor.WriteLine($"\t({cmd.Prototype})VulkanLibrary.GetExport(\"{cmd.Name}\").ToPointer();");
						}
					}
				}

				// Loop over the device functions
				using (var block = file.PushBlock("public unsafe sealed partial class DeviceFunctionTable")) {
					// Write the fields
					foreach (var cmd in res.Commands.Values.Where(c => c.Scope == CommandScope.Device)) {
						block.WriteLine($"public readonly {cmd.Prototype} {cmd.Name} = null;");
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
					using (var ctor = block.PushBlock("public DeviceFunctionTable(Vk.Device dev)")) {
						ctor.WriteLine("void* addr = (void*)0;");
						ctor.WriteLine();

						// Loop over the loadable instance functions
						foreach (var cmd in res.Commands.Values.Where(c => c.Scope == CommandScope.Device)) {
							if (cmd.IsAlias) {
								ctor.WriteLine($"{cmd.Name} = {cmd.Alias!.Name};");
							}
							else if (cmd.IsCore) { // Throw exception when we cant load core functions
								ctor.WriteLine($"{cmd.Name} =");
								ctor.WriteLine($"\t({cmd.Prototype})LoadFunc(dev, \"{cmd.Name}\");");
							}
							else { // Don't throw exception for vendor functions
								ctor.WriteLine($"if (TryLoadFunc(dev, \"{cmd.Name}\", out addr)) {{");
								ctor.WriteLine($"\t{cmd.Name} =");
								ctor.WriteLine($"\t\t({cmd.Prototype})addr;");
								ctor.WriteLine($"}}");
							}
						}
					}
				}
			}
			catch (Exception ex) {
				Program.PrintError($"Failed to generate API functions - {ex.Message}");
				return false;
			}

			return true;
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
					using var structBlock = file.PushBlock($"public unsafe partial struct {structSpec.Name}");

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
					file.WriteLine("[StructLayout(LayoutKind.Explicit, Size = 8)]");
					using var handleBlock = file.PushBlock($"public unsafe partial struct {handleSpec.Name} : IEquatable<{handleSpec.Name}>");

					// Write the fields
					handleBlock.WriteLine($"public static readonly {handleSpec.Name} Null = new(0);");
					handleBlock.WriteLine();
					handleBlock.WriteLine("[FieldOffset(0)] public readonly void* Handle;");
					handleBlock.WriteLine("public readonly ulong LongHandle => (ulong)Handle;");
					handleBlock.WriteLine();

					// Write the constructors
					handleBlock.WriteLine($"public {handleSpec.Name}(void* handle) => Handle = handle;");
					handleBlock.WriteLine($"public {handleSpec.Name}(ulong handle) => Handle = (void*)handle;");
					handleBlock.WriteLine($"public {handleSpec.Name}(IntPtr handle) => Handle = handle.ToPointer();");
					handleBlock.WriteLine();

					// Write the functions
					handleBlock.WriteLine($"readonly bool IEquatable<{handleSpec.Name}>.Equals({handleSpec.Name} other) => other.Handle == Handle;");
					handleBlock.WriteLine($"public readonly override bool Equals(object? other) => (other is {handleSpec.Name} handle) && handle.Handle == Handle;");
					handleBlock.WriteLine($"public readonly override int GetHashCode() => (int)(LongHandle >> 32) ^ (int)(LongHandle & 0xFFFFFFFF);");
					handleBlock.WriteLine($"public readonly override string ToString() => $\"[{handleSpec.Name} 0x{{LongHandle:X16}}]\";");
					handleBlock.WriteLine();

					// Write the operators
					handleBlock.WriteLine($"public static bool operator == ({handleSpec.Name} l, {handleSpec.Name} r) => l.Handle == r.Handle;");
					handleBlock.WriteLine($"public static bool operator != ({handleSpec.Name} l, {handleSpec.Name} r) => l.Handle != r.Handle;");
					handleBlock.WriteLine($"public static implicit operator bool ({handleSpec.Name} handle) => handle.Handle != null;");
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
