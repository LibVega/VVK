/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Gen
{
	// Processed version of ConstantSpec
	public sealed class ConstantValue
	{
		// Constant types
		public enum ValueType { Int, Long, Float };

		#region Fields
		// The spec for this constant
		public readonly ConstantSpec Spec;

		// The constant name as it appears in the constant class
		public readonly string Name;
		// If the constant is an integer value (false implies float)
		public readonly ValueType Type;

		// The constant values
		public readonly ulong ValueInt;
		public readonly float ValueFloat;
		#endregion // Fields

		private ConstantValue(ConstantSpec spec, uint value)
		{
			Spec = spec;
			Name = spec.Name.Substring("VK_".Length);
			Type = ValueType.Int;
			ValueInt = value;
		}

		private ConstantValue(ConstantSpec spec, float value)
		{
			Spec = spec;
			Name = spec.Name.Substring("VK_".Length);
			Type = ValueType.Float;
			ValueFloat = value;
		}

		private ConstantValue(ConstantSpec spec, ulong value)
		{
			Spec = spec;
			Name = spec.Name.Substring("VK_".Length);
			Type = ValueType.Long;
			ValueInt = value;
		}

		// Try process
		public static bool TryProcess(ConstantSpec spec, out ConstantValue? value)
		{
			value = null;

			// Parse value
			if (spec.Value[^1] == 'f') {
				if (!Single.TryParse(spec.Value.Substring(0, spec.Value.Length - 1), NumberStyles.Float, null, 
						out var val)) {
					Program.PrintError($"Constant '{spec.Name}' has invalid float value '{spec.Value}'");
					return false;
				}
				value = new(spec, val);
				return true;
			}
			else if (Char.IsDigit(spec.Value[0])) {
				if (!UInt32.TryParse(spec.Value, out var val)) {
					Program.PrintError($"Constant '{spec.Name}' has invalid integer value '{spec.Value}'");
					return false;
				}
				value = new(spec, val);
				return true;
			}
			else if (spec.Value.Contains("~0U")) {
				var isLong = spec.Value.Contains("LL");
				var val = UInt64.MaxValue;
				var minusIdx = spec.Value.IndexOf('-');
				if (minusIdx != -1) { // Some are UINT64_MAX minus some value
					var digitCount = spec.Value.Skip(minusIdx + 1).Count(ch => Char.IsDigit(ch));
					if (!UInt32.TryParse(spec.Value.AsSpan(minusIdx + 1, digitCount), out var minusVal)) {
						Program.PrintError($"Cannot parse value '{spec.Value}' for constant '{spec.Name}'");
						return false;
					}
					value = isLong ? new(spec, val - minusVal) : new(spec, (uint)val - minusVal);
				}
				else {
					value = isLong ? new(spec, val) : new(spec, (uint)val);
				}
				return true;
			}
			else {
				Program.PrintError($"Failed to parse value '{spec.Value}' for constant '{spec.Name}'");
				return false;
			}
		}
	}
}
