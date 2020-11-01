/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.CompilerServices;

namespace Vk
{
	// Extensions for VkInstance
	public unsafe partial class Instance
	{
		/// <summary>vkEnumeratePhysicalDevices</summary>
		public Vk.Result EnumeratePhysicalDevices(out Vk.PhysicalDevice[] physicalDevices)
		{
			if (Functions.vkEnumeratePhysicalDevices == null) throw new Vk.Extras.FunctionNotLoadedException("vkEnumeratePhysicalDevices");
			physicalDevices = Array.Empty<Vk.PhysicalDevice>();

			uint count = 0;
			var res = Functions.vkEnumeratePhysicalDevices(Handle, &count, null);
			if (res != Vk.Result.Success) {
				return res;
			}

			var hptr = stackalloc Vk.Handle<Vk.PhysicalDevice>[(int)count];
			res = Functions.vkEnumeratePhysicalDevices(Handle, &count, hptr);
			if (res == Vk.Result.Success) {
				physicalDevices = new Vk.PhysicalDevice[count];
				for (uint i = 0; i < count; ++i) {
					physicalDevices[i] = new Vk.PhysicalDevice(this, hptr[i]);
				}
			}
			return res;
		}
	}

	// Extensions for VkPhysicalDevice
	public unsafe partial class PhysicalDevice
	{
		/// <summary>vkGetDisplayPlaneSupportedDisplaysKHR</summary>
		public Vk.Result GetDisplayPlaneSupportedDisplaysKHR(uint planeIndex, out Vk.KHR.Display[] displays)
		{
			if (Functions.vkGetDisplayPlaneSupportedDisplaysKHR == null) throw new Vk.Extras.FunctionNotLoadedException("vkGetDisplayPlaneSupportedDisplaysKHR");
			displays = Array.Empty<Vk.KHR.Display>();

			uint count = 0;
			var res = Functions.vkGetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, &count, null);
			if (res != Vk.Result.Success) {
				return res;
			}

