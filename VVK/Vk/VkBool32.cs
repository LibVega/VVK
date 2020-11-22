/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.InteropServices;

namespace Vulkan
{
	/// <summary>
	/// Represents VkBool32, a 4-byte type that tolds true/false values.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct VkBool32 : IEquatable<VkBool32>, IEquatable<bool>
	{
		/// <summary>
		/// The constant for the true value.
		/// </summary>
		public static readonly VkBool32 True = new VkBool32(1);
		/// <summary>
		/// The constant for the false value.
		/// </summary>
		public static readonly VkBool32 False = new VkBool32(0);

		#region Fields
		/// <summary>
		/// The underlying value of the boolean, zero is false, non-zero is true.
		/// </summary>
		public uint Value
		{
			readonly get => _value;
			set => _value = (value >= 1 ? 1 : 0);
		}
		[FieldOffset(0)] private uint _value;
		#endregion // Fields

		/// <summary>
		/// Constructs a new boolean object.
		/// </summary>
		/// <param name="value">The integer value of the boolean. Zero is false, non-zero is true.</param>
		public VkBool32(uint value) => _value = (value >= 1 ? 1 : 0);
		/// <summary>
		/// Constructs a new boolean object.
		/// </summary>
		/// <param name="value">The boolean value to assign.</param>
		public VkBool32(bool value) => _value = value ? 1 : 0;

		#region Overrides
		readonly bool IEquatable<VkBool32>.Equals(VkBool32 other) => other.Value == Value;

		readonly bool IEquatable<bool>.Equals(bool other) => other == (Value != 0);

		public readonly override bool Equals(object? obj) => (obj is VkBool32 b32) && (b32.Value == Value);

		public readonly override int GetHashCode() => (int)Value;

		public readonly override string ToString() => (Value == 0) ? "false" : "true";
		#endregion // Overrides

		#region Operators
		public static bool operator == (in VkBool32 l, in VkBool32 r) => l.Value == r.Value;

		public static bool operator != (in VkBool32 l, in VkBool32 r) => l.Value != r.Value;
		#endregion // Operators

		#region Casting
		public static implicit operator bool (in VkBool32 b) => b.Value != 0;
		
		public static implicit operator uint (in VkBool32 b) => b.Value;

		public static implicit operator int (in VkBool32 b) => (int)b.Value;

		public static implicit operator VkBool32(bool b) => new VkBool32(b);

		public static explicit operator VkBool32(uint b) => new VkBool32(b);

		public static explicit operator VkBool32(int b) => new VkBool32((uint)b);
		#endregion // Casting
	}
}
