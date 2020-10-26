﻿/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

/// This file was generated by VVKGen. Edits to this file will be lost on next generation.

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace VVK
{

public unsafe sealed partial class VulkanPhysicalDevice
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetProperties(Vk.PhysicalDeviceProperties* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceProperties(Handle, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetProperties(out Vk.PhysicalDeviceProperties properties)
		=> Parent.Functions.GetPhysicalDeviceProperties(Handle, out properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetQueueFamilyProperties(uint* pQueueFamilyPropertyCount, Vk.QueueFamilyProperties* pQueueFamilyProperties)
		=> Parent.Functions.vkGetPhysicalDeviceQueueFamilyProperties(Handle, pQueueFamilyPropertyCount, pQueueFamilyProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetQueueFamilyProperties(out uint queueFamilyPropertyCount, in Span<Vk.QueueFamilyProperties> queueFamilyProperties)
		=> Parent.Functions.GetPhysicalDeviceQueueFamilyProperties(Handle, out queueFamilyPropertyCount, queueFamilyProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetMemoryProperties(Vk.PhysicalDeviceMemoryProperties* pMemoryProperties)
		=> Parent.Functions.vkGetPhysicalDeviceMemoryProperties(Handle, pMemoryProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetMemoryProperties(out Vk.PhysicalDeviceMemoryProperties memoryProperties)
		=> Parent.Functions.GetPhysicalDeviceMemoryProperties(Handle, out memoryProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFeatures(Vk.PhysicalDeviceFeatures* pFeatures)
		=> Parent.Functions.vkGetPhysicalDeviceFeatures(Handle, pFeatures);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFeatures(out Vk.PhysicalDeviceFeatures features)
		=> Parent.Functions.GetPhysicalDeviceFeatures(Handle, out features);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFormatProperties(Vk.Format format, Vk.FormatProperties* pFormatProperties)
		=> Parent.Functions.vkGetPhysicalDeviceFormatProperties(Handle, format, pFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFormatProperties(Vk.Format format, out Vk.FormatProperties formatProperties)
		=> Parent.Functions.GetPhysicalDeviceFormatProperties(Handle, format, out formatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetImageFormatProperties(Vk.Format format, Vk.ImageType type, Vk.ImageTiling tiling, Vk.ImageUsageFlags usage, Vk.ImageCreateFlags flags, Vk.ImageFormatProperties* pImageFormatProperties)
		=> Parent.Functions.vkGetPhysicalDeviceImageFormatProperties(Handle, format, type, tiling, usage, flags, pImageFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetImageFormatProperties(Vk.Format format, Vk.ImageType type, Vk.ImageTiling tiling, Vk.ImageUsageFlags usage, Vk.ImageCreateFlags flags, out Vk.ImageFormatProperties imageFormatProperties)
		=> Parent.Functions.GetPhysicalDeviceImageFormatProperties(Handle, format, type, tiling, usage, flags, out imageFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result CreateDevice(Vk.DeviceCreateInfo* pCreateInfo, Vk.AllocationCallbacks* pAllocator, Vk.Device* pDevice)
		=> Parent.Functions.vkCreateDevice(Handle, pCreateInfo, pAllocator, pDevice);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result CreateDevice(in Vk.DeviceCreateInfo createInfo, in Vk.AllocationCallbacks allocator, out Vk.Device device)
		=> Parent.Functions.CreateDevice(Handle, createInfo, allocator, out device);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result EnumerateDeviceLayerProperties(uint* pPropertyCount, Vk.LayerProperties* pProperties)
		=> Parent.Functions.vkEnumerateDeviceLayerProperties(Handle, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result EnumerateDeviceLayerProperties(out uint propertyCount, in Span<Vk.LayerProperties> properties)
		=> Parent.Functions.EnumerateDeviceLayerProperties(Handle, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result EnumerateDeviceExtensionProperties(byte* pLayerName, uint* pPropertyCount, Vk.ExtensionProperties* pProperties)
		=> Parent.Functions.vkEnumerateDeviceExtensionProperties(Handle, pLayerName, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result EnumerateDeviceExtensionProperties(VVK.NativeString layerName, out uint propertyCount, in Span<Vk.ExtensionProperties> properties)
		=> Parent.Functions.EnumerateDeviceExtensionProperties(Handle, layerName, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetSparseImageFormatProperties(Vk.Format format, Vk.ImageType type, Vk.SampleCountFlags samples, Vk.ImageUsageFlags usage, Vk.ImageTiling tiling, uint* pPropertyCount, Vk.SparseImageFormatProperties* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceSparseImageFormatProperties(Handle, format, type, samples, usage, tiling, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetSparseImageFormatProperties(Vk.Format format, Vk.ImageType type, Vk.SampleCountFlags samples, Vk.ImageUsageFlags usage, Vk.ImageTiling tiling, out uint propertyCount, in Span<Vk.SparseImageFormatProperties> properties)
		=> Parent.Functions.GetPhysicalDeviceSparseImageFormatProperties(Handle, format, type, samples, usage, tiling, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPropertiesKHR(uint* pPropertyCount, Vk.KHR.DisplayProperties* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceDisplayPropertiesKHR(Handle, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPropertiesKHR(out uint propertyCount, in Span<Vk.KHR.DisplayProperties> properties)
		=> Parent.Functions.GetPhysicalDeviceDisplayPropertiesKHR(Handle, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlanePropertiesKHR(uint* pPropertyCount, Vk.KHR.DisplayPlaneProperties* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(Handle, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlanePropertiesKHR(out uint propertyCount, in Span<Vk.KHR.DisplayPlaneProperties> properties)
		=> Parent.Functions.GetPhysicalDeviceDisplayPlanePropertiesKHR(Handle, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlaneSupportedDisplaysKHR(uint planeIndex, uint* pDisplayCount, Vk.KHR.Display* pDisplays)
		=> Parent.Functions.vkGetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, pDisplayCount, pDisplays);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlaneSupportedDisplaysKHR(uint planeIndex, out uint displayCount, in Span<Vk.KHR.Display> displays)
		=> Parent.Functions.GetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, out displayCount, displays);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayModePropertiesKHR(Vk.KHR.Display display, uint* pPropertyCount, Vk.KHR.DisplayModeProperties* pProperties)
		=> Parent.Functions.vkGetDisplayModePropertiesKHR(Handle, display, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayModePropertiesKHR(Vk.KHR.Display display, out uint propertyCount, in Span<Vk.KHR.DisplayModeProperties> properties)
		=> Parent.Functions.GetDisplayModePropertiesKHR(Handle, display, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result CreateDisplayModeKHR(Vk.KHR.Display display, Vk.KHR.DisplayModeCreateInfo* pCreateInfo, Vk.AllocationCallbacks* pAllocator, Vk.KHR.DisplayMode* pMode)
		=> Parent.Functions.vkCreateDisplayModeKHR(Handle, display, pCreateInfo, pAllocator, pMode);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result CreateDisplayModeKHR(Vk.KHR.Display display, in Vk.KHR.DisplayModeCreateInfo createInfo, in Vk.AllocationCallbacks allocator, out Vk.KHR.DisplayMode mode)
		=> Parent.Functions.CreateDisplayModeKHR(Handle, display, createInfo, allocator, out mode);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlaneCapabilitiesKHR(Vk.KHR.DisplayMode mode, uint planeIndex, Vk.KHR.DisplayPlaneCapabilities* pCapabilities)
		=> Parent.Functions.vkGetDisplayPlaneCapabilitiesKHR(Handle, mode, planeIndex, pCapabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlaneCapabilitiesKHR(Vk.KHR.DisplayMode mode, uint planeIndex, out Vk.KHR.DisplayPlaneCapabilities capabilities)
		=> Parent.Functions.GetDisplayPlaneCapabilitiesKHR(Handle, mode, planeIndex, out capabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceSupportKHR(uint queueFamilyIndex, Vk.KHR.Surface surface, Vk.Bool32* pSupported)
		=> Parent.Functions.vkGetPhysicalDeviceSurfaceSupportKHR(Handle, queueFamilyIndex, surface, pSupported);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceSupportKHR(uint queueFamilyIndex, Vk.KHR.Surface surface, out Vk.Bool32 supported)
		=> Parent.Functions.GetPhysicalDeviceSurfaceSupportKHR(Handle, queueFamilyIndex, surface, out supported);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceCapabilitiesKHR(Vk.KHR.Surface surface, Vk.KHR.SurfaceCapabilities* pSurfaceCapabilities)
		=> Parent.Functions.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(Handle, surface, pSurfaceCapabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceCapabilitiesKHR(Vk.KHR.Surface surface, out Vk.KHR.SurfaceCapabilities surfaceCapabilities)
		=> Parent.Functions.GetPhysicalDeviceSurfaceCapabilitiesKHR(Handle, surface, out surfaceCapabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceFormatsKHR(Vk.KHR.Surface surface, uint* pSurfaceFormatCount, Vk.KHR.SurfaceFormat* pSurfaceFormats)
		=> Parent.Functions.vkGetPhysicalDeviceSurfaceFormatsKHR(Handle, surface, pSurfaceFormatCount, pSurfaceFormats);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceFormatsKHR(Vk.KHR.Surface surface, out uint surfaceFormatCount, in Span<Vk.KHR.SurfaceFormat> surfaceFormats)
		=> Parent.Functions.GetPhysicalDeviceSurfaceFormatsKHR(Handle, surface, out surfaceFormatCount, surfaceFormats);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfacePresentModesKHR(Vk.KHR.Surface surface, uint* pPresentModeCount, Vk.KHR.PresentMode* pPresentModes)
		=> Parent.Functions.vkGetPhysicalDeviceSurfacePresentModesKHR(Handle, surface, pPresentModeCount, pPresentModes);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfacePresentModesKHR(Vk.KHR.Surface surface, out uint presentModeCount, in Span<Vk.KHR.PresentMode> presentModes)
		=> Parent.Functions.GetPhysicalDeviceSurfacePresentModesKHR(Handle, surface, out presentModeCount, presentModes);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Bool32 GetWaylandPresentationSupportKHR(uint queueFamilyIndex, void* display)
		=> Parent.Functions.vkGetPhysicalDeviceWaylandPresentationSupportKHR(Handle, queueFamilyIndex, display);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Bool32 GetWin32PresentationSupportKHR(uint queueFamilyIndex)
		=> Parent.Functions.vkGetPhysicalDeviceWin32PresentationSupportKHR(Handle, queueFamilyIndex);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Bool32 GetXlibPresentationSupportKHR(uint queueFamilyIndex, void* dpy, ulong visualID)
		=> Parent.Functions.vkGetPhysicalDeviceXlibPresentationSupportKHR(Handle, queueFamilyIndex, dpy, visualID);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Bool32 GetXcbPresentationSupportKHR(uint queueFamilyIndex, void* connection, uint visual_id)
		=> Parent.Functions.vkGetPhysicalDeviceXcbPresentationSupportKHR(Handle, queueFamilyIndex, connection, visual_id);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Bool32 GetDirectFBPresentationSupportEXT(uint queueFamilyIndex, void* dfb)
		=> Parent.Functions.vkGetPhysicalDeviceDirectFBPresentationSupportEXT(Handle, queueFamilyIndex, dfb);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetExternalImageFormatPropertiesNV(Vk.Format format, Vk.ImageType type, Vk.ImageTiling tiling, Vk.ImageUsageFlags usage, Vk.ImageCreateFlags flags, Vk.NV.ExternalMemoryHandleTypeFlags externalHandleType, Vk.NV.ExternalImageFormatProperties* pExternalImageFormatProperties)
		=> Parent.Functions.vkGetPhysicalDeviceExternalImageFormatPropertiesNV(Handle, format, type, tiling, usage, flags, externalHandleType, pExternalImageFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetExternalImageFormatPropertiesNV(Vk.Format format, Vk.ImageType type, Vk.ImageTiling tiling, Vk.ImageUsageFlags usage, Vk.ImageCreateFlags flags, Vk.NV.ExternalMemoryHandleTypeFlags externalHandleType, out Vk.NV.ExternalImageFormatProperties externalImageFormatProperties)
		=> Parent.Functions.GetPhysicalDeviceExternalImageFormatPropertiesNV(Handle, format, type, tiling, usage, flags, externalHandleType, out externalImageFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFeatures2(Vk.PhysicalDeviceFeatures2* pFeatures)
		=> Parent.Functions.vkGetPhysicalDeviceFeatures2(Handle, pFeatures);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFeatures2(out Vk.PhysicalDeviceFeatures2 features)
		=> Parent.Functions.GetPhysicalDeviceFeatures2(Handle, out features);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFeatures2KHR(Vk.PhysicalDeviceFeatures2* pFeatures)
		=> Parent.Functions.vkGetPhysicalDeviceFeatures2KHR(Handle, pFeatures);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFeatures2KHR(out Vk.PhysicalDeviceFeatures2 features)
		=> Parent.Functions.GetPhysicalDeviceFeatures2KHR(Handle, out features);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetProperties2(Vk.PhysicalDeviceProperties2* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceProperties2(Handle, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetProperties2(out Vk.PhysicalDeviceProperties2 properties)
		=> Parent.Functions.GetPhysicalDeviceProperties2(Handle, out properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetProperties2KHR(Vk.PhysicalDeviceProperties2* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceProperties2KHR(Handle, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetProperties2KHR(out Vk.PhysicalDeviceProperties2 properties)
		=> Parent.Functions.GetPhysicalDeviceProperties2KHR(Handle, out properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFormatProperties2(Vk.Format format, Vk.FormatProperties2* pFormatProperties)
		=> Parent.Functions.vkGetPhysicalDeviceFormatProperties2(Handle, format, pFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFormatProperties2(Vk.Format format, out Vk.FormatProperties2 formatProperties)
		=> Parent.Functions.GetPhysicalDeviceFormatProperties2(Handle, format, out formatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFormatProperties2KHR(Vk.Format format, Vk.FormatProperties2* pFormatProperties)
		=> Parent.Functions.vkGetPhysicalDeviceFormatProperties2KHR(Handle, format, pFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetFormatProperties2KHR(Vk.Format format, out Vk.FormatProperties2 formatProperties)
		=> Parent.Functions.GetPhysicalDeviceFormatProperties2KHR(Handle, format, out formatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetImageFormatProperties2(Vk.PhysicalDeviceImageFormatInfo2* pImageFormatInfo, Vk.ImageFormatProperties2* pImageFormatProperties)
		=> Parent.Functions.vkGetPhysicalDeviceImageFormatProperties2(Handle, pImageFormatInfo, pImageFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetImageFormatProperties2(in Vk.PhysicalDeviceImageFormatInfo2 imageFormatInfo, out Vk.ImageFormatProperties2 imageFormatProperties)
		=> Parent.Functions.GetPhysicalDeviceImageFormatProperties2(Handle, imageFormatInfo, out imageFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetImageFormatProperties2KHR(Vk.PhysicalDeviceImageFormatInfo2* pImageFormatInfo, Vk.ImageFormatProperties2* pImageFormatProperties)
		=> Parent.Functions.vkGetPhysicalDeviceImageFormatProperties2KHR(Handle, pImageFormatInfo, pImageFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetImageFormatProperties2KHR(in Vk.PhysicalDeviceImageFormatInfo2 imageFormatInfo, out Vk.ImageFormatProperties2 imageFormatProperties)
		=> Parent.Functions.GetPhysicalDeviceImageFormatProperties2KHR(Handle, imageFormatInfo, out imageFormatProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetQueueFamilyProperties2(uint* pQueueFamilyPropertyCount, Vk.QueueFamilyProperties2* pQueueFamilyProperties)
		=> Parent.Functions.vkGetPhysicalDeviceQueueFamilyProperties2(Handle, pQueueFamilyPropertyCount, pQueueFamilyProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetQueueFamilyProperties2(out uint queueFamilyPropertyCount, in Span<Vk.QueueFamilyProperties2> queueFamilyProperties)
		=> Parent.Functions.GetPhysicalDeviceQueueFamilyProperties2(Handle, out queueFamilyPropertyCount, queueFamilyProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetQueueFamilyProperties2KHR(uint* pQueueFamilyPropertyCount, Vk.QueueFamilyProperties2* pQueueFamilyProperties)
		=> Parent.Functions.vkGetPhysicalDeviceQueueFamilyProperties2KHR(Handle, pQueueFamilyPropertyCount, pQueueFamilyProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetQueueFamilyProperties2KHR(out uint queueFamilyPropertyCount, in Span<Vk.QueueFamilyProperties2> queueFamilyProperties)
		=> Parent.Functions.GetPhysicalDeviceQueueFamilyProperties2KHR(Handle, out queueFamilyPropertyCount, queueFamilyProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetMemoryProperties2(Vk.PhysicalDeviceMemoryProperties2* pMemoryProperties)
		=> Parent.Functions.vkGetPhysicalDeviceMemoryProperties2(Handle, pMemoryProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetMemoryProperties2(out Vk.PhysicalDeviceMemoryProperties2 memoryProperties)
		=> Parent.Functions.GetPhysicalDeviceMemoryProperties2(Handle, out memoryProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetMemoryProperties2KHR(Vk.PhysicalDeviceMemoryProperties2* pMemoryProperties)
		=> Parent.Functions.vkGetPhysicalDeviceMemoryProperties2KHR(Handle, pMemoryProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetMemoryProperties2KHR(out Vk.PhysicalDeviceMemoryProperties2 memoryProperties)
		=> Parent.Functions.GetPhysicalDeviceMemoryProperties2KHR(Handle, out memoryProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetSparseImageFormatProperties2(Vk.PhysicalDeviceSparseImageFormatInfo2* pFormatInfo, uint* pPropertyCount, Vk.SparseImageFormatProperties2* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceSparseImageFormatProperties2(Handle, pFormatInfo, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetSparseImageFormatProperties2(in Vk.PhysicalDeviceSparseImageFormatInfo2 formatInfo, out uint propertyCount, in Span<Vk.SparseImageFormatProperties2> properties)
		=> Parent.Functions.GetPhysicalDeviceSparseImageFormatProperties2(Handle, formatInfo, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetSparseImageFormatProperties2KHR(Vk.PhysicalDeviceSparseImageFormatInfo2* pFormatInfo, uint* pPropertyCount, Vk.SparseImageFormatProperties2* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceSparseImageFormatProperties2KHR(Handle, pFormatInfo, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetSparseImageFormatProperties2KHR(in Vk.PhysicalDeviceSparseImageFormatInfo2 formatInfo, out uint propertyCount, in Span<Vk.SparseImageFormatProperties2> properties)
		=> Parent.Functions.GetPhysicalDeviceSparseImageFormatProperties2KHR(Handle, formatInfo, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalBufferProperties(Vk.PhysicalDeviceExternalBufferInfo* pExternalBufferInfo, Vk.ExternalBufferProperties* pExternalBufferProperties)
		=> Parent.Functions.vkGetPhysicalDeviceExternalBufferProperties(Handle, pExternalBufferInfo, pExternalBufferProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalBufferProperties(in Vk.PhysicalDeviceExternalBufferInfo externalBufferInfo, out Vk.ExternalBufferProperties externalBufferProperties)
		=> Parent.Functions.GetPhysicalDeviceExternalBufferProperties(Handle, externalBufferInfo, out externalBufferProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalBufferPropertiesKHR(Vk.PhysicalDeviceExternalBufferInfo* pExternalBufferInfo, Vk.ExternalBufferProperties* pExternalBufferProperties)
		=> Parent.Functions.vkGetPhysicalDeviceExternalBufferPropertiesKHR(Handle, pExternalBufferInfo, pExternalBufferProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalBufferPropertiesKHR(in Vk.PhysicalDeviceExternalBufferInfo externalBufferInfo, out Vk.ExternalBufferProperties externalBufferProperties)
		=> Parent.Functions.GetPhysicalDeviceExternalBufferPropertiesKHR(Handle, externalBufferInfo, out externalBufferProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalSemaphoreProperties(Vk.PhysicalDeviceExternalSemaphoreInfo* pExternalSemaphoreInfo, Vk.ExternalSemaphoreProperties* pExternalSemaphoreProperties)
		=> Parent.Functions.vkGetPhysicalDeviceExternalSemaphoreProperties(Handle, pExternalSemaphoreInfo, pExternalSemaphoreProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalSemaphoreProperties(in Vk.PhysicalDeviceExternalSemaphoreInfo externalSemaphoreInfo, out Vk.ExternalSemaphoreProperties externalSemaphoreProperties)
		=> Parent.Functions.GetPhysicalDeviceExternalSemaphoreProperties(Handle, externalSemaphoreInfo, out externalSemaphoreProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalSemaphorePropertiesKHR(Vk.PhysicalDeviceExternalSemaphoreInfo* pExternalSemaphoreInfo, Vk.ExternalSemaphoreProperties* pExternalSemaphoreProperties)
		=> Parent.Functions.vkGetPhysicalDeviceExternalSemaphorePropertiesKHR(Handle, pExternalSemaphoreInfo, pExternalSemaphoreProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalSemaphorePropertiesKHR(in Vk.PhysicalDeviceExternalSemaphoreInfo externalSemaphoreInfo, out Vk.ExternalSemaphoreProperties externalSemaphoreProperties)
		=> Parent.Functions.GetPhysicalDeviceExternalSemaphorePropertiesKHR(Handle, externalSemaphoreInfo, out externalSemaphoreProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalFenceProperties(Vk.PhysicalDeviceExternalFenceInfo* pExternalFenceInfo, Vk.ExternalFenceProperties* pExternalFenceProperties)
		=> Parent.Functions.vkGetPhysicalDeviceExternalFenceProperties(Handle, pExternalFenceInfo, pExternalFenceProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalFenceProperties(in Vk.PhysicalDeviceExternalFenceInfo externalFenceInfo, out Vk.ExternalFenceProperties externalFenceProperties)
		=> Parent.Functions.GetPhysicalDeviceExternalFenceProperties(Handle, externalFenceInfo, out externalFenceProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalFencePropertiesKHR(Vk.PhysicalDeviceExternalFenceInfo* pExternalFenceInfo, Vk.ExternalFenceProperties* pExternalFenceProperties)
		=> Parent.Functions.vkGetPhysicalDeviceExternalFencePropertiesKHR(Handle, pExternalFenceInfo, pExternalFenceProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetExternalFencePropertiesKHR(in Vk.PhysicalDeviceExternalFenceInfo externalFenceInfo, out Vk.ExternalFenceProperties externalFenceProperties)
		=> Parent.Functions.GetPhysicalDeviceExternalFencePropertiesKHR(Handle, externalFenceInfo, out externalFenceProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result ReleaseDisplayEXT(Vk.KHR.Display display)
		=> Parent.Functions.vkReleaseDisplayEXT(Handle, display);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result AcquireXlibDisplayEXT(void* dpy, Vk.KHR.Display display)
		=> Parent.Functions.vkAcquireXlibDisplayEXT(Handle, dpy, display);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetRandROutputDisplayEXT(void* dpy, ulong rrOutput, Vk.KHR.Display* pDisplay)
		=> Parent.Functions.vkGetRandROutputDisplayEXT(Handle, dpy, rrOutput, pDisplay);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetRandROutputDisplayEXT(void* dpy, ulong rrOutput, out Vk.KHR.Display display)
		=> Parent.Functions.GetRandROutputDisplayEXT(Handle, dpy, rrOutput, out display);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceCapabilities2EXT(Vk.KHR.Surface surface, Vk.EXT.SurfaceCapabilities2* pSurfaceCapabilities)
		=> Parent.Functions.vkGetPhysicalDeviceSurfaceCapabilities2EXT(Handle, surface, pSurfaceCapabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceCapabilities2EXT(Vk.KHR.Surface surface, out Vk.EXT.SurfaceCapabilities2 surfaceCapabilities)
		=> Parent.Functions.GetPhysicalDeviceSurfaceCapabilities2EXT(Handle, surface, out surfaceCapabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetPresentRectanglesKHR(Vk.KHR.Surface surface, uint* pRectCount, Vk.Rect2D* pRects)
		=> Parent.Functions.vkGetPhysicalDevicePresentRectanglesKHR(Handle, surface, pRectCount, pRects);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetPresentRectanglesKHR(Vk.KHR.Surface surface, out uint rectCount, in Span<Vk.Rect2D> rects)
		=> Parent.Functions.GetPhysicalDevicePresentRectanglesKHR(Handle, surface, out rectCount, rects);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetMultisamplePropertiesEXT(Vk.SampleCountFlags samples, Vk.EXT.MultisampleProperties* pMultisampleProperties)
		=> Parent.Functions.vkGetPhysicalDeviceMultisamplePropertiesEXT(Handle, samples, pMultisampleProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetMultisamplePropertiesEXT(Vk.SampleCountFlags samples, out Vk.EXT.MultisampleProperties multisampleProperties)
		=> Parent.Functions.GetPhysicalDeviceMultisamplePropertiesEXT(Handle, samples, out multisampleProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceCapabilities2KHR(Vk.KHR.PhysicalDeviceSurfaceInfo2* pSurfaceInfo, Vk.KHR.SurfaceCapabilities2* pSurfaceCapabilities)
		=> Parent.Functions.vkGetPhysicalDeviceSurfaceCapabilities2KHR(Handle, pSurfaceInfo, pSurfaceCapabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceCapabilities2KHR(in Vk.KHR.PhysicalDeviceSurfaceInfo2 surfaceInfo, out Vk.KHR.SurfaceCapabilities2 surfaceCapabilities)
		=> Parent.Functions.GetPhysicalDeviceSurfaceCapabilities2KHR(Handle, surfaceInfo, out surfaceCapabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceFormats2KHR(Vk.KHR.PhysicalDeviceSurfaceInfo2* pSurfaceInfo, uint* pSurfaceFormatCount, Vk.KHR.SurfaceFormat2* pSurfaceFormats)
		=> Parent.Functions.vkGetPhysicalDeviceSurfaceFormats2KHR(Handle, pSurfaceInfo, pSurfaceFormatCount, pSurfaceFormats);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfaceFormats2KHR(in Vk.KHR.PhysicalDeviceSurfaceInfo2 surfaceInfo, out uint surfaceFormatCount, in Span<Vk.KHR.SurfaceFormat2> surfaceFormats)
		=> Parent.Functions.GetPhysicalDeviceSurfaceFormats2KHR(Handle, surfaceInfo, out surfaceFormatCount, surfaceFormats);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayProperties2KHR(uint* pPropertyCount, Vk.KHR.DisplayProperties2* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceDisplayProperties2KHR(Handle, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayProperties2KHR(out uint propertyCount, in Span<Vk.KHR.DisplayProperties2> properties)
		=> Parent.Functions.GetPhysicalDeviceDisplayProperties2KHR(Handle, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlaneProperties2KHR(uint* pPropertyCount, Vk.KHR.DisplayPlaneProperties2* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceDisplayPlaneProperties2KHR(Handle, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlaneProperties2KHR(out uint propertyCount, in Span<Vk.KHR.DisplayPlaneProperties2> properties)
		=> Parent.Functions.GetPhysicalDeviceDisplayPlaneProperties2KHR(Handle, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayModeProperties2KHR(Vk.KHR.Display display, uint* pPropertyCount, Vk.KHR.DisplayModeProperties2* pProperties)
		=> Parent.Functions.vkGetDisplayModeProperties2KHR(Handle, display, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayModeProperties2KHR(Vk.KHR.Display display, out uint propertyCount, in Span<Vk.KHR.DisplayModeProperties2> properties)
		=> Parent.Functions.GetDisplayModeProperties2KHR(Handle, display, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlaneCapabilities2KHR(Vk.KHR.DisplayPlaneInfo2* pDisplayPlaneInfo, Vk.KHR.DisplayPlaneCapabilities2* pCapabilities)
		=> Parent.Functions.vkGetDisplayPlaneCapabilities2KHR(Handle, pDisplayPlaneInfo, pCapabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetDisplayPlaneCapabilities2KHR(in Vk.KHR.DisplayPlaneInfo2 displayPlaneInfo, out Vk.KHR.DisplayPlaneCapabilities2 capabilities)
		=> Parent.Functions.GetDisplayPlaneCapabilities2KHR(Handle, displayPlaneInfo, out capabilities);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetCalibrateableTimeDomainsEXT(uint* pTimeDomainCount, Vk.EXT.TimeDomain* pTimeDomains)
		=> Parent.Functions.vkGetPhysicalDeviceCalibrateableTimeDomainsEXT(Handle, pTimeDomainCount, pTimeDomains);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetCalibrateableTimeDomainsEXT(out uint timeDomainCount, in Span<Vk.EXT.TimeDomain> timeDomains)
		=> Parent.Functions.GetPhysicalDeviceCalibrateableTimeDomainsEXT(Handle, out timeDomainCount, timeDomains);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetCooperativeMatrixPropertiesNV(uint* pPropertyCount, Vk.NV.CooperativeMatrixProperties* pProperties)
		=> Parent.Functions.vkGetPhysicalDeviceCooperativeMatrixPropertiesNV(Handle, pPropertyCount, pProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetCooperativeMatrixPropertiesNV(out uint propertyCount, in Span<Vk.NV.CooperativeMatrixProperties> properties)
		=> Parent.Functions.GetPhysicalDeviceCooperativeMatrixPropertiesNV(Handle, out propertyCount, properties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfacePresentModes2EXT(Vk.KHR.PhysicalDeviceSurfaceInfo2* pSurfaceInfo, uint* pPresentModeCount, Vk.KHR.PresentMode* pPresentModes)
		=> Parent.Functions.vkGetPhysicalDeviceSurfacePresentModes2EXT(Handle, pSurfaceInfo, pPresentModeCount, pPresentModes);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSurfacePresentModes2EXT(in Vk.KHR.PhysicalDeviceSurfaceInfo2 surfaceInfo, out uint presentModeCount, in Span<Vk.KHR.PresentMode> presentModes)
		=> Parent.Functions.GetPhysicalDeviceSurfacePresentModes2EXT(Handle, surfaceInfo, out presentModeCount, presentModes);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result EnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR(uint queueFamilyIndex, uint* pCounterCount, Vk.KHR.PerformanceCounter* pCounters, Vk.KHR.PerformanceCounterDescription* pCounterDescriptions)
		=> Parent.Functions.vkEnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR(Handle, queueFamilyIndex, pCounterCount, pCounters, pCounterDescriptions);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result EnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR(uint queueFamilyIndex, out uint counterCount, in Span<Vk.KHR.PerformanceCounter> counters, in Span<Vk.KHR.PerformanceCounterDescription> counterDescriptions)
		=> Parent.Functions.EnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR(Handle, queueFamilyIndex, out counterCount, counters, counterDescriptions);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetQueueFamilyPerformanceQueryPassesKHR(Vk.KHR.QueryPoolPerformanceCreateInfo* pPerformanceQueryCreateInfo, uint* pNumPasses)
		=> Parent.Functions.vkGetPhysicalDeviceQueueFamilyPerformanceQueryPassesKHR(Handle, pPerformanceQueryCreateInfo, pNumPasses);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void GetQueueFamilyPerformanceQueryPassesKHR(in Vk.KHR.QueryPoolPerformanceCreateInfo performanceQueryCreateInfo, out uint numPasses)
		=> Parent.Functions.GetPhysicalDeviceQueueFamilyPerformanceQueryPassesKHR(Handle, performanceQueryCreateInfo, out numPasses);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSupportedFramebufferMixedSamplesCombinationsNV(uint* pCombinationCount, Vk.NV.FramebufferMixedSamplesCombination* pCombinations)
		=> Parent.Functions.vkGetPhysicalDeviceSupportedFramebufferMixedSamplesCombinationsNV(Handle, pCombinationCount, pCombinations);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetSupportedFramebufferMixedSamplesCombinationsNV(out uint combinationCount, in Span<Vk.NV.FramebufferMixedSamplesCombination> combinations)
		=> Parent.Functions.GetPhysicalDeviceSupportedFramebufferMixedSamplesCombinationsNV(Handle, out combinationCount, combinations);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetToolPropertiesEXT(uint* pToolCount, Vk.EXT.PhysicalDeviceToolProperties* pToolProperties)
		=> Parent.Functions.vkGetPhysicalDeviceToolPropertiesEXT(Handle, pToolCount, pToolProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetToolPropertiesEXT(out uint toolCount, in Span<Vk.EXT.PhysicalDeviceToolProperties> toolProperties)
		=> Parent.Functions.GetPhysicalDeviceToolPropertiesEXT(Handle, out toolCount, toolProperties);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetFragmentShadingRatesKHR(uint* pFragmentShadingRateCount, Vk.KHR.PhysicalDeviceFragmentShadingRate* pFragmentShadingRates)
		=> Parent.Functions.vkGetPhysicalDeviceFragmentShadingRatesKHR(Handle, pFragmentShadingRateCount, pFragmentShadingRates);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vk.Result GetFragmentShadingRatesKHR(out uint fragmentShadingRateCount, in Span<Vk.KHR.PhysicalDeviceFragmentShadingRate> fragmentShadingRates)
		=> Parent.Functions.GetPhysicalDeviceFragmentShadingRatesKHR(Handle, out fragmentShadingRateCount, fragmentShadingRates);

}

} // namespace VVK
