/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VVK
{
	/// <summary>
	/// Wraps a VkPhysicalDevice object at a higher level, providing an OOP approach to VkPhysicalDevice resources and 
	/// functions.
	/// </summary>
	public unsafe sealed partial class VulkanPhysicalDevice
	{
		// Field infos for the device feature types
		private static readonly Type BOOL32_TYPE = typeof(Vk.Bool32);
		private static readonly List<FieldInfo> FIELDS_1_0 =
			typeof(Vk.PhysicalDeviceFeatures).GetFields().Where(fi => fi.FieldType == BOOL32_TYPE).ToList();
		private static readonly List<FieldInfo> FIELDS_1_1 =
			typeof(Vk.PhysicalDeviceVulkan11Features).GetFields().Where(fi => fi.FieldType == BOOL32_TYPE).ToList();
		private static readonly List<FieldInfo> FIELDS_1_2 =
			typeof(Vk.PhysicalDeviceVulkan12Features).GetFields().Where(fi => fi.FieldType == BOOL32_TYPE).ToList();

		#region Fields
		/// <summary>
		/// The parent instance that owns this physical device.
		/// </summary>
		public readonly VulkanInstance Parent;
		/// <summary>
		/// The API handle for this physical device.
		/// </summary>
		public readonly Vk.PhysicalDevice Handle;

		/// <summary>
		/// The properties for the physical device.
		/// </summary>
		public readonly Vk.PhysicalDeviceProperties Properties;
		/// <summary>
		/// The core Vulkan 1.1 properties for the physical device.
		/// </summary>
		public readonly Vk.PhysicalDeviceVulkan11Properties Properties11;
		/// <summary>
		/// The core Vulkan 1.2 properties for the physical device.
		/// </summary>
		public readonly Vk.PhysicalDeviceVulkan12Properties Properties12;
		/// <summary>
		/// The features for the physical device.
		/// </summary>
		public readonly Vk.PhysicalDeviceFeatures Features;
		/// <summary>
		/// The core Vulkan 1.1 features for the physical device.
		/// </summary>
		public readonly Vk.PhysicalDeviceVulkan11Features Features11;
		/// <summary>
		/// The core Vulkan 1.2 features for the physical device.
		/// </summary>
		public readonly Vk.PhysicalDeviceVulkan12Features Features12;
		/// <summary>
		/// The memory properties for the physical device.
		/// </summary>
		public readonly Vk.PhysicalDeviceMemoryProperties MemoryProperties;

		/// <summary>
		/// The properties for the queue families supported by the physical device.
		/// </summary>
		public IReadOnlyList<Vk.QueueFamilyProperties> QueueFamilies => _queueFamilies;
		private readonly List<Vk.QueueFamilyProperties> _queueFamilies;

		/// <summary>
		/// The list of extensions supported by the physical device.
		/// </summary>
		public IReadOnlyList<string> Extensions => _extensions;
		private readonly List<string> _extensions;

		/// <summary>
		/// The name of the device.
		/// </summary>
		public string Name => Properties.DeviceName;
		/// <summary>
		/// The device type.
		/// </summary>
		public Vk.PhysicalDeviceType Type => Properties.DeviceType;
		/// <summary>
		/// The maximum Vulkan API version supported by the device with the current instance.
		/// </summary>
		public Vk.Version ApiVersion => new(Math.Min(Properties.ApiVersion, Parent.ApiVersion));
		/// <summary>
		/// The version of the driver.
		/// </summary>
		public Vk.Version DriverVersion => new(Properties.DriverVersion);
		/// <summary>
		/// The vendor for the physical device.
		/// </summary>
		public uint VendorId => Properties.VendorID;
		#endregion // Fields

		/// <summary>
		/// Creates a new physical device wrapper from an existing instance and physical device handle.
		/// </summary>
		/// <param name="parent">The instance that owns the physical device.</param>
		/// <param name="handle">The existing physical device handle.</param>
		public VulkanPhysicalDevice(VulkanInstance parent, Vk.PhysicalDevice handle)
		{
			Parent = parent.Handle ? parent : throw new ArgumentNullException(nameof(parent), "Null parent instance");
			Handle = handle ? handle : throw new ArgumentNullException(nameof(handle), "Null physical device handle");

			// Get the features/properties
			GetDeviceProperties(this, out Properties, out Properties11, out Properties12);
			GetDeviceFeatures(this, out Features, out Features11, out Features12);
			Vk.PhysicalDeviceMemoryProperties memProps;
			GetMemoryProperties(&memProps);
			MemoryProperties = memProps;

			// Get the queue families
			uint count = 0;
			GetQueueFamilyProperties(&count, null);
			var queues = stackalloc Vk.QueueFamilyProperties[(int)count];
			GetQueueFamilyProperties(&count, queues);
			_queueFamilies = new();
			for (uint i = 0; i < count; ++i) {
				_queueFamilies.Add(queues[i]);
			}

			// Get the extensions
			count = 0;
			EnumerateDeviceExtensionProperties(null, &count, null);
			var exts = stackalloc Vk.ExtensionProperties[(int)count];
			EnumerateDeviceExtensionProperties(null, &count, exts);
			_extensions = new();
			for (uint i = 0; i < count; ++i) {
				_extensions.Add(exts[i].ExtensionName);
			}
		}

		/// <summary>
		/// Searches for a memory type that matches the given requirements, returns <c>null</c> if no type was found.
		/// </summary>
		/// <param name="mask">
		/// The type requirements mask taken from <see cref="Vk.MemoryRequirements.MemoryTypeBits"/>.
		/// </param>
		/// <param name="required">The memory flags required for the memory type.</param>
		/// <param name="preferred">The memory flags that are preferred for the memory type.</param>
		/// <param name="not">The flags that are preferred to not be present for the memory type.</param>
		public uint? FindMemoryType(uint mask, Vk.MemoryPropertyFlags required, Vk.MemoryPropertyFlags preferred,
			Vk.MemoryPropertyFlags not)
		{
			ReadOnlySpan<Vk.MemoryPropertyFlags> nots = stackalloc[] { not, Vk.MemoryPropertyFlags.NoFlags };
			ReadOnlySpan<Vk.MemoryPropertyFlags> yess = stackalloc[] { required | preferred, required };

			fixed (Vk.MemoryType* typePtr = &MemoryProperties.MemoryTypes_0) {
				foreach (var notMask in nots) {
					foreach (var yesMask in yess) {
						for (uint mi = 0, mbit = 1; mi < MemoryProperties.MemoryTypeCount; ++mi, mbit <<= 1) {
							var maskPass = (mask & mbit) != 0;
							var yesPass = (typePtr[mi].PropertyFlags & yesMask) == yesMask;
							var notPass = (typePtr[mi].PropertyFlags & notMask) == 0;
							if (maskPass && yesPass && notPass) {
								return mi;
							}
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the queue family with the given flags, returns <c>null</c> if a valid family was not found.
		/// </summary>
		/// <param name="flags">The required flags for the family.</param>
		/// <param name="not">The flags that cannot be present.</param>
		public uint? FindQueueFamilyType(Vk.QueueFlags flags, Vk.QueueFlags not = default)
		{
			uint fidx = 0;
			foreach (var family in _queueFamilies) {
				if (((family.QueueFlags & flags) == flags) && ((family.QueueFlags & not) == 0)) return fidx;
				++fidx;
			}
			return null;
		}

		/// <summary>
		/// Create a logical device object from the physical device.
		/// </summary>
		/// <param name="features">The core Vulkan 1.0 features to enable.</param>
		/// <param name="queues">The list of device queue infos.</param>
		/// <param name="extensions">The list of extensions to enable on the device.</param>
		/// <param name="features11">The core Vulkan 1.1 features to enable.</param>
		/// <param name="features12">The core Vulkan 1.2 features to enable.</param>
		public VulkanDevice CreateDevice(
			in Vk.PhysicalDeviceFeatures features,
			IEnumerable<Vk.DeviceQueueCreateInfo> queues,
			IEnumerable<string>? extensions = null,
			Vk.PhysicalDeviceVulkan11Features features11 = default, 
			Vk.PhysicalDeviceVulkan12Features features12 = default
		)
		{
			// Check 1.0 features
			foreach (var flag in FIELDS_1_0) {
				var req = (Vk.Bool32)flag.GetValue(features)!;
				var has = (Vk.Bool32)flag.GetValue(Features)!;
				if (req && !has) {
					throw new ArgumentException(
						$"Requested device feature '{flag.Name}' is not available", nameof(features));
				}
			}
			// Check 1.1 features
			foreach (var flag in FIELDS_1_1) {
				var req = (Vk.Bool32)flag.GetValue(features11)!;
				var has = (Vk.Bool32)flag.GetValue(Features11)!;
				if (req && !has) {
					throw new ArgumentException(
						$"Requested device feature '{flag.Name}' is not available", nameof(features));
				}
			}
			// Check 1.2 features
			foreach (var flag in FIELDS_1_2) {
				var req = (Vk.Bool32)flag.GetValue(features12)!;
				var has = (Vk.Bool32)flag.GetValue(Features12)!;
				if (req && !has) {
					throw new ArgumentException(
						$"Requested device feature '{flag.Name}' is not available", nameof(features));
				}
			}
			// Check extensions
			if (extensions?.FirstOrDefault(ext => !Extensions.Contains(ext)) is string missingExt) {
				throw new ArgumentException(
					$"The requested extension device '{missingExt}' is not available", nameof(extensions));
			}

			// Check queues
			var queueInfos = queues.ToArray();
			if (queueInfos.Sum(qi => qi.QueueCount) == 0) {
				throw new ArgumentException(
					"At least one queue must be created for the device", nameof(queues));
			}
			if (queueInfos.GroupBy(qi => qi.QueueFamilyIndex).Where(g => g.Count() > 1).FirstOrDefault() is 
					IGrouping<uint, Vk.DeviceQueueCreateInfo> dup) {
				throw new ArgumentException(
					$"Duplicate queue entry for family {dup.Key} is not allowed", nameof(queues));
			}
			if (queueInfos.Any(qi => qi.QueueFamilyIndex >= _queueFamilies.Count)) {
				throw new ArgumentException("Invalid queue family index provided", nameof(queues));
			}
			if (queueInfos.Any(qi => qi.QueueCount > _queueFamilies[(int)qi.QueueFamilyIndex].QueueCount)) {
				throw new ArgumentException("Invalid queue count (too many queues)", nameof(queues));
			}

			// Prepare the device info
			using var extNames = new NativeStringList(extensions ?? Enumerable.Empty<string>());
			Vk.PhysicalDeviceFeatures2.New(out var f2);
			f2.Features = features;
			if (features11.sType != Vk.PhysicalDeviceVulkan11Features.TYPE) {
				Vk.PhysicalDeviceVulkan11Features.New(out features11);
			}
			if (features12.sType != Vk.PhysicalDeviceVulkan12Features.TYPE) {
				Vk.PhysicalDeviceVulkan12Features.New(out features12);
			}
			if (ApiVersion >= Vk.Version.VK_VERSION_1_1) {
				f2.pNext = &features11;
			}
			if (ApiVersion >= Vk.Version.VK_VERSION_1_2) {
				features11.pNext = &features12;
			}

			// Create the device info
			Vk.Device handle;
			fixed (Vk.DeviceQueueCreateInfo* queuePtr = queueInfos) {
				Vk.DeviceCreateInfo.New(out var dci);
				dci.QueueCreateInfoCount = (uint)queueInfos.Length;
				dci.QueueCreateInfos = queuePtr;
				dci.EnabledExtensionCount = extNames.Count;
				dci.EnabledExtensionNames = extNames.Data;
				dci.EnabledFeatures = (ApiVersion < Vk.Version.VK_VERSION_1_1) ? &(f2.Features) : null;
				dci.pNext = (ApiVersion < Vk.Version.VK_VERSION_1_1) ? null : &f2;
				CreateDevice(&dci, null, &handle).Throw();
			}

			// Return
			return new VulkanDevice(Parent, this, handle, ApiVersion);
		}

		private static void GetDeviceProperties(
			VulkanPhysicalDevice device,
			out Vk.PhysicalDeviceProperties props10,
			out Vk.PhysicalDeviceVulkan11Properties props11,
			out Vk.PhysicalDeviceVulkan12Properties props12
		)
		{
			Vk.PhysicalDeviceProperties props;
			device.GetProperties(&props);
			props10 = props;

			// Can only use properties2 on Vulkan >= 1.1
			Vk.Version apiv = new(Math.Min(props.ApiVersion, device.Parent.ApiVersion));
			if (apiv >= Vk.Version.VK_VERSION_1_1) {
				Vk.PhysicalDeviceProperties2.New(out var p10);
				Vk.PhysicalDeviceVulkan11Properties.New(out var p11);
				Vk.PhysicalDeviceVulkan12Properties.New(out var p12);
				p10.pNext = &p11;
				if (device.ApiVersion >= Vk.Version.VK_VERSION_1_2) {
					p11.pNext = &p12;
				}
				device.GetProperties2(&p10);
				props10 = p10.Properties;
				props11 = p11;
				props12 = p12;
				props11.pNext = null;
			}
			else {
				Vk.PhysicalDeviceVulkan11Properties.New(out props11);
				Vk.PhysicalDeviceVulkan12Properties.New(out props12);
			}
		}

		private static void GetDeviceFeatures(
			VulkanPhysicalDevice device,
			out Vk.PhysicalDeviceFeatures feats10,
			out Vk.PhysicalDeviceVulkan11Features feats11,
			out Vk.PhysicalDeviceVulkan12Features feats12
		)
		{
			Vk.PhysicalDeviceProperties props;
			device.GetProperties(&props);

			// Can only use features2 on Vulkan >= 1.1
			Vk.Version apiv = new(Math.Min(props.ApiVersion, device.Parent.ApiVersion));
			if (apiv < Vk.Version.VK_VERSION_1_1) {
				Vk.PhysicalDeviceFeatures feats;
				device.GetFeatures(&feats);
				feats10 = feats;
				Vk.PhysicalDeviceVulkan11Features.New(out feats11);
				Vk.PhysicalDeviceVulkan12Features.New(out feats12);
			}
			else {
				Vk.PhysicalDeviceFeatures2.New(out var f10);
				Vk.PhysicalDeviceVulkan11Features.New(out var f11);
				Vk.PhysicalDeviceVulkan12Features.New(out var f12);
				f10.pNext = &f11;
				if (device.ApiVersion >= Vk.Version.VK_VERSION_1_2) {
					f11.pNext = &f12;
				}
				device.GetFeatures2(&f10);
				feats10 = f10.Features;
				feats11 = f11;
				feats12 = f12;
				feats11.pNext = null;
			}
		}
	}
}
