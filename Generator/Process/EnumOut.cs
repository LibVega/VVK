/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// The processed partner of EnumSpec
	public sealed class EnumOut
	{
		#region Fields
		// The processed name of the type (will be written to the C# source)
		public readonly string Name;
		// The extension namespace of the type (will be empty for the core namespace)
		public readonly string Extension;
		// The spec object that this object was generated from
		public readonly EnumSpec Spec;

		// Forward values
		public bool IsBitmask => Spec.IsBitmask;
		public bool IsAlias => Spec.IsAlias;
		public string? Comment => Spec.Comment;
		#endregion // Fields

		private EnumOut(string name, string ext, EnumSpec spec)
		{
			Name = name;
			Extension = ext;
			Spec = spec;
		}

		// Processes an EnumSpec object into an EnumOut object
		public static bool TryProcess(EnumSpec spec, out EnumOut? output)
		{
			output = null;

			// Try to parse the name and extension
			if (!NameUtils.ConvertTypeName(spec.Name, out var outName, out var outExt)) {
				Program.PrintWarning($"Failed to process enum '{spec.Name}'");
				return false;
			}

			// Create and return
			output = new(outName, outExt, spec);
			return true;
		}
	}
}
