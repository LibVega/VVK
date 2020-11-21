/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Processed version of HandleSpec
	public sealed class HandleType
	{
		// Parent overrides (some types don't report the correct parent)
		private static readonly Dictionary<string, string> PARENT_OVERRIDES = new() {
			{ "VkSwapchainKHR", "VkDevice" }
		};

		#region Fields
		// The spec used to generate this type
		public readonly HandleSpec Spec;

		// Handle name
		public string Name => Spec.Name;
		// The name of the parent spec 
		public string? ParentName => _parentOverride ?? Spec.Parent;
		private readonly string? _parentOverride;

		// The parent type (not populated until the second pass)
		public HandleType? Parent { get; private set; }
		#endregion // Fields

		private HandleType(HandleSpec spec, string? parentOverride)
		{
			Spec = spec;
			_parentOverride = parentOverride;
		}

		// Try to find and set the parent
		public bool TrySetParent(Dictionary<string, HandleType> found)
		{
			if (ParentName is null) {
				return true;
			}

			// Assign parent
			if (!found.TryGetValue(ParentName, out var parent)) {
				Program.PrintError($"Handle type '{Name}' has unknown parent type '{ParentName}'");
				return false;
			}
			Parent = parent;
			return true;
		}

		// Try process
		public static bool TryProcess(HandleSpec spec, VulkanSpec vkspec, out HandleType? type)
		{
			// Check for override
			string? pover = null;
			if (PARENT_OVERRIDES.TryGetValue(spec.Name, out var over)) {
				pover = over;
			}

			// Return
			type = new(spec, pover);
			return true;
		}
	}
}
