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
	// Contains the spec file parsing functionality and unprocessed results
	public sealed class ParseResults
	{
		#region Fields
		// The valid extension names
		public readonly List<string> ExtensionNames;
		// The bitmask specs
		public readonly Dictionary<string, BitmaskSpec> Bitmasks;
		// The enum specs
		public readonly Dictionary<string, EnumSpec> Enums;
		// The handle specs
		public readonly Dictionary<string, HandleSpec> Handles;
		// The struct specs
		public readonly Dictionary<string, StructSpec> Structs;
		// The function pointer specs
		public readonly Dictionary<string, FuncSpec> FuncPointers;
		#endregion Fields

		private ParseResults()
		{
			ExtensionNames = new();
			Bitmasks = new();
			Enums = new();
			Handles = new();
			Structs = new();
			FuncPointers = new();
		}

		// Performs the top-level spec file parsing
		public static bool TryParse(string filePath, out ParseResults? spec)
		{
			spec = null;

			// Try to load the XML tree object
			XmlDocument xml = new XmlDocument();
			try {
				Console.WriteLine("Reading input file...");
				using var reader = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
				xml.Load(reader);
			}
			catch (Exception e) {
				Program.PrintError("Failed to parse input file as valid XML.");
				Program.PrintError($"Reason: {e.Message}");
				return false;
			}

			// Basic top-level validation
			if ((xml.DocumentElement?.Name ?? "") != "registry") {
				Program.PrintError("Unexpected root XML node, this is not the Vulkan spec file");
				return false;
			}
			XmlNode regNode = xml.DocumentElement!;
			spec = new ParseResults();

			// Perform initial type discovery
			if (!TryInitialTypeDiscovery(regNode, spec)) {
				return false;
			}

			return true;
		}

		// Performs extension and initial type discovery
		private static bool TryInitialTypeDiscovery(XmlNode regNode, ParseResults spec)
		{
			// Ensure required nodes are present
			if (regNode.SelectSingleNode("tags") is not XmlNode tagsNode) {
				Program.PrintError("Missing 'tags' spec for extension names");
				return false;
			}
			if (regNode.SelectSingleNode("types") is not XmlNode typesNode) {
				Program.PrintError("Missing 'types' spec for types");
				return false;
			}

			// Scan the valid extension names
			Console.WriteLine("Discovering extensions...");
			foreach (var child in tagsNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode tagNode) || (tagNode.Name != "tag")) {
					continue;
				}

				// Add as known extension
				if ((tagNode.Attributes?["name"] is XmlAttribute nameAttr)) {
					spec.ExtensionNames.Add(nameAttr.Value);
					Program.PrintVerbose($"\tFound extension {nameAttr.Value}");
				}
			}

			// Scan the types
			Console.WriteLine("Discovering types...");
			foreach (var child in typesNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode typeNode) || (typeNode.Name != "type") ||
					(typeNode.Attributes?["category"] is not XmlAttribute catAttr)) {
					continue;
				}

				// Try to parse based on the type
				switch (catAttr.Value) {
					case "bitmask": {
						if (BitmaskSpec.TryParse(typeNode, out var bitmaskSpec)) {
							spec.Bitmasks.Add(bitmaskSpec!.Name, bitmaskSpec!);
							Program.PrintVerbose($"\tFound bitmask type {bitmaskSpec!.Name}");
						}
						else {
							Program.PrintError("Failed to parse bitmask type");
							return false;
						}
					} break;
					case "enum": {
						if (EnumSpec.TryParse(typeNode, spec.Enums, out var enumSpec)) {
							spec.Enums.Add(enumSpec!.Name, enumSpec!);
							Program.PrintVerbose($"\tFound enum type {enumSpec!.Name}" +
								$"{(enumSpec!.IsAlias ? $" -> {enumSpec!.Alias!.Name}" : "")}");
						}
						else {
							Program.PrintError("Failed to parse enum type");
							return false;
						}
					} break;
					case "handle": {
						if (HandleSpec.TryParse(typeNode, spec.Handles, out var handleSpec)) {
							spec.Handles.Add(handleSpec!.Name, handleSpec!);
							Program.PrintVerbose($"\tFound handle type {handleSpec!.Name}" +
								$"{(handleSpec!.IsAlias ? $" -> {handleSpec!.AliasName!}" : "")}");
						}
						else {
							Program.PrintError("Failed to parse handle type");
							return false;
						}
					} break;
					case "struct": {
						if (StructSpec.TryParse(typeNode, spec.Structs, out var structSpec)) {
							spec.Structs.Add(structSpec!.Name, structSpec!);
							Program.PrintVerbose($"\tFound struct type {structSpec!.Name}" +
								$"{(structSpec!.IsAlias ? $" -> {structSpec!.Alias!.Name}" : "")}");
						}
						else {
							Program.PrintError("Failed to parse struct type");
							return false;
						}
					} break;
					case "funcpointer": {
						if (FuncSpec.TryParse(typeNode, out var funcSpec)) {
							spec.FuncPointers.Add(funcSpec!.Name, funcSpec!);
							Program.PrintVerbose($"\tFound function pointer type {funcSpec!.Name}");
						}
						else {
							Program.PrintError("Failed to parse function pointer type");
							return false;
						}
					} break;
				}
			}

			// Match up bitmasks to their enum backers
			foreach (var bitmask in spec.Bitmasks) {
				if (bitmask.Value.BackingName is null) {
					continue;
				}

				if (spec.Enums.TryGetValue(bitmask.Value.BackingName, out var backer)) {
					bitmask.Value.BackingType = backer;
				}
				else {
					Program.PrintError($"Failed to find backing type for bitmask '{bitmask.Key}' " +
						$"({bitmask.Value.BackingName})");
					return false;
				}
			}

			// Success
			return true;
		}
	}
}
