/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Manages the processing steps to convert from the raw spec objects in ParseResult to a usable output format
	public sealed class ProcessResult
	{
		#region Fields
		// The extensions that have been found in the spec
		public readonly Dictionary<string, Extension> Extensions;
		#endregion // Fields

		private ProcessResult()
		{
			Extensions = new();
			Extensions.Add(String.Empty, new Extension(String.Empty)); // The root (Vulkan core) extension
		}

		public Extension GetOrCreateExtension(string name)
		{
			if (Extensions.TryGetValue(name, out var ext)) {
				return ext;
			}
			else {
				ext = new Extension(name);
				Extensions.Add(name, ext);
				return ext;
			}
		}

		// Performs top-level processing
		public static bool Process(ParseResult spec, out ProcessResult proc)
		{
			Console.WriteLine("Processing spec types...");
			proc = new();

			// Process the types in order
			if (!ProcessEnums(spec, proc)) {
				return false;
			}

			return true;
		}

		// Processing for enum types
		private static bool ProcessEnums(ParseResult spec, ProcessResult proc)
		{
			Console.WriteLine("Processing enum types...");

			foreach (var enumSpec in spec.Enums) {
				// Try to parse
				if (!EnumOut.TryProcess(enumSpec, out var enumOut)) {
					continue;
				}

				// Add to extension
				var ext = proc.GetOrCreateExtension(enumOut!.Extension);
				ext.Enums.Add(enumOut);
				if (ArgParse.Verbose) {
					Console.Write($"\tProcessed enum {enumSpec.Name} -> ");
					Console.WriteLine(ext.IsCore ? $"Vk.{enumOut.Name}" : $"Vk.{enumOut.Extension}.{enumOut.Name}");
				}
			}

			return true;
		}
	}
}
