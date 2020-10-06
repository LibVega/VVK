/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace VVK
{
	/// <summary>
	/// Wraps a VkInstance object at a higher level, providing an OOP approach to VkInstance resources and functions.
	/// </summary>
	public unsafe sealed partial class VulkanInstance : IDisposable
	{
		#region Static Fields
		/// <summary>
		/// A list of instance extensions that are supported by the runtime.
		/// </summary>
		public static IReadOnlyList<string> Extensions => _Extensions ?? PopulateExtensions();
		private static List<string>? _Extensions = null;
		/// <summary>
		/// A list of layers available on the runtime.
		/// </summary>
		public static IReadOnlyList<string> Layers => _Layers ?? PopulateLayers();
		private static List<string>? _Layers = null;
		#endregion // Static Fields

		#region Fields
		/// <summary>
		/// The VkInstance handle wrapped by this object.
		/// </summary>
		public Vk.Instance Handle { get; private set; } = Vk.Instance.Null;
		/// <summary>
		/// The core API version that the instance was created with.
		/// </summary>
		public readonly Vk.Version ApiVersion;
		/// <summary>
		/// The table of loaded functions for the instance.
		/// </summary>
		public Vk.InstanceFunctionTable Functions { get; private set; }

		/// <summary>
		/// List of the physical devices on the system available to the instance.
		/// </summary>
		public IReadOnlyList<VulkanPhysicalDevice> PhysicalDevices => _devices;
		private readonly List<VulkanPhysicalDevice> _devices = new();

		/// <summary>
		/// Gets if the instance has the VK_EXT_debug_utils extension enabled.
		/// </summary>
		public bool HasDebugUtils => _debugHandle;
		private Vk.EXT.DebugUtilsMessenger _debugHandle = Vk.EXT.DebugUtilsMessenger.Null;
		private int* _debugToken; // Token for dispatching util messages
		/// <summary>
		/// Event that is called when a message is received from VK_EXT_debug_utils.
		/// </summary>
		public event DebugUtilsCallback? OnDebugUtilMessage = null;

		// Static dispatch dictionary
		private static readonly List<VulkanInstance?> _DebugDispatch = new();
		#endregion // Fields

		/// <summary>
		/// Creates a new instance wrapper from the existing instance object handle.
		/// </summary>
		/// <param name="inst">The pre-created instance handle.</param>
		/// <param name="version">The highest core API version to support on the instance.</param>
		public VulkanInstance(Vk.Instance inst, Vk.Version version)
		{
			Handle = inst;
			ApiVersion = version;
			Functions = new(inst, ApiVersion);

			// Enumerate the physical devices
			uint count = 0;
			EnumeratePhysicalDevices(&count, null).Throw();
			var devices = stackalloc Vk.PhysicalDevice[(int)count];
			EnumeratePhysicalDevices(&count, devices).Throw();
			for (uint i = 0; i < count; ++i) {
				_devices.Add(new VulkanPhysicalDevice(this, devices[i]));
			}

			// Enable debug utils if available
			if (Functions.vkCreateDebugUtilsMessengerEXT != null) {
				// Setup dispatch
				_debugToken = (int*)Marshal.AllocHGlobal(sizeof(int)).ToPointer();
				*_debugToken = _DebugDispatch.Count;
				_DebugDispatch.Add(this);

				// Create the object
				Vk.EXT.DebugUtilsMessengerCreateInfo.New(out var mci);
				mci.Flags = Vk.EXT.DebugUtilsMessengerCreateFlags.NoFlags;
				mci.MessageSeverity =
					Vk.EXT.DebugUtilsMessageSeverityFlags.ErrorEXT | Vk.EXT.DebugUtilsMessageSeverityFlags.WarningEXT | 
					Vk.EXT.DebugUtilsMessageSeverityFlags.InfoEXT | Vk.EXT.DebugUtilsMessageSeverityFlags.VerboseEXT;
				mci.MessageType = 
					Vk.EXT.DebugUtilsMessageTypeFlags.GeneralEXT | Vk.EXT.DebugUtilsMessageTypeFlags.PerformanceEXT | 
					Vk.EXT.DebugUtilsMessageTypeFlags.ValidationEXT;
				mci.UserCallback = &_DebugUtilsCallback;
				mci.UserData = _debugToken;
				Vk.EXT.DebugUtilsMessenger debugHandle = Vk.EXT.DebugUtilsMessenger.Null;
				CreateDebugUtilsMessengerEXT(&mci, null, &debugHandle).Throw();
				_debugHandle = debugHandle;
			}
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
			if (_debugHandle) {
				DestroyDebugUtilsMessengerEXT(_debugHandle, null);
				_DebugDispatch[*_debugToken] = null;
				Marshal.FreeHGlobal(new IntPtr(_debugToken));
				_debugHandle = Vk.EXT.DebugUtilsMessenger.Null;
				_debugToken = null;
			}
			if (Handle) {
				DestroyInstance(null);
				Handle = Vk.Instance.Null;
				Functions = new();
				_devices.Clear();
			}
		}
		#endregion // IDisposable

		/// <summary>
		/// Creates a new Vulkan instance from the passed information.
		/// </summary>
		/// <param name="appName">The name of the application.</param>
		/// <param name="appVersion">The version of the application.</param>
		/// <param name="apiVersion">The minimum required version for the Vulkan API.</param>
		/// <param name="extensions">
		/// The list of extensions to enable. An exception will be thrown for extensions that are not available. Check
		/// <see cref="VulkanInstance.Extensions"/> to see the available functions.
		/// </param>
		/// <param name="layers">
		/// The list of layers to enable. An exception will be thrown for layers that are not available. Check
		/// <see cref="VulkanInstance.Layers"/> to see the available layers.
		/// </param>
		/// <returns>The new Vulkan instance object.</returns>
		/// <exception cref="VulkanResultException">One of the Vulkan API calls failed.</exception>
		/// <exception cref="PlatformNotSupportedException">The API version, extension, or layer is not supported.</exception>
		public static VulkanInstance Create(string appName, Vk.Version appVersion, Vk.Version apiVersion, 
			IEnumerable<string>? extensions = null, IEnumerable<string>? layers = null)
		{
			// Check instance version
			Vk.Version vers = new();
			EnumerateInstanceVersion(&vers.Value).Throw();
			if (vers < apiVersion) {
				throw new PlatformNotSupportedException(
					$"Supported API version ({vers}) is less than requested ({apiVersion})");
			}

			// Check extensions
			if (extensions?.FirstOrDefault(s => !Extensions.Contains(s)) is string extMiss) {
				throw new PlatformNotSupportedException($"The instance extension '{extMiss}' is not supported");
			}
			if (layers?.FirstOrDefault(s => !Layers.Contains(s)) is string layerMiss) {
				throw new PlatformNotSupportedException($"The debug layer '{layerMiss}' is not supported");
			}

			// Convert to native strings
			using var extNames = new NativeStringList(extensions ?? Enumerable.Empty<string>());
			using var layerNames = new NativeStringList(layers ?? Enumerable.Empty<string>());
			using var appNameStr = new NativeString(appName);
			using var engNameStr = new NativeString("VVK");

			// Application info
			var vvkv = typeof(VulkanInstance).Assembly.GetName().Version!;
			Vk.ApplicationInfo.New(out var appi);
			appi.ApplicationName = appNameStr.Data;
			appi.ApplicationVersion = appVersion;
			appi.EngineName = engNameStr.Data;
			appi.EngineVersion = new Vk.Version((uint)vvkv.Major, (uint)vvkv.Minor, (uint)vvkv.Revision);
			appi.ApiVersion = apiVersion;

			// Instance create
			Vk.InstanceCreateInfo.New(out var ici);
			ici.ApplicationInfo = &appi;
			ici.EnabledLayerCount = layerNames.Count;
			ici.EnabledLayerNames = layerNames.Data;
			ici.EnabledExtensionCount = extNames.Count;
			ici.EnabledExtensionNames = extNames.Data;
			Vk.Instance handle = Vk.Instance.Null;
			CreateInstance(&ici, null, &handle).Throw();

			// Return
			return new(handle, apiVersion);
		}

		// Callback function for the debug utils messenger object
		private static Vk.Bool32 _DebugUtilsCallback(
			Vk.EXT.DebugUtilsMessageSeverityFlags severity,
			Vk.EXT.DebugUtilsMessageTypeFlags type,
			Vk.EXT.DebugUtilsMessengerCallbackData* data,
			void* userData
		)
		{
			_DebugDispatch[*(int*)userData]?.OnDebugUtilMessage?.Invoke(severity, type, data);
			return Vk.Bool32.False;
		}

		private static List<string> PopulateExtensions()
		{
			_Extensions = new();

			// Get extension properties
			uint count = 0;
			EnumerateInstanceExtensionProperties(null, &count, null).Throw();
			var exts = stackalloc Vk.ExtensionProperties[(int)count];
			EnumerateInstanceExtensionProperties(null, &count, exts).Throw();

			// Convert to string list
			for (int i = 0; i < count; ++i) {
				_Extensions.Add(exts[i].ExtensionName);
			}

			return _Extensions;
		}

		private static List<string> PopulateLayers()
		{
			_Layers = new();

			// Get layer properties
			uint count = 0;
			EnumerateInstanceLayerProperties(&count, null).Throw();
			var layers = stackalloc Vk.LayerProperties[(int)count];
			EnumerateInstanceLayerProperties(&count, layers).Throw();

			// Convert to string list
			for (int i = 0; i < count; ++i) {
				_Layers.Add(layers[i].LayerName);
			}

			return _Layers;
		}
	}

	/// <summary>
	/// Callback for messages from VK_EXT_debug_utils.
	/// </summary>
	/// <param name="severity">The message severity.</param>
	/// <param name="type">The message type.</param>
	/// <param name="data">The message data.</param>
	public unsafe delegate void DebugUtilsCallback(
		Vk.EXT.DebugUtilsMessageSeverityFlags severity,
		Vk.EXT.DebugUtilsMessageTypeFlags type,
		Vk.EXT.DebugUtilsMessengerCallbackData* data
	);
}
