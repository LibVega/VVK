/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Vk
{
	/// <summary>
	/// Wrapper type around a list of unmanaged string memory blocks. This type emulates the byte** expected by C/C++
	/// interop for lists of strings.
	/// </summary>
	public unsafe sealed class NativeStringList : IDisposable
	{
		/// <summary>
		/// The default capacity for the list.
		/// </summary>
		public const uint DEFAULT_CAPACITY = 32;

		#region Fields
		/// <summary>
		/// The pointer-to-pointer data block for the string list.
		/// </summary>
		public byte** Data { get; private set; } = null;
		/// <summary>
		/// The current number of strings in the list.
		/// </summary>
		public uint Count { get; private set; } = 0;
		/// <summary>
		/// The current list capacity.
		/// </summary>
		public uint Capacity { get; private set; } = 0;
		#endregion // Fields

		/// <summary>
		/// Creates a new empty native string list with the given capacity.
		/// </summary>
		/// <param name="capacity">The initial capacity of the list.</param>
		public NativeStringList(uint capacity = DEFAULT_CAPACITY)
		{
			Data = InitPointerBlock(Math.Max(capacity, 1));
			Count = 0;
			Capacity = Math.Max(capacity, 1);
		}
		/// <summary>
		/// Creates a new string list by copying the existing set of strings into unmanaged memory.
		/// </summary>
		/// <param name="strings">The set of strings to copy into the list.</param>
		public NativeStringList(IEnumerable<string> strings)
			: this((uint)strings.Count())
		{
			AddRange(strings);
		}
		~NativeStringList()
		{
			dispose();
		}

		/// <summary>
		/// Adds the string to the list.
		/// </summary>
		/// <param name="str">The string to add.</param>
		public void Add(string str)
		{
			// Reallocate if full
			if (Count == Capacity) {
				Data = ReallocPointerBlock(Data, Count, Capacity * 2);
				Capacity *= 2;
			}

			// Add unmanaged string
			var strdata = stackalloc byte[str.Length * 4]; // Worst-case length
			var len = Encoding.UTF8.GetBytes(str, new Span<byte>(strdata, str.Length * 4));
			var nstr = (byte*)Marshal.AllocHGlobal(len + 1).ToPointer();
			System.Buffer.MemoryCopy(strdata, nstr, len, len);
			nstr[len] = 0;
			Data[Count++] = nstr;
		}

		/// <summary>
		/// Adds the set of strings to the string list.
		/// </summary>
		/// <param name="strings">The set of strings to add.</param>
		public void AddRange(IEnumerable<string> strings)
		{
			foreach (var str in strings) {
				Add(str);
			}
		}

		#region IDisposable
		public void Dispose()
		{
			dispose();
			GC.SuppressFinalize(this);
		}

		private void dispose()
		{
			if (Data != null) {
				for (uint i = 0; i < Count; ++i) {
					Marshal.FreeHGlobal(new IntPtr(Data[i]));
				}
				Marshal.FreeHGlobal(new IntPtr(Data));
				Data = null;
			}
		}
		#endregion // IDisposable

		private static byte** InitPointerBlock(uint capacity)
		{
			var data = (byte**)Marshal.AllocHGlobal((int)capacity * sizeof(byte*)).ToPointer();
			Unsafe.InitBlock(data, 0, capacity * (uint)sizeof(byte*));
			return data;
		}

		private static byte** ReallocPointerBlock(byte** data, uint oldSize, uint newSize)
		{
			var @new = InitPointerBlock(newSize);
			System.Buffer.MemoryCopy(data, @new, newSize * sizeof(byte*), oldSize * sizeof(byte*));
			Marshal.FreeHGlobal(new IntPtr(data));
			return @new;
		}
	}
}
