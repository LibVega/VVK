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
	// Processed version of EnumSpec
	public sealed class EnumType
	{
		// Enum entry
		public sealed record Entry(
			string Name, // Entry name
			int Value    // Entry value
		);

		#region Fields
		// The spec for this type
		public readonly EnumSpec Spec;

		// Enum name
		public string Name => Spec.Name;
		// Enum values
		public IReadOnlyList<Entry> Entries => Alias?.Entries ?? _entries!;
		private readonly List<Entry>? _entries;

		// Potential alias
		public readonly EnumType? Alias;
		#endregion // Fields

		private EnumType(EnumSpec spec, List<Entry> entries)
		{
			Spec = spec;
			_entries = entries;
		}

		private EnumType(EnumSpec spec, EnumType alias)
		{
			Spec = spec;
			Alias = alias;
		}

		// Try process
		public static bool TryProcess(EnumSpec spec, VulkanSpec vkspec, Dictionary<string, EnumType> found,
			out EnumType? type)
		{
			type = null;

			// Check for alias type
			if (spec.Alias is not null) {
				if (!found.TryGetValue(spec.Alias.Name, out var alias)) {
					Program.PrintError($"Enum type '{spec.Name}' is aliased to unknown type '{spec.Alias.Name}'");
					return false;
				}

				// Return with alias
				type = new(spec, alias);
				return true;
			}

			// Process entries
			List<Entry> entries = new();
			foreach (var value in spec.Values) {
				// Process name
				if (NameHelper.ConvertEnumValue(spec.Name, value.Name) is not string procName) {
					Program.PrintError($"Failed to process enum value name '{value.Name}' for enum '{spec.Name}'");
					return false;
				}

				// Process value
				int procValue;
				if (value.ValueStr.StartsWith("0x")) {
					if (!Int32.TryParse(value.ValueStr.Substring("0x".Length), NumberStyles.AllowHexSpecifier, null,
							out procValue)) {
						Program.PrintError(
							$"Failed to parse hex value '{value.ValueStr}' for enum value '{value.Name}'");
						return false;
					}
				}
				else if (!Int32.TryParse(value.ValueStr, out procValue)) {
					Program.PrintError(
						$"Failed to parse decimal value '{value.ValueStr}' for enum value '{value.Name}'");
					return false;
				}
				if (value.IsBitpos) {
					procValue = 1 << procValue;
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
