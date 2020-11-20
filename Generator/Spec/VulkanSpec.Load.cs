/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.IO;
using System.Xml;

namespace Gen
{
	// Load functions for the Vulkan spec
	public sealed partial class VulkanSpec
	{
		// Performs the loading and parsing of the spec from the file
		public static bool TryLoad(string inFile, out VulkanSpec? spec)
		{
			spec = null;

			// Load the XML file
			XmlDocument xml = new();
			try {
				Program.Print("Loading specification file");
				using var reader = File.Open(inFile, FileMode.Open, FileAccess.Read, FileShare.None);
				xml.Load(reader);
			}
			catch (Exception e) {
				Program.PrintError("Failed to parse input file as valid XML");
				Program.PrintError($"Reason: {e.Message}");
				return false;
			}

			// Get the registry root node
			if ((xml.DocumentElement?.Name ?? String.Empty) != "registry") {
				Program.PrintError("Input file is not a Vulkan XML spec file");
				return false;
			}
			var regNode = xml.DocumentElement!;
			spec = new();

			// Initial type load
			if (!LoadTypes(regNode, spec)) {
				return false;
			}

			// Load extra values
			if (!LoadValues(regNode, spec)) {
				return false;
			}

			// Load commands
			if (!LoadCommands(regNode, spec)) {
				return false;
			}

			// Load extensions
			if (!LoadExtensions(regNode, spec)) {

			}

			return true;
		}

		// Performs the initial type loading (everything in <types>)
		private static bool LoadTypes(XmlElement regNode, VulkanSpec spec)
		{
			// Validate types node
			if (regNode.SelectSingleNode("types") is not XmlElement typesNode) {
				Program.PrintError("Cannot find required 'types' node in spec file");
				return false;
			}

			// Loop over the type nodes and dispatch on category
			foreach (var child in typesNode.ChildNodes) {
				// Validate type and category
				if ((child is not XmlElement typeNode) || (typeNode.Name != "type")) {
					continue;
				}
				if (typeNode.GetAttributeNode("category") is not XmlAttribute catAttr) {
					continue;
				}

				// Dispatch on category
				switch (catAttr.Value) {
					case "bitmask": {
						if (!BitmaskSpec.TryParse(typeNode, spec._bitmasks, out var typespec)) return false;
						spec._bitmasks.Add(typespec!.Name, typespec);
						spec._allTypes.Add(typespec.Name, SpecType.Bitmask);
						Program.PrintVerbose($"Found bitmask type '{typespec.Name}'");
					} break;
					case "handle": {
						if (!HandleSpec.TryParse(typeNode, spec._handles, out var typespec)) return false;
						spec._handles.Add(typespec!.Name, typespec);
						spec._allTypes.Add(typespec.Name, SpecType.Handle);
						Program.PrintVerbose($"Found handle type '{typespec.Name}'");
					} break;
					case "enum": {
						if (!EnumSpec.TryParse(typeNode, spec._enums, out var typespec)) return false;
						spec._enums.Add(typespec!.Name, typespec);
						spec._allTypes.Add(typespec.Name, SpecType.Enum);
						Program.PrintVerbose($"Found enum type '{typespec.Name}'");
					} break;
					case "struct":
					case "union": {
						if (!StructSpec.TryParse(typeNode, spec._structs, out var typespec)) return false;
						spec._structs.Add(typespec!.Name, typespec);
						spec._allTypes.Add(typespec.Name, SpecType.Struct);
						Program.PrintVerbose($"Found struct type '{typespec.Name}'");
					} break;
					case "funcpointer": {
						if (!FuncSpec.TryParse(typeNode, out var typespec)) return false;
						spec._functions.Add(typespec!.Name, typespec);
						spec._allTypes.Add(typespec.Name, SpecType.Function);
						Program.PrintVerbose($"Found function type '{typespec.Name}'");
					} break;
				}
			}

			// Report
			Program.Print(
				$"Loaded specification types: {spec._bitmasks.Count} bitmasks, {spec._handles.Count} handles, " +
				$"{spec._enums.Count} enums, {spec._structs.Count} structs, {spec._functions.Count} functions"
			);

			return true;
		}

