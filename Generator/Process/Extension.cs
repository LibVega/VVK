/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.IO;

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

		// Gets if this is the core namespace
		public bool IsCore => Name.Length == 0;
		#endregion // Fields

		public Extension(string name)
		{
			Name = name;
		}
	}
}
