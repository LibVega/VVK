/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.IO;

namespace Gen
{
	// Manages the generation context to generate C# source from the parsed spec
	public static partial class Generator
	{
		// Top-level generation function
		public static bool Generate(ProcessResult spec)
		{
			Console.WriteLine("Generating source...");

			// Make sure all of the extension folders are generated
			foreach (var ext in spec.Extensions) {
				var dirPath = Path.Combine(ArgParse.OutputPath, ext.Value.FolderName);
				if (!Directory.Exists(dirPath)) {
					Directory.CreateDirectory(dirPath);
				}
			}

			// Generate each of the types in order
			Console.WriteLine("Generating enums...");
			foreach (var ext in spec.Extensions) {
				if (!GenerateEnums(ext.Value)) {
					return false;
				} 
			}

			return true;
		}

		// Enum and bitmask generation
		private static bool GenerateEnums(Extension ext)
		{
			if (ArgParse.Verbose) {
				Console.WriteLine($"Generating enums for {ext.DisplayName}...");
			}

			// Top-level file and namespace block
			var fileName = Path.Combine(ext.FolderName, $"{ext.DisplayName}.Enums.cs");
			var fileCom = $"Contains the {(ext.IsCore ? "core" : $"{ext.DisplayName} extension")} enum types.";
			using var file = new FileGenerator(fileName, fileCom);
			using (var nsBlock = file.PushBlock("namespace Vk")) {

			}

			return true;
		}
	}
}
