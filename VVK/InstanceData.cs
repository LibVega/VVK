/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Vk.Extras;

namespace Vk
{
	/// <summary>
	/// Contains functionality for collating and checking information for a <see cref="Vk.Instance"/> handle.
	/// </summary>
	public unsafe class InstanceData
	{
		#region Fields
		/// <summary>
		/// The instance associated with this data.
		/// </summary>
		public readonly Vk.Instance Instance;

		/// <summary>
		/// The highest supported version of the API for the instance.
		/// </summary>
		public readonly Vk.Version ApiVersion;
		/// <summary>
		/// The list of physical devices available to the instance.
		/// </summary>
		public IReadOnlyList<Vk.PhysicalDevice> PhysicalDevices => _devices;
		private readonly Vk.PhysicalDevice[] _devices;

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
		public InstanceData(Vk.Instance instance)
		{
			if (!instance) {
				throw new ArgumentNullException(nameof(instance), "Cannot pass null instance or null instance handle");
			}
			Instance = instance;

			// Get Data
			ApiVersion = GetApiVersion();
			instance.EnumeratePhysicalDevices(out _devices).Throw("EnumeratePhysicalDevices");
		}

		/// <summary>
		/// Gets the highest API version supported by the current system.
		/// </summary>
		public static Vk.Version GetApiVersion()
		{
			uint version = 0;
			InstanceFunctionTable.EnumerateInstanceVersion(&version).Throw("EnumerateInstanceVersion");
			return new Vk.Version(version);
		}

		/// <summary>
		/// Gets the instance extensions supported by the current system.
		/// </summary>
		/// <param name="layerName">The optional layer name in which to look for extensions.</param>
		public static Vk.ExtensionProperties[] GetExtensions(string? layerName)
		{
			uint count = 0;
			Vk.ExtensionProperties[] props;

			if (layerName is not null) {
				using var nativeName = new Vk.NativeString(layerName);
				InstanceFunctionTable.EnumerateInstanceExtensionProperties(nativeName.Data, &count, null)
					.Throw("EnumerateInstanceExtensionProperties");
				props = new Vk.ExtensionProperties[count];
				fixed (Vk.ExtensionProperties* ptr = props) {
					InstanceFunctionTable.EnumerateInstanceExtensionProperties(nativeName.Data, &count, ptr)
						.Throw("EnumerateInstanceExtensionProperties");
				}
			}
			else {
				InstanceFunctionTable.EnumerateInstanceExtensionProperties(null, &count, null)
					.Throw("EnumerateInstanceExtensionProperties");
				props = new Vk.ExtensionProperties[count];
				fixed (Vk.ExtensionProperties* ptr = props) {
					InstanceFunctionTable.EnumerateInstanceExtensionProperties(null, &count, ptr)
						.Throw("EnumerateInstanceExtensionProperties");
				}
			}

			return props;
		}

		/// <summary>
		/// Gets the debug layers supported by the current system.
		/// </summary>
		public static Vk.LayerProperties[] GetLayers()
		{
			uint count = 0;
			InstanceFunctionTable.EnumerateInstanceLayerProperties(&count, null)
					.Throw("EnumerateInstanceExtensionProperties");
			var props = new Vk.LayerProperties[count];
			fixed (Vk.LayerProperties* ptr = props) {
				InstanceFunctionTable.EnumerateInstanceLayerProperties(&count, ptr)
					.Throw("EnumerateInstanceExtensionProperties");
			}
			return props;
		}

		static InstanceData()
		{
			_extensions = GetExtensions(null).Select(ext => ext.ExtensionName.ToString()).ToArray();
			_layers = GetLayers().Select(lay => lay.LayerName.ToString()).ToArray();
		}
	}
}
