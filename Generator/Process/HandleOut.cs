/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// A processed handle type
	public sealed class HandleOut
	{
		#region Fields
		// The spec that this handle was processed from
		public readonly HandleSpec Spec;

		// The output name of the struct
		public readonly string Name;
		// The vendor for the struct
		public readonly string VendorName;
		// The processed name as Vk.<Vendor>.<Name>
		public string ProcessedName => (VendorName.Length == 0) ? $"Vk.{Name}" : $"Vk.{VendorName}.{Name}";
		#endregion // Fields

		private HandleOut(HandleSpec spec, string name, string vendor)
		{
			Spec = spec;
			Name = name;
			VendorName = vendor;
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
			return new(spec, handleName, vendor ?? "");
		}
	}
}
