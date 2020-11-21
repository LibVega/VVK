/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Gen
{
	// Contains utilities for converting spec names
	public static class NameHelper
	{
		// String builder instance
		private static readonly StringBuilder BUILDER = new(1024);
		// TitleCase info object
		private static readonly TextInfo TEXT_INFO = CultureInfo.InvariantCulture.TextInfo;
		// Enum values that are known to not follow naming conventions 
		private static readonly List<string> BAD_NAMES = new() {
			"VK_QUERY_SCOPE_COMMAND_BUFFER_KHR",
			"VK_QUERY_SCOPE_RENDER_PASS_KHR",
			"VK_QUERY_SCOPE_COMMAND_KHR"
		};
		// Vulkan types that are remapped to other types
		private static readonly Dictionary<string, string> VULKAN_REMAP_TYPES = new() {
			{ "VkDeviceSize",    "ulong" },
			{ "VKDeviceAddress", "ulong" },
			{ "VkSampleMask",    "uint"  },
			{ "VkFlags",         "uint"  }
		};
		// Mapping of builtin spec types
		private static readonly Dictionary<string, string> BUILTIN_TYPE_MAPPING = new() { 
			{ "void",    "void"  },
			{ "char",    "byte"  },
			{ "float",   "float" }, { "double",   "double" },
			{ "uint8_t", "byte"  }, { "uint16_t", "ushort" }, { "uint32_t", "uint" }, { "uint64_t", "ulong" },
			{ "int8_t",  "sbyte" }, { "int16_t",  "short"  }, { "int32_t",  "int"  }, { "int64_t",  "long"  },
			{ "size_t",  "ulong" }, { "int",      "int"    }
		};
		// Mapping of platform spec types
		private static readonly Dictionary<string, string> PLATFORM_TYPE_MAPPING = new() {
			{ "ANativeWindow",       "void"    },
			{ "wl_display",          "void"    },
			{ "wl_surface",          "void"    },
			{ "HINSTANCE",           "void*"   },
			{ "HWND",                "void*"   },
			{ "Display",             "void"    },
			{ "Window",              "ulong"   },
			{ "xcb_connection_t",    "void"    },
			{ "xcb_window_t",        "uint"    },
			{ "IDirectFB",           "void"    },
			{ "IDirectFBSurface",    "void"    },
			{ "zx_handle_t",         "uint"    },
			{ "GgpStreamDescriptor", "uint"    },
			{ "HANDLE",              "void*"   },
			{ "SECURITY_ATTRIBUTES", "void"    },
			{ "DWORD",               "uint"    },
			{ "LPCWSTR",             "ushort*" },
			{ "CAMetalLayer",        "void"    },
			{ "AHardwareBuffer",     "void"    },
			{ "GgpFrameToken",       "uint"    },
			{ "HMONITOR",            "void*"   },
			{ "VisualID",            "ulong"   },
			{ "xcb_visualid_t",      "uint"    },
			{ "RROutput",            "ulong"   }
		};
		// Mapping of API constant array sizes to types
		private static readonly Dictionary<string, string> FIXED_TYPE_MAPPING = new() {
			{ "VK_MAX_PHYSICAL_DEVICE_NAME_SIZE", "VVK.DeviceName" },
			{ "VK_UUID_SIZE", "VVK.UUID" },
			{ "VK_LUID_SIZE", "VVK.LUID" },
			{ "VK_MAX_EXTENSION_NAME_SIZE", "VVK.ExtensionName" },
			{ "VK_MAX_DESCRIPTION_SIZE", "VVK.Description" },
			{ "VK_MAX_DRIVER_NAME_SIZE", "VVK.DriverName" },
			{ "VK_MAX_DRIVER_INFO_SIZE", "VVK.DriverInfo" },
		};
		// Known types that can be used in fixed buffers
		private static readonly List<string> FIXED_BUFFER_TYPES = new() { 
			"byte", "sbyte", "ushort", "short", "uint", "int", "ulong", "long", "float", "double"
		};

		// Known handle types
		private static readonly List<string> HANDLE_TYPES = new();
		public static void SetHandleTypes(List<string> handles)
		{
			HANDLE_TYPES.Clear();
			HANDLE_TYPES.AddRange(handles);
		}

		// Known vendor/author tags
		private static readonly List<string> VENDOR_TAGS = new();
		public static void SetVendorTags(List<string> vendors)
		{
			VENDOR_TAGS.Clear();
			VENDOR_TAGS.AddRange(vendors);
		}

		// Attempts to convert a raw spec enum value name to a processed, with the following steps:
		//   1. Remove the leading "VK_"
		//   2. Remove the leading enum name
		//   3. Remove the trailing vendor tag IFF the enum name ends with the same vendor tag
		//   4. Remove the "_BIT_" for bitmask values
		//   5. Compactify the remaining enum value name and convert to TitleCase
		public static string? ConvertEnumValue(string enumName, string valueName)
		{
			// Special case for VkResult, and other values that are known to break naming convensions
			if ((enumName == "VkResult") || BAD_NAMES.Contains(valueName)) {
				// Split and simply remove leading "VK_"
				var split = valueName.Split('_');
				BUILDER.Clear();
				foreach (var part in split.Skip(1)) {
					BUILDER.Append(TEXT_INFO.ToTitleCase(part.ToLowerInvariant()));
				}
				return BUILDER.ToString();
			}

			// Get the enum name info
			var enumVendor = VENDOR_TAGS.FirstOrDefault(tag => enumName.EndsWith(tag));
			var enumWordLen = enumName.Count(ch => Char.IsUpper(ch));
			var isBits = enumName.Contains("FlagBits");
			if (enumVendor is not null) {
				enumWordLen -= enumVendor.Length;
			}
			if (isBits) {
				enumWordLen -= 2; // Remove 2 for "FlagBits"
			}

			// Split value name into components, find the components to cut from the end
			var nameSplit = valueName.Split('_');
			var valueVendor = VENDOR_TAGS.FirstOrDefault(tag => tag == nameSplit[^1]);
			int endCutCount = 0;
			if (enumVendor is not null) {
				if (valueVendor is not null) {
					endCutCount += 1; // Cut vendor tag for values that are already in a vendored enum 
				}
				if (isBits && (nameSplit[^2] == "BIT")) {
					endCutCount += 1; // Cut "_BIT_" from final name
				}
			}
			else if (isBits && (nameSplit[(valueVendor is not null) ? ^2 : ^1] == "BIT")) {
				endCutCount = 1;
				if (valueVendor is not null) {
					nameSplit[^2] = valueVendor; // Makes the cut logic simpler below
				}
			}

			// Recombine the components while fixing capitalization
			BUILDER.Clear();
			foreach (var part in nameSplit.Skip(enumWordLen).Take(nameSplit.Length - enumWordLen - endCutCount)) {
				BUILDER.Append(TEXT_INFO.ToTitleCase(part.ToLowerInvariant()));
			}
			var newName = BUILDER.ToString();
			return Char.IsDigit(newName[0]) ? ('E' + newName) : newName; // Cant start names with a digit
		}

		// Converts a struct field in the spec to a struct field in the generated C#
		// Fixes capitalization to TitleCase
		// Removes hungarian notation (e.g. "p", "pp", "pfn" for pointers, "s" for types)
		// Keeps sType and pNext the same, since they are special values
		public static string ConvertStructField(string fieldName)
		{
			// Keep sType and pNext unchanged
			if ((fieldName == "sType") || (fieldName == "pNext")) {
				return fieldName;
			}

			// Check for known hungarian notations
			if ((fieldName.Length >= 2) && ((fieldName[0] == 'p') || (fieldName[0] == 's')) && Char.IsUpper(fieldName[1])) {
				return fieldName.Substring(1); // "p" or "s"
			}
			if ((fieldName.Length >= 3) && (fieldName[0] == 'p') && (fieldName[1] == 'p') && Char.IsUpper(fieldName[2])) {
				return fieldName.Substring(2); // "pp"
			}
			if ((fieldName.Length >= 4) && fieldName.StartsWith("pfn") && Char.IsUpper(fieldName[3])) {
				return fieldName.Substring(3); // "pfn"
			}

			// Not hungarian notation, just fix up first caps
			return Char.ToUpperInvariant(fieldName[0]) + fieldName.Substring(1);
		}

		// Converts a spec type to a C# output type
		// Any type that begins with "Vk" is passed through unaltered
		// Builtin and platform-specific types are also handled here
		// The output type name will have pointers, if needed
		public static string? ConvertToOutputType(string typeName, uint ptrDepth = 0, string? arraySize = null)
		{
			// Starts with Vk? - just pass through (unless specific remapped type)
			if (typeName.StartsWith("Vk")) {
				if (VULKAN_REMAP_TYPES.TryGetValue(typeName, out var remap)) {
					return (ptrDepth == 0) ? remap : (remap + new string('*', (int)ptrDepth));
				}
				if (HANDLE_TYPES.Contains(typeName)) {
					typeName = $"VkHandle<{typeName}>";
				}
				return (ptrDepth == 0) ? typeName : (typeName + new string('*', (int)ptrDepth));
			}

			// Check for function pointers
			if (typeName.StartsWith("PFN_")) {
				return typeName; // Must be handled externally
			}

			// Check string/uuid/luid types
			if ((arraySize is not null) && FIXED_TYPE_MAPPING.TryGetValue(arraySize, out var mapName)) {
				return mapName;
			}

			// Check builtin types
			if (BUILTIN_TYPE_MAPPING.TryGetValue(typeName, out mapName)) {
				return (ptrDepth == 0) ? mapName : (mapName + new string('*', (int)ptrDepth));
			}

			// Check platform types
			if (PLATFORM_TYPE_MAPPING.TryGetValue(typeName, out mapName)) {
				return (ptrDepth == 0) ? mapName : (mapName + new string('*', (int)ptrDepth));
			}

			return null;
		}

		// Gets the vendor for the type, if any is present
		public static string? GetTypeVendor(string typeName)
		{
			foreach (var vtag in VENDOR_TAGS) {
				if (typeName.EndsWith(vtag)) {
					return vtag;
				}
			}
			return null;
		}

		// Checks if a type can be used in a C# fixed buffer
		public static bool IsFixedBufferType(string typeName) => FIXED_BUFFER_TYPES.Contains(typeName);
	}
}
