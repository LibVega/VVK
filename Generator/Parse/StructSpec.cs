/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Gen
{
	// An unprocessed struct from the spec
	public sealed class StructSpec
	{
		// Repesents a field in the struct
		public record Field(string Name, string Type, string[]? Sizes);

		#region Fields
		// The name of the struct
		public readonly string Name;

		// The type that this struct is aliasing
		public readonly StructSpec? Alias;
		// If this struct type is an alias
		public bool IsAlias => Alias is not null;

		// The struct fields
		public List<Field> Fields => Alias?._fields! ?? _fields!;
		private readonly List<Field>? _fields;
		#endregion // Fields

		private StructSpec(string name)
		{
			Name = name;
			Alias = null;
			_fields = new();
		}
		private StructSpec(string name, StructSpec alias)
		{
			Name = name;
			Alias = alias;
			_fields = null;
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

		// Parse a struct field from xml, xml is a <member> in a struct <type>
		public static bool TryParseField(XmlNode xml, out Field? field)
		{
			field = null;

			// Get the raw type and name
			if (xml.SelectSingleNode("name") is not XmlNode nameNode) {
				Program.PrintError("Struct field does not have a name");
				return false;
			}
			if (xml.SelectSingleNode("type") is not XmlNode typeNode) {
				Program.PrintError($"Struct field {nameNode.InnerText} does not have a type");
				return false;
			}
			var ptrCount = xml.InnerText.Count(c => c == '*');

			// Get the size constant (Regex is *very* expensive, so check Contains() first)
			string[]? sizes = null;
			if (xml.InnerText.Contains('[') && !xml.InnerText.Contains("[]")) {
				// Do a regex search for "[<size>]" fields
				var matches = Regex.Matches(xml.InnerText, @"\[(.+?)\]");
				if (matches.Count == 0) {
					Program.PrintError($"Unable to parse size constants for struct field {nameNode.Value}");
					return false;
				}
				sizes = matches.Select(mch => mch.Groups[1].Value).ToArray();
			}

			// Return
			field = new(nameNode.InnerText, typeNode.InnerText + new string('*', ptrCount), sizes);
			return true;
		}
	}
}
