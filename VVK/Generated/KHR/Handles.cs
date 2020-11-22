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

public unsafe sealed partial class VkDescriptorUpdateTemplateKHR : IVulkanHandle<VkDescriptorUpdateTemplateKHR>
{
	public readonly VulkanHandle<VkDescriptorUpdateTemplateKHR> Handle;
	public readonly VkDevice Parent;
	public readonly DeviceFunctionTable Functions;
	public bool IsValid => Handle.IsValid;

	public VkDescriptorUpdateTemplateKHR(VulkanHandle<VkDescriptorUpdateTemplateKHR> handle, VkDevice parent)
	{
		Handle = handle;
		Parent = parent;
		Functions = parent.Functions;
	}

	public override int GetHashCode() => Handle.GetHashCode();
	public override string? ToString() => Handle.ToString();
	public override bool Equals(object? o) => (o is VkDescriptorUpdateTemplateKHR h) && (h.Handle == Handle);
	bool IEquatable<VkDescriptorUpdateTemplateKHR>.Equals(VkDescriptorUpdateTemplateKHR? other) => other?.Handle == Handle;

	public static bool operator == (VkDescriptorUpdateTemplateKHR? l, VkDescriptorUpdateTemplateKHR? r) => l?.Handle == r?.Handle;
	public static bool operator != (VkDescriptorUpdateTemplateKHR? l, VkDescriptorUpdateTemplateKHR? r) => l?.Handle != r?.Handle;

	public static implicit operator VulkanHandle<VkDescriptorUpdateTemplateKHR> (VkDescriptorUpdateTemplateKHR? h) => h?.Handle ?? VulkanHandle<VkDescriptorUpdateTemplateKHR>.Null;
	public static implicit operator bool (VkDescriptorUpdateTemplateKHR? h) => h?.IsValid ?? false;

}

public unsafe sealed partial class VkSamplerYcbcrConversionKHR : IVulkanHandle<VkSamplerYcbcrConversionKHR>
{
	public readonly VulkanHandle<VkSamplerYcbcrConversionKHR> Handle;
	public readonly VkDevice Parent;
	public readonly DeviceFunctionTable Functions;
	public bool IsValid => Handle.IsValid;

	public VkSamplerYcbcrConversionKHR(VulkanHandle<VkSamplerYcbcrConversionKHR> handle, VkDevice parent)
	{
		Handle = handle;
		Parent = parent;
		Functions = parent.Functions;
	}

	public override int GetHashCode() => Handle.GetHashCode();
	public override string? ToString() => Handle.ToString();
	public override bool Equals(object? o) => (o is VkSamplerYcbcrConversionKHR h) && (h.Handle == Handle);
	bool IEquatable<VkSamplerYcbcrConversionKHR>.Equals(VkSamplerYcbcrConversionKHR? other) => other?.Handle == Handle;

	public static bool operator == (VkSamplerYcbcrConversionKHR? l, VkSamplerYcbcrConversionKHR? r) => l?.Handle == r?.Handle;
	public static bool operator != (VkSamplerYcbcrConversionKHR? l, VkSamplerYcbcrConversionKHR? r) => l?.Handle != r?.Handle;

	public static implicit operator VulkanHandle<VkSamplerYcbcrConversionKHR> (VkSamplerYcbcrConversionKHR? h) => h?.Handle ?? VulkanHandle<VkSamplerYcbcrConversionKHR>.Null;
	public static implicit operator bool (VkSamplerYcbcrConversionKHR? h) => h?.IsValid ?? false;

}

public unsafe sealed partial class VkAccelerationStructureKHR : IVulkanHandle<VkAccelerationStructureKHR>
{
	public readonly VulkanHandle<VkAccelerationStructureKHR> Handle;
	public readonly VkDevice Parent;
	public readonly DeviceFunctionTable Functions;
	public bool IsValid => Handle.IsValid;

