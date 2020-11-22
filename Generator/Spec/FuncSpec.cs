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
	// Loaded specification for a category="funcspec" type
	public sealed class FuncSpec
	{
		// Function argument type
		public sealed record Argument(
			string Name,   // The argument name
			string Type,   // The argument type
			uint PtrDepth, // The pointer depth of the argument
			bool Const     // If the argument is const
		);

		#region Fields
		// Function pointer name (starts with PFN_)
		public readonly string Name;
		// Function return type
		public readonly string ReturnType;
		// The arguments to the function
		public IReadOnlyList<Argument> Args => _args;
		private readonly List<Argument> _args;
		#endregion // Fields

		private FuncSpec(string name, string retType, List<Argument> args)
		{
			Name = name;
			ReturnType = retType;
			_args = args;
		}

		// Try parse
		public static bool TryParse(XmlElement node, out FuncSpec? spec)
		{
			spec = null;

			// Get the name
			if (node.SelectSingleNode("name") is not XmlElement nameNode) {
				Program.PrintError("Function pointer type does not have a name");
				return false;
			}
			var name = nameNode.InnerText;

			// Get the return type
			var returnType = node.InnerText.Substring("typedef".Length);
			returnType = returnType.Substring(0, returnType.IndexOf('('));
			returnType = returnType.Trim();

			// Preprocess the argument string
			var argsString = node.InnerText.Substring(node.InnerText.LastIndexOf('(') + 1);
			argsString = argsString.Substring(0, argsString.LastIndexOf(')'));
			var argSplit = argsString.Split(',');
			var argTypes = node.SelectNodes("type");

			// Process arguments
			List<Argument> args = new();
			if (argsString != "void") { // Special no-args void function check
				if (argSplit.Length != argTypes!.Count) {
					Program.PrintError($"Function pointer '{name}' has argument count mismatch");
					return false;
				}

				for (int i = 0; i < argSplit.Length; ++i) {
					var aname = argSplit[i].Split()[^1];
					var atype = argTypes[i]!.InnerText;
					var ptrdepth = (uint)argSplit[i].Count(ch => ch == '*');
					var @const = argSplit[i].StartsWith("const");
					args.Add(new(aname, atype, ptrdepth, @const));
				}
			}

			// Return
			spec = new(name, returnType, args);
			return true;
		}
	}
}
