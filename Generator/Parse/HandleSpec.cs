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
	// Represents an unprocessed NON_DISPATCHABLE_HANDLE type from the spec
	public sealed class HandleSpec
	{
		#region Fields
		// The name of the handle
		public readonly string Name;
		// The optional aliased handle type, by this name
		public readonly string? AliasName;
		// The parent type of the handle
		public readonly string? ParentType;

		// If the handle type is aliasing another
		public bool IsAlias => AliasName is not null;
		#endregion // Fields

		private HandleSpec(string name, string? alias, string? parent)
		{
			Name = name;
			AliasName = alias;
			ParentType = parent;
		}

		// Parse the spec xml
		public static bool TryParse(XmlNode xml, Dictionary<string, HandleSpec> seen, out HandleSpec? spec)
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
				Program.PrintError("Handle does not have a name");
				return false;
			}

			// Get parent
			string? parent = null;
			if (xml.Attributes?["parent"] is XmlAttribute parentAttr) {
				parent = parentAttr.Value;
			}

			// Optional alias
			string? alias = null;
			if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				alias = aliasAttr.Value;
				if (!seen.TryGetValue(alias, out var aliasSpec)) {
					Program.PrintError($"Unknown handle alias target '{alias}'");
					return false;
				}
				parent = aliasSpec.ParentType;
			}

			// Return
			spec = new(name, alias, parent);
			return true;
		}
	}
}
