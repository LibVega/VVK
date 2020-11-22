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
			string Name,   // The param name
			string Type,   // The param type
			uint PtrDepth, // The pointer depth of the param type
			bool Const,    // If the param is marked as const
			bool Optional  // If the param is marked as optional
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
		// The command parameters
		public IReadOnlyList<Param> Params => _params;
		private readonly List<Param> _params;
		// The command scope
		public readonly CommandScope Scope;
		// If the command is a core command
		public readonly bool IsCore;
		#endregion // Fields

		private CommandType(CommandSpec spec, string typestr, string retType, List<Param> pars, CommandScope scope, 
			bool core)
		{
			Spec = spec;
			TypeString = typestr;
			ReturnType = retType;
			_params = pars;
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
			List<Param> @params = new();
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
				var parname = NameHelper.GetSafeArgName(par.Name);
				@params.Add(new(parname, parType, par.PtrDepth, par.Const, par.Optional));
				BUILDER.Append(parType);
				BUILDER.Append(", ");
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
				scope = @params[0].Type switch {
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

			// Return
			type = new(spec, BUILDER.ToString(), retType, @params, scope, core);
			return true;
		}
	}
}
