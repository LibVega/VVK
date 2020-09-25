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
		// Known Vk* typedefs
		private static readonly Dictionary<string, string> VK_TYPEDEFS = new() {
			{ "VkDeviceAddress", "ulong" },
			{ "VkSampleMask", "uint" },
			{ "VkFlags", "uint" }
		};
		// Known C++ -> C# types
		private static readonly Dictionary<string, string> CPP_TYPES = new() {
			{ "void", "void" },
			{ "char", "byte" },
			{ "float", "float" }, { "double", "double" },
			{ "uint8_t", "byte" }, { "uint16_t", "ushort" }, { "uint32_t", "uint" }, { "uint64_t", "ulong" },
			{ "int8_t", "sbyte" }, { "int16_t", "short" }, { "int32_t", "int" }, { "int64_t", "long" },
			{ "size_t", "ulong" }, { "int", "int" }
		};

		#region Fields
		// The list of known vendor names from the spec
		public readonly List<string> VendorNames;
		#endregion // Fields

		public NameHelper(List<string> vendors)
		{
			VendorNames = vendors;
		}

		// Processes a spec enum or struct name into components for an output type
		// Ex: VkEnumNameEXT -> Vk.EXT.EnumName     (vendor extraction)
		// Ex: VkBitmaskNameFlagBits -> Vk.BitmaskNameFlags     (bitmask aware)
		public bool ProcessVkTypeName(string vkname, out string outname, out string? vendor)
		{
			outname = vkname;
			vendor = null;

			// Validate
			if (!vkname.StartsWith("Vk")) {
				return false;
			}

			// Check for the typedefs
			if (VK_TYPEDEFS.TryGetValue(vkname, out var typedef)) {
				outname = typedef;
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

		// Processes a generaal type name (either some spec name, or some default type)
		public bool ProcessGeneralTypeName(string name, out string outname)
		{
			outname = String.Empty;

			// Save and strip pointer info
			string ptrstr = String.Empty;
			if (name.EndsWith('*')) {
				var ptrCount = name.Count(c => c == '*');
				ptrstr = new string('*', ptrCount);
				name = name.Substring(0, name.Length - ptrCount);
			}

			// Try to parse a Vulkan type first
			if (ProcessVkTypeName(name, out outname, out var vendor)) {
				outname = (vendor is not null) ? $"Vk.{vendor}.{outname}" : $"Vk.{outname}";
				outname += ptrstr;
				return true;
			}

			// Check Vulkan typedefs and known types
			if (VK_TYPEDEFS.TryGetValue(name, out var typedef)) {
				outname = typedef + ptrstr;
				return true;
			}
			if (CPP_TYPES.TryGetValue(name, out var cppType)) {
				outname = cppType + ptrstr;
				return true;
			}

			// Report failure
			Program.PrintWarning($"Unknown typename '{name}'");
			outname = "UNKNOWN" + ptrstr;
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

		// Processes a struct field into a C#-style name
		// Ex: structFieldName -> StructFieldName           (TitleCase)
		// Ex: pName -> Name              (Remove type prefixes)
		public bool ProcessStructFieldName(string fname, out string outname)
		{
			outname = fname;

			// Check for type prefix
			if (fname[0] == 'p' || fname[0] == 's') {
				if (fname.StartsWith("pfn")) outname = outname.Substring(3);
				else if (fname.Length > 2 && Char.IsUpper(fname[1])) outname = outname.Substring(1);
				else if (fname.Length > 3 && fname[1] == 'p' && Char.IsUpper(fname[2])) outname = outname.Substring(2);
			}

			// Switch to TitleCase
			if (Char.IsLower(outname[0])) {
				outname = Char.ToUpperInvariant(outname[0]) + outname.Substring(1);
			}

			return true;
		}

		// Check if a type name can be a fixed buffer
		public bool IsFixedType(string typename) => CPP_TYPES.Values.Contains(typename);
	}
}
