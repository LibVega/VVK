/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Processed partner of StructSpec
	public sealed class StructOut
	{
		// Processed partner of StructSpec.Field
		public record Field(
			string Name, 
			string Type, 
			string? Comment,
			uint Offset,
			uint Size
		);

		#region Fields
		// The generated name of the struct
		public readonly string Name;
		// The extension namespace of the struct
		public readonly string Extension;
		// The spec object that this was generated from
		public readonly StructSpec Spec;
		// The fields of the object
		public readonly List<Field> Fields;

		// Full name for type, including the namespace chain
		public string FullName => Extension.Length == 0 ? $"Vk.{Name}" : $"Vk.{Extension}.{Name}";

		// Forward
		public bool IsTyped => Spec.IsTyped;
		#endregion // Fields

		private StructOut(string name, string ext, StructSpec spec, List<Field> fields)
		{
			Name = name;
			Extension = ext;
			Spec = spec;
			Fields = fields;
		}

		// Attempt to process a spec struct into an output struct
		public static bool TryProcess(StructSpec spec, out StructOut? output)
		{
			output = null;

			// Try to convert the type name
			if (!NameUtils.ConvertTypeName(spec.Name, out var outName, out var outExt)) {
				Program.PrintError($"Failed to process struct name '{spec.Name}'");
				return false;
			}

			// Convert all of the fields
			List<Field> fields = new();
			foreach (var fld in spec.Fields) {
				if (!NameUtils.ConvertStructField(fld.Name, out var fieldName)) {
					Program.PrintError($"Failed to process struct field '{fld.Name}'");
					return false;
				}
				fields.Add(new(fieldName, fld.Type, fld.Comment, 0, 0));
			}

			// Create and return
			output = new(outName, outExt, spec, fields);
			return true;
		}
	}
}
