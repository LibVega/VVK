/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Processed version of VulkanSpec, holding the processed objects
	public sealed class ProcessedSpec
	{
		// List of known struct types that should be skipped for processing
		private static readonly List<string> SKIP_STRUCTS = new() { 
			"VkBaseOutStructure", "VkBaseInStructure",
			"VkNativeBufferANDROID", "VkSwapchainImageCreateInfoANDROID",
			"VkPhysicalDevicePresentationPropertiesANDROID", "VkNativeBufferUsage2ANDROID"
		};

		#region Fields
		// Processed bitmask types
		public IReadOnlyDictionary<string, BitmaskType> Bitmasks => _bitmasks;
		private readonly Dictionary<string, BitmaskType> _bitmasks = new();

		// Processed handle types
		public IReadOnlyDictionary<string, HandleType> Handles => _handles;
		private readonly Dictionary<string, HandleType> _handles = new();

		// Processed enum types
		public IReadOnlyDictionary<string, EnumType> Enums => _enums;
		private readonly Dictionary<string, EnumType> _enums = new();

		// Processed constant values
		public IReadOnlyDictionary<string, ConstantValue> Constants => _constants;
		private readonly Dictionary<string, ConstantValue> _constants = new();

		// Processed function types
		public IReadOnlyDictionary<string, FuncType> Functions => _functions;
		private readonly Dictionary<string, FuncType> _functions = new();

		// Processed struct types
		public IReadOnlyDictionary<string, StructType> Structs => _structs;
		private readonly Dictionary<string, StructType> _structs = new();

		// Processed command types
		public IReadOnlyDictionary<string, CommandType> Commands => _commands;
		private readonly Dictionary<string, CommandType> _commands = new();

		// Passthrough of the unchanged extension types
		public IReadOnlyDictionary<string, ExtensionSpec> Extensions => _extensions;
		private readonly Dictionary<string, ExtensionSpec> _extensions = new();

		// The vendors
		public IReadOnlyDictionary<string, Vendor> Vendors => _vendors;
		private readonly Dictionary<string, Vendor> _vendors = new();
		#endregion // Fields

		private ProcessedSpec() { }

		// Top level processing function
		public static bool TryProcess(VulkanSpec vkspec, out ProcessedSpec? spec)
		{
			spec = new();

			// Process bitmasks
			foreach (var bitmask in vkspec.Bitmasks) {
				if (!BitmaskType.TryProcess(bitmask.Value, vkspec, spec._bitmasks, out var type)) {
					return false;
				}
				spec._bitmasks.Add(type!.Name, type);
				Program.PrintVerbose($"Processed bitmask type '{type.Name}'");
			}
			Program.Print($"Processed {spec._bitmasks.Count} bitmask types");

			// Process handles
			foreach (var handle in vkspec.Handles) {
				if (!HandleType.TryProcess(handle.Value, out var type)) {
					return false;
				}
				spec._handles.Add(type!.Name, type);
			}
			foreach (var handle in spec._handles) {
				if (!handle.Value.TrySetParent(spec._handles)) {
					return false;
				}
			}
			foreach (var handle in spec._handles) {
				if (!handle.Value.TrySetFunctionParent(spec._handles)) {
					return false;
				}
				Program.PrintVerbose($"Processed handle type '{handle.Value.Name}'");
			}
			Program.Print($"Processed {spec._handles.Count} handle types");

			// Process enums
			foreach (var @enum in vkspec.Enums) {
				if (@enum.Value.Name.Contains("FlagBits")) {
					continue; // Skip enums that have already been used by bitmasks
				}
				if (!EnumType.TryProcess(@enum.Value, vkspec, spec._enums, out var type)) {
					return false;
				}
				spec._enums.Add(type!.Name, type);
				Program.PrintVerbose($"Processed enum type '{type.Name}'");
			}
			Program.Print($"Processed {spec._enums.Count} enum types");

			// Process constants
			foreach (var @const in vkspec.Constants) {
				if (!ConstantValue.TryProcess(@const.Value, out var value)) {
					return false;
				}
				spec._constants.Add(value!.Name, value);
				Program.PrintVerbose($"Processed constant value '{value.Name}'");
			}
			Program.Print($"Processed {spec._constants.Count} constants");

			// Process functions
			foreach (var func in vkspec.Functions) {
				if (!FuncType.TryProcess(func.Value, out var type)) {
					return false;
				}
				spec._functions.Add(type!.TypeName, type);
				Program.PrintVerbose($"Processed function type '{type.TypeName}'");
			}
			Program.Print($"Processed {spec._functions.Count} functions");

			// Process structs
			foreach (var @struct in vkspec.Structs) {
				if (SKIP_STRUCTS.Contains(@struct.Key)) {
					continue;
				}
				if (!StructType.TryProcess(@struct.Value, vkspec, spec._structs, spec._constants, spec._functions,
						out var type)) {
					return false;
				}
				spec._structs.Add(type!.Name, type);
				Program.PrintVerbose($"Processed struct type '{type.Name}'");
			}
			Program.Print($"Processed {spec._structs.Count} struct types");

			// Process commnads
			foreach (var cmd in vkspec.Commands) {
				if (!CommandType.TryProcess(cmd.Value, spec._functions, out var type)) {
					return false;
				}
				spec._commands.Add(type!.Name, type);
				Program.PrintVerbose($"Processed command type '{type.Name}'");
			}
			Program.Print($"Processed {spec._commands.Count} commands");

			// Pass through extensions
			foreach (var ext in vkspec.Extensions) {
				spec._extensions.Add(ext.Key, ext.Value);
			}
			Program.Print($"Processed {spec._extensions.Count} extensions");

			// Create and assign vendors
			spec._vendors.Add("Core", new("Core"));
			foreach (var vtag in vkspec.Vendors) {
				spec._vendors.Add(vtag, new(vtag));
			}
			AssignVendors(spec);
			Program.Print($"Processed {spec._vendors.Count} vendors");

			return true;
		}

		// Assigns types to vendors
		private static void AssignVendors(ProcessedSpec spec)
		{
			var core = spec.Vendors["Core"];

			// Assign types
			foreach (var type in spec._bitmasks) {
				var tag = NameHelper.GetTypeVendor(type.Key);
				if ((tag is not null) && spec.Vendors.TryGetValue(tag, out var vendor)) {
					vendor.AddType(type.Value);
				}
				else {
					core.AddType(type.Value);
				}
			}
			foreach (var type in spec._handles) {
				var tag = NameHelper.GetTypeVendor(type.Key);
				if ((tag is not null) && spec.Vendors.TryGetValue(tag, out var vendor)) {
					vendor.AddType(type.Value);
				}
				else {
					core.AddType(type.Value);
				}
			}
			foreach (var type in spec._enums) {
				var tag = NameHelper.GetTypeVendor(type.Key);
				if ((tag is not null) && spec.Vendors.TryGetValue(tag, out var vendor)) {
					vendor.AddType(type.Value);
				}
				else {
					core.AddType(type.Value);
				}
			}
			foreach (var type in spec._structs) {
				var tag = NameHelper.GetTypeVendor(type.Key);
				if ((tag is not null) && spec.Vendors.TryGetValue(tag, out var vendor)) {
					vendor.AddType(type.Value);
				}
				else {
					core.AddType(type.Value);
				}
			}

			// Remove vendors that have no entries
			List<string> toremove = new();
			foreach (var ven in spec._vendors.Values) {
				int bc = ven.Bitmasks.Count,
					hc = ven.Handles.Count,
					ec = ven.Enums.Count,
					sc = ven.Structs.Count;
				if ((bc + hc + ec + sc) > 0) {
					Program.PrintVerbose(
						$"Processed vendor '{ven.Tag}' - {bc} bitmasks, {hc} handles, {ec} enums, {sc} structs");
				}
				else {
					toremove.Add(ven.Tag);
				}
			}
			foreach (var rem in toremove) {
				spec._vendors.Remove(rem);
			}
		}
	}
}
