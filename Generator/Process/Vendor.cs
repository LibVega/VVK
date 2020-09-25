/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Contains the types and enums present based on vendor
	public sealed class Vendor
	{
		#region Fields
		// The name of the vendor (used to parse spec names and as the C# output namespace)
		public readonly string Name;
		// The qualified name of the C# namespace for this vendor
		public string NamespaceName => IsCore ? "VVK.Vk" : $"VVK.Vk.{Name}";
		// If this is the core vendor
		public bool IsCore => Name.Length == 0;

		// The enums contained in this Vendor
		public readonly Dictionary<string, EnumOut> Enums;
		#endregion // Fields

		public Vendor(string name)
		{
			Name = name;
			Enums = new();
		}
	}
}
