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
public unsafe partial struct VkImportAndroidHardwareBufferInfoANDROID : IEquatable<VkImportAndroidHardwareBufferInfoANDROID>
{
	public const VkStructureType TYPE = VkStructureType.ImportAndroidHardwareBufferInfoAndroid;

	public VkStructureType sType;
	public void* pNext;
	public void* Buffer;

	public VkImportAndroidHardwareBufferInfoANDROID(
		void* buffer = default
	) {
		sType = TYPE;
		pNext = null;
		Buffer = buffer;
	}

	public readonly override bool Equals(object? o) => (o is VkImportAndroidHardwareBufferInfoANDROID s) && (this == s);
	readonly bool IEquatable<VkImportAndroidHardwareBufferInfoANDROID>.Equals(VkImportAndroidHardwareBufferInfoANDROID o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ ((ulong)Buffer).GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkImportAndroidHardwareBufferInfoANDROID l, in VkImportAndroidHardwareBufferInfoANDROID r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.Buffer == r.Buffer)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkImportAndroidHardwareBufferInfoANDROID l, in VkImportAndroidHardwareBufferInfoANDROID r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.Buffer != r.Buffer)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkImportAndroidHardwareBufferInfoANDROID s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkAndroidHardwareBufferUsageANDROID : IEquatable<VkAndroidHardwareBufferUsageANDROID>
{
	public const VkStructureType TYPE = VkStructureType.AndroidHardwareBufferUsageAndroid;

	public VkStructureType sType;
	public void* pNext;
	public ulong AndroidHardwareBufferUsage;

	public VkAndroidHardwareBufferUsageANDROID(
		ulong androidHardwareBufferUsage = default
	) {
		sType = TYPE;
		pNext = null;
		AndroidHardwareBufferUsage = androidHardwareBufferUsage;
	}

	public readonly override bool Equals(object? o) => (o is VkAndroidHardwareBufferUsageANDROID s) && (this == s);
	readonly bool IEquatable<VkAndroidHardwareBufferUsageANDROID>.Equals(VkAndroidHardwareBufferUsageANDROID o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ AndroidHardwareBufferUsage.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkAndroidHardwareBufferUsageANDROID l, in VkAndroidHardwareBufferUsageANDROID r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.AndroidHardwareBufferUsage == r.AndroidHardwareBufferUsage)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkAndroidHardwareBufferUsageANDROID l, in VkAndroidHardwareBufferUsageANDROID r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.AndroidHardwareBufferUsage != r.AndroidHardwareBufferUsage)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkAndroidHardwareBufferUsageANDROID s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkAndroidHardwareBufferPropertiesANDROID : IEquatable<VkAndroidHardwareBufferPropertiesANDROID>
{
	public const VkStructureType TYPE = VkStructureType.AndroidHardwareBufferPropertiesAndroid;

	public VkStructureType sType;
	public void* pNext;
	public ulong AllocationSize;
	public uint MemoryTypeBits;

	public VkAndroidHardwareBufferPropertiesANDROID(
		ulong allocationSize = default,
		uint memoryTypeBits = default
	) {
		sType = TYPE;
		pNext = null;
		AllocationSize = allocationSize;
		MemoryTypeBits = memoryTypeBits;
	}

	public readonly override bool Equals(object? o) => (o is VkAndroidHardwareBufferPropertiesANDROID s) && (this == s);
	readonly bool IEquatable<VkAndroidHardwareBufferPropertiesANDROID>.Equals(VkAndroidHardwareBufferPropertiesANDROID o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ AllocationSize.GetHashCode() ^ MemoryTypeBits.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkAndroidHardwareBufferPropertiesANDROID l, in VkAndroidHardwareBufferPropertiesANDROID r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.AllocationSize == r.AllocationSize) && (l.MemoryTypeBits == r.MemoryTypeBits)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkAndroidHardwareBufferPropertiesANDROID l, in VkAndroidHardwareBufferPropertiesANDROID r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.AllocationSize != r.AllocationSize) || (l.MemoryTypeBits != r.MemoryTypeBits)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkAndroidHardwareBufferPropertiesANDROID s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkMemoryGetAndroidHardwareBufferInfoANDROID : IEquatable<VkMemoryGetAndroidHardwareBufferInfoANDROID>
{
	public const VkStructureType TYPE = VkStructureType.MemoryGetAndroidHardwareBufferInfoAndroid;

	public VkStructureType sType;
	public void* pNext;
	public VulkanHandle<VkDeviceMemory> Memory;

	public VkMemoryGetAndroidHardwareBufferInfoANDROID(
		VulkanHandle<VkDeviceMemory> memory = default
	) {
		sType = TYPE;
		pNext = null;
		Memory = memory;
	}

	public readonly override bool Equals(object? o) => (o is VkMemoryGetAndroidHardwareBufferInfoANDROID s) && (this == s);
	readonly bool IEquatable<VkMemoryGetAndroidHardwareBufferInfoANDROID>.Equals(VkMemoryGetAndroidHardwareBufferInfoANDROID o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ Memory.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkMemoryGetAndroidHardwareBufferInfoANDROID l, in VkMemoryGetAndroidHardwareBufferInfoANDROID r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.Memory == r.Memory)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkMemoryGetAndroidHardwareBufferInfoANDROID l, in VkMemoryGetAndroidHardwareBufferInfoANDROID r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.Memory != r.Memory)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkMemoryGetAndroidHardwareBufferInfoANDROID s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkAndroidHardwareBufferFormatPropertiesANDROID : IEquatable<VkAndroidHardwareBufferFormatPropertiesANDROID>
{
	public const VkStructureType TYPE = VkStructureType.AndroidHardwareBufferFormatPropertiesAndroid;

	public VkStructureType sType;
	public void* pNext;
	public VkFormat Format;
	public ulong ExternalFormat;
	public VkFormatFeatureFlags FormatFeatures;
	public VkComponentMapping SamplerYcbcrConversionComponents;
	public VkSamplerYcbcrModelConversion SuggestedYcbcrModel;
	public VkSamplerYcbcrRange SuggestedYcbcrRange;
	public VkChromaLocation SuggestedXChromaOffset;
	public VkChromaLocation SuggestedYChromaOffset;

	public VkAndroidHardwareBufferFormatPropertiesANDROID(
		VkFormat format = default,
		ulong externalFormat = default,
		VkFormatFeatureFlags formatFeatures = default,
		VkComponentMapping samplerYcbcrConversionComponents = default,
		VkSamplerYcbcrModelConversion suggestedYcbcrModel = default,
		VkSamplerYcbcrRange suggestedYcbcrRange = default,
		VkChromaLocation suggestedXChromaOffset = default,
		VkChromaLocation suggestedYChromaOffset = default
	) {
		sType = TYPE;
		pNext = null;
		Format = format;
		ExternalFormat = externalFormat;
		FormatFeatures = formatFeatures;
		SamplerYcbcrConversionComponents = samplerYcbcrConversionComponents;
		SuggestedYcbcrModel = suggestedYcbcrModel;
		SuggestedYcbcrRange = suggestedYcbcrRange;
		SuggestedXChromaOffset = suggestedXChromaOffset;
		SuggestedYChromaOffset = suggestedYChromaOffset;
	}

	public readonly override bool Equals(object? o) => (o is VkAndroidHardwareBufferFormatPropertiesANDROID s) && (this == s);
	readonly bool IEquatable<VkAndroidHardwareBufferFormatPropertiesANDROID>.Equals(VkAndroidHardwareBufferFormatPropertiesANDROID o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ Format.GetHashCode() ^ ExternalFormat.GetHashCode()
			^ FormatFeatures.GetHashCode() ^ SamplerYcbcrConversionComponents.GetHashCode() ^ SuggestedYcbcrModel.GetHashCode() ^ SuggestedYcbcrRange.GetHashCode()
			^ SuggestedXChromaOffset.GetHashCode() ^ SuggestedYChromaOffset.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkAndroidHardwareBufferFormatPropertiesANDROID l, in VkAndroidHardwareBufferFormatPropertiesANDROID r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.Format == r.Format) && (l.ExternalFormat == r.ExternalFormat)
			&& (l.FormatFeatures == r.FormatFeatures) && (l.SamplerYcbcrConversionComponents == r.SamplerYcbcrConversionComponents) && (l.SuggestedYcbcrModel == r.SuggestedYcbcrModel) && (l.SuggestedYcbcrRange == r.SuggestedYcbcrRange)
			&& (l.SuggestedXChromaOffset == r.SuggestedXChromaOffset) && (l.SuggestedYChromaOffset == r.SuggestedYChromaOffset)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkAndroidHardwareBufferFormatPropertiesANDROID l, in VkAndroidHardwareBufferFormatPropertiesANDROID r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.Format != r.Format) || (l.ExternalFormat != r.ExternalFormat)
			|| (l.FormatFeatures != r.FormatFeatures) || (l.SamplerYcbcrConversionComponents != r.SamplerYcbcrConversionComponents) || (l.SuggestedYcbcrModel != r.SuggestedYcbcrModel) || (l.SuggestedYcbcrRange != r.SuggestedYcbcrRange)
			|| (l.SuggestedXChromaOffset != r.SuggestedXChromaOffset) || (l.SuggestedYChromaOffset != r.SuggestedYChromaOffset)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkAndroidHardwareBufferFormatPropertiesANDROID s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkExternalFormatANDROID : IEquatable<VkExternalFormatANDROID>
{
	public const VkStructureType TYPE = VkStructureType.ExternalFormatAndroid;

	public VkStructureType sType;
	public void* pNext;
	public ulong ExternalFormat;

	public VkExternalFormatANDROID(
		ulong externalFormat = default
	) {
		sType = TYPE;
		pNext = null;
		ExternalFormat = externalFormat;
	}

	public readonly override bool Equals(object? o) => (o is VkExternalFormatANDROID s) && (this == s);
	readonly bool IEquatable<VkExternalFormatANDROID>.Equals(VkExternalFormatANDROID o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ ExternalFormat.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkExternalFormatANDROID l, in VkExternalFormatANDROID r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.ExternalFormat == r.ExternalFormat)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkExternalFormatANDROID l, in VkExternalFormatANDROID r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.ExternalFormat != r.ExternalFormat)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkExternalFormatANDROID s) => s = new() { sType = TYPE };
}


} // namespace Vulkan

