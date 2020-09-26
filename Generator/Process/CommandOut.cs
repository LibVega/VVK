/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Processed command from the spec
	public sealed class CommandOut
	{
		// The known global scope functions
		private static readonly List<string> GLOBAL_FUNCTIONS = new() {
			"vkCreateInstance", "vkEnumerateInstanceExtensionProperties",
			"vkEnumerateInstanceLayerProperties", "vkEnumerateInstanceVersion"
		};

		#region // Fields
		// The spec that this command was generated from
		public readonly CommandSpec Spec;
		// The name of the function
		public readonly string Name;
		// The C# function pointer prototype
		public readonly string Prototype;
		// The load level for the function
		public readonly CommandScope Scope;
		#endregion // Fields

		private CommandOut(CommandSpec spec, string name, string proto, CommandScope scope)
		{
			Spec = spec;
			Name = name;
			Prototype = proto;
			Scope = scope;
		}

		// Process
		public static CommandOut? TryProcess(CommandSpec spec, NameHelper names)
		{
			// Get the arg types
			var args = new string[spec.Arguments.Count + 1];
			int aidx = 0;
			foreach (var arg in spec.Arguments) {
				if (!names.ProcessGeneralTypeName(arg.Type, out args[aidx++])) {
					Program.PrintError($"Failed to parse argument type {arg.Type}");
					return null;
				}
			}

			// Get the return type
			if (!names.ProcessGeneralTypeName(spec.ReturnType, out var retType)) {
				Program.PrintError($"Failed to parse func return type '{spec.ReturnType}'");
				return null;
			}
			args[^1] = retType;

			// Figure the scope of the function
			CommandScope scope;
			if (GLOBAL_FUNCTIONS.Contains(spec.Name)) {
				scope = CommandScope.Global;
			}
			else if (args[0] == "Vk.Instance" || args[0] == "Vk.PhysicalDevice" || 
					spec.Name == "vkGetDeviceProcAddr") {
				scope = CommandScope.Instance;
			}
			else {
				scope = CommandScope.Device;
			}

			// Return
			return new(spec, spec.Name, $"delegate* unmanaged<{String.Join(", ", args)}>", scope);
		}
	}

	public enum CommandScope
	{
		Global, // Function is loaded at the global level
		Instance, // Function is loaded on an instant object
		Device, // Function is loaded on a device object
	}
}
