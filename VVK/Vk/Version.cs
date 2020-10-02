/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.InteropServices;

namespace VVK.Vk
{
	/// <summary>
	/// Wraps a version value (compatible with VK_MAKE_VERSION).
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct Version : IEquatable<Version>, IComparable<Version>
	{
		/// <summary>
		/// API version 1.0.0.
		/// </summary>
		public static readonly Version VK_VERSION_1_0 = new Version(1, 0, 0);
		/// <summary>
		/// API version 1.1.0.
		/// </summary>
		public static readonly Version VK_VERSION_1_1 = new Version(1, 1, 0);
		/// <summary>
		/// API version 1.2.0.
		/// </summary>
		public static readonly Version VK_VERSION_1_2 = new Version(1, 2, 0);

		#region Fields
		/// <summary>
		/// The packed version value.
		/// </summary>
		[FieldOffset(0)] public readonly uint Value;

		/// <summary>
		/// Gets the major version number.
		/// </summary>
		public readonly uint Major => Value >> 22;
		/// <summary>
		/// Gets the minor version number.
		/// </summary>
		public readonly uint Minor => (Value >> 12) & 0x3FF;
		/// <summary>
		/// Gets the revision version number.
		/// </summary>
		public readonly uint Revision => Value & 0xFFF;
		#endregion // Fields

		/// <summary>
		/// Creates a new version value from the given version components.
		/// </summary>
		/// <param name="major">The major version number.</param>
		/// <param name="minor">The minor version number.</param>
		/// <param name="revision">The revision version number.</param>
		public Version(uint major, uint minor, uint revision) => Value = (major << 22) | (minor << 12) | revision;
		/// <summary>
		/// Creates a new version from an existing packed version value.
		/// </summary>
		/// <param name="value">The packed version value.</param>
		public Version(uint value) => Value = value;

		#region Overrides
		readonly bool IEquatable<Version>.Equals(Version other) => other.Value == Value;

		readonly int IComparable<Version>.CompareTo(Version other) => (int)(other.Value - Value);

		public readonly override bool Equals(object? obj) => (obj is Version ver) && (ver.Value == Value);

		public readonly override int GetHashCode() => (int)Value;

		public readonly override string ToString() => $"{Major}.{Minor}.{Revision}";
		#endregion // Overrides

		#region Operators
		public static bool operator == (in Version l, in Version r) => l.Value == r.Value;
		public static bool operator != (in Version l, in Version r) => l.Value != r.Value;
		public static bool operator < (in Version l, in Version r) => l.Value < r.Value;
		public static bool operator <= (in Version l, in Version r) => l.Value <= r.Value;
		public static bool operator > (in Version l, in Version r) => l.Value > r.Value;
		public static bool operator >= (in Version l, in Version r) => l.Value >= r.Value;

		public static bool operator == (in Version l, uint r) => l.Value == r;
		public static bool operator != (in Version l, uint r) => l.Value != r;

		public static bool operator == (uint l, in Version r) => l == r.Value;
		public static bool operator != (uint l, in Version r) => l != r.Value;

		public static implicit operator uint (in Version v) => v.Value;
		#endregion // Operators
	}
}
