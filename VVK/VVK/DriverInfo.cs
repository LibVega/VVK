/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Vulkan.VVK
{
	/// <summary>
	/// Represents a fixed string buffer of size <see cref="VkConstants.MAX_DRIVER_INFO_SIZE"/>.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = (int)VkConstants.MAX_DRIVER_INFO_SIZE)]
	public unsafe struct DriverInfo : IFixedString
	{
		/// <summary>
		/// The size of the fixed string buffer in this type.
		/// </summary>
		public const int SIZE = (int)VkConstants.MAX_DRIVER_INFO_SIZE;

		#region Fields
		uint IFixedString.Capacity => SIZE - 1;

		/// <summary>
		/// Gets the byte (character) at the index.
		/// </summary>
		/// <param name="index">The character index to get.</param>
		public readonly byte this [int index] => 
			(index >= 0 && index <= SIZE) ? _data[index] : throw new ArgumentOutOfRangeException();

		[FieldOffset(0)] private fixed byte _data[SIZE];
		#endregion // Fields

		#region Ctor
		/// <summary>
		/// Creates a new fixed string by copying the string contents into the buffer.
		/// </summary>
		/// <param name="str">The string to copy into the buffer.</param>
		public DriverInfo(string str)
		{
			var len = Math.Min(str.Length, SIZE - 1);
			var strData = Encoding.UTF8.GetBytes(str);

			for (int i = 0; i < len; ++i) {
				_data[i] = strData[i];
			}
			for (int i = len; len < SIZE; ++i) {
				_data[i] = 0; // Fill rest of buffer will null terminator
			}
		}
		/// <summary>
		/// Creates a new fixed string by copying existing string data into the buffer.
		/// </summary>
		/// <param name="str">The existing string data to copy.</param>
		/// <param name="length">The length of the existing string data.</param>
		public DriverInfo(byte* str, int length)
		{
			var len = Math.Min(length, SIZE - 1);

			for (int i = 0; i < len; ++i) {
				_data[i] = str[i];
			}
			for (int i = len; len < SIZE; ++i) {
				_data[i] = 0; // Fill rest of buffer will null terminator
			}
		}
		#endregion // Ctor

		public override int GetHashCode() => ((IFixedString)this).GetHashCode();
		public override bool Equals(object? obj) => ((IFixedString)this).Equals(obj);

		ref byte IFixedString.GetPinnableReference() => ref _data[0];

		#region Operators
		public static bool operator == (in DriverInfo str1, in DriverInfo str2)
		{
			fixed (byte* ptr1 = str1._data, ptr2 = str2._data) {
				return VVK.NativeString.Strcmp(ptr1, ptr2) == 0;
			}
		}
		
		public static bool operator != (in DriverInfo str1, in DriverInfo str2)
		{
			fixed (byte* ptr1 = str1._data, ptr2 = str2._data) {
				return VVK.NativeString.Strcmp(ptr1, ptr2) != 0;
			}
		}

		public static bool operator == (in DriverInfo str1, string? str2)
		{
			fixed (byte* ptr1 = str1._data) {
				fixed (char* ptr2 = str2) {
					return VVK.NativeString.Strcmp(ptr1, (byte*)ptr2) == 0;
				}
			}
		}
		
		public static bool operator != (in DriverInfo str1, string? str2)
		{
			fixed (byte* ptr1 = str1._data) {
				fixed (char* ptr2 = str2) {
					return VVK.NativeString.Strcmp(ptr1, (byte*)ptr2) != 0;
				}
			}
		}

		public static bool operator == (string? str1, in DriverInfo str2)
		{
			fixed (byte* ptr2 = str2._data) {
				fixed (char* ptr1 = str1) {
					return VVK.NativeString.Strcmp(ptr2, (byte*)ptr1) == 0;
				}
			}
		}
		
		public static bool operator != (string? str1, in DriverInfo str2)
		{
			fixed (byte* ptr2 = str2._data) {
				fixed (char* ptr1 = str1) {
					return VVK.NativeString.Strcmp(ptr2, (byte*)ptr1) != 0;
				}
			}
		}
		#endregion // Operators

		#region Casting
		public static implicit operator DriverInfo (string str) => new DriverInfo(str);

		public static implicit operator string (in DriverInfo fs) => fs.ToString() ?? String.Empty;
		#endregion // Casting
	}
}
