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
			Extensions.Add("", new Extension("")); // The root (Vulkan core) extension
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

			return true;
		}
	}
}
