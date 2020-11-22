/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// Performs the value generation functions
	public static partial class APIGenerator
	{
		// Generate the constants
		private static void GenerateConstants(ProcessedSpec spec)
		{
			// Open constants file and class
			using var file = new SourceFile("Constants.cs");
			using var block = file.PushBlock("public static class VkConstants");

			// Write the constants
			block.WriteLine("/* API Constants */");
			foreach (var @const in spec.Constants.Values) {
				var typestr = @const.Type switch { 
					ConstantValue.ValueType.Float => "float",
					ConstantValue.ValueType.Int => "uint",
					_ => "ulong"
				};
				var valstr = (@const.Type == ConstantValue.ValueType.Float)
					? $"{@const.ValueFloat}f"
					: $"{@const.ValueInt}";
				block.WriteLine($"public const {typestr} {@const.Name} = {valstr};");
			}
			block.WriteLine();

			// Write the extension values
			block.WriteLine("/* Extensions */");
			foreach (var ext in spec.Extensions.Values) {
				var upper = ext.Name.Substring("VK_".Length).ToUpperInvariant();
				var namename = upper + "_EXTENSION_NAME";
				var versname = upper + "_SPEC_VERSION";
				block.WriteLine($"public const string {namename} = \"{ext.Name}\";");
				block.WriteLine($"public const uint {versname} = {ext.Version};");
			}
		}
	}
}
