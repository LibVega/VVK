/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vulkan.VVK
{
	/// <summary>
	/// Collects information and data about the capabilities of a <see cref="VkInstance"/>.
	/// </summary>
	public unsafe class InstanceInfo
	{
		#region Fields
		/// <summary>
		/// The instance associated with this data.
		/// </summary>
		public readonly VkInstance Instance;

		/// <summary>
		/// The highest supported version of the API for the instance.
		/// </summary>
		public readonly VkVersion ApiVersion;
		/// <summary>
		/// The list of physical devices available to the instance.
		/// </summary>
		public IReadOnlyList<VkPhysicalDevice> PhysicalDevices => _devices;
		private readonly VkPhysicalDevice[] _devices;

		/// <summary>
		/// The list of names of extensions supported by the instance.
		/// </summary>
		public static IReadOnlyList<string> ExtensionNames => _extensions;
		private static readonly string[] _extensions;
		/// <summary>
		/// The list of names of debug layers supported by the instance.
		/// </summary>
		public static IReadOnlyList<string> LayerNames => _layers;
		private static readonly string[] _layers;
		#endregion // Fields

		/// <summary>
		/// Create a new data object for the passed instance.
		/// </summary>
		/// <param name="instance">The instance to collate data for.</param>
		public InstanceInfo(VkInstance instance)
		{
			if (!instance) {
				throw new ArgumentNullException(nameof(instance), "Cannot pass null instance or null instance handle");
			}
			Instance = instance;

			// Get Data
			ApiVersion = GetApiVersion();
			uint devCount;
			instance.EnumeratePhysicalDevices(&devCount, null).Throw("EnumeratePhysicalDevices");
			var handles = stackalloc VulkanHandle<VkPhysicalDevice>[(int)devCount];
			_devices = new VkPhysicalDevice[devCount];
			instance.EnumeratePhysicalDevices(&devCount, handles).Throw("EnumeratePhysicalDevices");
			for (uint i = 0; i < devCount; ++i) {
				_devices[i] = new(handles[i], instance);
			}
		}

		/// <summary>
		/// Gets the highest API version supported by the current system.
		/// </summary>
		public static VkVersion GetApiVersion()
		{
			uint version = 0;
			InstanceFunctionTable.EnumerateInstanceVersion(&version).Throw("EnumerateInstanceVersion");
			return new VkVersion(version);
		}

		/// <summary>
		/// Gets the instance extensions supported by the current system.
		/// </summary>
		/// <param name="layerName">The optional layer name in which to look for extensions.</param>
		public static VkExtensionProperties[] GetExtensions(string? layerName)
		{
			uint count = 0;
			VkExtensionProperties[] props;

			if (layerName is not null) {
				using var nativeName = new NativeString(layerName);
				InstanceFunctionTable.EnumerateInstanceExtensionProperties(nativeName.Data, &count, null)
					.Throw("EnumerateInstanceExtensionProperties");
				props = new VkExtensionProperties[count];
				fixed (VkExtensionProperties* ptr = props) {
					InstanceFunctionTable.EnumerateInstanceExtensionProperties(nativeName.Data, &count, ptr)
						.Throw("EnumerateInstanceExtensionProperties");
				}
			}
			else {
				InstanceFunctionTable.EnumerateInstanceExtensionProperties(null, &count, null)
					.Throw("EnumerateInstanceExtensionProperties");
				props = new VkExtensionProperties[count];
				fixed (VkExtensionProperties* ptr = props) {
					InstanceFunctionTable.EnumerateInstanceExtensionProperties(null, &count, ptr)
						.Throw("EnumerateInstanceExtensionProperties");
				}
			}

			return props;
		}

		/// <summary>
		/// Gets the debug layers supported by the current system.
		/// </summary>
		public static VkLayerProperties[] GetLayers()
		{
			uint count = 0;
			InstanceFunctionTable.EnumerateInstanceLayerProperties(&count, null)
					.Throw("EnumerateInstanceExtensionProperties");
			var props = new VkLayerProperties[count];
			fixed (VkLayerProperties* ptr = props) {
				InstanceFunctionTable.EnumerateInstanceLayerProperties(&count, ptr)
					.Throw("EnumerateInstanceExtensionProperties");
			}
			return props;
		}

		static InstanceInfo()
		{
			_extensions = GetExtensions(null).Select(ext => ext.ExtensionName.ToString() ?? String.Empty).ToArray();
			_layers = GetLayers().Select(lay => lay.LayerName.ToString() ?? String.Empty).ToArray();
		}
	}
}
