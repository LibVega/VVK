/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace Gen
{
	// Represents an unprocessed function pointer type in the spec
	public sealed class FuncSpec
	{
		// Represents an argument to the function type
		public record Argument(string Name, string Type);

		#region Fields
		// The name of the function
		public readonly string Name;
		// The return type name
		public readonly string ReturnType;
		// The function arguments
		public readonly List<Argument> Arguments;
		#endregion // Fields

		private FuncSpec(string name, string retType)
		{
			Name = name;
			ReturnType = retType;
			Arguments = new();
		}

		// Parses from a spec xml node
		public static bool TryParse(XmlNode xml, out FuncSpec? spec)
		{
			spec = null;

			var signature = xml.InnerText;
			var sigSplit = signature.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			// Get the name
			if (xml.SelectSingleNode("name") is not XmlNode nameNode) {
				Program.PrintError("Function pointer type does not have name");
				return false;
			}

			// Get the return type (always second value after [0] == "typedef")
			var retType = sigSplit[1];

			// Return
			spec = new(nameNode.InnerText, retType);
			return true;
		}

		// Parses the arguments for a func spec (xml is <type> category="funcpointer")
		public static bool TryParseArgs(XmlNode xml, List<Argument> arguments)
		{
			// Get matches against each <type>... name,
			var matches = Regex.Matches(xml.InnerXml, @"\<type\>(.+?)\<\/type\>(\**)\s+(\w+?)[,\)]");
			if (matches.Count == 0) {
				arguments.Clear();
				return true;
			}

			// Populate
			foreach (Match match in matches) {
				arguments.Add(new(match.Groups[3].Value, match.Groups[1].Value + match.Groups[2].Value));
			}

			return true;
		}
	}
}
