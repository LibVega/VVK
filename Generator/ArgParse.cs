/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
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
		public static bool Help { get; private set; } = false;

		// The specified input file (defaults to "./vk.xml")
		public static string InputFile { get; private set; } = "./vk.xml";

		// If the output should be verbose
		public static bool Verbose { get; private set; } = false;

		// The output folder to place the generated files in (defaults to "./Generated")
		public static string OutputPath { get; private set; } = "./Generated";
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
			if (rawargs.Length == 0) {
				return true;
			}
			var args = rawargs.Select(_normalize).ToList();

			// Check for help flag and return immediately
			if (args.Contains("-h") || args.Contains("-help") || args.Contains("-?")) {
				Help = true;
				return true;
			}

			// Check for the input file override
			{
				var idx = args.IndexOf("-i");
				if (idx != -1) {
					if (idx == (args.Count - 1)) {
						Program.PrintError("No input file specified", true);
						return false;
					}
					InputFile = args[idx + 1];
				}
			}

			// Check for verbose flag
			if (args.Contains("-v")) {
				Verbose = true;
			}

			// Check for the output path override
			{
				int idx = args.IndexOf("-o");
				if (idx != -1) {
					if (idx == (args.Count - 1)) {
						Program.PrintError("No output path specified", true);
						return false;
					}
					OutputPath = args[idx + 1];
				}
			}

			// Return success
			return true;
		}

		// Prints the help statement
		public static void PrintHelp() => Console.WriteLine(
			 "\n" +
			$"Usage: {AppDomain.CurrentDomain.FriendlyName} [args]\n" +
			 "\n" +
			 "By default, searches the current directory for the 'vk.xml' file as the input.\n" +
			 "A different input file can be specified with the '-i' argument.\n" +
			 "\n" +
			 "Arguments:\n" +
			 "\n" +
			 "   h;help;?         - Prints this help statement, then exits.\n" +
			 "   i                - Specifies the input file (defaults to './vk.xml').\n" +
			 "   v                - Use more verbose output messages.\n" +
			 "   o                - Specifies the output directory (defaults to './Generated').\n" +
			 "\n" +
			 "   All arguments can be specified with either '-', '--', or '/'.\n"
		);
	}
}
