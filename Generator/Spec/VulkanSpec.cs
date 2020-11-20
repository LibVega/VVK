/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Contains the spec objects loaded from the XML spec file
	public sealed partial class VulkanSpec
	{
		#region Fields
		// All known type names and their corresponding spec types
		public IReadOnlyDictionary<string, SpecType> AllTypes => _allTypes;
		private readonly Dictionary<string, SpecType> _allTypes = new();

		// All known bitmasks
		public IReadOnlyDictionary<string, BitmaskSpec> Bitmasks => _bitmasks;
		private readonly Dictionary<string, BitmaskSpec> _bitmasks = new();

		// All known handle types
		public IReadOnlyDictionary<string, HandleSpec> Handles => _handles;
		private readonly Dictionary<string, HandleSpec> _handles = new();

		// All known enum types
		public IReadOnlyDictionary<string, EnumSpec> Enums => _enums;
		private readonly Dictionary<string, EnumSpec> _enums = new();

		// All known struct/union types
		public IReadOnlyDictionary<string, StructSpec> Structs => _structs;
		private readonly Dictionary<string, StructSpec> _structs = new();

		// All known func pointer types
		public IReadOnlyDictionary<string, FuncSpec> Functions => _functions;
		private readonly Dictionary<string, FuncSpec> _functions = new();

		// All known constants
		public IReadOnlyDictionary<string, ConstantSpec> Constants => _constants;
		private readonly Dictionary<string, ConstantSpec> _constants = new();

		// All known commands
		public IReadOnlyDictionary<string, CommandSpec> Commands => _commands;
		private readonly Dictionary<string, CommandSpec> _commands = new();

		// All known extensions
		public IReadOnlyDictionary<string, ExtensionSpec> Extensions => _extensions;
		private readonly Dictionary<string, ExtensionSpec> _extensions = new();
		#endregion // Fields

		private VulkanSpec()
		{

		}
	}

	// The object types in the spec
	public enum SpecType : byte
	{
		Bitmask,
		Handle,
		Enum,
		Struct,
		Function
	}
}
