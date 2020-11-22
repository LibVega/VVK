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
	/// Contains a table of static function pointers for the Vulkan API. This is designed to allow direct calls with
	/// <c>using static Vulkan.StaticFunctionTable;</c>. 
	/// <para>
	/// The <c>InitFunctionTable(VkInstance)</c> and <c>InitFunctionTable(VkDevice)</c> functions must be called before 
	/// using the functions.
	/// </para>
	/// </summary>
	public unsafe static partial class StaticFunctionTable
	{
		/// <summary>
		/// The core API version used to load the instance-level functions.
		/// </summary>
		public static VkVersion InstanceVersion { get; private set; } = new();

		/// <summary>
		/// The core API version used to load the device-level functions.
		/// </summary>
		public static VkVersion DeviceVersion { get; private set; } = new();

		// Load an instance function, throwing an exception if not available
		private static void* LoadFunc(VulkanHandle<VkInstance> inst, string name)
		{
			var namePtr = stackalloc byte[name.Length * 4]; // Worst case scenario
			int len = Encoding.ASCII.GetBytes(name, new Span<byte>(namePtr, name.Length * 4));
			namePtr[len] = 0;

			void* addr = _vkGetInstanceProcAddr(inst, namePtr);
			if (addr == null) {
				throw new InvalidOperationException($"Function '{name}' is not a loadable function");
			}
			return addr;
		}

		// Load an instance function, returning of the load was successful
		private static bool TryLoadFunc(VulkanHandle<VkInstance> inst, string name, out void* addr)
		{
			var namePtr = stackalloc byte[name.Length * 4]; // Worst case scenario
			int len = Encoding.ASCII.GetBytes(name, new Span<byte>(namePtr, name.Length * 4));
			namePtr[len] = 0;

			addr = _vkGetInstanceProcAddr(inst, namePtr);
			return (addr != null);
		}

		// Load a device function, throwing an exception if not available
		private static void* LoadFunc(VulkanHandle<VkDevice> dev, string name)
		{
			var namePtr = stackalloc byte[name.Length * 4]; // Worst case scenario
			int len = Encoding.ASCII.GetBytes(name, new Span<byte>(namePtr, name.Length * 4));
			namePtr[len] = 0;

			void* addr = _vkGetDeviceProcAddr(dev, namePtr);
			if (addr == null) {
				throw new InvalidOperationException($"Function '{name}' is not a loadable function");
			}
			return addr;
		}

		// Load a device function, returning of the load was successful
		private static bool TryLoadFunc(VulkanHandle<VkDevice> dev, string name, out void* addr)
		{
			var namePtr = stackalloc byte[name.Length * 4]; // Worst case scenario
			int len = Encoding.ASCII.GetBytes(name, new Span<byte>(namePtr, name.Length * 4));
			namePtr[len] = 0;

			addr = _vkGetDeviceProcAddr(dev, namePtr);
			return (addr != null);
		}
	}
}
