/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Xml;

namespace Gen
{
	// Represents an enum type defined in the spec
	public sealed class EnumSpec
	{
		// Represents a name/value pair contained in an enum spec type
		public record Entry(string Name, int Value, string? Comment);

		#region Fields
		// The name of the enum
		public readonly string Name;
		// The optional comment of the enum
		public string? Comment => Alias?.Comment ?? _comment;
		// If the enum type is a bitmask (annotates with [Flags])
		public bool IsBitmask => Alias?.IsBitmask ?? _isBitmask;
		// The enum values
		public List<Entry> Entries => Alias?.Entries ?? _entries!;
		// The enum type that is aliased by this one.
		public readonly EnumSpec? Alias;
		// If this enum is an alias of another
		public bool IsAlias => Alias is not null;

		// Internal values
		private readonly string? _comment;
		private readonly bool _isBitmask;
		private readonly List<Entry>? _entries;
		#endregion // Fields

		private EnumSpec(string name, string? comment, bool bitmask)
		{
			Name = name;
			_comment = comment;
			_isBitmask = bitmask;
			_entries = new();
			Alias = null;
		}
		private EnumSpec(string name, EnumSpec alias)
		{
			Name = name;
			_comment = null;
			_isBitmask = alias.IsBitmask;
			_entries = null;
			Alias = alias;
		}

		// Parser for enum definition nodes
		public static bool TryParseEnum(XmlNode xml, out EnumSpec? spec)
		{
			spec = null;

			// Validate the name and type attributes
			if ((xml.Attributes?["name"] is not XmlAttribute nameAttr) ||
				(xml.Attributes["type"] is not XmlAttribute typeAttr)) {
				return false;
			}
			bool bitmask = typeAttr.Value == "bitmask";
			if ((typeAttr.Value != "enum") && !bitmask) {
				return false;
			}

			// Construct initial object
			spec = new(nameAttr.Value, xml.Attributes["comment"]?.Value, bitmask);

			// Loop over all enum values
			foreach (var child in xml.ChildNodes) {
				// Filter for enum values
				if ((child is not XmlNode enumNode) ||
					(enumNode.Name != "enum")) {
					continue;
				}

				// Get attributes
				var enumName = enumNode.Attributes?["name"]?.Value;
				var enumComment = enumNode.Attributes?["comment"]?.Value;
				bool bitpos = false;
				var enumValue = enumNode.Attributes?["value"]?.Value;
				if (enumValue is null) {
					enumValue = enumNode.Attributes?["bitpos"]?.Value;
					bitpos = true;
				}
				if (enumName is null || enumValue is null) {
					continue;
				}
				var isHex = enumValue.StartsWith("0x");
				var value = Int32.Parse(enumValue.AsSpan().Slice(isHex ? 2 : 0), 
					isHex ? NumberStyles.HexNumber : NumberStyles.Integer);

				// Add entry
				spec.Entries.Add(new(enumName, bitpos ? (1 << value) : value, enumComment));
			}
			
			// Successful parse
			return true;
		}

		// Parser for enum alias nodes
		public static bool TryParseAlias(XmlNode xml, List<EnumSpec> enums, out EnumSpec? spec)
		{
			spec = null;

			// Validate the name, category, and alias attributes
			if ((xml.Attributes?["name"] is not XmlAttribute nameAttr) ||
				(xml.Attributes?["category"] is not XmlAttribute catAttr) ||
				(xml.Attributes?["alias"] is not XmlAttribute aliasAttr)) {
				return false;
			}
			if (catAttr.Value != "enum") {
				return false;
			}

			// Pull the values
			var enumName = nameAttr.Value;
			var enumAlias = aliasAttr.Value;

			// Find the aliased object and assign
			var alias = enums.Find(e => e.Name == enumAlias);
			if (alias is not null) {
				spec = new(enumName, alias);
				return true;
			}
			else {
				Program.PrintWarning($"The enum alias '{enumAlias}' does not exist, skipping...");
				return false;
			}
		}
	}
}
