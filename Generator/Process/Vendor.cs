/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Gen
{
	// Contains the types and enums present based on vendor
	public sealed class Vendor
	{
		#region Fields
		// The name of the vendor (used to parse spec names and as the C# output namespace)
		public readonly string Name;
		// The qualified name of the C# namespace for this vendor
		public string NamespaceName => IsCore ? "Vk" : $"Vk.{Name}";
		// If this is the core vendor
		public bool IsCore => Name.Length == 0;
		// The display name of the vendor (taking into account if the vendor is core)
		public string DisplayName => IsCore ? "Core Vulkan" : Name;

		// The enums contained in this Vendor
		public readonly Dictionary<string, EnumOut> Enums;
		// The structs contained in this Vendor
		public readonly Dictionary<string, StructOut> Structs;
		// The handles contained in this Vendor
		public readonly Dictionary<string, HandleOut> Handles;
		#endregion // Fields

		public Vendor(string name)
		{
			Name = name;
			Enums = new();
			Structs = new();
			Handles = new();
		}

		// Combines the filename with the directory for the vendor
		public string GetSourceFilename(string srctype) =>
			IsCore ? $"Vk.{srctype}.cs" : Path.Combine(Name, $"{Name}.{srctype}.cs");
	}
}
