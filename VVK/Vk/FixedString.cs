﻿/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VVK.Vk
{
	/// <summary>
	/// Represents a fixed string buffer of size <see cref="Constants.MAX_PHYSICAL_DEVICE_NAME_SIZE"/>. Technically,
	/// fixed strings in Vulkan can have different sizes, but for now all string constant sizes are the same.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = (int)Constants.MAX_PHYSICAL_DEVICE_NAME_SIZE)]
	public unsafe struct FixedString : IEquatable<FixedString>, IEquatable<string>
	{
		/// <summary>
		/// The size of the fixed string buffer in this type.
		/// </summary>
		public const int SIZE = (int)Constants.MAX_PHYSICAL_DEVICE_NAME_SIZE;

		#region Fields
		/// <summary>
		/// Calculates the length of the current string stored in this buffer.
		/// </summary>
		public readonly int Length => GetLength();
		/// <summary>
		/// Gets the C# string object representation of this data.
		/// </summary>
		public readonly string StringValue => ToString();

		/// <summary>
		/// Gets the byte (character) at the index.
		/// </summary>
		/// <param name="index">The character index to get.</param>
		public readonly byte this [int index] => 
			(index >= 0 && index <= SIZE) ? _data[index] : throw new ArgumentOutOfRangeException();

		[FieldOffset(0)] private fixed byte _data[(int)Constants.MAX_PHYSICAL_DEVICE_NAME_SIZE];
		#endregion // Fields

		/// <summary>
		/// Creates a new fixed string by copying the string contents into the buffer.
		/// </summary>
		/// <param name="str">The string to copy into the buffer.</param>
		public FixedString(string str)
		{
			var len = Math.Min(str.Length, SIZE - 1);
			var strData = Encoding.ASCII.GetBytes(str);

			fixed (byte* ptr = _data) {
				for (int i = 0; i < len; ++i) {
					ptr[i] = strData[i];
				}
				for (int i = len; len < SIZE; ++i) {
					ptr[i] = 0; // Fill rest of buffer will null terminator
				}
			}
		}

		#region Overrides
		readonly bool IEquatable<FixedString>.Equals(FixedString other) => other == this;

		readonly bool IEquatable<string>.Equals(string? other) => (other is not null) && other == this;

		public readonly override bool Equals(object? obj) =>
			((obj is FixedString fs) && (fs == this)) || ((obj is string str) && (str == this));


		public readonly override int GetHashCode() => ToString().GetHashCode();

		public readonly override string ToString()
		{
			fixed (byte* ptr = _data) {
				return Marshal.PtrToStringAnsi(new IntPtr(ptr), Strlen(ptr));
			}
		}
		#endregion // Overrides

		#region Operators
		public static bool operator == (in FixedString str1, in FixedString str2)
		{
			fixed (byte* ptr1 = str1._data, ptr2 = str2._data) {
				return Strcmp(ptr1, ptr2) == 0;
			}
		}
		
		public static bool operator != (in FixedString str1, in FixedString str2)
		{
			fixed (byte* ptr1 = str1._data, ptr2 = str2._data) {
				return Strcmp(ptr1, ptr2) != 0;
			}
		}

		public static bool operator == (in FixedString str1, string? str2)
		{
			fixed (byte* ptr1 = str1._data) {
				fixed (char* ptr2 = str2) {
					return Strcmp(ptr1, (byte*)ptr2) == 0;
				}
			}
		}
		
		public static bool operator != (in FixedString str1, string? str2)
		{
			fixed (byte* ptr1 = str1._data) {
				fixed (char* ptr2 = str2) {
					return Strcmp(ptr1, (byte*)ptr2) != 0;
				}
			}
		}

		public static bool operator == (string? str1, in FixedString str2)
		{
			fixed (byte* ptr2 = str2._data) {
				fixed (char* ptr1 = str1) {
					return Strcmp(ptr2, (byte*)ptr1) == 0;
				}
			}
		}
		
		public static bool operator != (string? str1, in FixedString str2)
		{
			fixed (byte* ptr2 = str2._data) {
				fixed (char* ptr1 = str1) {
					return Strcmp(ptr2, (byte*)ptr1) != 0;
				}
			}
		}
		#endregion // Operators

		#region Casting
		public static implicit operator FixedString (string str) => new FixedString(str);

		public static implicit operator string (in FixedString fs) => fs.ToString();
		#endregion // Casting

		/// <summary>
		/// Calculates the length of the fixed string
		/// </summary>
		/// <returns></returns>
		public readonly int GetLength()
		{
			fixed (byte* ptr = _data) { return Strlen(ptr); }
		}

		#region C-String
		/// <summary>
		/// Returns the length of the c-string by counting to the null terminator (like <c>strlen()</c>).
		/// </summary>
		/// <param name="data">The c-string to find the length of.</param>
		public static int Strlen(byte* data)
		{
			if (data == (byte*)0) {
				return 0;
			}
			int len = 0;
			while ((*(data++) != 0) && (len < SIZE)) ++len;
			return len;
		}

		/// <summary>
		/// Performs an ordinal comparison between the strings, returning 0 if they are equal.
		/// </summary>
		/// <param name="str1">The first string to compare.</param>
		/// <param name="str2">The seconds string to comapre.</param>
		public static int Strcmp(byte* str1, byte* str2)
		{
			if (str1 == (byte*)0 && str2 == (byte*)0) {
				return 0;
			}
			if (str1 == (byte*)0 && str2 != (byte*)0) {
				return Int32.MinValue;
			}
			if (str2 != (byte*)0 && str2 == (byte*)0) {
				return Int32.MaxValue;
			}

			int off = 0;
			while ((str1[off] != 0) && (str1[off] == str2[off])) { ++off; }

			return (int)str1[off] - (int)str2[off];
		}
		#endregion // C-String
	}
}