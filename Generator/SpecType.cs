/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
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
