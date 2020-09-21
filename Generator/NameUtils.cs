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
		// Known typing prefixes for field names
		private static readonly string[] FIELD_PREFIXES = new[] {
			"p", "pp", "ppp", "pppp", "s", "pfn"
		};
		// Typedefs in the Vulkan spec
		private static readonly Dictionary<string, string> VK_TYPEDEFS = new() {
			{ "VkDeviceSize", "ulong" }, { "VkDeviceAddress", "ulong" }, { "VkSampleMask", "uint" }
		};
		// Known builtin primitive types and their mappings
		private static readonly Dictionary<string, string> PRIMITIVE_TYPES = new() {
			{ "uint8_t", "byte" }, { "int8_t", "sbyte" },
			{ "uint16_t", "ushort" }, { "int16_t", "short" },
			{ "uint32_t", "uint" }, { "int32_t", "int" }, { "int", "int" },
			{ "uint64_t", "ulong" }, { "int64_t", "long" },
			{ "size_t", "ulong" }, { "char", "byte" },
			{ "float", "float" }, { "double", "double" },
			{ "void", "void" }
		};
		// Platform-specific types that show up in the spec (mostly WSI stuff)
		// "void*" marks it as actually being a void* value
		// "void" marks it as a struct that is unknown, and should be used as a pointer
		private static readonly Dictionary<string, string> PLATFORM_TYPES = new() {
			{ "ANativeWindow", "void" }, { "ANativeBuffer", "void" }, { "AHardwareBuffer", "void" },
			{ "HINSTANCE", "void*" }, { "HWND", "void*" }, { "HANDLE", "void*" },
				{ "HMONITOR", "void*" },
			{ "wl_display", "void" }, { "wl_surface", "void" },
			{ "Display", "void" }, { "Window", "ulong" },
			{ "xcb_connection_t", "void" }, { "xcb_window_t", "uint" },
			{ "IDirectFB", "void" }, { "IDirectFBSurface", "void" },
			{ "zx_handle_t", "uint" },
			{ "GgpStreamDescriptor", "uint" }, { "GgpFrameToken", "uint" },
			{ "SECURITY_ATTRIBUTES", "void" }, { "DWORD", "uint" }, { "LPCWSTR", "ushort*" },
			{ "CAMetalLayer", "void" },

		};


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

		// Converts struct field names from the spec format to C# format
		// Performs the following operations:
		//   1. Removes leading "s", "p", "pp", "pfn" type notation characters
		//   2. Converts the name to TitleCase
		public static bool ConvertStructField(string vkname, out string name)
		{
			// Cut off the type prefix
			var capIdx = vkname.TakeWhile(nc => char.IsLower(nc)).Count();
			var prefix = vkname.Substring(0, capIdx);
			if (prefix.Length > 0 && FIELD_PREFIXES.Contains(prefix)) {
				vkname = vkname.Substring(capIdx);
			}

			// Create TitleCase
			name = char.IsLower(vkname[0]) ? (char.ToUpper(vkname[0]) + vkname.Substring(1)) : vkname;
			return true;
		}

		// Converts the general spec type name into the corresponding C# type name
		// It checks for the following:
		//   1. A small subset of Vulkan typedefs
		//   2. Checks if it is a Vulkan type (starts with "Vk")
		//   3. Checks if the type is a builtin primitive
		//   4. Returns "void" if the type is a known opaque handle type (should be treated as "void*")
		public static bool ConvertFieldTypeName(string vkname, out string name)
		{
			// Check against the Vulkan typedefs
			if (VK_TYPEDEFS.TryGetValue(vkname, out var typedef)) {
				name = typedef;
				return true;
			}

			// Check if it is a function handle
			if (vkname.StartsWith("PFN_")) {
				name = "FUNCTION";
				return true;
			}

			// Try to convert it to a Vulkan type (this is very fast if the name does not start with "Vk")
			if (ConvertTypeName(vkname, out var vkName, out var vkExt)) {
				name = (vkExt.Length == 0) ? $"Vk.{vkName}" : $"Vk.{vkExt}.{vkName}";
				return true;
			}

			// Check the primitive names
			if (PRIMITIVE_TYPES.TryGetValue(vkname, out var primType)) {
				name = primType;
				return true;
			}

			// Last chance, see if it is a known platform type
			if (PLATFORM_TYPES.TryGetValue(vkname, out var platType)) {
				name = platType;
				return true;
			}

			// Could not convert
			name = String.Empty;
			return false;
		}
	}
}
