﻿/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

/// This file was generated by VVKGen. Edits to this file will be lost on next generation.

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace Vk.INTEL
{

public unsafe partial struct PerformanceConfiguration : IHandleType<PerformanceConfiguration>
{
	public static readonly PerformanceConfiguration Null = new();

	public readonly Vk.Device Parent;
	public readonly Vk.DeviceFunctionTable Functions;
	public readonly Vk.Instance Instance;
	public readonly Vk.Device Device;
	internal readonly Handle<PerformanceConfiguration> _handle;
	readonly Handle<PerformanceConfiguration> IHandleType<PerformanceConfiguration>.Handle => _handle;
	public readonly bool IsValid => _handle.IsValid;

	public PerformanceConfiguration(in Vk.Device parent, Vk.Handle<PerformanceConfiguration> handle)
	{
		Parent = parent;
		Functions = parent.Functions;
		Instance = parent.Instance;
		Device = parent;
		_handle = handle;
	}

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[PerformanceConfiguration 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is PerformanceConfiguration t) && (t._handle == _handle);
	readonly bool IEquatable<PerformanceConfiguration>.Equals(PerformanceConfiguration other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<PerformanceConfiguration> (in PerformanceConfiguration handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (PerformanceConfiguration l, PerformanceConfiguration r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (PerformanceConfiguration l, PerformanceConfiguration r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (PerformanceConfiguration handle) => handle._handle.IsValid;

	/// <summary>vkReleasePerformanceConfigurationINTEL</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result ReleasePerformanceConfigurationINTEL()
		=> Functions.vkReleasePerformanceConfigurationINTEL(Device._handle, _handle);

}

} // namespace Vk.INTEL
