/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace Gen
{
	// Loaded specification for a category="bitmask" type
	public sealed class BitmaskSpec
	{
		#region Fields
		// The name of the bitmask
		public readonly string Name;
		// The name of the enum containing the bitmask value, or null if no enum values
		public string? BitsName => (Alias is not null) ? Alias.BitsName : _bitsName;
		private readonly string? _bitsName;

		// The aliased spec type
		public readonly BitmaskSpec? Alias;
		#endregion // Fields

		private BitmaskSpec(string name, string? bitsname)
		{
			Name = name;
			_bitsName = bitsname;
		}

		private BitmaskSpec(string name, BitmaskSpec alias)
		{
			Name = name;
			Alias = alias;
		}

		// Try to parse
		public static bool TryParse(XmlElement node, Dictionary<string, BitmaskSpec> found, out BitmaskSpec? spec)
		{
			spec = null;

			// Get the name
			string name;
			if (node.SelectSingleNode("name") is XmlElement nameNode) { // Standard bitmask
				name = nameNode.InnerText;
			}
			else if (node.GetAttributeNode("name") is XmlAttribute nameAttr) { // Aliased bitmask
				name = nameAttr.Value;
			}
			else {
				Program.PrintError("Bitmask type does not have a name");
				return false;
			}

			// Check for alias type
			if (node.GetAttributeNode("alias") is XmlAttribute aliasAttr) {
				if (!found.TryGetValue(aliasAttr.Value, out var aliasSpec)) {
					Program.PrintError($"Bitmask '{name}' is aliased to unknown type '{aliasAttr.Value}'");
					return false;
				}

				// Return early with alias
				spec = new(name, aliasSpec);
				return true;
			}

			// Get the bits name
			string? bitsname = null;
			if (node.GetAttributeNode("requires") is XmlAttribute reqAttr) {
				bitsname = reqAttr.Value;
			}

			// Return
			spec = new(name, bitsname);
			return true;
		}
	}
}
