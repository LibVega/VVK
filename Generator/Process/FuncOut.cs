/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// Represents a processed function pointer type
	public sealed class FuncOut
	{
		#region Fields
		// The spec that the function was processed from
		public readonly FuncSpec Spec;
		// The name of this function (as the original PFN_* name)
		public readonly string Name;
		// The C# function pointer prototype
		public readonly string Prototype;
		#endregion // Fields

		private FuncOut(FuncSpec spec, string name, string proto)
		{
			Spec = spec;
			Name = name;
			Prototype = proto;
		}

		// Process
		public static FuncOut? TryProcess(FuncSpec spec, NameHelper names)
		{
			// Get the arg type names
			var args = new string[spec.Arguments.Count];
			int aidx = 0;
			foreach (var arg in spec.Arguments) {
				if (!names.ProcessGeneralTypeName(arg.Type, out args[aidx++])) {
					Program.PrintError($"Failed to parse argument type {arg.Type}");
					return null;
				}
			}

			// Return
			return new(spec, spec.Name, $"delegate* managed<{String.Join(", ", args)}>");
		}
	}
}
