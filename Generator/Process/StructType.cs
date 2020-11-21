/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Processed version of StructSpec
	public sealed class StructType
	{
		// Special fields that are known to have unique fixed sizes
		private static readonly Dictionary<string, uint> ARRAY_SIZE_OVERRIDES = new() {
			{ "VkTransformMatrixKHR.Matrix", 12 }
		};

		// Struct field
		public sealed record Field(
			string Name,     // The name of the field
			string Type,     // The output (C#) field type name
			uint PtrDepth,   // The pointer depth of the field type ('*' is already included in type name)
			uint? ArraySize, // The optional array size, if the field is a fixed array
			bool IsFixed     // If the field is fixed (an array, and also a type that can be fixed)
		);

		#region Fields
		// The spec for this type
		public readonly StructSpec Spec;

		// The struct name
		public string Name => Spec.Name;
		// If the struct is a union
		public bool IsUnion => Spec.IsUnion;
		// If the struct is typed
		public bool IsTyped => Type is not null;
		// The struct type as the processed VkStructureType value, if typed
		public readonly string? Type;
		// The struct fields
		public IReadOnlyList<Field> Fields => Alias?.Fields ?? _fields!;
		private readonly List<Field>? _fields;

		// The potential alias
		public readonly StructType? Alias;
		#endregion // Fields

		private StructType(StructSpec spec, List<Field> fields, string? type)
		{
			Spec = spec;
			_fields = fields;
			Type = type;
		}

		private StructType(StructSpec spec, StructType alias, string? type)
		{
			Spec = spec;
			Alias = alias;
			Type = type;
		}

		// Try process
		public static bool TryProcess(StructSpec spec, VulkanSpec vkspec, Dictionary<string, StructType> found,
			Dictionary<string, ConstantValue> constants, Dictionary<string, FuncType> functions, out StructType? type)
		{
			type = null;

			// Get the potential structure type
			string? sType = null;
			if ((spec.Fields.Count > 0) && (spec.Fields[0].Name == "sType")) {
				if (NameHelper.ConvertEnumValue("VkStructureType", spec.Fields[0].Value!) is not string typeName) {
					Program.PrintError($"Struct '{spec.Name}' has invalid structure type '{spec.Fields[0]!.Value}'");
					return false;
				}
				sType = typeName;
			}

			// Try for alias
			if (spec.Alias is not null) {
				if (!found.TryGetValue(spec.Alias.Name, out var alias)) {
					Program.PrintError($"Struct type '{spec.Name}' is aliased to unknown type '{spec.Alias.Name}'");
					return false;
				}

				// Exit with alias
				type = new(spec, alias, sType);
				return true;
			}

			// Process the fields
			List<Field> fields = new();
			foreach (var fld in spec.Fields) {
				// Get name and type fields
				var fName = NameHelper.ConvertStructField(fld.Name);
				if (NameHelper.ConvertToOutputType(fld.Type, fld.PtrDepth, fld.ArraySize) is not string fType) {
					Program.PrintError(
						$"Struct field '{spec.Name}.{fld.Name}' is unknown type '{fld.Type}' (ptr={fld.PtrDepth})");
					return false;
				}
				if (fType.StartsWith("PFN_")) {
					if (!functions.TryGetValue(fType, out var funcType)) {
						Program.PrintError($"Failed to find function pointer '{fType}'");
						return false;
					}
					fType = funcType.TypeString;
				}

				// Get array size
				uint? arraySize = null;
				if (fType.StartsWith("VVK")) {
					arraySize = null; // The array type got replaced with a single known type
				}
				else if (fld.ArraySize is not null) {
					if (ARRAY_SIZE_OVERRIDES.TryGetValue($"{spec.Name}.{fName}", out var asize)) {
						arraySize = asize;
					}
					else if (Char.IsDigit(fld.ArraySize[0])) {
						if (!UInt32.TryParse(fld.ArraySize, out asize)) {
							Program.PrintError(
								$"Invalid integer array size '{fld.ArraySize}' for '{spec.Name}.{fld.Name}'");
							return false;
						}
						arraySize = asize;
					}
					else if (fld.ArraySize.StartsWith("VK_")) {
						if (!constants.TryGetValue(fld.ArraySize.Substring("VK_".Length), out var @const)) {
							Program.PrintError($"Unknown constant '{fld.ArraySize}' for '{spec.Name}.{fld.Name}'");
							return false;
						}
						if (@const.Type != ConstantValue.ValueType.Int) {
							Program.PrintError($"Invalid constant type for '{spec.Name}.{fld.Name}'");
							return false;
						}
						arraySize = (uint)@const.ValueInt;
					}
					else {
						Program.PrintError($"Unknown array size '{fld.ArraySize}' for '{spec.Name}.{fld.Name}'");
						return false;
					}
				}

				// Get fixed status
				bool fix = false;
				if (fld.ArraySize is not null) {
					fix = NameHelper.IsFixedBufferType(fType);
				}

				// Add
				fields.Add(new(fName, fType, fld.PtrDepth, arraySize, fix));
			}
			
			// Return
			type = new(spec, fields, sType);
			return true;
		}
	}
}
