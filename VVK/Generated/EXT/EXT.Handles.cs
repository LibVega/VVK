﻿/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

/// This file was generated by VVKGen. Edits to this file will be lost on next generation.

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace Vk.EXT
{

public unsafe partial struct ValidationCache : IHandleType<ValidationCache>
{
	public static readonly ValidationCache Null = new();

	public readonly Vk.Device Parent;
	public readonly Vk.DeviceFunctionTable Functions;
	public readonly Vk.Instance Instance;
	public readonly Vk.Device Device;
	internal readonly Handle<ValidationCache> _handle;
	readonly Handle<ValidationCache> IHandleType<ValidationCache>.Handle => _handle;
	public readonly bool IsValid => _handle.IsValid;

	public ValidationCache(in Vk.Device parent, Vk.Handle<ValidationCache> handle)
	{
		Parent = parent;
		Functions = parent.Functions;
		Instance = parent.Instance;
		Device = parent;
		_handle = handle;
	}

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[ValidationCache 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is ValidationCache t) && (t._handle == _handle);
	readonly bool IEquatable<ValidationCache>.Equals(ValidationCache other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<ValidationCache> (in ValidationCache handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (ValidationCache l, ValidationCache r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (ValidationCache l, ValidationCache r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (ValidationCache handle) => handle._handle.IsValid;

	/// <summary>vkDestroyValidationCacheEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyValidationCacheEXT(Vk.AllocationCallbacks* pAllocator)
		=> Functions.vkDestroyValidationCacheEXT(Device._handle, _handle, pAllocator);

	/// <summary>vkDestroyValidationCacheEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyValidationCacheEXT(in Vk.AllocationCallbacks allocator)
		=> Functions.DestroyValidationCacheEXT(Device._handle, _handle, allocator);

	/// <summary>vkGetValidationCacheDataEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetValidationCacheDataEXT(ulong* pDataSize, void* pData)
		=> Functions.vkGetValidationCacheDataEXT(Device._handle, _handle, pDataSize, pData);

	/// <summary>vkGetValidationCacheDataEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetValidationCacheDataEXT(out ulong dataSize, void* pData)
		=> Functions.GetValidationCacheDataEXT(Device._handle, _handle, out dataSize, pData);

	/// <summary>vkMergeValidationCachesEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result MergeValidationCachesEXT(uint srcCacheCount, Vk.Handle<Vk.EXT.ValidationCache>* pSrcCaches)
		=> Functions.vkMergeValidationCachesEXT(Device._handle, _handle, srcCacheCount, pSrcCaches);

	/// <summary>vkMergeValidationCachesEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result MergeValidationCachesEXT(in ReadOnlySpan<Vk.Handle<Vk.EXT.ValidationCache>> srcCaches)
		=> Functions.MergeValidationCachesEXT(Device._handle, _handle, srcCaches);

}

public unsafe partial struct PrivateDataSlot : IHandleType<PrivateDataSlot>
{
	public static readonly PrivateDataSlot Null = new();

	public readonly Vk.Device Parent;
	public readonly Vk.DeviceFunctionTable Functions;
	public readonly Vk.Instance Instance;
	public readonly Vk.Device Device;
	internal readonly Handle<PrivateDataSlot> _handle;
	readonly Handle<PrivateDataSlot> IHandleType<PrivateDataSlot>.Handle => _handle;
	public readonly bool IsValid => _handle.IsValid;

	public PrivateDataSlot(in Vk.Device parent, Vk.Handle<PrivateDataSlot> handle)
	{
		Parent = parent;
		Functions = parent.Functions;
		Instance = parent.Instance;
		Device = parent;
		_handle = handle;
	}

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[PrivateDataSlot 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is PrivateDataSlot t) && (t._handle == _handle);
	readonly bool IEquatable<PrivateDataSlot>.Equals(PrivateDataSlot other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<PrivateDataSlot> (in PrivateDataSlot handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (PrivateDataSlot l, PrivateDataSlot r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (PrivateDataSlot l, PrivateDataSlot r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (PrivateDataSlot handle) => handle._handle.IsValid;

	/// <summary>vkDestroyPrivateDataSlotEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyPrivateDataSlotEXT(Vk.AllocationCallbacks* pAllocator)
		=> Functions.vkDestroyPrivateDataSlotEXT(Device._handle, _handle, pAllocator);

	/// <summary>vkDestroyPrivateDataSlotEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyPrivateDataSlotEXT(in Vk.AllocationCallbacks allocator)
		=> Functions.DestroyPrivateDataSlotEXT(Device._handle, _handle, allocator);

}

public unsafe partial struct DebugReportCallback : IHandleType<DebugReportCallback>
{
	public static readonly DebugReportCallback Null = new();

	public readonly Vk.Instance Parent;
	public readonly Vk.InstanceFunctionTable Functions;
	public readonly Vk.Instance Instance;
	internal readonly Handle<DebugReportCallback> _handle;
	readonly Handle<DebugReportCallback> IHandleType<DebugReportCallback>.Handle => _handle;
	public readonly bool IsValid => _handle.IsValid;

	public DebugReportCallback(in Vk.Instance parent, Vk.Handle<DebugReportCallback> handle)
	{
		Parent = parent;
		Functions = parent.Functions;
		Instance = parent;
		_handle = handle;
	}

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[DebugReportCallback 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is DebugReportCallback t) && (t._handle == _handle);
	readonly bool IEquatable<DebugReportCallback>.Equals(DebugReportCallback other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<DebugReportCallback> (in DebugReportCallback handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (DebugReportCallback l, DebugReportCallback r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (DebugReportCallback l, DebugReportCallback r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (DebugReportCallback handle) => handle._handle.IsValid;

	/// <summary>vkDestroyDebugReportCallbackEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyDebugReportCallbackEXT(Vk.AllocationCallbacks* pAllocator)
		=> Functions.vkDestroyDebugReportCallbackEXT(Instance._handle, _handle, pAllocator);

	/// <summary>vkDestroyDebugReportCallbackEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyDebugReportCallbackEXT(in Vk.AllocationCallbacks allocator)
		=> Functions.DestroyDebugReportCallbackEXT(Instance._handle, _handle, allocator);

}

public unsafe partial struct DebugUtilsMessenger : IHandleType<DebugUtilsMessenger>
{
	public static readonly DebugUtilsMessenger Null = new();

	public readonly Vk.Instance Parent;
	public readonly Vk.InstanceFunctionTable Functions;
	public readonly Vk.Instance Instance;
	internal readonly Handle<DebugUtilsMessenger> _handle;
	readonly Handle<DebugUtilsMessenger> IHandleType<DebugUtilsMessenger>.Handle => _handle;
	public readonly bool IsValid => _handle.IsValid;

	public DebugUtilsMessenger(in Vk.Instance parent, Vk.Handle<DebugUtilsMessenger> handle)
	{
		Parent = parent;
		Functions = parent.Functions;
		Instance = parent;
		_handle = handle;
	}

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[DebugUtilsMessenger 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is DebugUtilsMessenger t) && (t._handle == _handle);
	readonly bool IEquatable<DebugUtilsMessenger>.Equals(DebugUtilsMessenger other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<DebugUtilsMessenger> (in DebugUtilsMessenger handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (DebugUtilsMessenger l, DebugUtilsMessenger r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (DebugUtilsMessenger l, DebugUtilsMessenger r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (DebugUtilsMessenger handle) => handle._handle.IsValid;

	/// <summary>vkDestroyDebugUtilsMessengerEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyDebugUtilsMessengerEXT(Vk.AllocationCallbacks* pAllocator)
		=> Functions.vkDestroyDebugUtilsMessengerEXT(Instance._handle, _handle, pAllocator);

	/// <summary>vkDestroyDebugUtilsMessengerEXT</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyDebugUtilsMessengerEXT(in Vk.AllocationCallbacks allocator)
		=> Functions.DestroyDebugUtilsMessengerEXT(Instance._handle, _handle, allocator);

}

} // namespace Vk.EXT
