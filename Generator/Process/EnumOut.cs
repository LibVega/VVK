/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Represents a processed enum type
	public sealed class EnumOut
	{
		// A processed enum value
		public record Entry(string Name, int Value);

		#region Fields
		// The spec that this enum was processed from
		public readonly EnumSpec Spec;

		// The final generated name of the enum
		public readonly string Name;
		// The vendor name for the enum
		public readonly string VendorName;
		// The processed name as Vk.<Vendor>.<Name>
		public string ProcessedName => (VendorName.Length == 0) ? $"Vk.{Name}" : $"Vk.{VendorName}.{Name}";

		// The enum entries
		public readonly List<Entry> Values;

		// Forward
		public bool IsBitmask => Spec.IsBitmask;
		public bool IsAlias => Spec.IsAlias;
		#endregion // Fields

		private EnumOut(EnumSpec spec, string name, string vendor, List<Entry> entries)
		{
			Spec = spec;
			Name = name;
			VendorName = vendor;
			Values = entries;
		}

		// Process from a spec
		public static EnumOut? TryProcess(EnumSpec spec, NameHelper names)
		{
			// Try to convert the name
			if (!names.ProcessEnumName(spec.Name, out var baseName, out var vendorName)) {
				return null;
			}

			// Try to convert all the fields
			List<Entry> entries = new();
			foreach (var ent in spec.Values) {
				if (!names.ProcessEnumValueName(ent.Name, baseName, out var entryName)) {
					return null;
				}
				entries.Add(new(entryName, ent.Value));
			}

			// Add named default value to bitmasks
			if (spec.IsBitmask) {
				entries.Add(new("NoFlags", 0));
			}

			return new(spec, baseName, vendorName ?? "", entries);
		}
	}
}
