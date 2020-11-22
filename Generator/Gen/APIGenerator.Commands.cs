/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Linq;

namespace Gen
{
	// Performs the command generation functions
	public static partial class APIGenerator
	{
		// Generate the function tables
		private static void GenerateCommands(ProcessedSpec spec)
		{
			// Generate instance table
			using (var file = new SourceFile("InstanceFunctionTable.cs"))
			using (var table = file.PushBlock("public unsafe sealed partial class InstanceFunctionTable")) {
				// Global functions
				table.WriteLine("/* Global Functions */");
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Global)) {
					table.WriteLine($"public static readonly {cmd.TypeString} {cmd.Name} = null;");
				}
				table.WriteLine();

				// Instance functions
				table.WriteLine("/* Instance Functions */");
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Instance)) {
					table.WriteLine($"public readonly {cmd.TypeString} {cmd.Name} = null;");
				}
				table.WriteLine();

				// Null ctor
				table.WriteLine("/// <summary>Creates a new function table with all null pointers.</summary>");
				table.WriteLine("public InstanceFunctionTable() { }");
				table.WriteLine();

				// Real ctor
				table.WriteLine("/// <summary>Creates a new function table and loads the functions.</summary>");
				table.WriteLine("/// <param name=\"inst\">The instance to load the functions for.</param>");
				table.WriteLine("/// <param name=\"version\">The core API version that the instance was created with.</param>");
				using (var ctor = table.PushBlock("public InstanceFunctionTable(VulkanHandle<VkInstance> inst, VkVersion version)")) {
					ctor.WriteLine("void* addr = null;");
					ctor.WriteLine("CoreVersion = version;");
					ctor.WriteLine();

					foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Instance)) {
						if (cmd.Spec.Alias is not null) {
							ctor.WriteLine($"{cmd.Name} = {cmd.Spec.Alias.Name};");
							ctor.WriteLine($"if (({cmd.Name} == null) && TryLoadFunc(inst, \"{cmd.Name}\", out addr)) {{");
							ctor.WriteLine($"\t{cmd.Name} = ({cmd.TypeString})addr;");
							ctor.WriteLine("}");
						}
						else if (cmd.IsCore) {
							if (cmd.Spec.FeatureLevel! > 10) {
								ctor.WriteLine($"if (version >= VkVersion.VK_VERSION_1_{cmd.Spec.FeatureLevel % 10}) {{");
								ctor.WriteLine($"\t{cmd.Name} = ({cmd.TypeString})LoadFunc(inst, \"{cmd.Name}\");");
								ctor.WriteLine("}");
							}
							else {
								ctor.WriteLine($"{cmd.Name} = ({cmd.TypeString})LoadFunc(inst, \"{cmd.Name}\");");
							}
						}
						else {
							ctor.WriteLine($"if (TryLoadFunc(inst, \"{cmd.Name}\", out addr)) {{");
							ctor.WriteLine($"\t{cmd.Name} = ({cmd.TypeString})addr;");
							ctor.WriteLine("}");
						}
					}
				}

				// Static ctor
				using (var ctor = table.PushBlock("static InstanceFunctionTable()")) {
					foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Global)) {
						ctor.WriteLine(cmd.Name + " =");
						ctor.WriteLine($"\t({cmd.TypeString})VulkanLibrary.GetExport(\"{cmd.Name}\").ToPointer();");
					}
				}
			}

			// Generate safe calls for the instance table
			using (var file = new SourceFile("InstanceFunctionTable.Calls.cs"))
			using (var table = file.PushBlock("public unsafe sealed partial class InstanceFunctionTable")) {
				// Global functions
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Global)) {
					var protostr = String.Join(", ", cmd.ParamSets[0].Select(par => $"{par.Type} {par.Name}"));
					var callstr = String.Join(", ", cmd.ParamSets[0].Select(par => par.Name));
					var fnname = cmd.Name.Substring("vk".Length);
					table.WriteLine( "[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using var func = table.PushBlock($"public static {cmd.ReturnType} {fnname}({protostr})");
					if ((cmd.Spec.FeatureLevel is null) || (cmd.Spec.FeatureLevel > 10)) {
						func.WriteLine($"if ({cmd.Name} == null) throw new VVK.FunctionNotLoadedException(\"{cmd.Name}\");");
					}
					func.WriteLine($"{((cmd.ReturnType != "void") ? "return " : "")}{cmd.Name}({callstr});");
				}

				// Instance functions
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Instance)) {
					var protostr = String.Join(", ", cmd.ParamSets[0].Select(par => $"{par.Type} {par.Name}"));
					var callstr = String.Join(", ", cmd.ParamSets[0].Select(par => par.Name));
					var fnname = cmd.Name.Substring("vk".Length);
					table.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using var func = table.PushBlock($"public {cmd.ReturnType} {fnname}({protostr})");
					if ((cmd.Spec.FeatureLevel is null) || (cmd.Spec.FeatureLevel > 10)) {
						func.WriteLine($"if ({cmd.Name} == null) throw new VVK.FunctionNotLoadedException(\"{cmd.Name}\");");
					}
					func.WriteLine($"{((cmd.ReturnType != "void") ? "return " : "")}{cmd.Name}({callstr});");
				}
			}

			// Generate device table
			using (var file = new SourceFile("DeviceFunctionTable.cs"))
			using (var table = file.PushBlock("public unsafe sealed partial class DeviceFunctionTable")) {
				// Device functions
				table.WriteLine("/* Device Functions */");
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Device)) {
					table.WriteLine($"public readonly {cmd.TypeString} {cmd.Name} = null;");
				}
				table.WriteLine();

				// Null ctor
				table.WriteLine("/// <summary>Creates a new function table with all null pointers.</summary>");
				table.WriteLine("public DeviceFunctionTable() { }");
				table.WriteLine();

				// Real ctor
				table.WriteLine("/// <summary>Creates a new function table and loads the functions.</summary>");
				table.WriteLine("/// <param name=\"device\">The device to load the functions for.</param>");
				table.WriteLine("/// <param name=\"version\">The core API version that the device was created with.</param>");
				using (var ctor = table.PushBlock("public DeviceFunctionTable(VulkanHandle<VkDevice> dev, VkVersion version)")) {
					ctor.WriteLine("void* addr = null;");
					ctor.WriteLine("CoreVersion = version;");
					ctor.WriteLine();

					foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Device)) {
						if (cmd.Spec.Alias is not null) {
							ctor.WriteLine($"{cmd.Name} = {cmd.Spec.Alias.Name};");
							ctor.WriteLine($"if (({cmd.Name} == null) && TryLoadFunc(dev, \"{cmd.Name}\", out addr)) {{");
							ctor.WriteLine($"\t{cmd.Name} = ({cmd.TypeString})addr;");
							ctor.WriteLine("}");
						}
						else if (cmd.IsCore) {
							if (cmd.Spec.FeatureLevel! > 10) {
								ctor.WriteLine($"if (version >= VkVersion.VK_VERSION_1_{cmd.Spec.FeatureLevel % 10}) {{");
								ctor.WriteLine($"\t{cmd.Name} = ({cmd.TypeString})LoadFunc(dev, \"{cmd.Name}\");");
								ctor.WriteLine("}");
							}
							else {
								ctor.WriteLine($"{cmd.Name} = ({cmd.TypeString})LoadFunc(dev, \"{cmd.Name}\");");
							}
						}
						else {
							ctor.WriteLine($"if (TryLoadFunc(dev, \"{cmd.Name}\", out addr)) {{");
							ctor.WriteLine($"\t{cmd.Name} = ({cmd.TypeString})addr;");
							ctor.WriteLine("}");
						}
					}
				}
			}

			// Generate safe calls for device table
			using (var file = new SourceFile("DeviceFunctionTable.Calls.cs"))
			using (var table = file.PushBlock("public unsafe sealed partial class DeviceFunctionTable")) {
				// Device functions
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Device)) {
					var protostr = String.Join(", ", cmd.ParamSets[0].Select(par => $"{par.Type} {par.Name}"));
					var callstr = String.Join(", ", cmd.ParamSets[0].Select(par => par.Name));
					var fnname = cmd.Name.Substring("vk".Length);
					table.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using var func = table.PushBlock($"public {cmd.ReturnType} {fnname}({protostr})");
					if ((cmd.Spec.FeatureLevel is null) || (cmd.Spec.FeatureLevel > 10)) {
						func.WriteLine($"if ({cmd.Name} == null) throw new VVK.FunctionNotLoadedException(\"{cmd.Name}\");");
					}
					func.WriteLine($"{((cmd.ReturnType != "void") ? "return " : "")}{cmd.Name}({callstr});");
				}
			}

			// Generate static function table
			using (var file = new SourceFile("StaticFunctionTable.cs"))
			using (var table = file.PushBlock("public unsafe static partial class StaticFunctionTable")) {
				// Global functions
				table.WriteLine("/* Global Functions */");
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Global)) {
					table.WriteLine($"private static readonly {cmd.TypeString} _{cmd.Name} = null;");
				}
				table.WriteLine();

				// Instance functions
				table.WriteLine("/* Instance Functions */");
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Instance)) {
					table.WriteLine($"private static {cmd.TypeString} _{cmd.Name} = null;");
				}
				table.WriteLine();

				// Device functions
				table.WriteLine("/* Device Functions */");
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Device)) {
					table.WriteLine($"private static {cmd.TypeString} _{cmd.Name} = null;");
				}
				table.WriteLine();

				// Instance initializer
				table.WriteLine("/// <summary>Initializes the instance-level functions in the table.</summary>");
				using (var init = table.PushBlock("static void InitFunctionTable(VulkanHandle<VkInstance> inst, VkVersion version)")) {
					init.WriteLine("if (!inst) throw new ArgumentException(\"Cannot initialize function table with null instance\");");
					init.WriteLine("void* addr = null;");
					init.WriteLine("InstanceVersion = version;");
					init.WriteLine();

					foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Instance)) {
						if (cmd.Spec.Alias is not null) {
							init.WriteLine($"_{cmd.Name} = _{cmd.Spec.Alias.Name};");
							init.WriteLine($"if ((_{cmd.Name} == null) && TryLoadFunc(inst, \"{cmd.Name}\", out addr)) {{");
							init.WriteLine($"\t_{cmd.Name} = ({cmd.TypeString})addr;");
							init.WriteLine("}");
						}
						else if (cmd.IsCore) {
							if (cmd.Spec.FeatureLevel! > 10) {
								init.WriteLine($"if (version >= VkVersion.VK_VERSION_1_{cmd.Spec.FeatureLevel % 10}) {{");
								init.WriteLine($"\t_{cmd.Name} = ({cmd.TypeString})LoadFunc(inst, \"{cmd.Name}\");");
								init.WriteLine("}");
							}
							else {
								init.WriteLine($"_{cmd.Name} = ({cmd.TypeString})LoadFunc(inst, \"{cmd.Name}\");");
							}
						}
						else {
							init.WriteLine($"if (TryLoadFunc(inst, \"{cmd.Name}\", out addr)) {{");
							init.WriteLine($"\t_{cmd.Name} = ({cmd.TypeString})addr;");
							init.WriteLine("}");
						}
					}
				}

				// Device initializer
				table.WriteLine("/// <summary>Initializes the device-level functions in the table.</summary>");
				using (var init = table.PushBlock("static void InitFunctionTable(VulkanHandle<VkDevice> dev, VkVersion version)")) {
					init.WriteLine("if (!dev) throw new ArgumentException(\"Cannot initialize function table with null device\");");
					init.WriteLine("void* addr = null;");
					init.WriteLine("DeviceVersion = version;");
					init.WriteLine();

					foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Instance)) {
						if (cmd.Spec.Alias is not null) {
							init.WriteLine($"_{cmd.Name} = _{cmd.Spec.Alias.Name};");
							init.WriteLine($"if ((_{cmd.Name} == null) && TryLoadFunc(dev, \"{cmd.Name}\", out addr)) {{");
							init.WriteLine($"\t_{cmd.Name} = ({cmd.TypeString})addr;");
							init.WriteLine("}");
						}
						else if (cmd.IsCore) {
							if (cmd.Spec.FeatureLevel! > 10) {
								init.WriteLine($"if (version >= VkVersion.VK_VERSION_1_{cmd.Spec.FeatureLevel % 10}) {{");
								init.WriteLine($"\t_{cmd.Name} = ({cmd.TypeString})LoadFunc(dev, \"{cmd.Name}\");");
								init.WriteLine("}");
							}
							else {
								init.WriteLine($"_{cmd.Name} = ({cmd.TypeString})LoadFunc(dev, \"{cmd.Name}\");");
							}
						}
						else {
							init.WriteLine($"if (TryLoadFunc(dev, \"{cmd.Name}\", out addr)) {{");
							init.WriteLine($"\t_{cmd.Name} = ({cmd.TypeString})addr;");
							init.WriteLine("}");
						}
					}
				}

				// Static ctor
				using (var ctor = table.PushBlock("static StaticFunctionTable()")) {
					foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Global)) {
						ctor.WriteLine($"_{cmd.Name} =");
						ctor.WriteLine($"\t({cmd.TypeString})VulkanLibrary.GetExport(\"{cmd.Name}\").ToPointer();");
					}
				}
			}

			// Generate calls for static table
			using (var file = new SourceFile("StaticFunctionTable.Calls.cs"))
			using (var table = file.PushBlock("public unsafe static partial class StaticFunctionTable")) {
				// Global functions
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Global)) {
					var protostr = String.Join(", ", cmd.ParamSets[0].Select(par => $"{par.Type} {par.Name}"));
					var callstr = String.Join(", ", cmd.ParamSets[0].Select(par => par.Name));
					table.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using var func = table.PushBlock($"public static {cmd.ReturnType} {cmd.Name}({protostr})");
					if ((cmd.Spec.FeatureLevel is null) || (cmd.Spec.FeatureLevel > 10)) {
						func.WriteLine($"if (_{cmd.Name} == null) throw new VVK.FunctionNotLoadedException(\"{cmd.Name}\");");
					}
					func.WriteLine($"{((cmd.ReturnType != "void") ? "return " : "")}_{cmd.Name}({callstr});");
				}

				// Instance functions
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Instance)) {
					var protostr = String.Join(", ", cmd.ParamSets[0].Select(par => $"{par.Type} {par.Name}"));
					var callstr = String.Join(", ", cmd.ParamSets[0].Select(par => par.Name));
					table.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using var func = table.PushBlock($"public static {cmd.ReturnType} {cmd.Name}({protostr})");
					func.WriteLine($"if (_{cmd.Name} == null) throw new VVK.FunctionNotLoadedException(\"{cmd.Name}\");");
					func.WriteLine($"{((cmd.ReturnType != "void") ? "return " : "")}_{cmd.Name}({callstr});");
				}

				// Device functions
				foreach (var cmd in spec.Commands.Values.Where(cmd => cmd.Scope == CommandType.CommandScope.Device)) {
					var protostr = String.Join(", ", cmd.ParamSets[0].Select(par => $"{par.Type} {par.Name}"));
					var callstr = String.Join(", ", cmd.ParamSets[0].Select(par => par.Name));
					table.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					using var func = table.PushBlock($"public static {cmd.ReturnType} {cmd.Name}({protostr})");
					func.WriteLine($"if (_{cmd.Name} == null) throw new VVK.FunctionNotLoadedException(\"{cmd.Name}\");");
					func.WriteLine($"{((cmd.ReturnType != "void") ? "return " : "")}_{cmd.Name}({callstr});");
				}
			}
		}
	}
}
