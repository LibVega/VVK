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
	// Represents a struct type defined in the spec
	public sealed class StructSpec
	{
		// Repesents a field in the struct
		public record Field(
			string Name, 
			string Type, 
			string? Comment,
			uint PointerDepth,
			string? EnumName // Filled out for fixed-size arrays with the array size
		);

		#region Fields
		// The name of the struct
		public readonly string Name;
		// The optional comment on the struct
		public string? Comment => Alias?.Comment ?? _comment;
		// The fields in the struct
		public List<Field> Fields => Alias?.Fields ?? _fields!;
		// If the first field of the struct is 'VkStructureType sType'
		public bool IsTyped => Alias?.IsTyped ?? _isTyped;
		// The enum type that is aliased by this one
		public readonly StructSpec? Alias;

		// If this struct entry is an alias
		public bool IsAlias => Alias is not null;

		// Internal values
		private readonly string? _comment;
		private readonly List<Field>? _fields;
		private bool _isTyped;
		#endregion // Fields

		private StructSpec(string name, string? comment)
		{
			Name = name;
			Alias = null;
			_comment = comment;
			_fields = new();
			_isTyped = false;
		}
		private StructSpec(string name, StructSpec spec)
		{
			Name = name;
			Alias = spec;
			_comment = null;
			_fields = null;
			_isTyped = false;
		}

		// Parser for struct definitions
		public static bool TryParseStruct(XmlNode xml, List<StructSpec> structs, out StructSpec? spec)
		{
			spec = null;

			// Validate type node
			if ((xml.Attributes?["category"] is not XmlAttribute catAttr) ||
				(xml.Attributes?["name"] is not XmlAttribute nameAttr) ||
				(catAttr.Value is null) ||
				(nameAttr.Value is null)) {
				return false;
			}
			if (catAttr.Value != "struct") {
				return false;
			}

			// Check for alias, exit early
			var aliasAttr = xml.Attributes?["alias"];
			if (aliasAttr is not null) {
				var alias = structs.Find(ss => ss.Name == aliasAttr.Value);
				if (alias is null) {
					Program.PrintError($"Failed to find alias struct type '{aliasAttr.Value}'");
					return false;
				}
				else {
					spec = new(nameAttr.Value, alias);
					return true;
				}
			}

			// Create object
			spec = new(nameAttr.Value, xml.Attributes?["comment"]?.Value);

			// Read all members
			var memberNodes = xml.SelectNodes("member")!;
			foreach (var node in memberNodes) {
				if ((node is XmlNode member) && TryParseMember(member, out var field)) {
					spec.Fields.Add(field!);
					if (field!.Name == "sType" && field!.Type == "VkStructureType") {
						spec._isTyped = true;
					}
				}
			}

			return true;
		}

		// Parser for struct members
		private static bool TryParseMember(XmlNode xml, out Field? field)
		{
			field = null;

			// Validate
			var typeNode = xml.SelectSingleNode("type");
			var nameNode = xml.SelectSingleNode("name");
			if ((typeNode is null) || (nameNode is null)) {
				return false;
			}

			// Get other values
			var cmtNode = xml.SelectSingleNode("comment");
			var enumNode = xml.SelectSingleNode("enum");
			var ptrCnt = xml.InnerText.Count(ch => ch == '*');

			// Create and return
			field = 
				new(nameNode.InnerText!, typeNode.InnerText!, cmtNode?.InnerText, (uint)ptrCnt, enumNode?.InnerText);
			return true;
		}
	}
}
