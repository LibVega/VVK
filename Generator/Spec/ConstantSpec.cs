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
	// Loaded specification for an enum value in name="API Constants"
	public sealed class ConstantSpec
	{
		#region Fields
		// Constant name
		public readonly string Name;
		// The constant value as the raw XML string
		public readonly string Value;
		#endregion // Fields

		private ConstantSpec(string name, string value)
		{
			Name = name;
			Value = value;
		}

		// Try parse
		public static bool TryParse(XmlElement node, Dictionary<string, ConstantSpec> found, out ConstantSpec? spec)
		{
			spec = null;

			// Get name
			if (node.GetAttributeNode("name") is not XmlAttribute nameAttr) {
				Program.PrintError("API constant does not have a name");
				return false;
			}
			var name = nameAttr.Value;

			// Check for alias
			if (node.GetAttributeNode("alias") is XmlAttribute aliasAttr) {
				if (!found.TryGetValue(aliasAttr.Value, out var alias)) {
					Program.PrintError($"API constant '{name}' is aliased to unknown constant '{aliasAttr.Value}'");
					return false;
				}

				// Exit with alias value
				spec = new(name, alias.Value);
				return true;
			}

			// Get value
			if (node.GetAttributeNode("value") is not XmlAttribute valueAttr) {
				Program.PrintError($"API constant '{name}' does not have a value");
				return false;
			}

			// Return
			spec = new(name, valueAttr.Value);
			return true;
		}
	}
}
