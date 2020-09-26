/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Xml;

namespace Gen
{
	// Represents an extension from the spec
	public sealed class ExtensionSpec
	{
		#region Fields
		// The extensions name
		public readonly string Name;
		// The extension version
		public readonly uint Version;
		// The extension number
		public readonly uint Number;
		// The constant specs for the extension version and name
		public readonly ConstantSpec NameSpec;
		public readonly ConstantSpec VersionSpec;
		#endregion // Fields

		private ExtensionSpec(string name, uint version, uint number, ConstantSpec nameSpec, ConstantSpec versionSpec)
		{
			Name = name;
			Version = version;
			Number = number;
			NameSpec = nameSpec;
			VersionSpec = versionSpec;
		}

		// Process (xml = <extension> node in <extensions>)
		public static bool TryParse(XmlNode xml, out ExtensionSpec? spec, out bool enabled)
		{
			spec = null;
			enabled = true;

			// Ensure the extension is enabled first
			if (xml.Attributes?["supported"] is XmlAttribute suppAttr) {
				if (suppAttr.Value == "disabled") {
					enabled = false;
					return false;
				}
			}

			// Get the extension name
			if (xml.Attributes?["name"] is not XmlAttribute extNameAttr) {
				Program.PrintError("Extension does not have name");
				return false;
			}
			var extName = extNameAttr.Value;

			// Get the name and version
			if (xml.SelectSingleNode("require") is not XmlNode requireNode) {
				Program.PrintError($"Extension '{extName}' is missing <require> node");
				return false;
			}
			ConstantSpec? nameSpec = null;
			ConstantSpec? versionSpec = null;
			foreach (var child in requireNode.ChildNodes) {
				// Filter
				if ((child is not XmlNode enumNode) || (enumNode.Name != "enum")) {
					continue;
				}
				
				// Check for the wanted values
				if ((enumNode.Attributes?["name"] is XmlAttribute nameAttr) &&
					(enumNode.Attributes?["value"] is XmlAttribute valueAttr)) {
					if (nameAttr.Value.EndsWith("_EXTENSION_NAME") && nameSpec is null) {
						nameSpec = new(nameAttr.Value, valueAttr.Value);
					}
					else if (nameAttr.Value.EndsWith("_SPEC_VERSION") && versionSpec is null) {
						versionSpec = new(nameAttr.Value, valueAttr.Value);
					}
				}
			}
			if (nameSpec is null) {
				Program.PrintError($"Extension '{extName}' is missing entension name constant.");
				return false;
			}
			if (versionSpec is null) {
				Program.PrintError($"Extension '{extName}' is missing extension version constant.");
				return false;
			}
			if (!UInt32.TryParse(versionSpec.Value, out var extVersion)) {
				Program.PrintError($"Failed to parse version integer for extension '{extName}'");
				return false;
			}

			// Get the extension number
			if (xml.Attributes?["number"] is not XmlAttribute numberAttr) {
				Program.PrintError($"Extension '{extName}' is missing extension number.");
				return false;
			}
			if (!UInt32.TryParse(numberAttr.Value, out var extNumber)) {
				Program.PrintError($"Extension number '{numberAttr.Value}' could not be parsed.");
				return false;
			}

			// Return
			spec = new(extName, extVersion, extNumber, nameSpec, versionSpec);
			return true;
		}
	}
}
