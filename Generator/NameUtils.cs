/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Gen
{
	// Contains functionality for working with and translating Vulkan spec names
	public static class NameUtils
	{
		// All known extension names (not populated until a ParseResult is created)
		public static readonly List<string> EXTENSION_NAMES = new();

		// TextInfo (for casing) for default culture
		private static readonly TextInfo TEXT_INFO = CultureInfo.InvariantCulture.TextInfo;


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

		// Converts the spec enum name ("VK_ENUM_NAME_VALUE_NAME") into a C# enum name ("ValueName")
		// Performs the following operations:
		//   1. Removes the leading name of the enum ("VK_ENUM_NAME_VALUE_NAME" -> "VALUE_NAME")
		//   2. Removes the trailing "*BIT" for bitmask values
		//   3. Removes the trailing extension name
		//   4. Prepends 'E' to names that start with a digit
		// Note that this function has a few special cases that need to be hardcoded, as detecting
		//   when special cases need to be applied is difficult.
		public static bool ConvertEnumValueName(string vkname, string enumName, out string name)
		{
			// Calculate the enum name length by counting capitals
			// -1 for "*Flags" since that does not appear in the raw enum names
			var enumLen = enumName.Count(char.IsUpper) - (enumName.EndsWith("Flags") ? 1 : 0);
			if (enumName == "Result") {
				enumLen = 0; // Special case
			}

			// Split the raw name into components after converting to TitleCase
			// ToLowerInvariant required since ToTitleCase() does not change all-caps words
			var vksplit = TEXT_INFO.ToTitleCase(vkname.ToLowerInvariant()).Split('_');

			// Re-join the string, removing "Vk", the enum name, and "_BIT"
			bool isExt = EXTENSION_NAMES.FindIndex(en => vkname.EndsWith(en)) != -1;
			if (enumName == "VendorId") {
				isExt = false; // Special case
			}
			bool isBit = vksplit[(isExt ? ^2 : ^1)] == "Bit";
			name = String.Join(null, vksplit, startIndex: enumLen + 1, // +1 for "Vk"
				vksplit.Length - enumLen - 1 - (isBit ? 1 : 0) - (isExt ? 1 : 0));

			// Check for starting with digit
			if (char.IsDigit(name[0])) {
				name = 'E' + name;
			}
			
			return true;
		}
	}
}
