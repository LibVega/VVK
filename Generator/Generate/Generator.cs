/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// Manages the generation context to generate C# source from the parsed spec
	public static partial class Generator
	{
		// Top-level generation function
		public static bool Generate(ParseResult spec)
		{
			Console.WriteLine("Generating source...");

			// Generate each of the types in order
			if (!GenerateEnums(spec)) {
				return false;
			}

			return true;
		}

		// Enum and bitmask generation
		private static bool GenerateEnums(ParseResult spec)
		{
			Console.WriteLine("Generating enums...");

			// Top-level file and namespace block
			using var file = new FileGenerator("Vk.Enums.cs", "Contains the enums for the core namespace.");
			using (var nsBlock = file.PushBlock("namespace Vk")) {

			}

			return true;
		}
	}
}
