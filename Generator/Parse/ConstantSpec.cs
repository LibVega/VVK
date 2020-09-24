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
	// Represents an unprocessed enum value used as an API constant
	public sealed class ConstantSpec
	{
		#region Fields
		// The name of the constant
		public readonly string Name;
		// The value of the constant (different types means a string to hold the value for now)
		public readonly string Value;
		#endregion // Fields

		private ConstantSpec(string name, string value)
		{
			Name = name;
			Value = value;
		}

		// Parse from xml (node is the "enums" node with name="API Constants")
		public static bool TryParse(XmlNode xml, Dictionary<string, ConstantSpec> seen, out ConstantSpec? spec)
		{
			spec = null;

			// Get the name
			if (xml.Attributes?["name"] is not XmlAttribute nameAttr) {
				Program.PrintError("API constant is missing name");
				return false;
			}

			// Get the value
			string value;
			if (xml.Attributes?["value"] is XmlAttribute valueAttr) {
				value = valueAttr.Value;
			}
			else if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				if (seen.TryGetValue(aliasAttr.Value, out var alias)) {
					value = alias.Value;
				}
				else {
					Program.PrintError($"Unknown constant alias target '{aliasAttr.Value}'");
					return false;
				}
			}
			else {
				Program.PrintError($"API constant {nameAttr.Value} does not have a value or alias");
				return false;
			}

			// Return
			spec = new(nameAttr.Value, value);
			return true;
		}
	}
}
