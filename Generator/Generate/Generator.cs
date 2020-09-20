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

			using (var file = new FileGenerator("./Vulkan.cs", "Main file")) {

			}

			return true;
		}
	}
}