			var hptr = stackalloc Vk.Handle<Vk.KHR.Display>[(int)count];
			res = Functions.vkGetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, &count, hptr);
			if (res == Vk.Result.Success) {
				displays = new Vk.KHR.Display[count];
				for (uint i = 0; i < count; ++i) {
					displays[i] = new Vk.KHR.Display(this, hptr[i]);
				}
			}
			return res;
		}
	}

	// Extensions for VkDevice
	public unsafe partial class Device
	{
		/// <summary>vkCreateGraphicsPipelines</summary>
		public Vk.Result CreateGraphicsPipelines(Vk.Handle<Vk.PipelineCache> pipelineCache, uint createInfoCount, Vk.GraphicsPipelineCreateInfo* pCreateInfos, Vk.AllocationCallbacks* pAllocator, out Vk.Pipeline[] pipelines)
		{
			if (Functions.vkCreateGraphicsPipelines == null) throw new Vk.Extras.FunctionNotLoadedException("vkCreateGraphicsPipelines");

			var handles = stackalloc Vk.Handle<Vk.Pipeline>[(int)createInfoCount];
			var res = Functions.vkCreateGraphicsPipelines(Handle, pipelineCache, createInfoCount, pCreateInfos, pAllocator, handles);
			if (res == Vk.Result.Success) {
				pipelines = new Vk.Pipeline[createInfoCount];
				for (uint i = 0; i < createInfoCount; ++i) {
					pipelines[i] = new Vk.Pipeline(this, handles[i]);
				}
			}
			else {
				pipelines = Array.Empty<Vk.Pipeline>();
			}
			return res;
		}

		/// <summary>vkCreateGraphicsPipelines</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vk.Result CreateGraphicsPipelines(Vk.Handle<Vk.PipelineCache> pipelineCache, in ReadOnlySpan<Vk.GraphicsPipelineCreateInfo> createInfos, in Vk.AllocationCallbacks allocator, out Vk.Pipeline[] pipelines)
		{
			fixed (Vk.GraphicsPipelineCreateInfo* pCreateInfos = createInfos) {
			fixed (Vk.AllocationCallbacks* pAllocator = &allocator) {
				return CreateGraphicsPipelines(pipelineCache, (uint)createInfos.Length, pCreateInfos, pAllocator, out pipelines);
			}
			}
		}

		/// <summary>vkCreateComputePipelines</summary>
		public Vk.Result CreateComputePipelines(Vk.Handle<Vk.PipelineCache> pipelineCache, uint createInfoCount, Vk.ComputePipelineCreateInfo* pCreateInfos, Vk.AllocationCallbacks* pAllocator, out Vk.Pipeline[] pipelines)
		{
			if (Functions.vkCreateComputePipelines == null) throw new Vk.Extras.FunctionNotLoadedException("vkCreateComputePipelines");

			var handles = stackalloc Vk.Handle<Vk.Pipeline>[(int)createInfoCount];
			var res = Functions.vkCreateComputePipelines(Handle, pipelineCache, createInfoCount, pCreateInfos, pAllocator, handles);
			if (res == Vk.Result.Success) {
				pipelines = new Vk.Pipeline[createInfoCount];
				for (uint i = 0; i < createInfoCount; ++i) {
					pipelines[i] = new Vk.Pipeline(this, handles[i]);
				}
			}
			else {
				pipelines = Array.Empty<Vk.Pipeline>();
			}
			return res;
		}

		/// <summary>vkCreateComputePipelines</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vk.Result CreateComputePipelines(Vk.Handle<Vk.PipelineCache> pipelineCache, in ReadOnlySpan<Vk.ComputePipelineCreateInfo> createInfos, in Vk.AllocationCallbacks allocator, out Vk.Pipeline[] pipelines)
		{
			fixed (Vk.ComputePipelineCreateInfo* pCreateInfos = createInfos) {
			fixed (Vk.AllocationCallbacks* pAllocator = &allocator) {
				return CreateComputePipelines(pipelineCache, (uint)createInfos.Length, pCreateInfos, pAllocator, out pipelines);
			}
			}
		}

		/// <summary>vkAllocateDescriptorSets</summary>
		public Vk.Result AllocateDescriptorSets(Vk.DescriptorSetAllocateInfo* pAllocateInfo, out Vk.DescriptorSet[] descriptorSets)
		{
			if (Functions.vkAllocateDescriptorSets == null) throw new Vk.Extras.FunctionNotLoadedException("vkAllocateDescriptorSets");

			var handles = stackalloc Vk.Handle<Vk.DescriptorSet>[(int)pAllocateInfo->DescriptorSetCount];
			var res = Functions.vkAllocateDescriptorSets(Handle, pAllocateInfo, handles);
			if (res == Vk.Result.Success) {
				var parent = new Vk.DescriptorPool(this, pAllocateInfo->DescriptorPool);
				descriptorSets = new Vk.DescriptorSet[pAllocateInfo->DescriptorSetCount];
				for (uint i = 0; i < pAllocateInfo->DescriptorSetCount; ++i) {
					descriptorSets[i] = new Vk.DescriptorSet(parent, handles[i]);
				}
			}
			else {
				descriptorSets = Array.Empty<Vk.DescriptorSet>();
			}
			return res;
		}

		/// <summary>vkAllocateDescriptorSets</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vk.Result AllocateDescriptorSets(in Vk.DescriptorSetAllocateInfo allocateInfo, out Vk.DescriptorSet[] descriptorSets)
		{
			fixed (Vk.DescriptorSetAllocateInfo* pAllocateInfo = &allocateInfo) {
				return AllocateDescriptorSets(pAllocateInfo, out descriptorSets);
			}
		}

		/// <summary>vkAllocateCommandBuffers</summary>
		public Vk.Result AllocateCommandBuffers(Vk.CommandBufferAllocateInfo* pAllocateInfo, out Vk.CommandBuffer[] commandBuffers)
		{
			if (Functions.vkAllocateCommandBuffers == null) throw new Vk.Extras.FunctionNotLoadedException("vkAllocateCommandBuffers");

			var handles = stackalloc Vk.Handle<Vk.CommandBuffer>[(int)pAllocateInfo->CommandBufferCount];
			var res = Functions.vkAllocateCommandBuffers(Handle, pAllocateInfo, handles);
			if (res == Vk.Result.Success) {
				var parent = new Vk.CommandPool(this, pAllocateInfo->CommandPool);
				commandBuffers = new Vk.CommandBuffer[pAllocateInfo->CommandBufferCount];
				for (uint i = 0; i < pAllocateInfo->CommandBufferCount; ++i) {
					commandBuffers[i] = new Vk.CommandBuffer(parent, handles[i]);
				}
			}
			else {
				commandBuffers = Array.Empty<Vk.CommandBuffer>();
			}
			return res;
		}

		/// <summary>vkAllocateCommandBuffers</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vk.Result AllocateCommandBuffers(in Vk.CommandBufferAllocateInfo allocateInfo, out Vk.CommandBuffer[] commandBuffers)
		{
			fixed (Vk.CommandBufferAllocateInfo* pAllocateInfo = &allocateInfo) {
				return AllocateCommandBuffers(pAllocateInfo, out commandBuffers);
			}
		}

		/// <summary>vkCreateSharedSwapchainsKHR</summary>
		public Vk.Result CreateSharedSwapchainsKHR(uint swapchainCount, Vk.KHR.SwapchainCreateInfo* pCreateInfos, Vk.AllocationCallbacks* pAllocator, out Vk.KHR.Swapchain[] swapchains)
		{
			if (Functions.vkCreateSharedSwapchainsKHR == null) throw new Vk.Extras.FunctionNotLoadedException("vkCreateSharedSwapchainsKHR");

			var handles = stackalloc Vk.Handle<Vk.KHR.Swapchain>[(int)swapchainCount];
			var res = Functions.vkCreateSharedSwapchainsKHR(Handle, swapchainCount, pCreateInfos, pAllocator, handles);
			if (res == Vk.Result.Success) {
				swapchains = new Vk.KHR.Swapchain[swapchainCount];
				for (uint i = 0; i < swapchainCount; ++i) {
					swapchains[i] = new Vk.KHR.Swapchain(this, handles[i]);
				}
			}
			else {
				swapchains = Array.Empty<Vk.KHR.Swapchain>();
			}
			return res;
		}

		/// <summary>vkCreateSharedSwapchainsKHR</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vk.Result CreateSharedSwapchainsKHR(in ReadOnlySpan<Vk.KHR.SwapchainCreateInfo> createInfos, in Vk.AllocationCallbacks allocator, out Vk.KHR.Swapchain[] swapchains)
		{
			fixed (Vk.KHR.SwapchainCreateInfo* pCreateInfos = createInfos) {
			fixed (Vk.AllocationCallbacks* pAllocator = &allocator) {
				return CreateSharedSwapchainsKHR((uint)createInfos.Length, pCreateInfos, pAllocator, out swapchains);
			}
			}
		}

		/// <summary>vkCreateRayTracingPipelinesNV</summary>
		public Vk.Result CreateRayTracingPipelinesNV(Vk.Handle<Vk.PipelineCache> pipelineCache, uint createInfoCount, Vk.NV.RayTracingPipelineCreateInfo* pCreateInfos, Vk.AllocationCallbacks* pAllocator, out Vk.Pipeline[] pipelines)
		{
			if (Functions.vkCreateRayTracingPipelinesNV == null) throw new Vk.Extras.FunctionNotLoadedException("vkCreateRayTracingPipelinesNV");

			var handles = stackalloc Vk.Handle<Vk.Pipeline>[(int)createInfoCount];
			var res = Functions.vkCreateRayTracingPipelinesNV(Handle, pipelineCache, createInfoCount, pCreateInfos, pAllocator, handles);
			if (res == Vk.Result.Success) {
				pipelines = new Vk.Pipeline[createInfoCount];
				for (uint i = 0; i < createInfoCount; ++i) {
					pipelines[i] = new Vk.Pipeline(this, handles[i]);
				}
			}
			else {
				pipelines = Array.Empty<Vk.Pipeline>();
			}
			return res;
		}

		/// <summary>vkCreateRayTracingPipelinesNV</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vk.Result CreateRayTracingPipelinesNV(Vk.Handle<Vk.PipelineCache> pipelineCache, in ReadOnlySpan<Vk.NV.RayTracingPipelineCreateInfo> createInfos, in Vk.AllocationCallbacks allocator, out Vk.Pipeline[] pipelines)
		{
			fixed (Vk.NV.RayTracingPipelineCreateInfo* pCreateInfos = createInfos) {
			fixed (Vk.AllocationCallbacks* pAllocator = &allocator) {
				return CreateRayTracingPipelinesNV(pipelineCache, (uint)createInfos.Length, pCreateInfos, pAllocator, out pipelines);
			}
			}
		}

		/// <summary>vkCreateRayTracingPipelinesKHR</summary>
		public Vk.Result CreateRayTracingPipelinesKHR(Vk.Handle<Vk.PipelineCache> pipelineCache, uint createInfoCount, Vk.KHR.RayTracingPipelineCreateInfo* pCreateInfos, Vk.AllocationCallbacks* pAllocator, out Vk.Pipeline[] pipelines)
		{
			if (Functions.vkCreateRayTracingPipelinesKHR == null) throw new Vk.Extras.FunctionNotLoadedException("vkCreateRayTracingPipelinesKHR");

			var handles = stackalloc Vk.Handle<Vk.Pipeline>[(int)createInfoCount];
			var res = Functions.vkCreateRayTracingPipelinesKHR(Handle, pipelineCache, createInfoCount, pCreateInfos, pAllocator, handles);
			if (res == Vk.Result.Success) {
				pipelines = new Vk.Pipeline[createInfoCount];
				for (uint i = 0; i < createInfoCount; ++i) {
					pipelines[i] = new Vk.Pipeline(this, handles[i]);
				}
			}
			else {
				pipelines = Array.Empty<Vk.Pipeline>();
			}
			return res;
		}

		/// <summary>vkCreateRayTracingPipelinesKHR</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vk.Result CreateRayTracingPipelinesKHR(Vk.Handle<Vk.PipelineCache> pipelineCache, in ReadOnlySpan<Vk.KHR.RayTracingPipelineCreateInfo> createInfos, in Vk.AllocationCallbacks allocator, out Vk.Pipeline[] pipelines)
		{
			fixed (Vk.KHR.RayTracingPipelineCreateInfo* pCreateInfos = createInfos) {
			fixed (Vk.AllocationCallbacks* pAllocator = &allocator) {
				return CreateRayTracingPipelinesKHR(pipelineCache, (uint)createInfos.Length, pCreateInfos, pAllocator, out pipelines);
			}
			}
		}
	}
}
