/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the
 * 'LICENSE' file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Linq;

namespace Gen
{
	// Manages the command line argument parsing and actions
	public static class ArgParse
	{
		#region Options
		// If the help flag was specified
		public static bool Help { get; private set; }
		#endregion // Options

		// Parses the args and sets the values, returns false if an error occured
		public static bool Parse(string[] rawargs)
		{
			// Normalizes flags to start with '-' and be lowercase
			static string _normalize(string arg)
			{
				bool flag = arg[0] == '-' || arg[0] == '/';
				bool dash = arg.Length > 1 && arg[1] == '-';
				return flag ? ('-' + arg.ToLowerInvariant().Substring(dash ? 2 : 1)) : arg;
			}

			// Normalize and extract arguments
			var args = rawargs.Select(_normalize).ToArray();

			// Check for help flag and return immediately
			if (args.Contains("-h") || args.Contains("-help") || args.Contains("-?")) {
				Help = true;
				return true;
			}
			Help = false;

			// Return success
			return true;
		}

		// Prints the help statement
		public static void PrintHelp() => Console.WriteLine(
			 "\n" +
			$"Usage: {AppDomain.CurrentDomain.FriendlyName} [args]\n" +
			 "\n" +
			 "By default, searches the current directory for the 'vk.xml' file as the input.\n" +
			 "\n" +
			 "Arguments:\n" +
			 "\n" +
			 "   h;help;?         - Prints this help statement, then exits.\n" +
			 "\n" +
			 "   All arguments can be specified with either '-', '--', or '/'.\n"
		);
	}
}
