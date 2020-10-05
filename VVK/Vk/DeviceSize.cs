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
	/// Represents VkDeviceSize, which is an 8-byte unsigned integer giving a memory size in bytes.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct DeviceSize : IEquatable<DeviceSize>, IComparable<DeviceSize>, IEquatable<ulong>, IComparable<ulong>
	{
		/// <summary>
		/// Constant size representing zero bytes.
		/// </summary>
		public static readonly DeviceSize Zero = new DeviceSize(0);
		/// <summary>
		/// Special size indicating to Vulkan that the entirety of a memory resoure be considered.
		/// </summary>
		public static readonly DeviceSize WholeSize = new DeviceSize(Constants.WHOLE_SIZE);

		#region Fields
		/// <summary>
		/// The size value.
		/// </summary>
		[FieldOffset(0)] public ulong Value;
		#endregion // Fields

		/// <summary>
		/// Constructs a new size object.
		/// </summary>
		/// <param name="size">The memory size, in bytes.</param>
		public DeviceSize(ulong size) => Value = size;

		#region Creation
		/// <summary>
		/// Create a new DeviceSize representing the number of bytes.
		/// </summary>
		/// <param name="count">The memory size in bytes.</param>
		public static DeviceSize Bytes(ulong count) => new DeviceSize(count);
		/// <summary>
		/// Create a new DeviceSize representing the number of kilobytes.
		/// </summary>
		/// <param name="count">The memory size in kilobytes.</param>
		public static DeviceSize Kilobytes(double count) => new DeviceSize((ulong)(count * 1024));
		/// <summary>
		/// Create a new DeviceSize representing the number of megabytes.
		/// </summary>
		/// <param name="count">The memory size in megabytes.</param>
		public static DeviceSize Megabytes(double count) => new DeviceSize((ulong)(count * 1024 * 1024));
		/// <summary>
		/// Create a new DeviceSize representing the number of gigabytes.
		/// </summary>
		/// <param name="count">The memory size in gigabytes.</param>
		public static DeviceSize Gigabytes(double count) => new DeviceSize((ulong)(count * 1024 * 1024 * 1024));
		#endregion // Creation

		#region Overrides
		readonly bool IEquatable<DeviceSize>.Equals(DeviceSize other) => other.Value == Value;

		readonly bool IEquatable<ulong>.Equals(ulong other) => other == Value;

		readonly int IComparable<DeviceSize>.CompareTo(DeviceSize other) =>
			(Value < other.Value) ? -1 : (Value == other.Value) ? 0 : 1;

		readonly int IComparable<ulong>.CompareTo(ulong other) =>
			(Value < other) ? -1 : (Value == other) ? 0 : 1;

		public readonly override bool Equals(object? obj) =>
			((obj is DeviceSize ds) && (ds.Value == Value)) || ((obj is ulong size) && (size == Value));

		public readonly override int GetHashCode() => (int)(Value >> 32) ^ (int)(Value & 0xFFFFFFFF);

		public readonly override string ToString() => Value.ToString();
		#endregion // Overrides

		#region Operators
		public static bool operator == (DeviceSize l, DeviceSize r) => l.Value == r.Value;

		public static bool operator != (DeviceSize l, DeviceSize r) => l.Value != r.Value;

		public static bool operator < (DeviceSize l, DeviceSize r) => l.Value < r.Value;

		public static bool operator > (DeviceSize l, DeviceSize r) => l.Value > r.Value;

		public static bool operator <= (DeviceSize l, DeviceSize r) => l.Value <= r.Value;

		public static bool operator >= (DeviceSize l, DeviceSize r) => l.Value >= r.Value;

		public static bool operator == (ulong l, DeviceSize r) => l == r.Value;

		public static bool operator != (ulong l, DeviceSize r) => l != r.Value;

		public static bool operator < (ulong l, DeviceSize r) => l < r.Value;

		public static bool operator > (ulong l, DeviceSize r) => l > r.Value;

		public static bool operator <= (ulong l, DeviceSize r) => l <= r.Value;

		public static bool operator >= (ulong l, DeviceSize r) => l >= r.Value;

		public static bool operator == (DeviceSize l, ulong r) => l.Value == r;

		public static bool operator != (DeviceSize l, ulong r) => l.Value != r;

		public static bool operator < (DeviceSize l, ulong r) => l.Value < r;

		public static bool operator > (DeviceSize l, ulong r) => l.Value > r;

		public static bool operator <= (DeviceSize l, ulong r) => l.Value <= r;

		public static bool operator >= (DeviceSize l, ulong r) => l.Value >= r;
		#endregion // Operators

		#region Casting
		public static implicit operator DeviceSize (ulong size) => new DeviceSize(size);

		public static explicit operator DeviceSize (long size) => new DeviceSize((ulong)size);

		public static implicit operator ulong (DeviceSize size) => size.Value;
		#endregion // Casting
	}
}
