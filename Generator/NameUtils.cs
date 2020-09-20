/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Contains functionality for working with and translating Vulkan spec names
	public static class NameUtils
	{
		// All known extension names (not populated until a ParseResult is created)
		public static readonly List<string> EXTENSION_NAMES = new();

		// Parses enum/bitmask/struct names into their component parts, returns if the name could be converted
		// Performs the following operations:
		//   1. Removes the leading "Vk" from the name
		//   2. Checks the end of the name for the extension, and removes/extracts that if present
		//   3. Converts "*FlagBits" into "*Flags" to remove the two-type distinction present in C
		public static bool ConvertTypeName(string vkname, out string name, out string ext)
		{
			name = String.Empty;
			ext = String.Empty;

			// Check the name
			if (!vkname.StartsWith("Vk")) {
				return false;
			}

			// Check for the extension
			var extIdx = EXTENSION_NAMES.FindIndex(en => vkname.EndsWith(en));
			if (extIdx != -1) {
				ext = EXTENSION_NAMES[extIdx];
			}

			// Extract the base name
			var len = vkname.Length - 2 - (ext?.Length ?? 0); // -2 for "Vk"
			name = vkname.Substring(2, len);

			// Additionally convert bitmask names
			if (name.EndsWith("Bits")) {
				name = name.Substring(0, name.Length - "FlagBits".Length) + "Flags";
			}

			return true;
		}
	}
}
