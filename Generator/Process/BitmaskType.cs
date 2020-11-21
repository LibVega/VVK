/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Gen
{
	// Processed version of BitmaskSpec
	public sealed class BitmaskType
	{
		// Start type for bitmask values, and reference used for bitmasks with no values
		private static readonly List<Entry> DEFAULT_ENTRY_LIST = new() { new("NoFlags", 0) };

		// Values for a processed enum value
		public sealed record Entry(
			string Name, // The entry name
			uint Value   // The entry value
		);

		#region Fields
		// The spec for this type
		public readonly BitmaskSpec Spec;

		// The type name
		public string Name => Spec.Name;
		// The type values
		public IReadOnlyList<Entry> Entries => Alias?.Entries ?? _entries!;
		private readonly List<Entry>? _entries;

		// The potential aliased type
		public readonly BitmaskType? Alias;
		#endregion // Fields

		private BitmaskType(BitmaskSpec spec, List<Entry> entries)
		{
			Spec = spec;
			_entries = entries;
		}

		private BitmaskType(BitmaskSpec spec, BitmaskType alias)
		{
			Spec = spec;
			Alias = alias;
		}

		public static bool TryProcess(BitmaskSpec spec, VulkanSpec vkspec, Dictionary<string, BitmaskType> found,
			out BitmaskType? type)
		{
			type = null;

			// If no values, quick return
			if (spec.BitsName is null) {
				type = new(spec, DEFAULT_ENTRY_LIST);
				return true;
			}

			// Check for an alias type
			if (spec.Alias is not null) {
				if (!found.TryGetValue(spec.Alias.Name, out var alias)) {
					Program.PrintError($"Bitmask type '{spec.Name}' is aliased to unknown type '{spec.Alias.Name}'");
					return false;
				}
				type = new(spec, alias);
				return true;
			}

			// Get the enum containing the flags, and process them
			if (!vkspec.Enums.TryGetValue(spec.BitsName, out var bitsSpec)) {
				Program.PrintError($"Bitmask type '{spec.Name}' gets values from unknown type '{spec.BitsName}'");
				return false;
			}
			List<Entry> entries = new(DEFAULT_ENTRY_LIST);
			foreach (var value in bitsSpec.Values) {
				// Process name
				if (NameHelper.ConvertEnumValue(bitsSpec.Name, value.Name) is not string procName) {
					Program.PrintError($"Failed to process enum value name '{value.Name}' for bitmask '{spec.Name}'");
					return false;
				}

				// Process value
				uint procValue;
				if (value.ValueStr.StartsWith("0x")) {
					if (!UInt32.TryParse(value.ValueStr.Substring("0x".Length), NumberStyles.AllowHexSpecifier, null, 
							out procValue)) {
						Program.PrintError(
							$"Failed to parse hex value '{value.ValueStr}' for enum value '{value.Name}'");
						return false;
					}
				}
				else if (!UInt32.TryParse(value.ValueStr, out procValue)) {
					Program.PrintError(
						$"Failed to parse decimal value '{value.ValueStr}' for enum value '{value.Name}'");
					return false;
				}
				if (value.IsBitpos) {
					procValue = (uint)(1 << (int)procValue);
				}

				// Check for invalid duplicates (sometimes there are valid duplicates)
				var dup = entries.FirstOrDefault(ent => ent.Name == procName);
				if ((dup is not null) && (dup.Value != procValue)) {
					Program.PrintError($"Invalid duplicate enum value name '{procName}'");
					return false;
				}

				if (dup is null) {
					entries.Add(new(procName, procValue)); 
				}
			}

			// Return
			type = new(spec, entries);
			return true;
		}
	}
}
