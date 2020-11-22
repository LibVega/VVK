/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.IO;

namespace Gen
{
	// Controls the top level generation of the API code
	public static partial class APIGenerator
	{
		// Perform generation
		public static bool Generate(ProcessedSpec spec)
		{
			// Ensure output directories are created
			try {
				if (!Directory.Exists(ArgParse.OutputPath)) {
					Directory.CreateDirectory(ArgParse.OutputPath);
				}
				foreach (var vendor in spec.Vendors) {
					if (!vendor.Value.IsCore) {
						var vdir = Path.Combine(ArgParse.OutputPath, vendor.Key);
						if (!Directory.Exists(vdir)) {
							Directory.CreateDirectory(vdir);
						}
					}
				}
			}
			catch (Exception e) {
				Program.PrintError($"Failed to generate output directories - {e.Message}");
				return false;
			}

			// Create the constants
			try {
				GenerateConstants(spec);
			}
			catch (Exception e) {
				Program.PrintError($"Failed to generate constants - {e.Message}");
				return false;
			}
			Program.Print("Generated constants");

			// Create the function tables
			try {
				GenerateCommands(spec);
			}
			catch (Exception e) {
				Program.PrintError($"Failed to generate function tables - {e.Message}");
				return false;
			}
			Program.Print("Generated function tables");

			// Generate the vendor types
			foreach (var vendor in spec.Vendors.Values) {
				try {
					GenerateBitmasks(vendor);
				}
				catch (Exception e) {
					Program.PrintError($"Failed to generate bitmasks for '{vendor.Tag}' - {e.Message}");
					return false;
				}

				try {
					GenerateHandles(vendor);
				}
				catch (Exception e) {
					Program.PrintError($"Failed to generate handles for '{vendor.Tag}' - {e.Message}");
					return false;
				}

				try {
					GenerateEnums(vendor);
				}
				catch (Exception e) {
					Program.PrintError($"Failed to generate enums for '{vendor.Tag}' - {e.Message}");
					return false;
				}

				try {
					GenerateStructs(vendor);
				}
				catch (Exception e) {
					Program.PrintError($"Failed to generate structs for '{vendor.Tag}' - {e.Message}");
					return false;
				}

				Program.Print($"Generated types for vendor '{vendor.Tag}'");
			}

			return true;
		}
	}
}
