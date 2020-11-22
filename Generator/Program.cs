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

			// Load the specification
			VulkanSpec? vkspec = null;
#if DEBUG
			if (!VulkanSpec.TryLoad(ArgParse.InputFile, out vkspec)) {
				PrintError("Failed to load specification file");
				return;
			}
#else
			try {
				if (!VulkanSpec.TryLoad(ArgParse.InputFile, out vkspec)) {
					PrintError("Failed to load specification file");
					return;
				}
			}
			catch (Exception e) {
				PrintError($"Unhandled specification load exception");
				PrintError($"{e.GetType()} - {e.Message}");
			}
#endif // DEBUG

			// Process the specification
			ProcessedSpec? procspec = null;
#if DEBUG
			if (!ProcessedSpec.TryProcess(vkspec!, out procspec)) {
				PrintError("Failed to process specification");
				return;
			}
#else
			try {
				if (!ProcessedSpec.TryProcess(vkspec!, out procspec)) {
					PrintError("Failed to process specification");
					return;
				}
			}
			catch (Exception e) {
				PrintError($"Unhandled specification process exception");
				PrintError($"{e.GetType()} - {e.Message}");
			}
#endif // DEBUG

// Generate the specification
#if DEBUG
			if (!APIGenerator.Generate(procspec!)) {
				PrintError("Failed to generate API");
				return;
			}
#else
			try {
				if (!APIGenerator.Generate(procspec!)) {
					PrintError("Failed to generate API");
					return;
				}
			}
			catch (Exception e) {
				PrintError($"Unhandled api generation exception");
				PrintError($"{e.GetType()} - {e.Message}");
			}
#endif // DEBUG
		}

		// Prints a standard message to the console
		public static void Print(string msg)
		{
			if (ArgParse.Quiet) return;
			Console.WriteLine(msg);
		}

		// Prints a colored error message to the console
		public static void PrintError(string msg)
		{
			var old = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Error:  " + msg);
			Console.ForegroundColor = old;
		}

		// Prints a colored warning message to the console
		public static void PrintWarning(string msg)
		{
			if (ArgParse.Quiet) return;
			var old = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Warning: " + msg);
			Console.ForegroundColor = old;
		}

		// Prints a normal message to the console, if verbose logging has been requested
		public static void PrintVerbose(string msg)
		{
			if (ArgParse.Quiet || !ArgParse.Verbose) return;
			var old = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(msg);
			Console.ForegroundColor = old;
		}
	}
}
