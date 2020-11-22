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
public unsafe partial struct VkPipelineRasterizationStateRasterizationOrderAMD : IEquatable<VkPipelineRasterizationStateRasterizationOrderAMD>
{
	public const VkStructureType TYPE = VkStructureType.PipelineRasterizationStateRasterizationOrderAmd;

	public VkStructureType sType;
	public void* pNext;
	public VkRasterizationOrderAMD RasterizationOrder;

	public VkPipelineRasterizationStateRasterizationOrderAMD(
		VkRasterizationOrderAMD rasterizationOrder = default
	) {
		sType = TYPE;
		pNext = null;
		RasterizationOrder = rasterizationOrder;
	}

	public readonly override bool Equals(object? o) => (o is VkPipelineRasterizationStateRasterizationOrderAMD s) && (this == s);
	readonly bool IEquatable<VkPipelineRasterizationStateRasterizationOrderAMD>.Equals(VkPipelineRasterizationStateRasterizationOrderAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ RasterizationOrder.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkPipelineRasterizationStateRasterizationOrderAMD l, in VkPipelineRasterizationStateRasterizationOrderAMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.RasterizationOrder == r.RasterizationOrder)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkPipelineRasterizationStateRasterizationOrderAMD l, in VkPipelineRasterizationStateRasterizationOrderAMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.RasterizationOrder != r.RasterizationOrder)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkPipelineRasterizationStateRasterizationOrderAMD s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkDisplayNativeHdrSurfaceCapabilitiesAMD : IEquatable<VkDisplayNativeHdrSurfaceCapabilitiesAMD>
{
	public const VkStructureType TYPE = VkStructureType.DisplayNativeHdrSurfaceCapabilitiesAmd;

	public VkStructureType sType;
	public void* pNext;
	public VkBool32 LocalDimmingSupport;

	public VkDisplayNativeHdrSurfaceCapabilitiesAMD(
		VkBool32 localDimmingSupport = default
	) {
		sType = TYPE;
		pNext = null;
		LocalDimmingSupport = localDimmingSupport;
	}

	public readonly override bool Equals(object? o) => (o is VkDisplayNativeHdrSurfaceCapabilitiesAMD s) && (this == s);
	readonly bool IEquatable<VkDisplayNativeHdrSurfaceCapabilitiesAMD>.Equals(VkDisplayNativeHdrSurfaceCapabilitiesAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ LocalDimmingSupport.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkDisplayNativeHdrSurfaceCapabilitiesAMD l, in VkDisplayNativeHdrSurfaceCapabilitiesAMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.LocalDimmingSupport == r.LocalDimmingSupport)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkDisplayNativeHdrSurfaceCapabilitiesAMD l, in VkDisplayNativeHdrSurfaceCapabilitiesAMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.LocalDimmingSupport != r.LocalDimmingSupport)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkDisplayNativeHdrSurfaceCapabilitiesAMD s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkSwapchainDisplayNativeHdrCreateInfoAMD : IEquatable<VkSwapchainDisplayNativeHdrCreateInfoAMD>
{
	public const VkStructureType TYPE = VkStructureType.SwapchainDisplayNativeHdrCreateInfoAmd;

	public VkStructureType sType;
	public void* pNext;
	public VkBool32 LocalDimmingEnable;

	public VkSwapchainDisplayNativeHdrCreateInfoAMD(
		VkBool32 localDimmingEnable = default
	) {
		sType = TYPE;
		pNext = null;
		LocalDimmingEnable = localDimmingEnable;
	}

	public readonly override bool Equals(object? o) => (o is VkSwapchainDisplayNativeHdrCreateInfoAMD s) && (this == s);
	readonly bool IEquatable<VkSwapchainDisplayNativeHdrCreateInfoAMD>.Equals(VkSwapchainDisplayNativeHdrCreateInfoAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ LocalDimmingEnable.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkSwapchainDisplayNativeHdrCreateInfoAMD l, in VkSwapchainDisplayNativeHdrCreateInfoAMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.LocalDimmingEnable == r.LocalDimmingEnable)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkSwapchainDisplayNativeHdrCreateInfoAMD l, in VkSwapchainDisplayNativeHdrCreateInfoAMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.LocalDimmingEnable != r.LocalDimmingEnable)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkSwapchainDisplayNativeHdrCreateInfoAMD s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkTextureLODGatherFormatPropertiesAMD : IEquatable<VkTextureLODGatherFormatPropertiesAMD>
{
	public const VkStructureType TYPE = VkStructureType.TextureLodGatherFormatPropertiesAmd;

	public VkStructureType sType;
	public void* pNext;
	public VkBool32 SupportsTextureGatherLODBiasAMD;

	public VkTextureLODGatherFormatPropertiesAMD(
		VkBool32 supportsTextureGatherLODBiasAMD = default
	) {
		sType = TYPE;
		pNext = null;
		SupportsTextureGatherLODBiasAMD = supportsTextureGatherLODBiasAMD;
	}

	public readonly override bool Equals(object? o) => (o is VkTextureLODGatherFormatPropertiesAMD s) && (this == s);
	readonly bool IEquatable<VkTextureLODGatherFormatPropertiesAMD>.Equals(VkTextureLODGatherFormatPropertiesAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ SupportsTextureGatherLODBiasAMD.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkTextureLODGatherFormatPropertiesAMD l, in VkTextureLODGatherFormatPropertiesAMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.SupportsTextureGatherLODBiasAMD == r.SupportsTextureGatherLODBiasAMD)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkTextureLODGatherFormatPropertiesAMD l, in VkTextureLODGatherFormatPropertiesAMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.SupportsTextureGatherLODBiasAMD != r.SupportsTextureGatherLODBiasAMD)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkTextureLODGatherFormatPropertiesAMD s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkShaderResourceUsageAMD : IEquatable<VkShaderResourceUsageAMD>
{
	public uint NumUsedVgprs;
	public uint NumUsedSgprs;
	public uint LdsSizePerLocalWorkGroup;
	public ulong LdsUsageSizeInBytes;
	public ulong ScratchMemUsageInBytes;

	public VkShaderResourceUsageAMD(
		uint numUsedVgprs = default,
		uint numUsedSgprs = default,
		uint ldsSizePerLocalWorkGroup = default,
		ulong ldsUsageSizeInBytes = default,
		ulong scratchMemUsageInBytes = default
	) {
		NumUsedVgprs = numUsedVgprs;
		NumUsedSgprs = numUsedSgprs;
		LdsSizePerLocalWorkGroup = ldsSizePerLocalWorkGroup;
		LdsUsageSizeInBytes = ldsUsageSizeInBytes;
		ScratchMemUsageInBytes = scratchMemUsageInBytes;
	}

	public readonly override bool Equals(object? o) => (o is VkShaderResourceUsageAMD s) && (this == s);
	readonly bool IEquatable<VkShaderResourceUsageAMD>.Equals(VkShaderResourceUsageAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			NumUsedVgprs.GetHashCode() ^ NumUsedSgprs.GetHashCode() ^ LdsSizePerLocalWorkGroup.GetHashCode() ^ LdsUsageSizeInBytes.GetHashCode()
			^ ScratchMemUsageInBytes.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkShaderResourceUsageAMD l, in VkShaderResourceUsageAMD r)
	{
		return
			(l.NumUsedVgprs == r.NumUsedVgprs) && (l.NumUsedSgprs == r.NumUsedSgprs) && (l.LdsSizePerLocalWorkGroup == r.LdsSizePerLocalWorkGroup) && (l.LdsUsageSizeInBytes == r.LdsUsageSizeInBytes)
			&& (l.ScratchMemUsageInBytes == r.ScratchMemUsageInBytes)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkShaderResourceUsageAMD l, in VkShaderResourceUsageAMD r)
	{
		return
			(l.NumUsedVgprs != r.NumUsedVgprs) || (l.NumUsedSgprs != r.NumUsedSgprs) || (l.LdsSizePerLocalWorkGroup != r.LdsSizePerLocalWorkGroup) || (l.LdsUsageSizeInBytes != r.LdsUsageSizeInBytes)
			|| (l.ScratchMemUsageInBytes != r.ScratchMemUsageInBytes)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkShaderResourceUsageAMD s) => s = new();
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkShaderStatisticsInfoAMD : IEquatable<VkShaderStatisticsInfoAMD>
{
	public VkShaderStageFlags ShaderStageMask;
	public VkShaderResourceUsageAMD ResourceUsage;
	public uint NumPhysicalVgprs;
	public uint NumPhysicalSgprs;
	public uint NumAvailableVgprs;
	public uint NumAvailableSgprs;
	public fixed uint ComputeWorkGroupSize[3];

	public VkShaderStatisticsInfoAMD(
		VkShaderStageFlags shaderStageMask = default,
		VkShaderResourceUsageAMD resourceUsage = default,
		uint numPhysicalVgprs = default,
		uint numPhysicalSgprs = default,
		uint numAvailableVgprs = default,
		uint numAvailableSgprs = default,
		uint computeWorkGroupSize_0 = default,
		uint computeWorkGroupSize_1 = default,
		uint computeWorkGroupSize_2 = default
	) {
		ShaderStageMask = shaderStageMask;
		ResourceUsage = resourceUsage;
		NumPhysicalVgprs = numPhysicalVgprs;
		NumPhysicalSgprs = numPhysicalSgprs;
		NumAvailableVgprs = numAvailableVgprs;
		NumAvailableSgprs = numAvailableSgprs;
		ComputeWorkGroupSize[0] = computeWorkGroupSize_0;
		ComputeWorkGroupSize[1] = computeWorkGroupSize_1;
		ComputeWorkGroupSize[2] = computeWorkGroupSize_2;
	}

	public readonly override bool Equals(object? o) => (o is VkShaderStatisticsInfoAMD s) && (this == s);
	readonly bool IEquatable<VkShaderStatisticsInfoAMD>.Equals(VkShaderStatisticsInfoAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			ShaderStageMask.GetHashCode() ^ ResourceUsage.GetHashCode() ^ NumPhysicalVgprs.GetHashCode() ^ NumPhysicalSgprs.GetHashCode()
			^ NumAvailableVgprs.GetHashCode() ^ NumAvailableSgprs.GetHashCode() ^ ComputeWorkGroupSize[0].GetHashCode() ^ ComputeWorkGroupSize[1].GetHashCode()
			^ ComputeWorkGroupSize[2].GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkShaderStatisticsInfoAMD l, in VkShaderStatisticsInfoAMD r)
	{
		return
			(l.ShaderStageMask == r.ShaderStageMask) && (l.ResourceUsage == r.ResourceUsage) && (l.NumPhysicalVgprs == r.NumPhysicalVgprs) && (l.NumPhysicalSgprs == r.NumPhysicalSgprs)
			&& (l.NumAvailableVgprs == r.NumAvailableVgprs) && (l.NumAvailableSgprs == r.NumAvailableSgprs) && (l.ComputeWorkGroupSize[0] == r.ComputeWorkGroupSize[0]) && (l.ComputeWorkGroupSize[1] == r.ComputeWorkGroupSize[1])
			&& (l.ComputeWorkGroupSize[2] == r.ComputeWorkGroupSize[2])
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkShaderStatisticsInfoAMD l, in VkShaderStatisticsInfoAMD r)
	{
		return
			(l.ShaderStageMask != r.ShaderStageMask) || (l.ResourceUsage != r.ResourceUsage) || (l.NumPhysicalVgprs != r.NumPhysicalVgprs) || (l.NumPhysicalSgprs != r.NumPhysicalSgprs)
			|| (l.NumAvailableVgprs != r.NumAvailableVgprs) || (l.NumAvailableSgprs != r.NumAvailableSgprs) || (l.ComputeWorkGroupSize[0] != r.ComputeWorkGroupSize[0]) || (l.ComputeWorkGroupSize[1] != r.ComputeWorkGroupSize[1])
			|| (l.ComputeWorkGroupSize[2] != r.ComputeWorkGroupSize[2])
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkShaderStatisticsInfoAMD s) => s = new();
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkPhysicalDeviceShaderCorePropertiesAMD : IEquatable<VkPhysicalDeviceShaderCorePropertiesAMD>
{
	public const VkStructureType TYPE = VkStructureType.PhysicalDeviceShaderCorePropertiesAmd;

	public VkStructureType sType;
	public void* pNext;
	public uint ShaderEngineCount;
	public uint ShaderArraysPerEngineCount;
	public uint ComputeUnitsPerShaderArray;
	public uint SimdPerComputeUnit;
	public uint WavefrontsPerSimd;
	public uint WavefrontSize;
	public uint SgprsPerSimd;
	public uint MinSgprAllocation;
	public uint MaxSgprAllocation;
	public uint SgprAllocationGranularity;
	public uint VgprsPerSimd;
	public uint MinVgprAllocation;
	public uint MaxVgprAllocation;
	public uint VgprAllocationGranularity;

	public VkPhysicalDeviceShaderCorePropertiesAMD(
		uint shaderEngineCount = default,
		uint shaderArraysPerEngineCount = default,
		uint computeUnitsPerShaderArray = default,
		uint simdPerComputeUnit = default,
		uint wavefrontsPerSimd = default,
		uint wavefrontSize = default,
		uint sgprsPerSimd = default,
		uint minSgprAllocation = default,
		uint maxSgprAllocation = default,
		uint sgprAllocationGranularity = default,
		uint vgprsPerSimd = default,
		uint minVgprAllocation = default,
		uint maxVgprAllocation = default,
		uint vgprAllocationGranularity = default
	) {
		sType = TYPE;
		pNext = null;
		ShaderEngineCount = shaderEngineCount;
		ShaderArraysPerEngineCount = shaderArraysPerEngineCount;
		ComputeUnitsPerShaderArray = computeUnitsPerShaderArray;
		SimdPerComputeUnit = simdPerComputeUnit;
		WavefrontsPerSimd = wavefrontsPerSimd;
		WavefrontSize = wavefrontSize;
		SgprsPerSimd = sgprsPerSimd;
		MinSgprAllocation = minSgprAllocation;
		MaxSgprAllocation = maxSgprAllocation;
		SgprAllocationGranularity = sgprAllocationGranularity;
		VgprsPerSimd = vgprsPerSimd;
		MinVgprAllocation = minVgprAllocation;
		MaxVgprAllocation = maxVgprAllocation;
		VgprAllocationGranularity = vgprAllocationGranularity;
	}

	public readonly override bool Equals(object? o) => (o is VkPhysicalDeviceShaderCorePropertiesAMD s) && (this == s);
	readonly bool IEquatable<VkPhysicalDeviceShaderCorePropertiesAMD>.Equals(VkPhysicalDeviceShaderCorePropertiesAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ ShaderEngineCount.GetHashCode() ^ ShaderArraysPerEngineCount.GetHashCode()
			^ ComputeUnitsPerShaderArray.GetHashCode() ^ SimdPerComputeUnit.GetHashCode() ^ WavefrontsPerSimd.GetHashCode() ^ WavefrontSize.GetHashCode()
			^ SgprsPerSimd.GetHashCode() ^ MinSgprAllocation.GetHashCode() ^ MaxSgprAllocation.GetHashCode() ^ SgprAllocationGranularity.GetHashCode()
			^ VgprsPerSimd.GetHashCode() ^ MinVgprAllocation.GetHashCode() ^ MaxVgprAllocation.GetHashCode() ^ VgprAllocationGranularity.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkPhysicalDeviceShaderCorePropertiesAMD l, in VkPhysicalDeviceShaderCorePropertiesAMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.ShaderEngineCount == r.ShaderEngineCount) && (l.ShaderArraysPerEngineCount == r.ShaderArraysPerEngineCount)
			&& (l.ComputeUnitsPerShaderArray == r.ComputeUnitsPerShaderArray) && (l.SimdPerComputeUnit == r.SimdPerComputeUnit) && (l.WavefrontsPerSimd == r.WavefrontsPerSimd) && (l.WavefrontSize == r.WavefrontSize)
			&& (l.SgprsPerSimd == r.SgprsPerSimd) && (l.MinSgprAllocation == r.MinSgprAllocation) && (l.MaxSgprAllocation == r.MaxSgprAllocation) && (l.SgprAllocationGranularity == r.SgprAllocationGranularity)
			&& (l.VgprsPerSimd == r.VgprsPerSimd) && (l.MinVgprAllocation == r.MinVgprAllocation) && (l.MaxVgprAllocation == r.MaxVgprAllocation) && (l.VgprAllocationGranularity == r.VgprAllocationGranularity)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkPhysicalDeviceShaderCorePropertiesAMD l, in VkPhysicalDeviceShaderCorePropertiesAMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.ShaderEngineCount != r.ShaderEngineCount) || (l.ShaderArraysPerEngineCount != r.ShaderArraysPerEngineCount)
			|| (l.ComputeUnitsPerShaderArray != r.ComputeUnitsPerShaderArray) || (l.SimdPerComputeUnit != r.SimdPerComputeUnit) || (l.WavefrontsPerSimd != r.WavefrontsPerSimd) || (l.WavefrontSize != r.WavefrontSize)
			|| (l.SgprsPerSimd != r.SgprsPerSimd) || (l.MinSgprAllocation != r.MinSgprAllocation) || (l.MaxSgprAllocation != r.MaxSgprAllocation) || (l.SgprAllocationGranularity != r.SgprAllocationGranularity)
			|| (l.VgprsPerSimd != r.VgprsPerSimd) || (l.MinVgprAllocation != r.MinVgprAllocation) || (l.MaxVgprAllocation != r.MaxVgprAllocation) || (l.VgprAllocationGranularity != r.VgprAllocationGranularity)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkPhysicalDeviceShaderCorePropertiesAMD s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkPhysicalDeviceShaderCoreProperties2AMD : IEquatable<VkPhysicalDeviceShaderCoreProperties2AMD>
{
	public const VkStructureType TYPE = VkStructureType.PhysicalDeviceShaderCoreProperties2Amd;

	public VkStructureType sType;
	public void* pNext;
	public VkShaderCorePropertiesFlagsAMD ShaderCoreFeatures;
	public uint ActiveComputeUnitCount;

	public VkPhysicalDeviceShaderCoreProperties2AMD(
		VkShaderCorePropertiesFlagsAMD shaderCoreFeatures = default,
		uint activeComputeUnitCount = default
	) {
		sType = TYPE;
		pNext = null;
		ShaderCoreFeatures = shaderCoreFeatures;
		ActiveComputeUnitCount = activeComputeUnitCount;
	}

	public readonly override bool Equals(object? o) => (o is VkPhysicalDeviceShaderCoreProperties2AMD s) && (this == s);
	readonly bool IEquatable<VkPhysicalDeviceShaderCoreProperties2AMD>.Equals(VkPhysicalDeviceShaderCoreProperties2AMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ ShaderCoreFeatures.GetHashCode() ^ ActiveComputeUnitCount.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkPhysicalDeviceShaderCoreProperties2AMD l, in VkPhysicalDeviceShaderCoreProperties2AMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.ShaderCoreFeatures == r.ShaderCoreFeatures) && (l.ActiveComputeUnitCount == r.ActiveComputeUnitCount)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkPhysicalDeviceShaderCoreProperties2AMD l, in VkPhysicalDeviceShaderCoreProperties2AMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.ShaderCoreFeatures != r.ShaderCoreFeatures) || (l.ActiveComputeUnitCount != r.ActiveComputeUnitCount)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkPhysicalDeviceShaderCoreProperties2AMD s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkDeviceMemoryOverallocationCreateInfoAMD : IEquatable<VkDeviceMemoryOverallocationCreateInfoAMD>
{
	public const VkStructureType TYPE = VkStructureType.DeviceMemoryOverallocationCreateInfoAmd;

	public VkStructureType sType;
	public void* pNext;
	public VkMemoryOverallocationBehaviorAMD OverallocationBehavior;

	public VkDeviceMemoryOverallocationCreateInfoAMD(
		VkMemoryOverallocationBehaviorAMD overallocationBehavior = default
	) {
		sType = TYPE;
		pNext = null;
		OverallocationBehavior = overallocationBehavior;
	}

	public readonly override bool Equals(object? o) => (o is VkDeviceMemoryOverallocationCreateInfoAMD s) && (this == s);
	readonly bool IEquatable<VkDeviceMemoryOverallocationCreateInfoAMD>.Equals(VkDeviceMemoryOverallocationCreateInfoAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ OverallocationBehavior.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkDeviceMemoryOverallocationCreateInfoAMD l, in VkDeviceMemoryOverallocationCreateInfoAMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.OverallocationBehavior == r.OverallocationBehavior)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkDeviceMemoryOverallocationCreateInfoAMD l, in VkDeviceMemoryOverallocationCreateInfoAMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.OverallocationBehavior != r.OverallocationBehavior)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkDeviceMemoryOverallocationCreateInfoAMD s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkPipelineCompilerControlCreateInfoAMD : IEquatable<VkPipelineCompilerControlCreateInfoAMD>
{
	public const VkStructureType TYPE = VkStructureType.PipelineCompilerControlCreateInfoAmd;

	public VkStructureType sType;
	public void* pNext;
	public VkPipelineCompilerControlFlagsAMD CompilerControlFlags;

	public VkPipelineCompilerControlCreateInfoAMD(
		VkPipelineCompilerControlFlagsAMD compilerControlFlags = default
	) {
		sType = TYPE;
		pNext = null;
		CompilerControlFlags = compilerControlFlags;
	}

	public readonly override bool Equals(object? o) => (o is VkPipelineCompilerControlCreateInfoAMD s) && (this == s);
	readonly bool IEquatable<VkPipelineCompilerControlCreateInfoAMD>.Equals(VkPipelineCompilerControlCreateInfoAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ CompilerControlFlags.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkPipelineCompilerControlCreateInfoAMD l, in VkPipelineCompilerControlCreateInfoAMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.CompilerControlFlags == r.CompilerControlFlags)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkPipelineCompilerControlCreateInfoAMD l, in VkPipelineCompilerControlCreateInfoAMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.CompilerControlFlags != r.CompilerControlFlags)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkPipelineCompilerControlCreateInfoAMD s) => s = new() { sType = TYPE };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct VkPhysicalDeviceCoherentMemoryFeaturesAMD : IEquatable<VkPhysicalDeviceCoherentMemoryFeaturesAMD>
{
	public const VkStructureType TYPE = VkStructureType.PhysicalDeviceCoherentMemoryFeaturesAmd;

	public VkStructureType sType;
	public void* pNext;
	public VkBool32 DeviceCoherentMemory;

	public VkPhysicalDeviceCoherentMemoryFeaturesAMD(
		VkBool32 deviceCoherentMemory = default
	) {
		sType = TYPE;
		pNext = null;
		DeviceCoherentMemory = deviceCoherentMemory;
	}

	public readonly override bool Equals(object? o) => (o is VkPhysicalDeviceCoherentMemoryFeaturesAMD s) && (this == s);
	readonly bool IEquatable<VkPhysicalDeviceCoherentMemoryFeaturesAMD>.Equals(VkPhysicalDeviceCoherentMemoryFeaturesAMD o) => o == this;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode()
	{
		return
			sType.GetHashCode() ^ ((ulong)pNext).GetHashCode() ^ DeviceCoherentMemory.GetHashCode()
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator == (in VkPhysicalDeviceCoherentMemoryFeaturesAMD l, in VkPhysicalDeviceCoherentMemoryFeaturesAMD r)
	{
		return
			(l.sType == r.sType) && (l.pNext == r.pNext) && (l.DeviceCoherentMemory == r.DeviceCoherentMemory)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool operator != (in VkPhysicalDeviceCoherentMemoryFeaturesAMD l, in VkPhysicalDeviceCoherentMemoryFeaturesAMD r)
	{
		return
			(l.sType != r.sType) || (l.pNext != r.pNext) || (l.DeviceCoherentMemory != r.DeviceCoherentMemory)
			;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void New(out VkPhysicalDeviceCoherentMemoryFeaturesAMD s) => s = new() { sType = TYPE };
}


} // namespace Vulkan
