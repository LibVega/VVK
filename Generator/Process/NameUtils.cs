/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Contains functionality for working with and translating Vulkan spec names
	public static class NameUtils
	{
		// All known extension names (not populated until a ParseResult is created)
		public static readonly List<string> EXTENSION_NAMES = new();
	}
}
