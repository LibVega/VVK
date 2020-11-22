/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Linq;
using System.Text;

namespace Gen
{
	// Processed version of FuncSpec
	public sealed class FuncType
	{
		private static readonly StringBuilder BUILDER = new(1024);

		#region Fields
		// The function spec
		public readonly FuncSpec Spec;

		// The name of the function type
		public string TypeName => Spec.Name;
		// The function pointer type
		public readonly string TypeString;
		#endregion // Fields

		private FuncType(FuncSpec spec, string typeStr)
		{
			Spec = spec;
			TypeString = typeStr;
		}

		// Try process
		public static bool TryProcess(FuncSpec spec, out FuncType? type)
		{
			type = null;

			// Get managed pointer type
			bool isManaged = spec.Name != "PFN_vkVoidFunction";

			// Build the function pointer string
			BUILDER.Clear();
			BUILDER.Append($"delegate* {(isManaged ? "managed" : "unmanaged")}<");
			foreach (var arg in spec.Args) {
				// Get type string
				if (NameHelper.ConvertToOutputType(arg.Type, arg.PtrDepth) is not string argType) {
					Program.PrintError($"Failed to parse argument type '{arg.Type}' for '{spec.Name}'");
					return false;
				}
				BUILDER.Append(argType);
				BUILDER.Append(", ");
			}

			// Append the return type
			var retPtrDepth = spec.ReturnType.Count(ch => ch == '*');
			if (NameHelper.ConvertToOutputType(
					spec.ReturnType.Substring(0, spec.ReturnType.Length - retPtrDepth), (uint)retPtrDepth) 
					is not string retType) {
				Program.PrintError($"Failed to parse return type '{spec.ReturnType}' for '{spec.Name}'");
				return false;
			}
			BUILDER.Append(retType);
			BUILDER.Append('>');

			// Return
			type = new(spec, BUILDER.ToString());
			return true;
		}
	}
}
