/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Xml;

namespace Gen
{
	// Represents an unprocessed bitmask type in the spec
	public sealed class BitmaskSpec
	{
		#region Fields
		// The name of the bitmask (should end in "Flags")
		public readonly string Name;
		// The name of the enum that contains the backing types (should end in "FlagBits"), null if no values
		public readonly string? BackingName;

		// The backing enum providing the types (if null, then there are no defined values for the bitmask)
		public EnumSpec? BackingType;
		#endregion // Fields

		private BitmaskSpec(string name, string? backingName)
		{
			Name = name;
			BackingName = backingName;
			BackingType = null;
		}

		// Performs parsing for the xml node
		// The node is guaranteed to be a type node with category="bitmask"
		public static bool TryParse(XmlNode xml, out BitmaskSpec? spec)
		{
			spec = null;

			// Get the name
			string name;
			if (xml.SelectSingleNode("name") is XmlNode nameNode) {
				name = nameNode.InnerText;
			}
			else if (xml.Attributes?["name"] is XmlAttribute nameAttr) {
				name = nameAttr.Value;
			}
			else {
				Program.PrintError("Bitmask does not have name");
				return false;
			}

			// Get the required attribute
			string? req = null;
			if (xml.Attributes?["requires"] is XmlAttribute reqAttr) {
				req = reqAttr.Value;
			}

			// Return
			spec = new(name, req);
			return true;
		}
	}
}
