/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.InteropServices;

namespace Vk
{
	/// <summary>
	/// Represents VkBool32, a 4-byte type that tolds true/false values.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Bool32 : IEquatable<Bool32>, IEquatable<bool>
	{
		/// <summary>
		/// The constant for the true value.
		/// </summary>
		public static readonly Bool32 True = new Bool32(1);
		/// <summary>
		/// The constant for the false value.
		/// </summary>
		public static readonly Bool32 False = new Bool32(0);

		#region Fields
		/// <summary>
		/// The underlying value of the boolean, zero is false, non-zero is true.
		/// </summary>
		public uint Value
		{
			readonly get => _value;
			set => _value = (value >= 1 ? 1 : 0);
		}
		private uint _value;
		#endregion // Fields

		/// <summary>
		/// Constructs a new boolean object.
		/// </summary>
		/// <param name="value">The integer value of the boolean. Zero is false, non-zero is true.</param>
		public Bool32(uint value) => _value = (value >= 1 ? 1 : 0);
		/// <summary>
		/// Constructs a new boolean object.
		/// </summary>
		/// <param name="value">The boolean value to assign.</param>
		public Bool32(bool value) => _value = value ? 1 : 0;

		#region Overrides
		readonly bool IEquatable<Bool32>.Equals(Bool32 other) => other.Value == Value;

		readonly bool IEquatable<bool>.Equals(bool other) => other == (Value != 0);

		public readonly override bool Equals(object? obj) => (obj is Bool32 b32) && (b32.Value == Value);

		public readonly override int GetHashCode() => (int)Value;

		public readonly override string ToString() => (Value == 0) ? "false" : "true";
		#endregion // Overrides

		#region Casting
		public static implicit operator bool (in Bool32 b) => b.Value != 0;
		
		public static implicit operator uint (in Bool32 b) => b.Value;

		public static implicit operator int (in Bool32 b) => (int)b.Value;

		public static implicit operator Bool32 (bool b) => new Bool32(b);

		public static explicit operator Bool32 (uint b) => new Bool32(b);

		public static explicit operator Bool32 (int b) => new Bool32((uint)b);
		#endregion // Casting
	}
}
