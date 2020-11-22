/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Represents an API vendor, and the associated types
	public sealed class Vendor
	{
		#region Fields
		// The vendor tag
		public readonly string Tag;
		// If this vendor represents the core objects
		public bool IsCore => Tag == "Core";

		// The types associated with this vendor
		public IReadOnlyDictionary<string, BitmaskType> Bitmasks => _bitmasks;
		private readonly Dictionary<string, BitmaskType> _bitmasks = new();
		public IReadOnlyDictionary<string, HandleType> Handles => _handles;
		private readonly Dictionary<string, HandleType> _handles = new();
		public IReadOnlyDictionary<string, EnumType> Enums => _enums;
		private readonly Dictionary<string, EnumType> _enums = new();
		public IReadOnlyDictionary<string, StructType> Structs => _structs;
		private readonly Dictionary<string, StructType> _structs = new();
		#endregion // Fields

		public Vendor(string tag)
		{
			Tag = tag;
		}

		public void AddType(BitmaskType type) => _bitmasks.Add(type.Name, type);
		public void AddType(HandleType type) => _handles.Add(type.Name, type);
		public void AddType(EnumType type) => _enums.Add(type.Name, type);
		public void AddType(StructType type) => _structs.Add(type.Name, type);
	}
}
