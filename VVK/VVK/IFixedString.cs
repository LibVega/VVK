/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.InteropServices;

namespace Vulkan.VVK
{
	/// <summary>
	/// Base interface for all fixed string types (those with <c>fixed char FIELD[]</c> fields).
	/// </summary>
	public unsafe interface IFixedString : IEquatable<IFixedString>, IEquatable<string>
	{
		/// <summary>
		/// Gets the total character capacity of the string (not including reserved null terminator space).
		/// </summary>
		uint Capacity { get; }
		/// <summary>
		/// Gets the C# string object representation of this data.
		/// </summary>
		string StringValue => ToString() ?? String.Empty;
		/// <summary>
		/// Gets the length of the current string stored in this buffer.
		/// </summary>
		uint Length => GetLength();

		/// <summary>
		/// Calculates the length of the string stored in this buffer.
		/// </summary>
		uint GetLength()
		{
			fixed (byte* data = this) {
				return (uint)NativeString.Strlen(data, Capacity);
			}
		}

		/// <summary>
		/// Gets a string representation of the fixed string.
		/// </summary>
		string? ToString()
		{
			fixed (byte* data = this) {
				return Marshal.PtrToStringUTF8(new IntPtr(data), (int)GetLength());
			}
		}

		/// <summary>
		/// Gets a hashcode for the string.
		/// </summary>
		int GetHashCode()
		{
			var len = Length;
			int hash = 0;
			int shift = 0;
			fixed (byte* data = this) {
				for (int i = 0; i < len; ++i) {
					hash ^= (data[i] << shift);
					shift = (shift + 8) % 32;
				}
			}
			return hash;
		}

		bool Equals(object? obj) =>
			((obj is IFixedString fs) && Equals(fs)) || ((obj is string str) && Equals(str));

		bool IEquatable<IFixedString>.Equals(IFixedString? other)
		{
			if (other is null) return false;
			fixed (byte* thisData = this, otherData = other) {
				return NativeString.Strcmp(thisData, otherData) == 0;
			}
		}

		bool IEquatable<string>.Equals(string? other)
		{
			if (other is null) return false;
			fixed (byte* data = this) {
				return NativeString.Strcmp(data, other) == 0;
			}
		}

		/// <summary>
		/// Gets a reference object that can be used to pin the string data in this type.
		/// </summary>
		ref byte GetPinnableReference();
	}
}
