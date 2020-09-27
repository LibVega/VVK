/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace VVK
{
	/// <summary>
	/// Wraps a VkDevice object at a higher level, providing an OOP approach to VKDevice resources and functions.
	/// </summary>
	public unsafe sealed partial class VulkanDevice : IDisposable
	{
		#region Fields
		/// <summary>
		/// The parent instance for this device.
		/// </summary>
		public VulkanInstance Parent { get; private set; }
		/// <summary>
		/// The VkInstance handle wrapped by this object.
		/// </summary>
		public Vk.Device Handle { get; private set; } = Vk.Device.Null;
		/// <summary>
		/// The table of loaded functions for the instance.
		/// </summary>
		public Vk.DeviceFunctionTable Functions { get; private set; }
		#endregion // Fields

		/// <summary>
		/// Creates a new device wrapper from the existing device object handle.
		/// </summary>
		/// <param name="inst">The instance that the device handle was created from.</param>
		/// <param name="device">The pre-created device handle.</param>
		public VulkanDevice(VulkanInstance inst, Vk.Device device)
		{
			Parent = inst;
			Handle = device;
			Functions = new(device);
		}
		~VulkanDevice()
		{
			dispose(false);
		}

		#region IDisposable
		/// <summary>
		/// If this represents a valid instance handle, this will destroy the instance object and free resources.
		/// </summary>
		public void Dispose()
		{
			dispose(true);
			GC.SuppressFinalize(this);
		}

		private void dispose(bool disposing)
		{
			if (Handle) {
				Functions.vkDestroyDevice(Handle, null);
			}
			Handle = Vk.Device.Null;
			Functions = new();
		}
		#endregion // IDisposable
	}
}
