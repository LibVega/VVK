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
	// Represents unprocessed enum/bitmask info loaded from the spec file
	public sealed class EnumSpec
	{
		#region Fields
		// The raw typename in the spec
		public readonly string Name;
		
		// The type that this type aliases
		public readonly EnumSpec? Alias;
		// If this type is aliasing another
		public bool IsAlias => Alias is not null;

		// If the type is a bitmask (VkFlags)
		public bool IsBitmask => Alias?._isBitmask ?? _isBitmask;
		private readonly bool _isBitmask;
		#endregion // Fields

		private EnumSpec(string name, bool isBitmask)
		{
			Name = name;
			Alias = null;
			_isBitmask = isBitmask;
		}
		private EnumSpec(string name, EnumSpec alias)
		{
			Name = name;
			Alias = alias;
		}

		// Parse the initial type from XML
		public static bool TryParse(XmlNode xml, Dictionary<string, EnumSpec> seen, out EnumSpec? spec)
		{
			spec = null;

			// Get name
			if (xml.Attributes?["name"] is not XmlAttribute nameAttr) {
				Program.PrintError("Enum does not have a name");
				return false;
			}

			// Optional alias
			if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				if (seen.TryGetValue(aliasAttr.Value, out var alias)) {
					spec = new(nameAttr.Value, alias);
					return true;
				}
				else {
					Program.PrintError($"Unknown enum alias target '{aliasAttr.Value}'");
					return false;
				}
			}

			// Normal return
			spec = new(nameAttr.Value, nameAttr.Value.Contains("FlagBits"));
			return true;
		}
	}
}
