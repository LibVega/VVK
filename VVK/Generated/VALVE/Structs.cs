﻿/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

/// This file was generated by VVKGen. Edits to this file will be lost on next generation.

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Vulkan
{

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE : IEquatable<VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE>
{
	public const VkStructureType TYPE = VkStructureType.PhysicalDeviceMutableDescriptorTypeFeaturesValve;

	public VkStructureType sType;
	public void* pNext;
	public VkBool32 MutableDescriptorType;

	public VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE(
		VkBool32 mutableDescriptorType = default
	) {
		sType = TYPE;
		pNext = null;
		MutableDescriptorType = mutableDescriptorType;
	}

	public readonly override bool Equals(object? o) => (o is VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE s) && (this == s);
	readonly bool IEquatable<VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE>.Equals(VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ MutableDescriptorType.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE l, in VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.MutableDescriptorType == r.MutableDescriptorType)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE l, in VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.MutableDescriptorType != r.MutableDescriptorType)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkPhysicalDeviceMutableDescriptorTypeFeaturesVALVE s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkMutableDescriptorTypeListVALVE : IEquatable<VkMutableDescriptorTypeListVALVE>
{
	public uint DescriptorTypeCount;
	public VkDescriptorType* DescriptorTypes;

	public VkMutableDescriptorTypeListVALVE(
		uint descriptorTypeCount = default,
		VkDescriptorType* descriptorTypes = default
	) {
		DescriptorTypeCount = descriptorTypeCount;
		DescriptorTypes = descriptorTypes;
	}

	public readonly override bool Equals(object? o) => (o is VkMutableDescriptorTypeListVALVE s) && (this == s);
	readonly bool IEquatable<VkMutableDescriptorTypeListVALVE>.Equals(VkMutableDescriptorTypeListVALVE o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			DescriptorTypeCount.GetHashCode() ^ ((ulong)DescriptorTypes).GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkMutableDescriptorTypeListVALVE l, in VkMutableDescriptorTypeListVALVE r)
	{
		return
			(l.DescriptorTypeCount == r.DescriptorTypeCount) && (l.DescriptorTypes == r.DescriptorTypes)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkMutableDescriptorTypeListVALVE l, in VkMutableDescriptorTypeListVALVE r)
	{
		return
			(l.DescriptorTypeCount != r.DescriptorTypeCount) || (l.DescriptorTypes != r.DescriptorTypes)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkMutableDescriptorTypeListVALVE s) => s = new();
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkMutableDescriptorTypeCreateInfoVALVE : IEquatable<VkMutableDescriptorTypeCreateInfoVALVE>
{
	public const VkStructureType TYPE = VkStructureType.MutableDescriptorTypeCreateInfoValve;

	public VkStructureType sType;
	public void* pNext;
	public uint MutableDescriptorTypeListCount;
	public VkMutableDescriptorTypeListVALVE* MutableDescriptorTypeLists;

	public VkMutableDescriptorTypeCreateInfoVALVE(
		uint mutableDescriptorTypeListCount = default,
		VkMutableDescriptorTypeListVALVE* mutableDescriptorTypeLists = default
	) {
		sType = TYPE;
		pNext = null;
		MutableDescriptorTypeListCount = mutableDescriptorTypeListCount;
		MutableDescriptorTypeLists = mutableDescriptorTypeLists;
	}

	public readonly override bool Equals(object? o) => (o is VkMutableDescriptorTypeCreateInfoVALVE s) && (this == s);
	readonly bool IEquatable<VkMutableDescriptorTypeCreateInfoVALVE>.Equals(VkMutableDescriptorTypeCreateInfoVALVE o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ MutableDescriptorTypeListCount.GetHashCode() ^ ((ulong)MutableDescriptorTypeLists).GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkMutableDescriptorTypeCreateInfoVALVE l, in VkMutableDescriptorTypeCreateInfoVALVE r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.MutableDescriptorTypeListCount == r.MutableDescriptorTypeListCount) && (l.MutableDescriptorTypeLists == r.MutableDescriptorTypeLists)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkMutableDescriptorTypeCreateInfoVALVE l, in VkMutableDescriptorTypeCreateInfoVALVE r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.MutableDescriptorTypeListCount != r.MutableDescriptorTypeListCount) || (l.MutableDescriptorTypeLists != r.MutableDescriptorTypeLists)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkMutableDescriptorTypeCreateInfoVALVE s) => s = new() { sType = TYPE };
}


} // namespace Vulkan

