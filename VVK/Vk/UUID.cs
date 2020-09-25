/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace VVK.Vk
{
	/// <summary>
	/// Represents a universally unique identifier in a fixed buffer of size <see cref="Constants.UUID_SIZE"/>.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = (int)Constants.UUID_SIZE)]
	public unsafe struct UUID : IEquatable<UUID>, IComparable<UUID>
	{
		/// <summary>
		/// The number of bytes in the UUID value.
		/// </summary>
		public const int SIZE = (int)Constants.UUID_SIZE;

		#region Fields
		/// <summary>
		/// The value bytes of the UUID.
		/// </summary>
		[FieldOffset(0)] public fixed byte Values[SIZE];

		// Private field for faster comparisons
		[FieldOffset(0)] private fixed int _intValues[SIZE / sizeof(int)];

		/// <summary>
		/// Gets or sets the byte at the given UUID byte index.
		/// </summary>
		/// <param name="index">The index of the byte to get/set.</param>
		public byte this[int index]
		{
			readonly get => (index >= 0) && (index < SIZE) ? Values[index] : throw new ArgumentOutOfRangeException();
			set => Values[(index >= 0) && (index < SIZE) ? index : throw new ArgumentOutOfRangeException()] = value;
		}
		#endregion // Fields

		/// <summary>
		/// Construct a new UUID from the list of values, filling in with zeros if the array is not large enough.
		/// </summary>
		/// <param name="values">The UUID value bytes.</param>
		public UUID(params byte[] values)
		{
			int len = Math.Min(values.Length, SIZE);
			for (int i = 0; i < len; ++i) {
				Values[i] = values[i];
			}
			for (int i = len; i < SIZE; ++i) {
				Values[i] = 0;
			}
		}
		/// <summary>
		/// Construct a new UUID by copying the list of values, filling with zero if necessary.
		/// </summary>
		/// <param name="values">The UUID bytes to copy.</param>
		/// <param name="length">The length of the bytes array.</param>
		public UUID(byte* values, int length)
		{
			int len = Math.Min(length, SIZE);
			for (int i = 0; i < len; ++i) {
				Values[i] = values[i];
			}
			for (int i = len; i < SIZE; ++i) {
				Values[i] = 0;
			}
		}

		#region Overrides
		readonly bool IEquatable<UUID>.Equals(UUID other) => Compare(other, this) == 0;

		readonly int IComparable<UUID>.CompareTo(UUID other) => Compare(other, this);

		public readonly override bool Equals(object? obj) => (obj is UUID uuid) && (Compare(uuid, this) == 0);

		public readonly override int GetHashCode()
		{
			const int LEN = SIZE / sizeof(int);
			int hash = 0;
			for (int i = 0; i < LEN; ++i) {
				hash ^= _intValues[i];
			}
			return hash;
		}

		public readonly override string ToString()
		{
			const int LEN = SIZE / sizeof(int);

			StringBuilder builder = new(SIZE);
			for (int i = 0; i < LEN; ++i) {
				builder.Append($"{_intValues[i]:X8}");
			}
			return builder.ToString();
		}
		#endregion // Overrides

		#region Operators
		public static bool operator == (in UUID l, in UUID r) => Compare(l, r) == 0;

		public static bool operator != (in UUID l, in UUID r) => Compare(l, r) != 0;
		#endregion // Operators

		/// <summary>
		/// Tries to parse the UUID string into a UUID object, throwing an exception if it cannot.
		/// </summary>
		/// <param name="str">The UUID hex-encoded string to parse.</param>
		public static UUID Parse(string str)
		{
			if (!TryParse(str, out var uuid)) {
				throw new ArgumentException("The string was not a valid UUID string.", nameof(str));
			}
			return uuid;
		}
		/// <summary>
		/// Tries to parse the UUID string into a UUID object, returning if it was successful.
		/// </summary>
		/// <param name="str">The UUID hex-encoded string to parse.</param>
		/// <param name="uuid">The output uuid object.</param>
		[SkipLocalsInit]
		public static bool TryParse(string str, out UUID uuid)
		{
			const int LEN = SIZE / sizeof(int);

			uuid = new();
			if (str.Length != (SIZE * 2)) { // 2 hex characters per byte
				return false;
			}

			var values = stackalloc int[LEN];
			for (int i = 0; i < LEN; ++i) {
				if (Int32.TryParse(str.AsSpan().Slice(i * 8, 8), NumberStyles.HexNumber, null, out values[i])) {
					return false;
				}
			}

			uuid = new((byte*)values, SIZE);
			return true;
		}

		/// <summary>
		/// Performs an ordinal comparison, as if the UUID values were character strings.
		/// </summary>
		/// <param name="l">The first UUID.</param>
		/// <param name="r">The second UUID.</param>
		public static int Compare(in UUID l, in UUID r)
		{
			const int LEN = SIZE / sizeof(int);

			for (int i = 0; i < LEN; ++i) {
				var diff = l._intValues[i] - r._intValues[i];
				if (diff != 0) {
					return diff;
				}
			}
			return 0;
		}
	}
}
