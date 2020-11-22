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

		// Known global functions
		private static readonly List<string> GLOBAL_FUNCTIONS = new() { 
			"vkCreateInstance", "vkGetDeviceProcAddr", "vkGetInstanceProcAddr", "vkEnumerateInstanceVersion",
			"vkEnumerateInstanceLayerProperties", "vkEnumerateInstanceExtensionProperties"
		};

		// Paramter record
		public sealed record Param(
			string Name,    // The param name
			string Type,    // The param type
			uint PtrDepth,  // The pointer depth of the param type
			bool Const,     // If the param is marked as const
			bool? Optional, // If the param is marked as optional
			string? LenStr, // The optional length string for pointers
			bool NeedsFix   // If the param needs to be fixed as a pointer before passing
		);

		// Possible command scopes
		public enum CommandScope { Global, Instance, Device };

		#region Fields
		// The command spec
		public readonly CommandSpec Spec;

		// The command name
		public string Name => Spec.Name;
		// The function pointer type string
		public readonly string TypeString;
		// The function return type
		public readonly string ReturnType;
		// The permutations of the command parameters, the first will always be the unaltered API args
		public IReadOnlyList<IReadOnlyList<Param>> ParamSets => _paramSets;
		private readonly List<List<Param>> _paramSets = new();
		// The command scope
		public readonly CommandScope Scope;
		// If the command is a core command
		public readonly bool IsCore;
		#endregion // Fields

		private CommandType(CommandSpec spec, string typestr, string retType, CommandScope scope, bool core)
		{
			Spec = spec;
			TypeString = typestr;
			ReturnType = retType;
			Scope = scope;
			IsCore = core;
		}

		// Try process
		public static bool TryProcess(CommandSpec spec, Dictionary<string, FuncType> functions, out CommandType? type)
		{
			type = null;

			// Reset type string
			BUILDER.Clear();
			BUILDER.Append("delegate* unmanaged<");

			// Process arguments
			string? firstArg = null;
			foreach (var par in spec.Params) {
				// Get output type
				if (NameHelper.ConvertToOutputType(par.Type, par.PtrDepth) is not string parType) {
					Program.PrintError($"Failed to parse argument '{par.Type} {par.Name}' for '{spec.Name}'");
					return false;
				}
				if (parType.StartsWith("PFN_")) {
					if (!functions.TryGetValue(parType, out var funcType)) {
						Program.PrintError($"Unknown parameter function type '{parType}' for '{spec.Name}'");
						return false;
					}
					parType = funcType.TypeString;
				}
				BUILDER.Append(parType);
				BUILDER.Append(", ");
				if (firstArg is null) {
					firstArg = parType;
				}
			}

			// Process return type
			if (NameHelper.ConvertToOutputType(spec.Return.Type, spec.Return.PtrDepth) is not string retType) {
				Program.PrintError(
					$"Failed to parse return type '{spec.Return.Type} (ptr={spec.Return.PtrDepth}) for '{spec.Name}'");
				return false;
			}
			if (retType.StartsWith("PFN_")) {
				if (!functions.TryGetValue(retType, out var funcType)) {
					Program.PrintError($"Unknown return function type '{retType}' for '{spec.Name}'");
					return false;
				}
				retType = funcType.TypeString;
			}
			BUILDER.Append(retType);
			BUILDER.Append('>');

			// Get scope
			CommandScope scope;
			if (GLOBAL_FUNCTIONS.Contains(spec.Name)) {
				scope = CommandScope.Global;
			}
			else {
				scope = firstArg switch {
					"VulkanHandle<VkInstance>" => CommandScope.Instance,
					"VulkanHandle<VkPhysicalDevice>" => CommandScope.Instance,
					_ => CommandScope.Device
				};
			}

			// Get if core
			var core = NameHelper.GetTypeVendor(spec.Name) is null;
			if (core && !spec.FeatureLevel.HasValue) {
				Program.PrintError($"The core command '{spec.Name}' does not have a feature level");
				return false;
			}

			// Create
			type = new(spec, BUILDER.ToString(), retType, scope, core);

			// Create parameter set variations
			// 0 = default, 1 = in/out variables
			for (int i = 0; i < 2; ++i) {
				List<Param> @params = new();
				var isDifferent = false;

				foreach (var par in spec.Params) {
					// Get output type
					var parType = NameHelper.ConvertToOutputType(par.Type, par.PtrDepth)!;
					if (parType.StartsWith("PFN_")) {
						var funcType = functions[parType]!;
						parType = funcType.TypeString;
					}
					var parName = NameHelper.GetSafeArgName(par.Name);

					// Switch on pass number
					if (i == 0) {
						@params.Add(new(parName, parType, par.PtrDepth, par.Const, par.Optional, par.LengthStr, false));
						isDifferent = true;
					}
					else if (i == 1) {
						// A single (non-array) and non-optional pointer
						var change = (par.PtrDepth > 0) && (par.LengthStr is null) && !par.Optional.GetValueOrDefault();
						// Sometimes void* (which gets mapped to void - an invalid type) slips through the cracks
						change = change && (parType != "void*");

						if (change) {
							parType = parType.Substring(0, parType.Length - 1); // Trim last '*'
							parType = (!par.Optional.HasValue ? "ref " : (par.Const ? "in " : "out ")) + parType;
							@params.Add(new(parName, parType, par.PtrDepth - 1, par.Const, par.Optional, par.LengthStr, true));
							isDifferent = true;
						}
						else {
							@params.Add(new(parName, parType, par.PtrDepth, par.Const, par.Optional, par.LengthStr, false));
						}
					}
				}
				
				if (isDifferent) {
					type._paramSets.Add(@params);
				}
			}

			// Return
			return true;
		}
	}
}
