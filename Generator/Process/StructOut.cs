/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Gen
{
	// Represents a processed struct spec
	public sealed class StructOut
	{
		// Represents a struct field
		public record Field(string Name, string Type, string? SizeLiteral, bool IsFixed = false);

		#region Fields
		// The spec that this struct was processed from
		public readonly StructSpec Spec;

		// The output name of the struct
		public readonly string Name;
		// The vendor for the struct
		public readonly string VendorName;
		// The processed name as Vk.<Vendor>.<Name>
		public string ProcessedName => (VendorName.Length == 0) ? $"Vk.{Name}" : $"Vk.{VendorName}.{Name}";

		// The struct fields
		public readonly List<Field> Fields;

		// If the struct has a (sType) field
		public readonly bool HasSType;
		// if the struct has a (pNext) field 
		public readonly bool HasPNext;

		// Forward
		public bool IsUnion => Spec.IsUnion;
		#endregion // Fields

		private StructOut(StructSpec spec, string name, string vendor, List<Field> fields)
		{
			Spec = spec;
			Name = name;
			VendorName = vendor;
			Fields = fields;

			HasSType = (fields.Count > 0) && fields[0] is ("Type", "Vk.StructureType", null, _);
			HasPNext = (fields.Count > 1) && fields[1] is ("Next", "void*", null, _);
		}

		// Processing for struct specs
		public static StructOut? TryProcess(StructSpec spec, NameHelper names)
		{
			// Try to get the new name and vendor
			if (!names.ProcessVkTypeName(spec.Name, out var baseName, out var vendorName)) {
				Program.PrintError($"Invalid struct type name '{spec.Name}'");
				return null;
			}

			// Process each of the fields
			List<Field> fields = new();
			foreach (var field in spec.Fields) {
				// Get the type and names
				if (!names.ProcessStructFieldName(field.Name, out var fieldName)) {
					Program.PrintError($"Invalid struct field name '{field.Name}'");
					return null;
				}
				if (!names.ProcessGeneralTypeName(field.Type, out var fieldType)) {
					Program.PrintError($"Invalid struct field type '{field.Type}'");
					return null;
				}

				// Fix error where names overlap
				if (fieldName == baseName) {
					fieldName += "_";
				}

				// Process the size literals
				string? size = null;
				bool @fixed = names.IsFixedType(fieldType);
				if (field.Sizes is not null) {
					// Detect use of FixedString type and API constant sizes
					if (field.Sizes.Length == 1 && field.Sizes[0].StartsWith("VK_")) {
						if (fieldType == "byte") {
							fieldType =
								field.Sizes[0].Contains("UUID") ? "Vk.UUID" :
								field.Sizes[0].Contains("LUID") ? "Vk.LUID" :
								"Vk.FixedString";
						}
						else size = $"(int)Vk.Constants.{field.Sizes[0].Substring(3)}";
					}
					else {
						int sizeVal = 1;
						foreach (var szstr in field.Sizes) {
							bool isHex = szstr.StartsWith("0x");
							if (!Int32.TryParse(isHex ? szstr.Substring(2) : szstr, 
									isHex ? NumberStyles.HexNumber : NumberStyles.Integer, null, out var parseVal)) {
								Program.PrintError($"The size literal '{szstr}' is not a valid integer");
								return null;
							}
							sizeVal *= parseVal;
						}
						size = sizeVal.ToString();
					}
				}

				fields.Add(new(fieldName, fieldType, size, @fixed));
			}

			// Return
			return new(spec, baseName, vendorName ?? "", fields);
		}
	}
}
