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
	public sealed class StructSpec
	{
		#region Fields
		// The name of the struct
		public readonly string Name;

		// The type that this struct is aliasing
		public readonly StructSpec? Alias;
		// If this struct type is an alias
		public bool IsAlias => Alias is not null;
		#endregion // Fields

		private StructSpec(string name)
		{
			Name = name;
			Alias = null;
		}
		private StructSpec(string name, StructSpec alias)
		{
			Name = name;
			Alias = alias;
		}

		// Parse an xml node into a struct type
		public static bool TryParse(XmlNode xml, Dictionary<string, StructSpec> seen, out StructSpec? spec)
		{
			spec = null;

			// Get the name
			if (xml.Attributes?["name"] is not XmlAttribute nameAttr) {
				Program.PrintError("Struct type does not have name");
				return false;
			}

			// Get alias and return early
			if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				if (seen.TryGetValue(aliasAttr.Value, out var alias)) {
					spec = new(nameAttr.Value, alias);
					return true;
				}
				else {
					Program.PrintError($"Unknown struct alias target '{aliasAttr.Value}'");
					return false;
				}
			}

			// Return
			spec = new(nameAttr.Value);
			return true;
		}
	}
}
