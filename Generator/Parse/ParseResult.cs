/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Gen
{
	// Contains the result of parsing the specification .xml
    public sealed class ParseResult
    {
		#region Constants
		// The name of the root node in the valid specification file
		private static readonly string ROOT_NODE_NAME = "registry";
		// Enum node type name
		private static readonly string ENUM_NODE_NAME = "enums";
		// General type node name
		private static readonly string TYPE_NODE_NAME = "type";
		#endregion // Constants

		#region Fields
		// All extension names discovered
		public readonly List<string> Extensions;
		// All enum or bitmask types discovered
		public readonly List<EnumSpec> Enums;
		#endregion // Fields

		private ParseResult(
			List<string> exts,
			List<EnumSpec> enums
		)
		{
			Extensions = exts;
			Enums = enums;
		}

		// Performs the top-level parsing
		public static bool Parse(string file, out ParseResult? result)
		{
			result = null;

			// Try to load the XML tree object
			XmlDocument xml = new XmlDocument();
			try {
				Console.WriteLine("Reading input file...");
				using var reader = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None);
				xml.Load(reader);
			}
			catch (Exception e) {
				Program.PrintError("Failed to parse input file as valid XML.");
				Program.PrintError($"Reason: {e.Message}");
				return false;
			}

			// Basic top-level validation
			if ((xml.DocumentElement?.Name ?? "") != ROOT_NODE_NAME) {
				Program.PrintError("Unexpected root XML node, this is not the Vulkan spec file");
				return false;
			}

			// Get the extensions
			Console.WriteLine("Discovering extension names...");
			var exts = ParseExtensionNames(xml.DocumentElement!);
			if (exts is null) {
				return false;
			}

			// Scan over each of the expected types
			Console.WriteLine("Parsing enum types...");
			var enums = ParseEnumTypes(xml.DocumentElement!);
			if (enums is null) {
				return false;
			}

			result = new(exts, enums);
			return true;
		}

		// Scans and parses the valid extension names
		private static List<string>? ParseExtensionNames(XmlNode root)
		{
			List<string> exts = new();

			// Get the tags node
			var tagsNode = root.SelectSingleNode("tags");
			if (tagsNode is null) {
				Program.PrintError("Invalid spec file - could not find tags node");
				return null;
			}

			// Loop over the extensions
			foreach (var child in tagsNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode tagNode) || (tagNode.Name != "tag")) {
					continue;
				}

				// Add the extension
				var nameAttr = tagNode.Attributes?["name"];
				if (nameAttr is not null) {
					exts.Add(nameAttr.Value);
				}
			}

			return exts;
		}

		// Scans and parses enum and bitmask types
		private static List<EnumSpec>? ParseEnumTypes(XmlNode root)
		{
			List<EnumSpec> enums = new();

			// Parse the enum definitions first
			foreach (var child in root.ChildNodes) {
				// Perform filtering
				if ((child is not XmlNode enumNode) || (enumNode.Name != ENUM_NODE_NAME)) {
					continue;
				}

				// Try to parse the node
				if (EnumSpec.TryParseEnum(enumNode, out var spec)) {
					enums.Add(spec!);
					if (ArgParse.Verbose) {
						Console.WriteLine($"\tFound {(spec!.IsBitmask ? "bitmask" : "enum")}: {spec!.Name}");
					}
				}
			}

			// Go back through the type listing to find enum aliases
			var typesNode = root.SelectSingleNode("types");
			if (typesNode is null) {
				Program.PrintError("Invalid spec file - could not find types node");
				return null;
			}
			foreach (var child in typesNode.ChildNodes) {
				// Perform filtering
				if ((child is not XmlNode typeNode) || (typeNode.Name != TYPE_NODE_NAME)) {
					continue;
				}

				// Try to parse the alias
				if (EnumSpec.TryParseAlias(typeNode, enums, out var alias)) {
					enums.Add(alias!);
					if (ArgParse.Verbose) {
						Console.WriteLine($"\tFound enum alias: {alias!.Name} -> {alias!.Alias!.Name}");
					}
				}
			}

			return enums;
		}
    }
}