		// Does loading for specific values, such as API constants and enum values
		private static bool LoadValues(XmlElement regNode, VulkanSpec spec)
		{
			uint enumValueCount = 0;

			// Iterate over all <enums> nodes, which will have enum values and API constants
			foreach (var childNode in regNode.SelectNodes("enums")!) {
				// Filter
				if ((childNode is not XmlElement enumsNode) || 
						(enumsNode.GetAttributeNode("name") is not XmlAttribute nameAttr)) {
					continue;
				}

				// API constants enum - special case
				if (nameAttr.Value == "API Constants") {
					foreach (var enumChild in enumsNode.SelectNodes("enum")!) {
						if (!ConstantSpec.TryParse((enumChild as XmlElement)!, spec._constants, out var valuespec)) {
							return false;
						}
						spec._constants.Add(valuespec!.Name, valuespec);
						Program.PrintVerbose($"Found API constant '{valuespec.Name}' = {valuespec.Value}");
					}
					continue;
				}

				// Normal enum case, find and add values
				if (!spec._enums.TryGetValue(nameAttr.Value, out var enumSpec)) {
					Program.PrintError($"Enum type '{nameAttr.Value}' was not found for populating values");
					return false;
				}
				uint thisCount = 0;
				foreach (var enumChild in enumsNode.SelectNodes("enum")!) {
					if (!enumSpec.TryAddValue((enumChild as XmlElement)!)) {
						return false;
					}
					++thisCount;
				}
				Program.PrintVerbose($"Loaded {thisCount} values for enum '{enumSpec.Name}'");
				enumValueCount += thisCount;
			}

			// Report
			Program.Print(
				$"Loaded specification values: {spec._constants.Count} API constants, {enumValueCount} enum values"
			);

			return true;
		}

		// Loads command types
		private static bool LoadCommands(XmlElement regNode, VulkanSpec spec)
		{
			// Validate the commands node
			if (regNode.SelectSingleNode("commands") is not XmlElement commandsNode) {
				Program.PrintError("Cannot find command types in specification file");
				return false;
			}

			// Iterate over command nodes
			foreach (var childNode in commandsNode.SelectNodes("command")!) {
				// Filter
				if (childNode is not XmlElement cmdNode) {
					continue;
				}

				if (!CommandSpec.TryParse(cmdNode, spec._commands, out var cmdspec)) {
					return false;
				}
				spec._commands.Add(cmdspec!.Name, cmdspec);
				Program.PrintVerbose($"Found command '{cmdspec.Name}' ({cmdspec.Params.Count} params)");
			}

			// Report
			Program.Print($"Loaded {spec._commands.Count} commands");
			return true;
		}

