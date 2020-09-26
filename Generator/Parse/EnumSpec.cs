/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace Gen
{
	// Represents unprocessed enum/bitmask info loaded from the spec file
	public sealed class EnumSpec
	{
		// Represents an entry in the enum
		public record Entry(string Name, int Value);

		#region Fields
		// The raw typename in the spec
		public readonly string Name;
		
		// The type that this type aliases
		public readonly EnumSpec? Alias;
		// If this type is aliasing another
		public bool IsAlias => Alias is not null;

		// If the type is a bitmask (VkFlags)
		public bool IsBitmask => Alias?._isBitmask ?? _isBitmask;
		private readonly bool _isBitmask;
		// The values of the enums
		public List<Entry> Values => Alias?._values! ?? _values!;
		private readonly List<Entry>? _values;
		#endregion // Fields

		private EnumSpec(string name, bool isBitmask)
		{
			Name = name;
			Alias = null;
			_isBitmask = isBitmask;
			_values = new();
		}
		private EnumSpec(string name, EnumSpec alias)
		{
			Name = name;
			Alias = alias;
			_isBitmask = false;
			_values = null;
		}

		// Parse the type from xml (<type> category="bitmask")
		// This will create a new enum called "...FlagBits", not "...Flags"
		// Some of these will be caught later with (<type> category="enum"), which is fine
		public static bool TryParseBitmask(XmlNode xml, Dictionary<string, EnumSpec> seen, out EnumSpec? spec)
		{
			spec = null;

			// Get name
			string name;
			if (xml.Attributes?["name"] is XmlAttribute nameAttr) {
				name = nameAttr.Value;
			}
			else if (xml.SelectSingleNode("name") is XmlNode nameNode) {
				name = nameNode.InnerText;
			}
			else {
				Program.PrintError($"Bitmask spec type does not have name");
				return false;
			}
			name = name.Replace("Flags", "FlagBits");

			// Optional alias
			if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				var aliasName = aliasAttr.Value.Replace("Flags", "FlagBits");
				if (seen.TryGetValue(aliasName, out var alias)) {
					spec = new(name, alias);
					return true;
				}
				else {
					Program.PrintError($"Unknown enum alias target '{aliasAttr.Value}'");
					return false;
				}
			}

			// Return
			spec = new(name, true);
			return true;
		}

		// Parse the initial type from XML (<type> category="enum")
		public static bool TryParseEnum(XmlNode xml, Dictionary<string, EnumSpec> seen, out EnumSpec? spec)
		{
			spec = null;

			// Get name
			if (xml.Attributes?["name"] is not XmlAttribute nameAttr) {
				Program.PrintError("Enum does not have a name");
				return false;
			}

			// Optional alias
			if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				if (seen.TryGetValue(aliasAttr.Value, out var alias)) {
					spec = new(nameAttr.Value, alias);
					return true;
				}
				else {
					Program.PrintError($"Unknown enum alias target '{aliasAttr.Value}'");
					return false;
				}
			}

			// Normal return
			spec = new(nameAttr.Value, nameAttr.Value.Contains("FlagBits"));
			return true;
		}

		// Parse an enum value (xml is an <enum> node within <enums>)
		public static bool TryParseValue(XmlNode xml, List<Entry> seen, out Entry? entry)
		{
			entry = null;

			// Get entry name
			if (xml.Attributes?["name"] is not XmlAttribute nameAttr) {
				Program.PrintError("Enum entry has no name");
				return false;
			}

			// Check for alias first
			if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				var alias = seen.FirstOrDefault(entry => entry.Name == aliasAttr.Value);
				if (alias is null) {
					Program.PrintError($"Unknown enum value alias target '{aliasAttr.Value}'");
					return false;
				}
				entry = new(nameAttr.Value, alias.Value);
				return true;
			}

			// Get the value
			int value;
			if (xml.Attributes?["value"] is XmlAttribute valueAttr) {
				bool isHex = valueAttr.Value.StartsWith("0x");
				var valueStr = isHex ? valueAttr.Value.Substring(2) : valueAttr.Value;
				if (!Int32.TryParse(valueStr, isHex ? NumberStyles.HexNumber : NumberStyles.Number, null, out value)) {
					Program.PrintError($"Could not parse value '{valueAttr.Value}' for enum {nameAttr.Value}");
					return false;
				}
			}
			else if (xml.Attributes?["bitpos"] is XmlAttribute bitposAttr) {
				if (!Int32.TryParse(bitposAttr.Value, out value)) {
					Program.PrintError($"Could not parse bitpos '{bitposAttr.Value}' for enum {nameAttr.Value}");
					return false;
				}
				value = 1 << value;
			}
			else {
				Program.PrintError($"Enum {nameAttr.Value} does not have a value or bitpos entry");
				return false;
			}

			// Return
			entry = new(nameAttr.Value, value);
			return true;
		}

		// Parse an enum addition value (an <enum> in a <require> node)
		public static bool TryParseAddition(XmlNode xml, Dictionary<string, EnumSpec> seen, uint? extBase, 
			out Entry? entry, out EnumSpec? enumSpec)
		{
			entry = null;
			enumSpec = null;

			// Get the enum name
			if (xml.Attributes?["name"] is not XmlAttribute nameAttr) {
				Program.PrintError("Extension enum does not have a name");
				return false;
			}
			var entryName = nameAttr.Value;

			// Get the extended enum type
			if (xml.Attributes?["extends"] is not XmlAttribute extAttr) {
				return true; // This + entry == null indicates a skip, not an error
			}
			if (!seen.TryGetValue(extAttr.Value, out enumSpec)) {
				Program.PrintError($"The enum extension target '{extAttr.Value}' does not exist.");
				return false;
			}

			// Check for an alias
			if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				var alias = enumSpec.Values.FirstOrDefault(v => v.Name == aliasAttr.Value);
				if (alias is null) {
					Program.PrintError($"The enum value alias target '{aliasAttr.Value}' does not exist");
					return false;
				}
				entry = new(entryName, alias.Value);
				return true;
			}

			// Get the offset, value, or bitpos value for the enum
			int value = 0;
			if (xml.Attributes?["offset"] is XmlAttribute offsetAttr) {
				// Get the extension number, if needed
				if (!extBase.HasValue) {
					if (xml.Attributes?["extnumber"] is not XmlAttribute extNumberAttr) {
						Program.PrintError($"The enum extension '{entryName}' is missing the extension number");
						return false;
					}
					if (!Int32.TryParse(extNumberAttr.Value, out var extNum)) {
						Program.PrintError($"Failed to parse extension number '{extNumberAttr.Value}' for '{entryName}'");
						return false;
					}
					extBase = (uint)(1_000_000_000 + ((extNum - 1) * 1_000));
				}

				if (!Int32.TryParse(offsetAttr.Value, out value)) {
					Program.PrintError($"Failed to parse offset '{offsetAttr.Value}' for '{entryName}'");
					return false;
				}
				value += (int)extBase.Value;
			}
			else if (xml.Attributes?["bitpos"] is XmlAttribute bitposAttr) {
				if (!Int32.TryParse(bitposAttr.Value, out value)) {
					Program.PrintError($"Failed to parse bitpos '{bitposAttr.Value}' for '{entryName}'");
					return false;
				}
				value = 1 << value;
			}
			else if (xml.Attributes?["value"] is XmlAttribute valueAttr) {
				if (!Int32.TryParse(valueAttr.Value, out value)) {
					Program.PrintError($"Failed to parse value '{valueAttr.Value}' for '{entryName}'");
					return false;
				}
			}
			else {
				Program.PrintError($"Enum value '{entryName}' is missing a value");
				return false;
			}

			// Return
			entry = new(entryName, value);
			return true;
		}
	}
}
