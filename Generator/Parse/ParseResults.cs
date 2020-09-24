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
		// The valid vendor names
		public readonly List<string> VendorNames;
		// The enum specs
		public readonly Dictionary<string, EnumSpec> Enums;
		// The handle specs
		public readonly Dictionary<string, HandleSpec> Handles;
		// The struct specs
		public readonly Dictionary<string, StructSpec> Structs;
		// The function pointer specs
		public readonly Dictionary<string, FuncSpec> FuncPointers;
		// The API constants
		public readonly Dictionary<string, ConstantSpec> Constants;
		// The commands (API functions)
		public readonly Dictionary<string, CommandSpec> Commands;
		#endregion Fields

		private ParseResults()
		{
			VendorNames = new();
			Enums = new();
			Handles = new();
			Structs = new();
			FuncPointers = new();
			Constants = new();
			Commands = new();
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

			// Populate the types
			if (!PopulateEnumValues(regNode, spec)) {
				return false;
			}
			if (!PopulateStructFields(regNode, spec)) {
				return false;
			}
			if (!PopulateFuncArguments(regNode, spec)) {
				return false;
			}

			// Get the commands
			if (!DiscoverCommands(regNode, spec)) {
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

			// Scan the valid vendor names
			Console.WriteLine("Discovering vendors...");
			foreach (var child in tagsNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode tagNode) || (tagNode.Name != "tag")) {
					continue;
				}

				// Add as known extension
				if ((tagNode.Attributes?["name"] is XmlAttribute nameAttr)) {
					spec.VendorNames.Add(nameAttr.Value);
					Program.PrintVerbose($"\tFound vendor {nameAttr.Value}");
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
						if (EnumSpec.TryParseBitmask(typeNode, spec.Enums, out var bitmaskSpec)) {
							spec.Enums.Add(bitmaskSpec!.Name, bitmaskSpec!);
							Program.PrintVerbose($"\tFound bitmask type {bitmaskSpec!.Name}");
						}
						else return false;
					} break;
					case "enum": {
						if (EnumSpec.TryParseEnum(typeNode, spec.Enums, out var enumSpec)) {
							spec.Enums[enumSpec!.Name] = enumSpec!;
							Program.PrintVerbose($"\tFound enum type {enumSpec!.Name}" +
								$"{(enumSpec!.IsAlias ? $" -> {enumSpec!.Alias!.Name}" : "")}");
						}
						else return false;
					} break;
					case "handle": {
						if (HandleSpec.TryParse(typeNode, spec.Handles, out var handleSpec)) {
							spec.Handles.Add(handleSpec!.Name, handleSpec!);
							Program.PrintVerbose($"\tFound handle type {handleSpec!.Name}" +
								$"{(handleSpec!.IsAlias ? $" -> {handleSpec!.AliasName!}" : "")}");
						}
						else return false;
					} break;
					case "struct": {
						if (StructSpec.TryParse(typeNode, spec.Structs, out var structSpec)) {
							spec.Structs.Add(structSpec!.Name, structSpec!);
							Program.PrintVerbose($"\tFound struct type {structSpec!.Name}" +
								$"{(structSpec!.IsAlias ? $" -> {structSpec!.Alias!.Name}" : "")}");
						}
						else return false;
					} break;
					case "funcpointer": {
						if (FuncSpec.TryParse(typeNode, out var funcSpec)) {
							spec.FuncPointers.Add(funcSpec!.Name, funcSpec!);
							Program.PrintVerbose($"\tFound function pointer type {funcSpec!.Name}");
						}
						else return false;
					} break;
				}
			}

			// Success
			return true;
		}

		// Populate the values for enums
		private static bool PopulateEnumValues(XmlNode regNode, ParseResults spec)
		{
			Console.WriteLine("Populating enum values...");

			// Iterate over all "<enums>" nodes
			foreach (var child in regNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode enumsNode) || (enumsNode.Name != "enums")) {
					continue;
				}
				if (enumsNode.Attributes?["name"] is not XmlAttribute nameAttr) {
					continue;
				}
				var typeAttr = enumsNode.Attributes?["type"];
				
				// Treat API constants separately
				if (typeAttr is null) {
					if (nameAttr.Value != "API Constants") {
						Program.PrintError("Found type-less enums entry");
						return false;
					}
					
					// Loop over enum entries
					foreach (var enumChild in enumsNode.ChildNodes) {
						// Filter
						if ((enumChild is not XmlNode enumNode) || (enumNode.Name != "enum")) {
							continue;
						}

						// Parse
						if (ConstantSpec.TryParse(enumNode, spec.Constants, out var constantSpec)) {
							spec.Constants.Add(constantSpec!.Name, constantSpec!);
							Program.PrintVerbose($"\tFound API constant {constantSpec!.Name} = {constantSpec!.Value}");
						}
						else return false;
					}
				}
				else { // Handle enums and bitmasks
					// Find the associated enum
					if (!spec.Enums.TryGetValue(nameAttr.Value, out var enumSpec)) {
						if (nameAttr.Value.EndsWith("FlagBits")) { // Likely a zero-value bitmask
							continue;
						}
						else {
							Program.PrintError($"Failed to find enum type '{nameAttr.Value}' for values");
							return false;
						}
					}

					// Loop over enum entries
					foreach (var enumChild in enumsNode.ChildNodes) {
						// Filter
						if ((enumChild is not XmlNode enumNode) || (enumNode.Name != "enum")) {
							continue;
						}

						// Parse
						if (EnumSpec.TryParseValue(enumNode, enumSpec.Values, out var entry)) {
							enumSpec.Values.Add(entry!);
						}
						else return false;
					}

					Program.PrintVerbose($"\tFound {enumSpec.Values.Count} entries for enum {nameAttr.Value}");
				}
			}

			return true;
		}

		// Populate struct fields
		private static bool PopulateStructFields(XmlNode regNode, ParseResults spec)
		{
			Console.WriteLine("Populating struct fields...");

			// Loop over type node
			var typeNode = regNode.SelectSingleNode("types")!;
			foreach (var child in typeNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode structNode) || (structNode.Name != "type")) {
					continue;
				}
				if ((structNode.Attributes?["category"] is not XmlAttribute catAttr) || (catAttr.Value != "struct")) {
					continue;
				}

				// Get the name and spec (skip alias structs)
				var name = structNode.Attributes["name"]!.Value;
				var structSpec = spec.Structs[name];
				if (structSpec.IsAlias) {
					continue;
				}

				// Parse out the members
				foreach (var memChild in structNode.ChildNodes) {
					// Filter
					if ((memChild is not XmlNode memNode) || (memNode.Name != "member")) {
						continue;
					}

					// Parse
					if (StructSpec.TryParseField(memNode, out var field)) {
						structSpec.Fields.Add(field!);
					}
					else return false;
				}

				Program.PrintVerbose($"\tFound {structSpec.Fields.Count} entries for struct {structSpec.Name}");
			}

			return true;
		}

		// Parses the arguments to func pointer types
		private static bool PopulateFuncArguments(XmlNode regNode, ParseResults spec)
		{
			Console.WriteLine("Populating function pointer arguments...");

			// Loop over the type nodes
			var typeNode = regNode.SelectSingleNode("types")!;
			foreach (var child in typeNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode funcNode) || (funcNode.Name != "type")) {
					continue;
				}
				if ((funcNode.Attributes?["category"] is not XmlAttribute catAttr) || (catAttr.Value != "funcpointer")) {
					continue;
				}

				// Get the name and spec (skip alias structs)
				var name = funcNode.SelectSingleNode("name")!.InnerText;
				var funcSpec = spec.FuncPointers[name];

				// Parse the arguments
				if (!FuncSpec.TryParseArgs(funcNode, funcSpec.Arguments)) {
					return false;
				}

				Program.PrintVerbose($"\tFound {funcSpec.Arguments.Count} arguments for {funcSpec.Name}");
			}

			return true;
		}

		// Parses the commands
		private static bool DiscoverCommands(XmlNode regNode, ParseResults spec)
		{
			Console.WriteLine("Discovering API functions...");

			// Try to get the commands node
			if (regNode.SelectSingleNode("commands") is not XmlNode commandsNode) {
				Program.PrintError("Spec does not have an entry for 'commands'");
				return false;
			}

			// Loop over the commands
			foreach (var child in commandsNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode cmdNode) || (cmdNode.Name != "command")) {
					continue;
				}

				// Try to parse
				if (CommandSpec.TryParse(cmdNode, spec.Commands, out var cmdSpec)) {
					spec.Commands.Add(cmdSpec!.Name, cmdSpec!);
					if (cmdSpec!.IsAlias) {
						Program.PrintVerbose($"\tFound API function alias {cmdSpec!.Name} -> {cmdSpec.Alias!.Name}");
					}
					else {
						Program.PrintVerbose($"\tFound API function {cmdSpec!.Name} ({cmdSpec!.Arguments.Count} args)");
					}
				}
				else return false;
			}

			return true;
		}
	}
}
