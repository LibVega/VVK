/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Collections.Generic;

namespace Gen
{
	// Contains the results from processing a ParseResults object
	public sealed class ProcessResults
	{
		#region Fields
		// The list of API constants
		public readonly Dictionary<string, ConstantOut> Constants;
		// The list of function pointer types
		public readonly Dictionary<string, FuncOut> FuncPointers;
		// The list of vendors
		public readonly Dictionary<string, Vendor> Vendors;
		#endregion // Fields

		private ProcessResults()
		{
			Constants = new();
			FuncPointers = new();
			Vendors = new();
			Vendors.Add("", new Vendor(""));
		}

		private Vendor getOrCreateVendor(string name) =>
			Vendors.TryGetValue(name, out var ven) ? ven : (Vendors[name] = new Vendor(name));

		// Top-level processing function
		public static bool TryProcess(ParseResults spec, out ProcessResults? proc)
		{
			proc = new();
			var names = new NameHelper(spec.VendorNames);

			// Process the constants
			Console.WriteLine("Processing API constants...");
			foreach (var constSpec in spec.Constants) {
				if (ConstantOut.TryProcess(constSpec.Value) is not ConstantOut constOut) {
					return false;
				}
				proc.Constants.Add(constOut.Name, constOut);
				Program.PrintVerbose($"\tProcessed constant {constOut.Name} = {constOut.Value}");
			}

			// Process the functions and add them to the name helper
			Console.WriteLine("Processing function pointers...");
			foreach (var funcSpec in spec.FuncPointers) {
				if (FuncOut.TryProcess(funcSpec.Value, names) is not FuncOut funcOut) {
					return false;
				}
				proc.FuncPointers.Add(funcOut.Name, funcOut);
				Program.PrintVerbose($"\tProcessed function {funcOut.Name}");
			}
			names.RegisterFunctions(proc.FuncPointers);

			// Process the enums
			Console.WriteLine("Processing enum types...");
			foreach (var enumSpec in spec.Enums) {
				if (EnumOut.TryProcess(enumSpec.Value, names) is not EnumOut enumOut) {
					return false;
				}
				proc.getOrCreateVendor(enumOut.VendorName).Enums.Add(enumOut.Name, enumOut);
				Program.PrintVerbose($"\tProcessed enum {enumOut.ProcessedName}");
			}

			// Process the structs
			Console.WriteLine("Processing struct types...");
			foreach (var structSpec in spec.Structs) {
				if (StructOut.TryProcess(structSpec.Value, names) is not StructOut structOut) {
					return false;
				}
				proc.getOrCreateVendor(structOut.VendorName).Structs.Add(structOut.Name, structOut);
				Program.PrintVerbose($"\tProcessed struct {structOut.Name}");
			}

			// Process the handles
			Console.WriteLine("Processing handle types...");
			foreach (var handleSpec in spec.Handles.Values) {
				if (HandleOut.TryProcess(handleSpec, names) is not HandleOut handleOut) {
					return false;
				}
				proc.getOrCreateVendor(handleOut.VendorName).Handles.Add(handleOut.Name, handleOut);
				Program.PrintVerbose($"\tProcessed handle {handleOut.Name}");
			}

			return true;
		}
	}
}
