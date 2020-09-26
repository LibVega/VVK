/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace VVK.Vk
{
	// Loader functions
	public unsafe sealed partial class InstanceFunctionTable
	{
		private static void* LoadFunc(Vk.Instance inst, string name)
		{
			fixed (char* nptr = name) {
				var addr = vkGetInstanceProcAddr(inst, (byte*)nptr);
				if (addr == (void*)0) {
					throw new ArgumentException($"The function '{name}' was not found", nameof(name));
				}
				return addr;
			}
		}

		private static bool TryLoadFunc(Vk.Instance inst, string name, out void* addr)
		{
			fixed (char* nptr = name) {
				addr = vkGetInstanceProcAddr(inst, (byte*)nptr);
				return addr != (void*)0;
			}
		}
	}

	// Loader functions
	public unsafe sealed partial class DeviceFunctionTable
	{
		private static void* LoadFunc(Vk.Device dev, string name)
		{
			fixed (char* nptr = name) {
				var addr = InstanceFunctionTable.vkGetDeviceProcAddr(dev, (byte*)nptr);
				if (addr == (void*)0) {
					throw new ArgumentException($"The function '{name}' was not found", nameof(name));
				}
				return addr;
			}
		}

		private static bool TryLoadFunc(Vk.Device dev, string name, out void* addr)
		{
			fixed (char* nptr = name) {
				addr = InstanceFunctionTable.vkGetDeviceProcAddr(dev, (byte*)nptr);
				return addr != (void*)0;
			}
		}
	}
}
