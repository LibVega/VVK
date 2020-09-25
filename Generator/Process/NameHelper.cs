/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Gen
{
	// Performs name translations and other utilities for a specific parsed spec file
	public sealed class NameHelper
	{
		// TextInfo (for casing) for default culture
		private static readonly TextInfo TEXT_INFO = CultureInfo.InvariantCulture.TextInfo;
		// Known overrides for hard-to-catch cases
		private static readonly Dictionary<string, string> OVERRIDES = new() {
			{ "VK_STENCIL_FRONT_AND_BACK", "FrontAndBack_ALIAS" },
			{ "VK_QUERY_SCOPE_COMMAND_BUFFER_KHR", "CommandBuffer_ALIAS" },
			{ "VK_QUERY_SCOPE_RENDER_PASS_KHR", "CommandBuffer_ALIAS" },
			{ "VK_QUERY_SCOPE_COMMAND_KHR", "CommandBuffer_ALIAS" }
		};

		#region Fields
		// The list of known vendor names from the spec
		public readonly List<string> VendorNames;
		#endregion // Fields

		public NameHelper(List<string> vendors)
		{
			VendorNames = vendors;
		}

		// Processes a spec enum name into components for an output enum
		// Ex: VkEnumNameEXT -> Vk.EXT.EnumName     (vendor extraction)
		// Ex: VkBitmaskNameFlagBits -> Vk.BitmaskNameFlags     (bitmask aware)
		public bool ProcessEnumName(string vkname, out string outname, out string? vendor)
		{
			outname = vkname;
			vendor = null;

			// Validate
			if (!vkname.StartsWith("Vk")) {
				Program.PrintError($"The enum name '{vkname}' is invalid");
				return false;
			}

			// Check for vendor ending
			var vidx = VendorNames.FindIndex(ven => vkname.EndsWith(ven));
			if (vidx >= 0) {
				vendor = VendorNames[vidx];
			}

			// Extract the base name
			outname = vkname.Substring(2, vkname.Length - 2 - (vendor ?? String.Empty).Length);

			// Replace bitmask names
			if (outname.EndsWith("FlagBits")) {
				outname = outname.Replace("FlagBits", "Flags");
			}

			return true;
		}

		// Processes a spec enum value name into a C# friendly name
		// Requires the processed enum name for correct reduction
		// Ex: VK_ENUM_NAME_ENUM_VALUE -> EnumValue         (enum name aware)
		// Ex: VK_ENUM_NAME_VALUE_EXT -> Value             (vendor name aware)
		// Ex: VK_ENUM_NAME_20 -> E20                     (digit aware)
		public bool ProcessEnumValueName(string vkname, string enumvkname, out string outname)
		{
			outname = vkname;

			// Check for overrides first
			if (OVERRIDES.TryGetValue(vkname, out var @override)) {
				outname = @override;
				return true;
			}
			if (enumvkname == "Result") { // Tricky enum that does not follow naming rules at all
				outname = TEXT_INFO.ToTitleCase(outname.ToLower()).Replace("_", "");
				outname = outname.Substring(outname.StartsWith("VkError") ? "VkError".Length : "Vk".Length);
				return true;
			}

			// Check for the vendor and _BIT
			if (enumvkname != "VendorId") {
				var vidx = VendorNames.FindIndex(ven => vkname.EndsWith(ven));
				if (vidx >= 0) {
					outname = outname.Substring(0, outname.Length - VendorNames[vidx].Length - 1);
				}
			}
			if (outname.EndsWith("_BIT")) {
				outname = outname.Substring(0, outname.Length - "_BIT".Length);
			}

			// Collapse into TitleCase
			outname = TEXT_INFO.ToTitleCase(outname.ToLower()).Replace("_", "");

			// Remove the enum name
			var enumLen = enumvkname.Length - (enumvkname.Contains("Flags") ? "Flags".Length : 0);
			outname = outname.Substring(enumLen + 2);

			// Prepend E for digits
			if (Char.IsDigit(outname[0])) {
				outname = 'E' + outname;
			}

			return true;
		}
	}
}
