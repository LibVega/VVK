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
	// Loaded specification for a category="handle" type
	public sealed class HandleSpec
	{
		#region Fields
		// Handle name
		public readonly string Name;
		// Parent handle type
		public string? Parent => (Alias is not null) ? Alias.Parent : _parent;
		private readonly string? _parent;

		// Optional alias
		public readonly HandleSpec? Alias;
		#endregion // Fields

		private HandleSpec(string name, string? parent, HandleSpec? alias)
		{
			Name = name;
			_parent = parent;
			Alias = alias;
		}

		// Try to parse
		public static bool TryParse(XmlElement node, Dictionary<string, HandleSpec> found, out HandleSpec? spec)
		{
			spec = null;

			// Get the name
			string name;
			if (node.SelectSingleNode("name") is XmlElement nameNode) { // Standard handle
				name = nameNode.InnerText;
			}
			else if (node.GetAttributeNode("name") is XmlAttribute nameAttr) { // Aliased handle
				name = nameAttr.Value;
			}
			else {
				Program.PrintError("Handle type does not have a name");
				return false;
			}

			// Check for parent
			string? parent = null;
			if (node.GetAttributeNode("parent") is XmlAttribute parentAttr) {
				parent = parentAttr.Value;
			}

			// Check for alias
			HandleSpec? alias = null;
			if (node.GetAttributeNode("alias") is XmlAttribute aliasAttr) {
				if (!found.TryGetValue(aliasAttr.Value, out alias)) {
					Program.PrintError($"Handle '{name}' is aliased to unknown type '{aliasAttr.Value}'");
					return false;
				}
			}

			// Return
			spec = new(name, parent, alias);
			return true;
		}
	}
}
