/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Gen
{
	// A processed handle type
	public sealed class HandleOut
	{
		// Needed by some extensions who don't report entirely accurate parent types
		// Map HandleOut.name -> vk name
		private static readonly Dictionary<string, string> PARENT_OVERRIDES = new() {
			{ "Swapchain", "VkDevice" },
			{ "Surface", "VkPhysicalDevice" }
		};
		// Certain functions appear to be valid for 2nd arg, but are actually used by the first arg
		// TODO: Look for an automatic way to detect this, but the list is short so manual overrides work for now
		private static readonly Dictionary<string, string> ARG_OVERRIDES = new() {
			{ "vkQueueSetPerformanceConfigurationINTEL", "Queue" },
			{ "vkDisplayPowerControlEXT", "Device" },
			{ "vkRegisterDisplayEventEXT", "Device" },
			{ "vkGetDisplayPlaneCapabilitiesKHR", "PhysicalDevice" },
			{ "vkGetDeviceGroupSurfacePresentModesKHR", "Device" }
		};

		public record HandleCommand(string Name, string SigStr, string CallStr);

		#region Fields
		// The spec that this handle was processed from
		public readonly HandleSpec Spec;

		// The output name of the handle
		public readonly string Name;
		// The vendor for the handle
		public readonly string VendorName;
		// The processed name as Vk.<Vendor>.<Name>
		public string ProcessedName => (VendorName.Length == 0) ? $"Vk.{Name}" : $"Vk.{VendorName}.{Name}";
		// The parent handle type processed name
		public HandleOut? Parent { get; private set; }

		// List of commands associated with the handle
		public IReadOnlyList<HandleCommand> Commands => _commands;
		private readonly List<HandleCommand> _commands;
		#endregion // Fields

		private HandleOut(HandleSpec spec, string name, string vendor)
		{
			Spec = spec;
			Name = name;
			VendorName = vendor;
			Parent = null;
			_commands = new();
		}

		// Process
		public static HandleOut? TryProcess(HandleSpec spec, NameHelper names)
		{
			// Convert the handle name
			if (!names.ProcessVkTypeName(spec.Name, out var handleName, out var vendor)) {
				Program.PrintError($"Failed to process handle name '{spec.Name}'");
				return null;
			}

			// Return
			return new(spec, handleName, vendor ?? String.Empty);
		}

		// Post-Process parent finding
		public bool PopulateParent(NameHelper names, Dictionary<string, Vendor> vendors)
		{
			string? pname = Spec.ParentType;
			if (PARENT_OVERRIDES.TryGetValue(Name, out var parover)) {
				pname = parover;
			}

			// Convert the parent name
			if (pname is not null) {
				if (!names.ProcessVkTypeName(pname, out var parentName, out var parentVendor)) {
					Program.PrintError($"Failed to process parent handle name '{pname}'");
					return false;
				}
				if (!vendors.TryGetValue(parentVendor ?? String.Empty, out var parven)) {
					Program.PrintError($"Failed to find handle parent vendor '{parentVendor}'");
					return false;
				}
				if (!parven.Handles.TryGetValue(parentName, out var parent)) {
					Program.PrintError($"Failed to find handle parent '{parentName}'");
					return false;
				}
				Parent = parent;
			}
			return true;
		}

		// Check if a command is associated with this handle, adds it if it is
		public bool TryAddCommand(CommandOut cmd)
		{
			// All global commands go in instance
			if ((cmd.CommandScope == CommandScope.Global) && Name == "Instance") {
				_commands.Add(MakeGlobalCommand(cmd, false));
				if (cmd.AlternateArgs is not null) {
					_commands.Add(MakeGlobalCommand(cmd, true));
				}
				return true;
			}

			// All "vkCmd*" automatically are part of command buffer
			if (cmd.Name.StartsWith("vkCmd") && (Name == "CommandBuffer")) {
				_commands.Add(MakeCommandBufferCommand(cmd, false));
				if (cmd.AlternateArgs is not null) {
					_commands.Add(MakeCommandBufferCommand(cmd, true));
				}
				return true;
			}

			// All arg2 overrides are treated as first arg commands
			if (ARG_OVERRIDES.TryGetValue(cmd.Name, out var otype)) {
				if (otype == Name) {
					_commands.Add(MakeFirstArgCommand(cmd, false));
					if (cmd.AlternateArgs is not null) {
						_commands.Add(MakeFirstArgCommand(cmd, true));
					}
					return true;
				}
				return false;
			}

			// Check second argument first
			if ((cmd.Arguments.Count >= 2) && cmd.Arguments[1].Type.StartsWith("Vk.Handle<")) {
				var hname = cmd.Arguments[1].Type.Substring("Vk.Handle<".Length).TrimEnd('>');
				if (hname == ProcessedName) {
					_commands.Add(MakeSecondArgCommand(Parent!, cmd, false));
					if (cmd.AlternateArgs is not null) {
						_commands.Add(MakeSecondArgCommand(Parent!, cmd, true));
					}
					return true;
				}
				return false; // This command can be used by another handle
			}

			// Check first argument
			if ((cmd.Arguments.Count >= 1) && cmd.Arguments[0].Type.StartsWith("Vk.Handle<")) {
				var hname = cmd.Arguments[0].Type.Substring("Vk.Handle<".Length);
				if (hname.TrimEnd('>') == ProcessedName) {
					_commands.Add(MakeFirstArgCommand(cmd, false));
					if (cmd.AlternateArgs is not null) {
						_commands.Add(MakeFirstArgCommand(cmd, true));
					}
					return true;
				}
			}

			return false;
		}

		private static HandleCommand MakeGlobalCommand(CommandOut cmd, bool alt)
		{
			var argStr = String.Join(", ", 
				(alt ? cmd.AlternateArgs! : cmd.Arguments).Select(arg => $"{arg.Type} {arg.Name}"));
			var callStr = String.Join(", ", 
				(alt ? cmd.AlternateArgs! : cmd.Arguments).Select(arg => arg.Type.StartsWith("out ") ? "out " + arg.Name : arg.Name));
			return new(
				cmd.Name,
				$"public static {cmd.ReturnType} {cmd.Name.Substring("vk".Length)}({argStr})",
				$"InstanceFunctionTable.{cmd.Name.Substring(alt ? "vk".Length : 0)}({callStr})"
			);			
		}

		private static HandleCommand MakeCommandBufferCommand(CommandOut cmd, bool alt)
		{
			var argStr = String.Join(", ",
				(alt ? cmd.AlternateArgs! : cmd.Arguments).Skip(1).Select(arg => $"{arg.Type} {arg.Name}"));
			var callStr = "_handle, " + String.Join(", ",
				(alt ? cmd.AlternateArgs! : cmd.Arguments).Skip(1)
					.Select(arg => arg.Type.StartsWith("out ") ? "out " + arg.Name : arg.Name));
			if (callStr.EndsWith(", ")) {
				callStr = "_handle";
			}
			return new(
				cmd.Name,
				$"public {cmd.ReturnType} {cmd.Name.Substring("vkCmd".Length)}({argStr})",
				$"Functions.{cmd.Name.Substring(alt ? "vk".Length : 0)}({callStr})"
			);
		}

		private static HandleCommand MakeSecondArgCommand(HandleOut parent, CommandOut cmd, bool alt)
		{
			var argStr = String.Join(", ",
				(alt ? cmd.AlternateArgs! : cmd.Arguments).Skip(2).Select(arg => $"{arg.Type} {arg.Name}"));
			var callStr = String.Join(", ",
				(alt ? cmd.AlternateArgs! : cmd.Arguments).Skip(2)
					.Select(arg => arg.Type.StartsWith("out ") ? "out " + arg.Name : arg.Name));
			var call0 = cmd.Arguments[0].Type.Substring("Vk.Handle<".Length).TrimEnd('>') switch { 
				"Vk.Instance" => "Instance._handle",
				"Vk.Device" => "Device._handle",
				_ => "Parent._handle"
			};
			callStr = call0 + ", _handle" + (callStr.Length > 0 ? $", {callStr}" : "");
			return new(
				cmd.Name,
				$"public {cmd.ReturnType} {cmd.Name.Substring("vk".Length)}({argStr})",
				$"Functions.{cmd.Name.Substring(alt ? "vk".Length : 0)}({callStr})"
			);
		}

		private static HandleCommand MakeFirstArgCommand(CommandOut cmd, bool alt)
		{
			var argStr = String.Join(", ",
				(alt ? cmd.AlternateArgs! : cmd.Arguments).Skip(1).Select(arg => $"{arg.Type} {arg.Name}"));
			var callStr = "_handle, " + String.Join(", ",
				(alt ? cmd.AlternateArgs! : cmd.Arguments).Skip(1)
					.Select(arg => arg.Type.StartsWith("out ") ? "out " + arg.Name : arg.Name));
			if (callStr.EndsWith(", ")) {
				callStr = "_handle";
			}
			return new(
				cmd.Name,
				$"public {cmd.ReturnType} {cmd.Name.Substring("vk".Length)}({argStr})",
				$"Functions.{cmd.Name.Substring(alt ? "vk".Length : 0)}({callStr})"
			);
		}
	}
}
