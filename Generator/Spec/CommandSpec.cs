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
	// Loaded specification for a <command> node
	public sealed class CommandSpec
	{
		// Command return type spec
		public sealed record ReturnType(string Type, uint PtrDepth);
		// Command parameter spec
		public sealed record Param(
			string Name,       // The param name
			string Type,       // The param type
			uint PtrDepth,     // The pointer depth for the param type
			bool Const,        // If the param is const
			string? LengthStr, // The optional length specifier for the param
			bool Optional      // If the param is marked as optional
		);

		#region Fields
		// The name of the command
		public readonly string Name;
		// The return type of the command
		public ReturnType Return => Alias?.Return ?? _return!;
		private readonly ReturnType? _return;
		// The command parameters
		public IReadOnlyList<Param> Params => Alias?.Params ?? _params!;
		private readonly List<Param>? _params;

		// The potential aliased command
		public readonly CommandSpec? Alias;
		#endregion // Fields

		private CommandSpec(string name, ReturnType ret, List<Param> @params)
		{
			Name = name;
			_return = ret;
			_params = @params;
		}

		private CommandSpec(string name, CommandSpec alias)
		{
			Name = name;
			Alias = alias;
		}

		// Try parse
		public static bool TryParse(XmlElement node, Dictionary<string, CommandSpec> found, out CommandSpec? spec)
		{
			spec = null;

			// Check for aliased command
			if (node.GetAttributeNode("alias") is XmlAttribute aliasAttr) {
				if (node.GetAttributeNode("name") is not XmlAttribute nameAttr) {
					Program.PrintError("Failed to get name for aliased command");
					return false;
				}
				if (!found.TryGetValue(aliasAttr.Value, out var alias)) {
					Program.PrintError($"Command '{nameAttr.Value}' is aliased to unknown command '{aliasAttr.Value}'");
					return false;
				}

				// Return early with alias
				spec = new(nameAttr.Value, alias);
				return true;
			}

			// Process proto field for name and return
			if (node.SelectSingleNode("proto") is not XmlElement protoNode) {
				Program.PrintError("Failed to get prototype for command");
				return false;
			}
			if (protoNode.SelectSingleNode("name") is not XmlElement nameNode) {
				Program.PrintError("Failed to get command name");
				return false;
			}
			var name = nameNode.InnerText;
			if (protoNode.SelectSingleNode("type") is not XmlElement retTypeNode) {
				Program.PrintError($"Failed to get return type for command '{name}'");
				return false;
			}
			ReturnType retType = new(retTypeNode.InnerText, (uint)protoNode.InnerText.Count(ch => ch == '*'));

			// Process parameters
			List<Param> @params = new();
			foreach (var childNode in node.SelectNodes("param")!) {
				if (!TryParseParam((childNode as XmlElement)!, out var par)) {
					return false;
				}
				@params.Add(par!);
			}

			// Return
			spec = new(name, retType, @params);
			return true;
		}

		// Try parse command parameter
		private static bool TryParseParam(XmlElement node, out Param? param)
		{
			param = null;

			// Get the name
			if (node.SelectSingleNode("name") is not XmlElement nameNode) {
				Program.PrintError("Command parameter does not have name");
				return false;
			}
			var name = nameNode.InnerText;

			// Get the type and pointer depth
			if (node.SelectSingleNode("type") is not XmlElement typeNode) {
				Program.PrintError($"Command parameter '{name}' does not have a type");
				return false;
			}
			var type = typeNode.InnerText;
			var ptrDepth = (uint)node.InnerText.Count(ch => ch == '*');
			if (node.InnerText.Contains('[') && node.InnerText.Contains(']')) {
				ptrDepth += 1;
			}

			// Const-ness
			var @const = node.InnerText.StartsWith("const");

			// Check for length string
			string? lenStr = null;
			if (node.GetAttributeNode("len") is XmlAttribute lenNode) {
				lenStr = lenNode.Value;
			}

			// Check for optional flag
			bool opt = false;
			if (node.GetAttributeNode("optional") is XmlAttribute optAttr) {
				opt = optAttr.Value == "true";
			}

			// Return
			param = new(name, type, ptrDepth, @const, lenStr, opt);
			return true;
		}
	}
}
