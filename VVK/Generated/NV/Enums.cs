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

public enum VkIndirectCommandsTokenTypeNV : int
{
	ShaderGroup = 0,
	StateFlags = 1,
	IndexBuffer = 2,
	VertexBuffer = 3,
	PushConstant = 4,
	DrawIndexed = 5,
	Draw = 6,
	DrawTasks = 7,
}

public enum VkViewportCoordinateSwizzleNV : int
{
	PositiveX = 0,
	NegativeX = 1,
	PositiveY = 2,
	NegativeY = 3,
	PositiveZ = 4,
	NegativeZ = 5,
	PositiveW = 6,
	NegativeW = 7,
}

public enum VkCoverageModulationModeNV : int
{
	None = 0,
	Rgb = 1,
	Alpha = 2,
	Rgba = 3,
}

public enum VkCoverageReductionModeNV : int
{
	Merge = 0,
	Truncate = 1,
}

public enum VkCopyAccelerationStructureModeNV : int
{
	Clone = 0,
	Compact = 1,
	Serialize = 2,
	Deserialize = 3,
}

public enum VkAccelerationStructureTypeNV : int
{
	TopLevel = 0,
	BottomLevel = 1,
}

public enum VkGeometryTypeNV : int
{
	Triangles = 0,
	Aabbs = 1,
	Instances = 1000150000,
}

public enum VkRayTracingShaderGroupTypeNV : int
{
	General = 0,
	TrianglesHitGroup = 1,
	ProceduralHitGroup = 2,
}

public enum VkAccelerationStructureMemoryRequirementsTypeNV : int
{
	Object = 0,
	BuildScratch = 1,
	UpdateScratch = 2,
}

public enum VkScopeNV : int
{
	Device = 1,
	Workgroup = 2,
	Subgroup = 3,
	QueueFamily = 5,
}

public enum VkComponentTypeNV : int
{
	Float16 = 0,
	Float32 = 1,
	Float64 = 2,
	Sint8 = 3,
	Sint16 = 4,
	Sint32 = 5,
	Sint64 = 6,
	Uint8 = 7,
	Uint16 = 8,
	Uint32 = 9,
	Uint64 = 10,
}

public enum VkFragmentShadingRateNV : int
{
	E1InvocationPerPixel = 0,
	E1InvocationPer1X2Pixels = 1,
	E1InvocationPer2X1Pixels = 4,
	E1InvocationPer2X2Pixels = 5,
	E1InvocationPer2X4Pixels = 6,
	E1InvocationPer4X2Pixels = 9,
	E1InvocationPer4X4Pixels = 10,
	E2InvocationsPerPixel = 11,
	E4InvocationsPerPixel = 12,
	E8InvocationsPerPixel = 13,
	E16InvocationsPerPixel = 14,
	NoInvocations = 15,
}

public enum VkFragmentShadingRateTypeNV : int
{
	FragmentSize = 0,
	Enums = 1,
}

public enum VkShadingRatePaletteEntryNV : int
{
	NoInvocations = 0,
	E16InvocationsPerPixel = 1,
	E8InvocationsPerPixel = 2,
	E4InvocationsPerPixel = 3,
	E2InvocationsPerPixel = 4,
	E1InvocationPerPixel = 5,
	E1InvocationPer2X1Pixels = 6,
	E1InvocationPer1X2Pixels = 7,
	E1InvocationPer2X2Pixels = 8,
	E1InvocationPer4X2Pixels = 9,
	E1InvocationPer2X4Pixels = 10,
	E1InvocationPer4X4Pixels = 11,
}

public enum VkCoarseSampleOrderTypeNV : int
{
	Default = 0,
	Custom = 1,
	PixelMajor = 2,
	SampleMajor = 3,
}


} // namespace Vulkan
