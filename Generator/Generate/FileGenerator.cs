/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Gen
{
	// Manages the generation context for a C# source file
	public sealed class FileGenerator : IDisposable
	{
		#region Fields
		// The handle to the file
		private readonly StreamWriter _file;

		// Indent values
		public uint IndentLevel { get; private set; } = 0;
		private string _indentString = "";

		// If the generator has been disposed
		public bool IsDisposed { get; private set; } = false;
		#endregion // Fields

		public FileGenerator(string path, string fileComment)
		{
			var dirName = Path.GetDirectoryName(path);
			if (!Directory.Exists(dirName)) {
				Directory.CreateDirectory(dirName!);
			}

			_file = new StreamWriter(
				File.Open(Path.Combine(ArgParse.OutputPath, path), FileMode.Create, FileAccess.Write, FileShare.None), 
				leaveOpen: false);

			writeHeader(fileComment);
		}
		~FileGenerator()
		{
			dispose(false);
		}

		#region File-Level
		// Writes a line to the file, respecting the current indent level
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void writeLine(string line) => _file.WriteLine(_indentString + line);

		// Writes a blank line to the file
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void writeLine() => _file.WriteLine();

		// Writes the file header
		private void writeHeader(string comment)
		{
			// License text
			writeLine("/*");
			writeLine(" * MIT License - Copyright (c) 2020 Sean Moss");
			writeLine(" * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'");
			writeLine(" * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.");
			writeLine(" */");

			// Write generation notice
			writeLine();
			writeLine("/* This file was generated using VVKGen. */");
			writeLine("/* This file should not be edited by hand. All edits will be lost on next generation. */");

			// Write comment
			writeLine();
			writeLine($"/* {comment} */");

			// Write standard using statements
			writeLine();
			writeLine("using System;");
			writeLine();
		}
		#endregion // File-Level

		#region IDisposable
		public void Dispose()
		{
			dispose(true);
			GC.SuppressFinalize(this);
		}

		private void dispose(bool disposing)
		{
			if (!IsDisposed) {
				if (disposing) {
					_file.Flush();
					_file.Close();
					_file.Dispose();
				}
			}
			IsDisposed = true;
		}
		#endregion // IDisposable
	}
}
