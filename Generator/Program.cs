/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the
 * 'LICENSE' file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	class Program
	{
		static void Main(string[] args)
		{
			// Parse arguments, help/return if requested or error
			if (!ArgParse.Parse(args) || ArgParse.Help) {
				ArgParse.PrintHelp();
				return;
			}
		}
	}
}
