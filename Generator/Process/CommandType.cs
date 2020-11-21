/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Gen
{
	// Processed version of CommandSpec
	public sealed class CommandType
	{
		private static readonly StringBuilder BUILDER = new(1024);

		// Paramter record
		public sealed record Param(
			string Name,   // The param name
			string Type,   // The param type
			uint PtrDepth, // The pointer depth of the param type
			bool Const,    // If the param is marked as const
			bool Optional  // If the param is marked as optional
		);

		#region Fields
		// The command spec
		public readonly CommandSpec Spec;

		// The command name
		public string Name => Spec.Name;
		// The function pointer type string
		public readonly string TypeString;
		// The function return type
		public readonly string ReturnType;
		// The command parameters
		public IReadOnlyList<Param> Params => _params;
		private readonly List<Param> _params;
		#endregion // Fields

		private CommandType(CommandSpec spec, string typestr, string retType, List<Param> pars)
		{
			Spec = spec;
			TypeString = typestr;
			ReturnType = retType;
			_params = pars;
		}

		// Try process
		public static bool TryProcess(CommandSpec spec, out CommandType? type)
		{
			type = null;

			// Reset type string
			BUILDER.Clear();
			BUILDER.Append("delegate* unmanaged<");

			// Process arguments
			List<Param> @params = new();
			foreach (var par in spec.Params) {
				// Get output type
				if (NameHelper.ConvertToOutputType(par.Type, par.PtrDepth) is not string parType) {
					Program.PrintError($"Failed to parse argument '{par.Type} {par.Name}' for '{spec.Name}'");
					return false;
				}
				@params.Add(new(par.Name, parType, par.PtrDepth, par.Const, par.Optional));
				BUILDER.Append(parType);
				BUILDER.Append(", ");
			}

			// Process return type
			if (NameHelper.ConvertToOutputType(spec.Return.Type, spec.Return.PtrDepth) is not string retType) {
				Program.PrintError(
					$"Failed to parse return type '{spec.Return.Type} (ptr={spec.Return.PtrDepth}) for '{spec.Name}'");
				return false;
			}
			BUILDER.Append(retType);
			BUILDER.Append('>');

			// Return
			type = new(spec, BUILDER.ToString(), retType, @params);
			return true;
		}
	}
}
