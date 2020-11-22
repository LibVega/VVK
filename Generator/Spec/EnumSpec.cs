/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Gen
{
	// Loaded specification for a category="enum" type
	public sealed class EnumSpec
	{
		// Enum value spec
		public sealed record Value(
			string Name,     // The name of the value
			string ValueStr, // The raw string for the value
			bool IsBitpos    // If the value string is a bitpos instead of a direct value
		);

		#region Fields
		// The enum name
		public readonly string Name;
		// The enum values
		public IReadOnlyList<Value> Values => (Alias is not null) ? Alias.Values : _values!;
		private readonly List<Value> _values = new();

		// Optional alias type
		public readonly EnumSpec? Alias;
		#endregion // Fields

		private EnumSpec(string name, EnumSpec? alias)
		{
			Name = name;
			Alias = alias;
		}

		// Try to parse and add enum value
		public bool TryAddValue(XmlElement node)
		{
			bool isBitmask = node.GetAttribute("type") == "bitmask";

			// Get value name
			if (node.GetAttributeNode("name") is not XmlAttribute nameAttr) {
				Program.PrintError($"Enum '{Name}' has value without name");
				return false;
			}
			var name = nameAttr.Value;

			// Check for alias
			if (node.GetAttributeNode("alias") is XmlAttribute aliasAttr) {
				if (_values.FirstOrDefault(val => val.Name == aliasAttr.Value) is not Value valueAlias) {
					Program.PrintError($"Enum value '{Name}.{name}' is aliased to unknown value '{aliasAttr.Value}'");
					return false;
				}

				// Exit early with alias
				_values.Add(new(name, valueAlias.ValueStr, valueAlias.IsBitpos));
				return true;
			}

			// Get the value string
			string valStr;
			bool isBitpos = false;
			if (node.GetAttributeNode("value") is XmlAttribute valueAttr) {
				valStr = valueAttr.Value;
			}
			else if (node.GetAttributeNode("bitpos") is XmlAttribute bitposAttr) {
				valStr = bitposAttr.Value;
				isBitpos = true;
			}
			else {
				Program.PrintError($"Could not find value for enum value '{Name}.{name}'");
				return false;
			}

			// Check for negative dir
			bool isNeg = false;
			if (node.GetAttributeNode("dir") is XmlAttribute dirAttr) {
				isNeg = dirAttr.Value == "-";
			}

			_values.Add(new(name, isNeg ? ('-' + valStr) : valStr, isBitpos));
			return true;
		}

		// Try to parse an extension or promoted value
		public bool TryAddExtensionValue(XmlElement node, string name, uint extNumber)
		{
			// Get the value based on bitpos or offset
			if (node.GetAttributeNode("bitpos") is XmlAttribute bitposAttr) {
				if (!UInt32.TryParse(bitposAttr.Value, out var bitpos)) {
					Program.PrintError($"Invalid bitpos value for enum value '{name}'");
					return false;
				}
				var value = bitpos;
				_values.Add(new(name, value.ToString(), true));
				return true;
			}
			else if (node.GetAttributeNode("offset") is XmlAttribute offsetAttr) {
				if (!UInt32.TryParse(offsetAttr.Value, out var offset)) {
					Program.PrintError($"Invalid offset value for enum value '{name}'");
					return false;
				}
				var value = (int)(1_000_000_000 + ((extNumber - 1) * 1000) + offset);
				if ((node.GetAttributeNode("dir") is XmlAttribute dirAttr) && (dirAttr.Value == "-")) {
					value = -value;
				}

				_values.Add(new(name, value.ToString(), false));
				return true;
			}
			else if (node.GetAttributeNode("value") is XmlAttribute valueAttr) {
				if (!Int32.TryParse(valueAttr.Value, out var val)) {
					Program.PrintError($"Invalid value for enum value '{name}'");
					return false;
				}
				_values.Add(new(name, val.ToString(), false));
				return true;
			}
			else {
				Program.PrintError($"Missing value for enum value '{name}'");
				return false;
			}
		}

		// Try to add an aliased value
		public bool TryAddAliasValue(XmlElement node, XmlAttribute aliasAttr)
		{
			if (node.GetAttributeNode("name") is not XmlAttribute nameAttr) {
				Program.PrintError("Enum extension alias does not have name");
				return false;
			}

			var alias = _values.FirstOrDefault(val => val.Name == aliasAttr.Value);
			if (alias is not null) {
				_values.Add(new(nameAttr.Value, alias.ValueStr, alias.IsBitpos));
				return true;
			}

			Program.PrintError($"Unknown enum value alias target '{aliasAttr.Value}'");
			return false;
		}

		// Try parse
		public static bool TryParse(XmlElement node, Dictionary<string, EnumSpec> found, out EnumSpec? spec)
		{
			spec = null;

			// Get name
			if (node.GetAttributeNode("name") is not XmlAttribute nameAttr) {
				Program.PrintError("Enum type does not have a name");
				return false;
			}
			string name = nameAttr.Value;

			// Check for alias
			EnumSpec? alias = null;
			if (node.GetAttributeNode("alias") is XmlAttribute aliasAttr) {
				if (!found.TryGetValue(aliasAttr.Value, out alias)) {
					Program.PrintError($"Enum type '{name}' is aliased to unknown type '{aliasAttr.Value}'");
					return false;
				}
			}

			// Return
			spec = new(name, alias);
			return true;
		}
	}
}
