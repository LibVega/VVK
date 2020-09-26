/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// Represents a processed API constant
	public sealed class ConstantOut
	{
		#region Fields
		// The original spec that the constant was processed from
		public readonly ConstantSpec Spec;
		// The name of the constant
		public readonly string Name;
		// The verbatim typename of the constant
		public readonly string TypeName;
		// The verbatim value of the constant
		public readonly string Value;
		#endregion // Fields

		private ConstantOut(ConstantSpec spec, string name, string type, string value)
		{
			Spec = spec;
			Name = name;
			TypeName = type;
			Value = value;
		}

		// Process
		public static ConstantOut? TryProcess(ConstantSpec spec)
		{
			// Simply remove the "VK_" from the start of the name
			var name = spec.Name.Substring("VK_".Length);

			// Calculate the type
			var type =
				spec.Value.EndsWith("f") ? "float" :
				spec.Value.Contains("0ULL") ? "ulong" :
				spec.Value.Contains("0U") ? "uint" :
				spec.Value.StartsWith('"') ? "string" : "uint";

			// Get the correct value string
			var value = type switch {
				"float" => spec.Value,
				"ulong" => spec.Value.Replace("ULL", "UL"),
				"uint" => spec.Value,
				"string" => spec.Value,
				_ => spec.Value
			};

			return new(spec, name, type, value);
		}
	}
}
