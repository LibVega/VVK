/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Vk
{
	/// <summary>
	/// Contains a table of loaded function pointers for VkInstance-scope functions. Also contains the static
	/// global-scope functions.
	/// </summary>
	public unsafe sealed partial class InstanceFunctionTable
	{
		/// <summary>
		/// The core API version that was loaded for this table (describes which core functions are available).
		/// </summary>
		public readonly Vk.Version CoreVersion = new();

		private static void* LoadFunc(Vk.Handle<Vk.Instance> inst, string name)
		{
			using var nname = new VVK.NativeString(name);
			var addr = vkGetInstanceProcAddr(inst, nname.Data);
			if (addr == null) {
				throw new ArgumentException($"The function '{nname}' was not found", nameof(name));
			}
			return addr;
		}

		private static bool TryLoadFunc(Vk.Handle<Vk.Instance> inst, string name, out void* addr)
		{
			using var nname = new VVK.NativeString(name);
			addr = vkGetInstanceProcAddr(inst, nname.Data);
			return addr != null;
		}
	}

	/// <summary>
	/// Contains a table of loaded function pointers for VkDevice-scope functions.
	/// </summary>
	public unsafe sealed partial class DeviceFunctionTable
	{
		/// <summary>
		/// The core API version that was loaded for this table (describes which core functions are available).
		/// </summary>
		public readonly Vk.Version CoreVersion = new();

		private static void* LoadFunc(Vk.Handle<Vk.Device> dev, string name)
		{
			using var nname = new VVK.NativeString(name);
			var addr = InstanceFunctionTable.vkGetDeviceProcAddr(dev, nname.Data);
			if (addr == null) {
				throw new ArgumentException($"The function '{name}' was not found", nameof(name));
			}
			return addr;
		}

		private static bool TryLoadFunc(Vk.Handle<Vk.Device> dev, string name, out void* addr)
		{
			using var nname = new VVK.NativeString(name);
			addr = InstanceFunctionTable.vkGetDeviceProcAddr(dev, nname.Data);
			return addr != null;
		}
	}
}
