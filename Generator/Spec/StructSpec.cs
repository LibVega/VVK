/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Gen
{
	// Loaded specification for a category="struct" or category="union" type
	public sealed class StructSpec
	{
		// Struct field type
		public sealed record Field(
			string Name,       // The name of the field
			string Type,       // The raw field type
			uint PtrDepth,     // The pointer depth of the field type
			string? ArraySize, // The size text of the array if the field is an array
			string? Length,    // The length reference for pointer-like fields
			string? Value,     // The potential known value of the field
			bool Optional      // If the field is optional
		);

		#region Fields
		// The struct name
		public readonly string Name;
		// If the type is a union type
		public bool IsUnion => Alias?.IsUnion ?? _isUnion;
		private readonly bool _isUnion;
		// The fields of the struct
		public IReadOnlyList<Field> Fields => Alias?.Fields ?? _fields!;
		private readonly List<Field>? _fields;

		// The optional alias type
		public readonly StructSpec? Alias;
		#endregion // Fields

		private StructSpec(string name, StructSpec alias)
		{
			Name = name;
			Alias = alias;
		}

		private StructSpec(string name, bool isUnion, List<Field> fields)
		{
			Name = name;
			_isUnion = isUnion;
			_fields = fields;
		}

		// Try parse
		public static bool TryParse(XmlElement node, Dictionary<string, StructSpec> found, out StructSpec? spec)
		{
			spec = null;

			// Get name
			if (node.GetAttributeNode("name") is not XmlAttribute nameAttr) {
				Program.PrintError("Struct type does not have name");
				return false;
			}
			string name = nameAttr.Value;

			// Check for alias
			if (node.GetAttributeNode("alias") is XmlAttribute aliasAttr) {
				if (!found.TryGetValue(aliasAttr.Value, out var alias)) {
					Program.PrintError($"Struct type '{name}' is aliased to unknown type '{aliasAttr.Value}'");
					return false;
				}

				// Exit early for aliases
				spec = new(name, alias);
				return true;
			}

			// Get type
			bool isUnion = node.GetAttributeNode("category")!.Value == "union";

			// Parse the fields
			List<Field> fields = new();
			foreach (var memNode in node.SelectNodes("member")!) {
				if (!TryParseField((memNode as XmlElement)!, name, out var field)) {
					return false;
				}
				fields.Add(field!);
			}

			// Return
			spec = new(name, isUnion, fields);
			return true;
		}

		// Try parse member node
		private static bool TryParseField(XmlElement node, string sname, out Field? field)
		{
			field = null;

			// Get name
			if (node.SelectSingleNode("name") is not XmlElement nameNode) {
				Program.PrintError($"Struct '{sname}' has member without name");
				return false;
			}
			var name = nameNode.InnerText;

			// Get comment (not used directly)
			var comment = String.Empty;
			if (node.SelectSingleNode("comment") is XmlElement cmtNode) {
				comment = cmtNode.InnerText;
			}
			var innerText = (comment.Length != 0)
				? node.InnerText.Replace(comment, null)
				: node.InnerText;

			// Get type
			if (node.SelectSingleNode("type") is not XmlElement typeNode) {
				Program.PrintError($"Struct member '{sname}.{name}' has no type");
				return false;
			}
			var type = typeNode.InnerText;
			var ptrDepth = (uint)node.InnerText.Count(ch => ch == '*');

			// Check array size
			string? arrSize = null;
			if (innerText.Contains('[') && innerText.Contains(']')) {
				arrSize = innerText.Substring(innerText.IndexOf('[') + 1);
				arrSize = arrSize.Substring(0, arrSize.LastIndexOf(']'));
			}

			// Check pointer length
			string? length = null;
			if (node.GetAttributeNode("len") is XmlAttribute lenAttr) {
				length = lenAttr.Value;
			}

			// Get values
			string? value = null;
			if (node.GetAttributeNode("values") is XmlAttribute valueAttr) {
				value = valueAttr.Value;
			}

			// Check optional
			bool optional = false;
			if (node.GetAttributeNode("optional") is XmlAttribute optAttr) {
				optional = optAttr.Value == "true";
			}

			// Return
			field = new(name, type, ptrDepth, arrSize, length, value, optional);
			return true;
		}
	}
}
