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
	// Represents an unprocessed command (API function) from the spec
	public sealed class CommandSpec
	{
		// A function argument
		public record Argument(string Name, string Value);

		#region Fields
		// The function name
		public readonly string Name;

		// The aliased command
		public readonly CommandSpec? Alias;
		// If this command aliases another
		public bool IsAlias => Alias is not null;

		// The return value type
		public string ReturnType => Alias?._returnType! ?? _returnType!;
		private readonly string? _returnType;

		// The function arguments
		public List<Argument> Arguments => Alias?._arguments! ?? _arguments!;
		private readonly List<Argument>? _arguments;
		#endregion // Fields

		private CommandSpec(string name, string ret, List<Argument> args)
		{
			Name = name;
			Alias = null;
			_returnType = ret;
			_arguments = args;
		}
		private CommandSpec(string name, CommandSpec alias)
		{
			Name = name;
			Alias = alias;
			_returnType = null;
			_arguments = null;
		}

		// Parse the command from the spec (xml is <command> node)
		public static bool TryParse(XmlNode xml, Dictionary<string, CommandSpec> seen, out CommandSpec? spec)
		{
			spec = null;

			// Exit early with aliases
			if (xml.Attributes?["alias"] is XmlAttribute aliasAttr) {
				if (xml.Attributes?["name"] is not XmlAttribute nameAttr) {
					Program.PrintError("API function alias does not have name");
					return false;
				}
				if (!seen.TryGetValue(aliasAttr.Value, out var alias)) {
					Program.PrintError($"Unknown API function alias target '{aliasAttr.Value}'");
					return false;
				}
				spec = new(nameAttr.Value, alias);
				return true;
			}

			// Extract name and return type from <proto>
			if ((xml.SelectSingleNode("proto") is not XmlNode protoNode) ||
				(protoNode.SelectSingleNode("type") is not XmlNode returnNode) ||
				(protoNode.SelectSingleNode("name") is not XmlNode nameNode)) {
				Program.PrintError($"API function does not have prototype");
				return false;
			}
			bool returnPtr = protoNode.InnerXml.Contains('*');

			// Loop over arguments
			List<Argument> args = new();
			foreach (var child in xml.ChildNodes) {
				// Filter
				if ((child is not XmlNode paramNode) || (paramNode.Name != "param")) {
					continue;
				}

				// Get param type and name
				if ((paramNode.SelectSingleNode("type") is not XmlNode paramTypeNode) ||
					(paramNode.SelectSingleNode("name") is not XmlNode paramNameNode)) {
					Program.PrintError($"Invalid parameter spec in API function {nameNode.InnerText}");
					return false;
				}
				var ptrCount = paramNode.InnerText.Count(c => c == '*');

				// Special check for array specifiers to add a pointer
				if (paramNode.InnerText.Contains('[')) {
					ptrCount += 1;
				}

				args.Add(new(paramNameNode.InnerText, paramTypeNode.InnerText + new string('*', ptrCount)));
			}

			// Return
			spec = new(nameNode.InnerText, returnNode.InnerText + (returnPtr ? "*" : ""), args);
			return true;
		}
	}
}
