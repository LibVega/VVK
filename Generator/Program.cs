/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.IO;

namespace Gen
{
	public static class Program
	{
		static void Main(string[] args)
		{
			// Parse arguments, help/return if requested or error
			if (!ArgParse.Parse(args)) {
				return;
			}
			if (ArgParse.Help) {
				ArgParse.PrintHelp();
				return;
			}

			// Check that the input file and output path are valid
			if (!File.Exists(ArgParse.InputFile)) {
				PrintError($"Input file '{ArgParse.InputFile}' does not exist or is an invalid path.");
				return;
			}
			if (!Directory.Exists(ArgParse.OutputPath)) {
				try {
					Directory.CreateDirectory(ArgParse.OutputPath);
				}
				catch {
					PrintError($"Failed to create output directory '{ArgParse.OutputPath}'");
					return;
				}
			}

			// Try to parse the spec file
			ParseResults? parseRes = null;
#if !DEBUG
			try {
#endif
				if (!ParseResults.TryParse(ArgParse.InputFile, out parseRes)) {
					PrintError("Failed to load specification file, exiting...");
					return;
				}
				//NameUtils.EXTENSION_NAMES.Clear();
				//NameUtils.EXTENSION_NAMES.AddRange(parseRes!.Extensions);
#if !DEBUG
			}
			catch (Exception e) {
				PrintError($"Unhandled parse exception ({e.GetType().Name}) - {e.Message}");
				return;
			}
#endif

			// Run the processing task
			ProcessResults? procRes = null;
#if !DEBUG
			try {
#endif
				if (!ProcessResults.TryProcess(parseRes!, out procRes)) {
					PrintError("Failed to process specifiction types, exiting...");
					return;
				}
#if !DEBUG
			}
			catch (Exception e) {
				PrintError($"Unhandled process exception ({e.GetType().Name}) - {e.Message}");
				return;
			}
#endif

			/*
			// Run the generation task
#if !DEBUG
			try {
#endif
				if (!Generator.Generate(procRes)) {
					PrintError("Failed to generate source, exiting...");
					return;
				}
#if !DEBUG
			}
			catch (Exception e) {
				PrintError($"Unhandled generation exception ({e.GetType().Name}) - {e.Message}");
				return;
			}
#endif
			*/
		}

		// Prints a colored error message to the console
		public static void PrintError(string msg, bool help = false)
		{
			var old = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Error:  " + msg);
			if (help) {
				Console.WriteLine("Use '-h', '-help', or '-?' to get command line help.");
			}
			Console.ForegroundColor = old;
		}

		// Prints a colored warning message to the console
		public static void PrintWarning(string msg)
		{
			var old = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine(msg);
			Console.ForegroundColor = old;
		}

		// Prints a normal message to the console, if verbose logging has been requested
		public static void PrintVerbose(string msg)
		{
			if (!ArgParse.Verbose) return;
			var old = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(msg);
			Console.ForegroundColor = old;
		}
	}
}
