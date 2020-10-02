/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace VVK
{
	/// <summary>
	/// IDisposable-friendly wrapper around an unmanaged pointer containing native string data. This type directly
	/// exposes the <c>byte*</c> handle for the string data.
	/// </summary>
	[DebuggerDisplay("String = {ToString()}")]
	public unsafe sealed class NativeString : IDisposable, IEquatable<NativeString>, IEquatable<string>,
		IComparable<NativeString>, IComparable<string>
	{
		/// <summary>
		/// Constant empty native string.
		/// </summary>
		public static readonly NativeString Empty = new();

		#region Fields
		/// <summary>
		/// The pointer to the raw string data.
		/// </summary>
		public byte* Data { get; private set; } = null;

		/// <summary>
		/// The length of the string in bytes.
		/// </summary>
		public readonly uint Length;

		/// <summary>
		/// Gets if the string is empty (length 0).
		/// </summary>
		public bool IsEmpty => Length == 0;
		#endregion // Fields

		/// <summary>
		/// Creates a new empty string of length 0 (<see cref="Data"/> contains a single null-terminator).
		/// </summary>
		public NativeString()
		{
			Data = (byte*)Marshal.AllocHGlobal(1).ToPointer();
			Data[0] = 0;
			Length = 0;
		}
		/// <summary>
		/// Creates a new native string by copying existing string data.
		/// </summary>
		/// <param name="data">The existing string data of arbitrary length.</param>
		public NativeString(byte* data)
		{
			var len = Strlen(data);
			Data = (byte*)Marshal.AllocHGlobal(len + 1).ToPointer();
			Buffer.MemoryCopy(data, Data, len, len);
			Data[len] = 0;
			Length = (uint)len;
		}
		/// <summary>
		/// Creates a new native string by copying existing string data.
		/// </summary>
		/// <param name="data">The existing string data of known fixed length.</param>
		/// <param name="length">The length of the existing string data.</param>
		public NativeString(byte* data, uint length)
		{
			Data = (byte*)Marshal.AllocHGlobal((int)length + 1).ToPointer();
			Buffer.MemoryCopy(data, Data, (int)length, (int)length);
			Data[(int)length] = 0;
			Length = length;
		}
		/// <summary>
		/// Creates a new native string by copying and existing string object.
		/// </summary>
		/// <param name="str">The string object to copy.</param>
		public NativeString(string str)
		{
			var strdata = stackalloc byte[str.Length * 4]; // Worst-case length
			var len = Encoding.UTF8.GetBytes(str, new Span<byte>(strdata, str.Length * 4));
			Data = (byte*)Marshal.AllocHGlobal(len + 1).ToPointer();
			Buffer.MemoryCopy(strdata, Data, len, len);
			Data[len] = 0;
			Length = (uint)len;
		}
		~NativeString()
		{
			dispose();
		}

		#region Overrides
		bool IEquatable<NativeString>.Equals(NativeString? other) => other == this;

		bool IEquatable<string>.Equals(string? other) => other == this;

		int IComparable<NativeString>.CompareTo(NativeString? other) => Strcmp(Data, (other ?? Empty).Data);

		int IComparable<string>.CompareTo(string? other) => Strcmp(Data, other ?? String.Empty);

		public override bool Equals(object? obj) => 
			((obj is NativeString ns) && (ns == this)) || ((obj is string str) && (str == this));

		public override int GetHashCode() => (int)Length; // Not ideal

		public override string ToString() => Marshal.PtrToStringAnsi(new IntPtr(Data)) ?? String.Empty;
		#endregion // Overrides

		#region Operators
		public static bool operator == (NativeString? l, NativeString? r) =>
			(l is null == r is null) && ((l is null) || (l!.Data == r!.Data) || (l!.Length == r!.Length && Strcmp(l.Data, r.Data) == 0));
		public static bool operator != (NativeString? l, NativeString? r) =>
			(l is null != r is null) || ((l is not null) && (l!.Data != r!.Data) && (l!.Length != r!.Length || Strcmp(l.Data, r.Data) != 0));

		public static bool operator == (NativeString? l, string? r) =>
			(l is null == r is null) && ((l is null) || (l!.Length == r!.Length && Strcmp(l!.Data, r!) == 0));
		public static bool operator != (NativeString? l, string? r) =>
			(l is null != r is null) || ((l is not null) && (l!.Length != r!.Length || Strcmp(l!.Data, r!) != 0));

		public static bool operator == (string? l, NativeString? r) =>
			(l is null == r is null) && ((l is null) || (l!.Length == r!.Length && Strcmp(r!.Data, l!) == 0));
		public static bool operator != (string? l, NativeString? r) =>
			(l is null != r is null) || ((l is not null) && (l!.Length != r!.Length || Strcmp(r!.Data, l!) != 0));

		public static implicit operator string (NativeString? str) => str?.ToString() ?? String.Empty;
		#endregion // Operators

		#region IDisposable
		public void Dispose()
		{
			dispose();
			GC.SuppressFinalize(this);
		}

		private void dispose()
		{
			if (Data != null) {
				Marshal.FreeHGlobal(new IntPtr(Data));
				Data = null;
			}
		}
		#endregion // IDisposable

		#region C-String
		/// <summary>
		/// Returns the length of the c-string by counting to the null terminator (like <c>strlen()</c>).
		/// </summary>
		/// <param name="data">The c-string to find the length of.</param>
		/// <param name="maxLength">The maximum length of the string, after which to stop searching.</param>
		public static int Strlen(byte* data, uint maxLength = UInt32.MaxValue)
		{
			if (data == (byte*)0) {
				return 0;
			}
			int len = 0;
			while ((*(data++) != 0) && (len < maxLength)) ++len;
			return len;
		}

		/// <summary>
		/// Performs an ordinal comparison between the strings, returning 0 if they are equal.
		/// </summary>
		/// <param name="str1">The first string to compare.</param>
		/// <param name="str2">The seconds string to comapre.</param>
		public static int Strcmp(byte* str1, byte* str2)
		{
			if (str1 == null && str2 == null) {
				return 0;
			}
			if (str1 == null && str2 != null) {
				return Int32.MinValue;
			}
			if (str2 != null && str2 == null) {
				return Int32.MaxValue;
			}

			int off = 0;
			while ((str1[off] != 0) && (str1[off] == str2[off])) { ++off; }

			return (int)str1[off] - (int)str2[off];
		}

		/// <summary>
		/// Performs an ordinal comparison between native string data and a string object, returning 0 if equal.
		/// </summary>
		/// <param name="str1">The raw string data.</param>
		/// <param name="str2">The string object.</param>
		public static int Strcmp(byte* str1, string str2)
		{
			using var str2native = new NativeString(str2);
			return Strcmp(str1, str2native.Data);
		}
		#endregion // C-String
	}
}