		// Loads extension types, and the constants associated with the extensions
		private static bool LoadExtensions(XmlElement regNode, VulkanSpec spec)
		{
			// Loop over the promoted features
			uint proCount = 0;
			foreach (var childNode in regNode.SelectNodes("feature")!) {
				// Filter
				if (childNode is not XmlElement featureNode) {
					continue;
				}

				// Get feature level
				if (featureNode.GetAttributeNode("number") is not XmlAttribute numAttr) {
					Program.PrintError("Feature level is missing number");
					return false;
				}
				if (numAttr.Value == "1.0") {
					continue; // Skip over feature level 1.0 since it doesn't add anything
				}

				// Loop over require nodes
				foreach (var childReq in featureNode.SelectNodes("require")!) {
					// Filter
					if (childReq is not XmlElement reqNode) {
						continue;
					}

					// Process
					uint _ = 0;
					if (!_ProcessRequireNode(reqNode, spec, null, out var count, ref _)) {
						return false;
					}

					// Report
					proCount += count;
					if (count != 0) {
						Program.PrintVerbose($"Loaded {count} additional enum values for feature level {numAttr.Value}"); 
					}
				}
			}

			// Loop over extensions
			uint extCount = 0;
			foreach (var childNode in regNode.SelectSingleNode("extensions")!.SelectNodes("extension")!) {
				// Filter
				if (childNode is not XmlElement extNode) {
					continue;
				}

				// Ignore extensions that are disabled
				if ((extNode.GetAttributeNode("supported") is XmlAttribute supAttr) && (supAttr.Value == "disabled")) {
					continue;
				}

				// Get extension name
				if (extNode.GetAttributeNode("name") is not XmlAttribute nameAttr) {
					Program.PrintError("Missing extension name");
					return false;
				}
				var extName = nameAttr.Value;

				// Get extension number
				if ((extNode.GetAttributeNode("number") is not XmlAttribute numAttr) ||
						!UInt32.TryParse(numAttr.Value, out var extNumber)) {
					Program.PrintError($"Missing or invalid extension number for '{extName}'");
					return false;
				}

				// Loop over require nodes
				uint extVersion = UInt32.MaxValue;
				foreach (var childReq in extNode.SelectNodes("require")!) {
					// Filter
					if (childReq is not XmlElement reqNode) {
						continue;
					}

					// Process
					if (!_ProcessRequireNode(reqNode, spec, extNumber, out var count, ref extVersion)) {
						return false;
					}

					// Report
					extCount += count;
					if (count != 0) {
						Program.PrintVerbose($"Loaded {count} additional enum values from extension {extName}"); 
					}
				}
				if (extVersion == UInt32.MaxValue) {
					Program.PrintError($"Could not find extension version for '{extName}'");
					return false;
				}

				// Register extension
				spec._extensions.Add(extName, new(extName, extNumber, extVersion));
				Program.PrintVerbose($"Found extension '{extName}'");
			}

			// Report
			Program.Print($"Found {proCount} promoted enum values, and {extCount} extension enum values");
			Program.Print($"Found {spec._extensions.Count} extensions");
			return true;

			// Processes a require node
			static bool _ProcessRequireNode(XmlElement reqNode, VulkanSpec spec, uint? ext, out uint count, ref uint extVersion)
			{
				// Select the <enum> nodes
				count = 0;
				foreach (var childNode in reqNode.SelectNodes("enum")!) {
					if (childNode is not XmlElement enumNode) {
						continue;
					}

					// Check if the enum is giving the version of an extension
					if (enumNode.GetAttributeNode("name") is not XmlAttribute nameAttr) {
						Program.PrintError("Missing name for enum extension value");
						return false;
					}
					var name = nameAttr.Value;
					if (name.EndsWith("_SPEC_VERSION")) {
						if (!UInt32.TryParse(enumNode.GetAttributeNode("value")?.Value, out extVersion)) {
							Program.PrintError("Failed to parse extension version");
							return false;
						}
						continue;
					}

					// Find the extended enum
					if (enumNode.GetAttributeNode("extends") is not XmlAttribute extendsAttr) {
						continue; // Can happen when API constants are added or promoted
					}
					if (!spec._enums.TryGetValue(extendsAttr.Value, out var enumSpec)) {
						Program.PrintError($"Unknown extension enum type '{extendsAttr.Value}'");
						return false;
					}

					// Check for alias
					if (enumNode.GetAttributeNode("alias") is XmlAttribute aliasAttr) {
						if (!enumSpec.TryAddAliasValue(enumNode, aliasAttr)) {
							return false;
						}
						continue; // Break early for aliases
					}

					// Get the extension number (enums with bitpos or value don't need this)
					var hasBitpos = enumNode.GetAttributeNode("bitpos") is not null;
					var hasValue = enumNode.GetAttributeNode("value") is not null;
					uint extNumber = ext.GetValueOrDefault(0);
					if (!hasBitpos && !hasValue && !ext.HasValue) {
						if ((enumNode.GetAttributeNode("extnumber") is not XmlAttribute extAttr) ||
								!UInt32.TryParse(extAttr.Value, out var exnum)) {
							Program.PrintError("Missing or invalid enum extension number");
							return false;
						}
						extNumber = exnum;
					}

					// Try add
					if (!enumSpec.TryAddExtensionValue(enumNode, name, extNumber)) {
						return false;
					}
					count++;
				}

				return true;
			}
		}
	}
}
