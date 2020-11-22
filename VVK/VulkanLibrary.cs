/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Vulkan
{
	/// <summary>
	/// Manages the open handle to the Vulkan runtime library, and provides symbol loading functionality.
	/// </summary>
	public static class VulkanLibrary
	{
		#region Fields
		/// <summary>
		/// The handle to the loaded Vulkan runtime library.
		/// </summary>
		public static readonly IntPtr Handle;
		/// <summary>
		/// The name of the loaded runtime library.
		/// </summary>
		public static readonly string LibraryName;
		#endregion // Fields

		/// <summary>
		/// Attempts to load the address of the exported symbol with the given name.
		/// </summary>
		/// <param name="name">The name of the exported symbol.</param>
		/// <returns>The address of the exported symbol.</returns>
		/// <exception cref="InvalidOperationException">The Vulkan library is not loaded.</exception>
		/// <exception cref="ArgumentException">The symbol with the given name was not found.</exception>
		public static IntPtr GetExport(string name)
		{
			if (Handle == IntPtr.Zero) {
				throw new InvalidOperationException("The Vulkan library is not loaded");
			}

			if (NativeLibrary.TryGetExport(Handle, name, out var addr)) {
				return addr;
			}
			else {
				throw new ArgumentException($"Could not load symbol '{name}'", nameof(name));
			}
		}

		/// <summary>
		/// Attempts to load the address of the exported symbol with the given name.
		/// </summary>
		/// <param name="name">The name of the exported symbol.</param>
		/// <param name="addr">The address of the symbol.</param>
		/// <returns>If the load was successful.</returns>
		public static bool TryGetExport(string name, out IntPtr addr)
		{
			if (Handle != IntPtr.Zero && NativeLibrary.TryGetExport(Handle, name, out addr)) {
				return true;
			}
			else {
				addr = IntPtr.Zero;
				return false;
			}
		}

		static VulkanLibrary()
		{
			// Try to load the runtime library
			foreach (var libName in EnumerateLibraryNameCandidates()) {
				if (NativeLibrary.TryLoad(libName, out var handle)) {
					Handle = handle;
					LibraryName = libName;
					return;
				}
			}

			// Report failure
			throw new PlatformNotSupportedException(
				"The Vulkan runtime library could not be found or loaded for the current platform"
			);
		}

		private static IEnumerable<string> EnumerateLibraryNameCandidates()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				yield return "vulkan-1.dll";
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				yield return "libMoltenVK.dylib"; // Works for iOS and tvOS as well
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
				yield return "libvulkan.so.1"; // Works on most dekstop Linux distros
				yield return "libvulkan.so"; // Catches other Linux distros and Android
			}
			else { // OSPlatform.FreeBSD
				yield return "libvulkan.so"; // Does this work? Vulkan support on FreeBSD is not well documented
			}
		}
	}
}
