/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.CompilerServices;

namespace Vk.KHR
{
	// Extensions for VkSwapchainKHR
	public unsafe partial class Swapchain
	{
		/// <summary>vkGetSwapchainImagesKHR</summary>
		public Vk.Result GetSwapchainImagesKHR(uint* pSwapchainImageCount, out Vk.Image[] swapchainImages)
		{
			if (Functions.vkGetSwapchainImagesKHR == null) throw new Vk.Extras.FunctionNotLoadedException("vkGetSwapchainImagesKHR");
			swapchainImages = Array.Empty<Vk.Image>();

			var res = Functions.vkGetSwapchainImagesKHR(Device, Handle, pSwapchainImageCount, null);
			if (res != Vk.Result.Success) {
				return res;
			}

			var hptr = stackalloc Vk.Handle<Vk.Image>[(int)*pSwapchainImageCount];
			res = Functions.vkGetSwapchainImagesKHR(Device, Handle, pSwapchainImageCount, hptr);
			if (res == Vk.Result.Success) {
				swapchainImages = new Vk.Image[*pSwapchainImageCount];
				for (uint i = 0; i < *pSwapchainImageCount; ++i) {
					swapchainImages[i] = new Vk.Image(Device, hptr[i]);
				}
			}
			return res;
		}

		/// <summary>vkGetSwapchainImagesKHR</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vk.Result GetSwapchainImagesKHR(out uint swapchainImageCount, out Vk.Image[] swapchainImages)
		{
			fixed (uint* pSwapchainImageCount = &swapchainImageCount) {
				return GetSwapchainImagesKHR(pSwapchainImageCount, out swapchainImages);
			}
		}
	}
}
