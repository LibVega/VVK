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
		#endregion // Constants

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

			// Scan over each of the expected types
			Console.WriteLine("Parsing enum types...");
			var enums = ParseEnumTypes(xml.DocumentElement!);

			result = new();
			return true;
		}

		// Scans and parses enum and bitmask types
		private static List<EnumSpec> ParseEnumTypes(XmlNode root)
		{
			List<EnumSpec> enums = new();

			foreach (var child in root.ChildNodes) {
				// Perform filtering
				if ((child is not XmlNode enumNode) ||
					(enumNode.Name != ENUM_NODE_NAME)) {
					continue;
				}

				// Try to parse the node
				if (EnumSpec.TryParse(enumNode, out var spec)) {
					enums.Add(spec!);
					if (ArgParse.Verbose) {
						Console.WriteLine($"\tFound {(spec!.Bitmask ? "bitmask" : "enum")}: {spec!.Name}");
					}
				}
			}

			return enums;
		}
    }
}
