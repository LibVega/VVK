/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vulkan.VVK
{
	/// <summary>
	/// Collects information and data about the capabilities of a <see cref="VkPhysicalDevice"/>.
	/// </summary>
	public unsafe class DeviceInfo
	{
		#region Fields
		/// <summary>
		/// The physical device instance that the data is associated with.
		/// </summary>
		public readonly VkPhysicalDevice PhysicalDevice;

		/// <summary>
		/// The Vulkan 1.0 device properties.
		/// </summary>
		public readonly VkPhysicalDeviceProperties Properties;
		/// <summary>
		/// The Vulkan 1.1 device properties.
		/// </summary>
		public readonly VkPhysicalDeviceVulkan11Properties Properties11;
		/// <summary>
		/// The Vulkan 1.2 device properties.
		/// </summary>
		public readonly VkPhysicalDeviceVulkan12Properties Properties12;
		/// <summary>
		/// The name of the device.
		/// </summary>
		public string DeviceName => Properties.DeviceName;
		/// <summary>
		/// The Vulkan API version supported by the device.
		/// </summary>
		public VkVersion ApiVersion => new VkVersion(Properties.ApiVersion);
		/// <summary>
		/// If the device is a discrete device.
		/// </summary>
		public bool IsDiscrete => Properties.DeviceType == VkPhysicalDeviceType.DiscreteGpu;
		/// <summary>
		/// If the device is an integrated device.
		/// </summary>
		public bool IsIntegrated => Properties.DeviceType == VkPhysicalDeviceType.IntegratedGpu;

		/// <summary>
		/// The Vulkan 1.0 device features.
		/// </summary>
		public readonly VkPhysicalDeviceFeatures Features;
		/// <summary>
		/// The Vulkan 1.1 device features.
		/// </summary>
		public readonly VkPhysicalDeviceVulkan11Features Features11;
		/// <summary>
		/// The Vulkan 1.2 device features.
		/// </summary>
		public readonly VkPhysicalDeviceVulkan12Features Features12;

		/// <summary>
		/// The memory types supported by the device.
		/// </summary>
		public IReadOnlyList<VkMemoryType> MemoryTypes => _memoryTypes;
		private readonly VkMemoryType[] _memoryTypes;
		/// <summary>
		/// The memory heaps available to the device.
		/// </summary>
		public IReadOnlyList<VkMemoryHeap> MemoryHeaps => _memoryHeaps;
		private readonly VkMemoryHeap[] _memoryHeaps;
		/// <summary>
		/// The number of memory types supported by the device.
		/// </summary>
		public uint MemoryTypeCount => (uint)_memoryTypes.Length;
		/// <summary>
		/// The number of memory heaps available to the device.
		/// </summary>
		public uint MemoryHeapCount => (uint)_memoryHeaps.Length;
		/// <summary>
		/// The total amount of dynamic memory available on the system (including host RAM).
		/// </summary>
		public readonly ulong TotalMemory;
		/// <summary>
		/// The total amount of dynamic memory local to the device (may include host RAM on integrated devices).
		/// </summary>
		public readonly ulong TotalLocalMemory;

		/// <summary>
		/// The queue families available on the device.
		/// </summary>
		public IReadOnlyList<VkQueueFamilyProperties> QueueFamilies => _queueFamilies;
		private readonly VkQueueFamilyProperties[] _queueFamilies;
		/// <summary>
		/// The number of queue families available on the device.
		/// </summary>
		public uint QueueFamilyCount => (uint)_queueFamilies.Length;

		/// <summary>
		/// The extensions supported by the device.
		/// </summary>
		public IReadOnlyList<string> ExtensionNames => _extensions;
		private readonly string[] _extensions;
		#endregion // Fields

		/// <summary>
		/// Create a new data object for the passed device.
		/// </summary>
		/// <param name="device">The physical device to collate data for.</param>
		public DeviceInfo(VkPhysicalDevice device)
		{
			if (!device) {
				throw new ArgumentNullException(nameof(device), "Cannot pass null device or device handle");
			}
			PhysicalDevice = device;

			// Get Data
			GetProperties(device, out Properties, out Properties11, out Properties12);
			GetFeatures(device, out Features, out Features11, out Features12);
			GetMemoryInfo(device, out _memoryTypes, out _memoryHeaps);
			_extensions = GetExtensions(device).Select(ext => ext.ExtensionName.ToString() ?? String.Empty).ToArray();
			_queueFamilies = GetQueueFamilies(device);

			// Process data
			TotalMemory = (ulong)_memoryHeaps.Sum(heap => (long)heap.Size);
			TotalLocalMemory = (ulong)_memoryHeaps.Sum(heap =>
				((heap.Flags & VkMemoryHeapFlags.DeviceLocal) > 0) ? (long)heap.Size : 0);
		}

		/// <summary>
		/// Searches the available memory types for one matching the set of passed flags. Search order is
		/// <list type="number">
		/// <item><c>notPref</c> and <c>req | pref</c></item>
		/// <item><c>notPref</c> and <c>req</c></item>
		/// <item><c>req | pref</c></item>
		/// <item><c>req</c></item>
		/// </list>
		/// </summary>
		/// <param name="mask">The memory type mask associated with memory requirements.</param>
		/// <param name="req">The required flags.</param>
		/// <param name="pref">The preferred flags.</param>
		/// <param name="prefNot">The flags that are preferred to not be present.</param>
		/// <returns>The memory type index, or <c>null</c> if a matching type was not found.</returns>
		public uint? FindMemoryType(uint mask, VkMemoryPropertyFlags req, VkMemoryPropertyFlags pref,
			VkMemoryPropertyFlags prefNot = VkMemoryPropertyFlags.NoFlags)
		{
			var noSet = stackalloc VkMemoryPropertyFlags[2] { prefNot, VkMemoryPropertyFlags.NoFlags };
			var yesSet = stackalloc VkMemoryPropertyFlags[2] { req | pref, req };

			foreach (var noMask in (new ReadOnlySpan<VkMemoryPropertyFlags>(noSet, 2))) {
				foreach (var yesMask in (new ReadOnlySpan<VkMemoryPropertyFlags>(yesSet, 2))) {
					for (uint mi = 0, mbit = 1; mi < MemoryTypeCount; ++mi, mbit <<= 1) {
						var maskPass = (mask & mbit) != 0;
						var yesPass = (_memoryTypes[mi].PropertyFlags & yesMask) == yesMask;
						var noPass = (_memoryTypes[mi].PropertyFlags & noMask) == 0;
						if (maskPass && yesPass && noPass) {
							return mi;
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Searches the available queue familities for one matching the passed flags.
		/// </summary>
		/// <param name="req">The queue flags required for the family.</param>
		/// <param name="reqNot">The queue flags required to be not present for the family.</param>
		/// <returns>The queue family index, or <c>null</c> if a matching family was not found.</returns>
		public uint? FindQueueFamily(VkQueueFlags req, VkQueueFlags reqNot = VkQueueFlags.NoFlags)
		{
			for (uint fi = 0; fi < QueueFamilyCount; ++fi) {
				var yesPass = (_queueFamilies[fi].QueueFlags & req) == req;
				var noPass = (_queueFamilies[fi].QueueFlags & reqNot) == 0;
				if (yesPass && noPass) {
					return fi;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the version-aware properties associated with the physical device.
		/// </summary>
		/// <param name="device">The device to get the properties for.</param>
		/// <param name="props">The Vulkan 1.0 properties.</param>
		/// <param name="props11">The Vulkan 1.1 properties.</param>
		/// <param name="props12">The Vulkan 1.2 properties.</param>
		public static void GetProperties(VkPhysicalDevice device,
			out VkPhysicalDeviceProperties props,
			out VkPhysicalDeviceVulkan11Properties props11,
			out VkPhysicalDeviceVulkan12Properties props12)
		{
			if (!device) {
				throw new ArgumentNullException(nameof(device), "Cannot pass null device or device handle");
			}

			VkPhysicalDeviceProperties.New(out props);
			VkPhysicalDeviceVulkan11Properties.New(out props11);
			VkPhysicalDeviceVulkan12Properties.New(out props12);
			var version = device.Parent.Functions.CoreVersion;

			// 1.0
			if (version < VkVersion.VK_VERSION_1_1) {
				device.GetPhysicalDeviceProperties(out props);
			}
			// 1.1
			else if (version < VkVersion.VK_VERSION_1_2) {
				VkPhysicalDeviceProperties2.New(out var props2);
				fixed (VkPhysicalDeviceVulkan11Properties* ptr11 = &props11) {
					props2.pNext = ptr11;
					device.GetPhysicalDeviceProperties2(&props2);
					props = props2.Properties;
				}
			}
			// 1.2
			else {
				VkPhysicalDeviceProperties2.New(out var props2);
				fixed (VkPhysicalDeviceVulkan11Properties* ptr11 = &props11)
				fixed (VkPhysicalDeviceVulkan12Properties* ptr12 = &props12) {
					props2.pNext = ptr11;
					ptr11->pNext = ptr12;
					device.GetPhysicalDeviceProperties2(&props2);
					props = props2.Properties;
					ptr11->pNext = null;
				}
			}
		}

		/// <summary>
		/// Gets the version-aware features associated with the physical device.
		/// </summary>
		/// <param name="device">The device to get the features for.</param>
		/// <param name="props">The Vulkan 1.0 features.</param>
		/// <param name="props11">The Vulkan 1.1 features.</param>
		/// <param name="props12">The Vulkan 1.2 features.</param>
		public static void GetFeatures(VkPhysicalDevice device,
			out VkPhysicalDeviceFeatures feats,
			out VkPhysicalDeviceVulkan11Features feats11,
			out VkPhysicalDeviceVulkan12Features feats12)
		{
			if (!device) {
				throw new ArgumentNullException(nameof(device), "Cannot pass null device or device handle");
			}

			VkPhysicalDeviceFeatures.New(out feats);
			VkPhysicalDeviceVulkan11Features.New(out feats11);
			VkPhysicalDeviceVulkan12Features.New(out feats12);
			var version = device.Parent.Functions.CoreVersion;

			// 1.0
			if (version < VkVersion.VK_VERSION_1_1) {
				device.GetPhysicalDeviceFeatures(out feats);
			}
			// 1.1
			else if (version < VkVersion.VK_VERSION_1_2) {
				VkPhysicalDeviceFeatures2.New(out var feats2);
				fixed (VkPhysicalDeviceVulkan11Features* ptr11 = &feats11) {
					feats2.pNext = ptr11;
					device.GetPhysicalDeviceFeatures2(&feats2);
					feats = feats2.Features;
				}
			}
			// 1.2
			else {
				VkPhysicalDeviceFeatures2.New(out var feats2);
				fixed (VkPhysicalDeviceVulkan11Features* ptr11 = &feats11)
				fixed (VkPhysicalDeviceVulkan12Features* ptr12 = &feats12) {
					feats2.pNext = ptr11;
					ptr11->pNext = ptr12;
					device.GetPhysicalDeviceFeatures2(&feats2);
					feats = feats2.Features;
					ptr11->pNext = null;
				}
			}
		}

		/// <summary>
		/// Gets the memory heaps and types for the device.
		/// </summary>
		/// <param name="device">The device to get the memory info for.</param>
		/// <param name="types">The memory types for the device.</param>
		/// <param name="heaps">The memory heaps for the device.</param>
		public static void GetMemoryInfo(VkPhysicalDevice device,
			out VkMemoryType[] types, out VkMemoryHeap[] heaps)
		{
			if (!device) {
				throw new ArgumentNullException(nameof(device), "Cannot pass null device or device handle");
			}

			VkPhysicalDeviceMemoryProperties.New(out var props);
			device.GetPhysicalDeviceMemoryProperties(&props);
			VkMemoryType* typePtr = &(props.MemoryTypes_0);
			VkMemoryHeap* heapPtr = &(props.MemoryHeaps_0);

			types = new VkMemoryType[props.MemoryTypeCount];
			heaps = new VkMemoryHeap[props.MemoryHeapCount];
			for (uint i = 0; i < props.MemoryTypeCount; ++i) {
				types[i] = typePtr[i];
			}
			for (uint i = 0; i < props.MemoryHeapCount; ++i) {
				heaps[i] = heapPtr[i];
			}
		}

		/// <summary>
		/// Gets the queue families available on the passed device.
		/// </summary>
		/// <param name="device">The device to get the queue families on.</param>
		public static VkQueueFamilyProperties[] GetQueueFamilies(VkPhysicalDevice device)
		{
			if (!device) {
				throw new ArgumentNullException(nameof(device), "Cannot pass null device or device handle");
			}

			uint famCount;
			device.GetPhysicalDeviceQueueFamilyProperties(&famCount, null);
			var fams = new VkQueueFamilyProperties[famCount];
			fixed (VkQueueFamilyProperties* famPtr = fams) {
				device.GetPhysicalDeviceQueueFamilyProperties(&famCount, famPtr);
			}
			return fams;
		}

		/// <summary>
		/// Gets the extension properties available on the passed device.
		/// </summary>
		/// <param name="device">The device to get the extensions on.</param>
		public static VkExtensionProperties[] GetExtensions(VkPhysicalDevice device)
		{
			if (!device) {
				throw new ArgumentNullException(nameof(device), "Cannot pass null device or device handle");
			}

			uint extCount;
			device.EnumerateDeviceExtensionProperties(null, &extCount, null).Throw("EnumerateDeviceExtensionProperties");
			var props = new VkExtensionProperties[extCount];
			fixed (VkExtensionProperties* extPtr = props) {
				device.EnumerateDeviceExtensionProperties(null, &extCount, extPtr).Throw("EnumerateDeviceExtensionProperties");
			}
			return props;
		}
	}
}