	public VkAccelerationStructureKHR(VulkanHandle<VkAccelerationStructureKHR> handle, VkDevice parent)
	{
		Handle = handle;
		Parent = parent;
		Functions = parent.Functions;
	}

	public override int GetHashCode() => Handle.GetHashCode();
	public override string? ToString() => Handle.ToString();
	public override bool Equals(object? o) => (o is VkAccelerationStructureKHR h) && (h.Handle == Handle);
	bool IEquatable<VkAccelerationStructureKHR>.Equals(VkAccelerationStructureKHR? other) => other?.Handle == Handle;

	public static bool operator == (VkAccelerationStructureKHR? l, VkAccelerationStructureKHR? r) => l?.Handle == r?.Handle;
	public static bool operator != (VkAccelerationStructureKHR? l, VkAccelerationStructureKHR? r) => l?.Handle != r?.Handle;

	public static implicit operator VulkanHandle<VkAccelerationStructureKHR> (VkAccelerationStructureKHR? h) => h?.Handle ?? VulkanHandle<VkAccelerationStructureKHR>.Null;
	public static implicit operator bool (VkAccelerationStructureKHR? h) => h?.IsValid ?? false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyAccelerationStructureKHR(VkAllocationCallbacks* pAllocator)
		=> Functions.vkDestroyAccelerationStructureKHR(Parent, Handle, pAllocator);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyAccelerationStructureNV(VkAllocationCallbacks* pAllocator)
		=> Functions.vkDestroyAccelerationStructureNV(Parent, Handle, pAllocator);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetAccelerationStructureHandleNV(ulong dataSize, void* pData)
		=> Functions.vkGetAccelerationStructureHandleNV(Parent, Handle, dataSize, pData);

}

public unsafe sealed partial class VkDeferredOperationKHR : IVulkanHandle<VkDeferredOperationKHR>
{
	public readonly VulkanHandle<VkDeferredOperationKHR> Handle;
	public readonly VkDevice Parent;
	public readonly DeviceFunctionTable Functions;
	public bool IsValid => Handle.IsValid;

	public VkDeferredOperationKHR(VulkanHandle<VkDeferredOperationKHR> handle, VkDevice parent)
	{
		Handle = handle;
		Parent = parent;
		Functions = parent.Functions;
	}

	public override int GetHashCode() => Handle.GetHashCode();
	public override string? ToString() => Handle.ToString();
	public override bool Equals(object? o) => (o is VkDeferredOperationKHR h) && (h.Handle == Handle);
	bool IEquatable<VkDeferredOperationKHR>.Equals(VkDeferredOperationKHR? other) => other?.Handle == Handle;

	public static bool operator == (VkDeferredOperationKHR? l, VkDeferredOperationKHR? r) => l?.Handle == r?.Handle;
	public static bool operator != (VkDeferredOperationKHR? l, VkDeferredOperationKHR? r) => l?.Handle != r?.Handle;

	public static implicit operator VulkanHandle<VkDeferredOperationKHR> (VkDeferredOperationKHR? h) => h?.Handle ?? VulkanHandle<VkDeferredOperationKHR>.Null;
	public static implicit operator bool (VkDeferredOperationKHR? h) => h?.IsValid ?? false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroyDeferredOperationKHR(VkAllocationCallbacks* pAllocator)
		=> Functions.vkDestroyDeferredOperationKHR(Parent, Handle, pAllocator);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint GetDeferredOperationMaxConcurrencyKHR()
		=> Functions.vkGetDeferredOperationMaxConcurrencyKHR(Parent, Handle);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetDeferredOperationResultKHR()
		=> Functions.vkGetDeferredOperationResultKHR(Parent, Handle);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult DeferredOperationJoinKHR()
		=> Functions.vkDeferredOperationJoinKHR(Parent, Handle);

}

public unsafe sealed partial class VkDisplayKHR : IVulkanHandle<VkDisplayKHR>
{
	public readonly VulkanHandle<VkDisplayKHR> Handle;
	public readonly VkPhysicalDevice Parent;
	public readonly InstanceFunctionTable Functions;
	public bool IsValid => Handle.IsValid;

