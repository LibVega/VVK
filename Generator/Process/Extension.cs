/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Represents an extension namespace in the Vulkan spec
	public sealed class Extension
	{
		#region Fields
		// The extension name (string is empty for core namespace)
		public readonly string Name;
		// The display name (same as Name, except uses "Vk" for core objects)
		public string DisplayName => !IsCore ? Name : "Vk";
		// The name of the folder (same as Name, except uses "." for core objects)
		public string FolderName => !IsCore ? Name : ".";
		// The namespace of the extension in the generated code
		public string NamespaceName => IsCore ? "Vk" : $"Vk.{Name}";

		// Gets if this is the core namespace
		public bool IsCore => Name.Length == 0;

		// Objects in extension namespace
		public readonly List<EnumOut> Enums;
		#endregion // Fields

		public Extension(string name)
		{
			Name = name;
			Enums = new();
		}
	}
}
