/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vulkan
{
	/// <summary>
	/// Represents an opaque but typed handle to an API object.
	/// </summary>
	/// <typeparam name="T">The handle class type that matches to this opaque type.</typeparam>
	[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 8)]
	public unsafe partial struct VulkanHandle<T> : IEquatable<VulkanHandle<T>>
		where T : class, IVulkanHandle<T>
	{
		/// <summary>
		/// Special constant <c>null</c> value representing an uninitialized handle.
		/// </summary>
		public static readonly VulkanHandle<T> Null = new(null);

		// Type name
		private static readonly string HANDLE_TYPE_NAME = typeof(T).Name;

		#region Fields
		// The raw handle
		private readonly void* _handle;

		/// <summary>
		/// The opaque handle value as a raw pointer.
		/// </summary>
		public readonly void* RawHandle => _handle;
		/// <summary>
		/// The opaque handle value as a 64-bit integer.
		/// </summary>
		public readonly ulong LongHandle => (ulong)_handle;
		/// <summary>
		/// The opaque handle value as an IntPtr.
		/// </summary>
		public readonly IntPtr PtrHandle => new IntPtr(_handle);

		/// <summary>
		/// Gets if the handle is not null.
		/// </summary>
		public readonly bool IsValid => _handle != null;
		#endregion // Fields

		/// <summary>
		/// Creates a new handle from the raw pointer. Validity is assumed, and typing is not checked.
		/// </summary>
		/// <param name="handle">The raw value to construct the typed handle from.</param>
		public VulkanHandle(void* handle) => _handle = handle;

		/// <summary>
		/// Creates a new handle from the raw integer value. Validity is assumed, and typing is not checked.
		/// </summary>
		/// <param name="handle">The raw value to construct the typed handle from.</param>
		public VulkanHandle(ulong handle) => _handle = (void*)handle;

		/// <summary>
		/// Creates a new handle from the IntPtr value. Validity is assumed, and typing is not checked.
		/// </summary>
		/// <param name="handle">The value to construct the typed handle from.</param>
		public VulkanHandle(IntPtr handle) => _handle = (void*)handle;

		#region Overrides
		public override readonly int GetHashCode() => (int)((ulong)_handle >> 32) ^ (int)((ulong)_handle & 0xFFFFFFFF);

		public override readonly string ToString() => $"[{HANDLE_TYPE_NAME},0x{(ulong)_handle:X16}]";

		public override readonly bool Equals(object? obj) => 
			(obj is VulkanHandle<T> handle) && (handle._handle == _handle);

		readonly bool IEquatable<VulkanHandle<T>>.Equals(VulkanHandle<T> other) => other._handle == _handle;
		#endregion // Overrides

		#region Operators
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator == (in VulkanHandle<T> l, in VulkanHandle<T> r) => l._handle == r._handle;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator != (in VulkanHandle<T> l, in VulkanHandle<T> r) => l._handle != r._handle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool (in VulkanHandle<T> handle) => handle.IsValid;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator void* (in VulkanHandle<T> handle) => handle._handle;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong (in VulkanHandle<T> handle) => (ulong)handle._handle;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntPtr (in VulkanHandle<T> handle) => new IntPtr(handle._handle);

		/// <summary>
		/// Special conversion from generic <c>null</c> pointer to the correct Null handle type. It is an error
		/// to use this conversion on non-null pointers.
		/// </summary>
		public static implicit operator VulkanHandle<T>(void* addr)
			=> (addr == null) ? Null : throw new ArgumentException("Cannot create VulkanHandle from non-null pointer");
		#endregion // Operators
	}

	/// <summary>
	/// Interface for all types that represent handles to API objects.
	/// </summary>
	/// <typeparam name="T">The (self-referential) handle class type that implements the interface.</typeparam>
	public interface IVulkanHandle<T> : IEquatable<T>
		where T : class, IVulkanHandle<T>
	{

	}
}
