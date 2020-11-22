/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Text;

namespace Vulkan
{
	/// <summary>
	/// Contains a table of function pointers for device scope functions. The functions are loaded with
	/// <c>vkGetDeviceProcAddr</c> where possible.
	/// </summary>
	public unsafe sealed partial class DeviceFunctionTable
	{
		/// <summary>
		/// The core API version used to load the functions.
		/// </summary>
		public readonly VkVersion CoreVersion = new();

		// Load an instance function, throwing an exception if not available
		private static void* LoadFunc(VulkanHandle<VkDevice> dev, string name)
		{
			var namePtr = stackalloc byte[name.Length * 4]; // Worst case scenario
			int len = Encoding.ASCII.GetBytes(name, new Span<byte>(namePtr, name.Length * 4));
			namePtr[len] = 0;

			void* addr = InstanceFunctionTable.vkGetDeviceProcAddr(dev, namePtr);
			if (addr == null) {
				throw new InvalidOperationException($"Function '{name}' is not a loadable function");
			}
			return addr;
		}

		// Load an instance function, returning of the load was successful
		private static bool TryLoadFunc(VulkanHandle<VkDevice> dev, string name, out void* addr)
		{
			var namePtr = stackalloc byte[name.Length * 4]; // Worst case scenario
			int len = Encoding.ASCII.GetBytes(name, new Span<byte>(namePtr, name.Length * 4));
			namePtr[len] = 0;

			addr = InstanceFunctionTable.vkGetDeviceProcAddr(dev, namePtr);
			return (addr != null);
		}
	}
}