	public VkDisplayKHR(VulkanHandle<VkDisplayKHR> handle, VkPhysicalDevice parent)
	{
		Handle = handle;
		Parent = parent;
		Functions = parent.Functions;
	}

	public override int GetHashCode() => Handle.GetHashCode();
	public override string? ToString() => Handle.ToString();
	public override bool Equals(object? o) => (o is VkDisplayKHR h) && (h.Handle == Handle);
	bool IEquatable<VkDisplayKHR>.Equals(VkDisplayKHR? other) => other?.Handle == Handle;

	public static bool operator == (VkDisplayKHR? l, VkDisplayKHR? r) => l?.Handle == r?.Handle;
	public static bool operator != (VkDisplayKHR? l, VkDisplayKHR? r) => l?.Handle != r?.Handle;

	public static implicit operator VulkanHandle<VkDisplayKHR> (VkDisplayKHR? h) => h?.Handle ?? VulkanHandle<VkDisplayKHR>.Null;
	public static implicit operator bool (VkDisplayKHR? h) => h?.IsValid ?? false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetDisplayModePropertiesKHR(uint* pPropertyCount, VkDisplayModePropertiesKHR* pProperties)
		=> Functions.vkGetDisplayModePropertiesKHR(Parent, Handle, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetDisplayModePropertiesKHR(ref uint pPropertyCount, VkDisplayModePropertiesKHR* pProperties)
	{
		fixed (uint* pPropertyCountFIXED = &pPropertyCount)
		return Functions.vkGetDisplayModePropertiesKHR(Parent, Handle, pPropertyCountFIXED, pProperties);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult CreateDisplayModeKHR(VkDisplayModeCreateInfoKHR* pCreateInfo, VkAllocationCallbacks* pAllocator, VulkanHandle<VkDisplayModeKHR>* pMode)
		=> Functions.vkCreateDisplayModeKHR(Parent, Handle, pCreateInfo, pAllocator, pMode);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult CreateDisplayModeKHR(in VkDisplayModeCreateInfoKHR pCreateInfo, VkAllocationCallbacks* pAllocator, out VulkanHandle<VkDisplayModeKHR> pMode)
	{
		fixed (VkDisplayModeCreateInfoKHR* pCreateInfoFIXED = &pCreateInfo)
		fixed (VulkanHandle<VkDisplayModeKHR>* pModeFIXED = &pMode)
		return Functions.vkCreateDisplayModeKHR(Parent, Handle, pCreateInfoFIXED, pAllocator, pModeFIXED);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult ReleaseDisplayEXT()
		=> Functions.vkReleaseDisplayEXT(Parent, Handle);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetDisplayModeProperties2KHR(uint* pPropertyCount, VkDisplayModeProperties2KHR* pProperties)
		=> Functions.vkGetDisplayModeProperties2KHR(Parent, Handle, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetDisplayModeProperties2KHR(ref uint pPropertyCount, VkDisplayModeProperties2KHR* pProperties)
	{
		fixed (uint* pPropertyCountFIXED = &pPropertyCount)
		return Functions.vkGetDisplayModeProperties2KHR(Parent, Handle, pPropertyCountFIXED, pProperties);
	}

}

public unsafe sealed partial class VkDisplayModeKHR : IVulkanHandle<VkDisplayModeKHR>
{
	public readonly VulkanHandle<VkDisplayModeKHR> Handle;
	public readonly VkDisplayKHR Parent;
	public readonly InstanceFunctionTable Functions;
	public bool IsValid => Handle.IsValid;

	public VkDisplayModeKHR(VulkanHandle<VkDisplayModeKHR> handle, VkDisplayKHR parent)
	{
		Handle = handle;
		Parent = parent;
		Functions = parent.Functions;
	}

	public override int GetHashCode() => Handle.GetHashCode();
	public override string? ToString() => Handle.ToString();
	public override bool Equals(object? o) => (o is VkDisplayModeKHR h) && (h.Handle == Handle);
	bool IEquatable<VkDisplayModeKHR>.Equals(VkDisplayModeKHR? other) => other?.Handle == Handle;

	public static bool operator == (VkDisplayModeKHR? l, VkDisplayModeKHR? r) => l?.Handle == r?.Handle;
	public static bool operator != (VkDisplayModeKHR? l, VkDisplayModeKHR? r) => l?.Handle != r?.Handle;

	public static implicit operator VulkanHandle<VkDisplayModeKHR> (VkDisplayModeKHR? h) => h?.Handle ?? VulkanHandle<VkDisplayModeKHR>.Null;
	public static implicit operator bool (VkDisplayModeKHR? h) => h?.IsValid ?? false;

}

public unsafe sealed partial class VkSurfaceKHR : IVulkanHandle<VkSurfaceKHR>
{
	public readonly VulkanHandle<VkSurfaceKHR> Handle;
	public readonly VkInstance Parent;
	public readonly InstanceFunctionTable Functions;
	public bool IsValid => Handle.IsValid;

	public VkSurfaceKHR(VulkanHandle<VkSurfaceKHR> handle, VkInstance parent)
	{
		Handle = handle;
		Parent = parent;
		Functions = parent.Functions;
	}

	public override int GetHashCode() => Handle.GetHashCode();
	public override string? ToString() => Handle.ToString();
	public override bool Equals(object? o) => (o is VkSurfaceKHR h) && (h.Handle == Handle);
	bool IEquatable<VkSurfaceKHR>.Equals(VkSurfaceKHR? other) => other?.Handle == Handle;

	public static bool operator == (VkSurfaceKHR? l, VkSurfaceKHR? r) => l?.Handle == r?.Handle;
	public static bool operator != (VkSurfaceKHR? l, VkSurfaceKHR? r) => l?.Handle != r?.Handle;

	public static implicit operator VulkanHandle<VkSurfaceKHR> (VkSurfaceKHR? h) => h?.Handle ?? VulkanHandle<VkSurfaceKHR>.Null;
	public static implicit operator bool (VkSurfaceKHR? h) => h?.IsValid ?? false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroySurfaceKHR(VkAllocationCallbacks* pAllocator)
		=> Functions.vkDestroySurfaceKHR(Parent, Handle, pAllocator);

}

public unsafe sealed partial class VkSwapchainKHR : IVulkanHandle<VkSwapchainKHR>
{
	public readonly VulkanHandle<VkSwapchainKHR> Handle;
	public readonly VkDevice Parent;
	public readonly DeviceFunctionTable Functions;
	public bool IsValid => Handle.IsValid;

	public VkSwapchainKHR(VulkanHandle<VkSwapchainKHR> handle, VkDevice parent)
	{
		Handle = handle;
		Parent = parent;
		Functions = parent.Functions;
	}

	public override int GetHashCode() => Handle.GetHashCode();
	public override string? ToString() => Handle.ToString();
	public override bool Equals(object? o) => (o is VkSwapchainKHR h) && (h.Handle == Handle);
	bool IEquatable<VkSwapchainKHR>.Equals(VkSwapchainKHR? other) => other?.Handle == Handle;

	public static bool operator == (VkSwapchainKHR? l, VkSwapchainKHR? r) => l?.Handle == r?.Handle;
	public static bool operator != (VkSwapchainKHR? l, VkSwapchainKHR? r) => l?.Handle != r?.Handle;

	public static implicit operator VulkanHandle<VkSwapchainKHR> (VkSwapchainKHR? h) => h?.Handle ?? VulkanHandle<VkSwapchainKHR>.Null;
	public static implicit operator bool (VkSwapchainKHR? h) => h?.IsValid ?? false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DestroySwapchainKHR(VkAllocationCallbacks* pAllocator)
		=> Functions.vkDestroySwapchainKHR(Parent, Handle, pAllocator);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetSwapchainImagesKHR(uint* pSwapchainImageCount, VulkanHandle<VkImage>* pSwapchainImages)
		=> Functions.vkGetSwapchainImagesKHR(Parent, Handle, pSwapchainImageCount, pSwapchainImages);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetSwapchainImagesKHR(ref uint pSwapchainImageCount, VulkanHandle<VkImage>* pSwapchainImages)
	{
		fixed (uint* pSwapchainImageCountFIXED = &pSwapchainImageCount)
		return Functions.vkGetSwapchainImagesKHR(Parent, Handle, pSwapchainImageCountFIXED, pSwapchainImages);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult AcquireNextImageKHR(ulong timeout, VulkanHandle<VkSemaphore> semaphore, VulkanHandle<VkFence> fence, uint* pImageIndex)
		=> Functions.vkAcquireNextImageKHR(Parent, Handle, timeout, semaphore, fence, pImageIndex);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult AcquireNextImageKHR(ulong timeout, VulkanHandle<VkSemaphore> semaphore, VulkanHandle<VkFence> fence, out uint pImageIndex)
	{
		fixed (uint* pImageIndexFIXED = &pImageIndex)
		return Functions.vkAcquireNextImageKHR(Parent, Handle, timeout, semaphore, fence, pImageIndexFIXED);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetSwapchainCounterEXT(VkSurfaceCounterFlagsEXT counter, ulong* pCounterValue)
		=> Functions.vkGetSwapchainCounterEXT(Parent, Handle, counter, pCounterValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetSwapchainCounterEXT(VkSurfaceCounterFlagsEXT counter, out ulong pCounterValue)
	{
		fixed (ulong* pCounterValueFIXED = &pCounterValue)
		return Functions.vkGetSwapchainCounterEXT(Parent, Handle, counter, pCounterValueFIXED);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetSwapchainStatusKHR()
		=> Functions.vkGetSwapchainStatusKHR(Parent, Handle);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetRefreshCycleDurationGOOGLE(VkRefreshCycleDurationGOOGLE* pDisplayTimingProperties)
		=> Functions.vkGetRefreshCycleDurationGOOGLE(Parent, Handle, pDisplayTimingProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetRefreshCycleDurationGOOGLE(out VkRefreshCycleDurationGOOGLE pDisplayTimingProperties)
	{
		fixed (VkRefreshCycleDurationGOOGLE* pDisplayTimingPropertiesFIXED = &pDisplayTimingProperties)
		return Functions.vkGetRefreshCycleDurationGOOGLE(Parent, Handle, pDisplayTimingPropertiesFIXED);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetPastPresentationTimingGOOGLE(uint* pPresentationTimingCount, VkPastPresentationTimingGOOGLE* pPresentationTimings)
		=> Functions.vkGetPastPresentationTimingGOOGLE(Parent, Handle, pPresentationTimingCount, pPresentationTimings);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult GetPastPresentationTimingGOOGLE(ref uint pPresentationTimingCount, VkPastPresentationTimingGOOGLE* pPresentationTimings)
	{
		fixed (uint* pPresentationTimingCountFIXED = &pPresentationTimingCount)
		return Functions.vkGetPastPresentationTimingGOOGLE(Parent, Handle, pPresentationTimingCountFIXED, pPresentationTimings);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetLocalDimmingAMD(VkBool32 localDimmingEnable)
		=> Functions.vkSetLocalDimmingAMD(Parent, Handle, localDimmingEnable);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult AcquireFullScreenExclusiveModeEXT()
		=> Functions.vkAcquireFullScreenExclusiveModeEXT(Parent, Handle);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VkResult ReleaseFullScreenExclusiveModeEXT()
		=> Functions.vkReleaseFullScreenExclusiveModeEXT(Parent, Handle);

}


} // namespace Vulkan

