/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace VVK
{
	/// <summary>
	/// Wraps a VkPhysicalDevice object at a higher level, providing an OOP approach to VkPhysicalDevice resources and 
	/// functions.
	/// </summary>
	public unsafe sealed partial class VulkanPhysicalDevice
	{
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
		/// The name of the device.
		/// </summary>
		public string Name => Properties.DeviceName;
		/// <summary>
		/// The device type.
		/// </summary>
		public Vk.PhysicalDeviceType Type => Properties.DeviceType;
		/// <summary>
		/// The maximum Vulkan API version supported by the device.
		/// </summary>
		public Vk.Version ApiVersion => new(Properties.ApiVersion);
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
			fixed (Vk.MemoryType* typePtr = &MemoryProperties.MemoryTypes_0) {
				foreach (var notMask in new[] { not, Vk.MemoryPropertyFlags.NoFlags }) {
					foreach (var yesMask in new[] { required | preferred, required }) {
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

		private static void GetDeviceProperties(
			VulkanPhysicalDevice device,
			out Vk.PhysicalDeviceProperties props10,
			out Vk.PhysicalDeviceVulkan11Properties props11,
			out Vk.PhysicalDeviceVulkan12Properties props12
		)
		{
			// Can only use properties2 on Vulkan >= 1.1
			if (device.ApiVersion < Vk.Version.VK_VERSION_1_1) {
				Vk.PhysicalDeviceProperties props;
				device.GetProperties(&props);
				props10 = props;
				Vk.PhysicalDeviceVulkan11Properties.New(out props11);
				Vk.PhysicalDeviceVulkan12Properties.New(out props12);
			}
			else {
				Vk.PhysicalDeviceProperties2.New(out var p10);
				Vk.PhysicalDeviceVulkan11Properties.New(out var p11);
				Vk.PhysicalDeviceVulkan12Properties.New(out var p12);
				p10.pNext = &p11;
				p11.pNext = &p12;
				device.GetProperties2(&p10);
				props10 = p10.Properties;
				props11 = p11;
				props12 = p12;
				props11.pNext = null;
			}
		}

		private static void GetDeviceFeatures(
			VulkanPhysicalDevice device,
			out Vk.PhysicalDeviceFeatures feats10,
			out Vk.PhysicalDeviceVulkan11Features feats11,
			out Vk.PhysicalDeviceVulkan12Features feats12
		)
		{
			// Can only use features2 on Vulkan >= 1.1
			if (device.ApiVersion < Vk.Version.VK_VERSION_1_1) {
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
				f11.pNext = &f12;
				device.GetFeatures2(&f10);
				feats10 = f10.Features;
				feats11 = f11;
				feats12 = f12;
				feats11.pNext = null;
			}
		}
	}
}
