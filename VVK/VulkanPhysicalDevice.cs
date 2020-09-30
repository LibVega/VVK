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
		}
	}
}
