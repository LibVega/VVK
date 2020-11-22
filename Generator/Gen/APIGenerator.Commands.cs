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
		}
	}
}
