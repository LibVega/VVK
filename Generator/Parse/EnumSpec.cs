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
		public readonly string? Comment;
		// If the enum type is a bitmask (annotates with [Flags])
		public readonly bool Bitmask;
		// The enum values
		public readonly List<Entry> Entries;
		#endregion // Fields

		private EnumSpec(string name, string? comment, bool bitmask)
		{
			Name = name;
			Comment = comment;
			Bitmask = bitmask;
			Entries = new();
		}

		public static bool TryParse(XmlNode xml, out EnumSpec? spec)
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
				var enumValue = enumNode.Attributes?["value"]?.Value ?? enumNode.Attributes?["bitpos"]?.Value;
				if (enumName == null || enumValue == null) {
					continue;
				}
				var isHex = enumValue.StartsWith("0x");
				var value = Int32.Parse(enumValue.AsSpan().Slice(isHex ? 2 : 0), 
					isHex ? NumberStyles.HexNumber : NumberStyles.Integer);

				// Add entry
				spec.Entries.Add(new(enumName, bitmask ? (1 << value) : value, enumComment));
			}
			
			// Successful parse
			return true;
		}
	}
}
