/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// The processed partner of EnumSpec
	public sealed class EnumOut
	{
		// The processed partner for EnumSpec.Entry
		public record Entry(string Name, int Value, string? Comment);
		
		#region Fields
		// The processed name of the type (will be written to the C# source)
		public readonly string Name;
		// The extension namespace of the type (will be empty for the core namespace)
		public readonly string Extension;
		// The spec object that this object was generated from
		public readonly EnumSpec Spec;
		// The processed enum values
		public readonly List<Entry> Entries;

		// Full name for type, including the namespace chain
		public string FullName => Extension.Length == 0 ? $"Vk.{Name}" : $"Vk.{Extension}.{Name}";

		// Forward values
		public bool IsBitmask => Spec.IsBitmask;
		public bool IsAlias => Spec.IsAlias;
		public string? Comment => Spec.Comment;
		#endregion // Fields

		private EnumOut(string name, string ext, EnumSpec spec, List<Entry> entries)
		{
			Name = name;
			Extension = ext;
			Spec = spec;
			Entries = entries;
		}

		// Processes an EnumSpec object into an EnumOut object
		public static bool TryProcess(EnumSpec spec, out EnumOut? output)
		{
			output = null;

			// Try to parse the name and extension
			if (!NameUtils.ConvertTypeName(spec.Name, out var outName, out var outExt)) {
				Program.PrintError($"Failed to process enum name '{spec.Name}'");
				return false;
			}

			// Try to parse each of the values
			List<Entry> ents = new();
			foreach (var entry in spec.Entries) {
				if (!NameUtils.ConvertEnumValueName(entry.Name, outName, out var entName)) {
					Program.PrintError($"Failed to process enum value '{entry.Name}'");
					return false;
				}
				ents.Add(new(entName, entry.Value, entry.Comment));
			}

			// Create and return
			output = new(outName, outExt, spec, ents);
			return true;
		}
	}
}
