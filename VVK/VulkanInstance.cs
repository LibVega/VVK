/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace VVK
{
	/// <summary>
	/// Wraps a VkInstance object at a higher level, providing an OOP approach to VkInstance resources and functions.
	/// </summary>
	public unsafe sealed partial class VulkanInstance : IDisposable
	{
		#region Fields
		/// <summary>
		/// The VkInstance handle wrapped by this object.
		/// </summary>
		public Vk.Instance Handle { get; private set; } = Vk.Instance.Null;
		/// <summary>
		/// The table of loaded functions for the instance.
		/// </summary>
		public Vk.InstanceFunctionTable Functions { get; private set; }
		#endregion // Fields

		/// <summary>
		/// Creates a new instance wrapper from the existing instance object handle.
		/// </summary>
		/// <param name="inst">The pre-created instance handle.</param>
		public VulkanInstance(Vk.Instance inst)
		{
			Handle = inst;
			Functions = new(inst);
		}
		~VulkanInstance()
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
				Functions.vkDestroyInstance(Handle, null);
			}
			Handle = Vk.Instance.Null;
			Functions = new();
		}
		#endregion // IDisposable
	}
}
