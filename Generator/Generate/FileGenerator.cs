/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using Gen.Generate;
using System;
using System.IO;
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
		public uint BlockLevel { get; private set; } = 0;
		private string _indentString = "";

		// If the generator has been disposed
		public bool IsDisposed { get; private set; } = false;
		#endregion // Fields

		public FileGenerator(string path, string fileComment)
		{
			var filePath = Path.Combine(ArgParse.OutputPath, path);
			var dirName = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(dirName)) {
				Directory.CreateDirectory(dirName!);
			}

			_file = new StreamWriter(File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None),
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
		public void WriteLine(string line) => _file.WriteLine(_indentString + line);

		// Writes a blank line to the file
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteLine() => _file.WriteLine();

		// Writes the file header
		private void writeHeader(string comment)
		{
			// License text
			WriteLine("/*");
			WriteLine(" * MIT License - Copyright (c) 2020 Sean Moss");
			WriteLine(" * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'");
			WriteLine(" * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.");
			WriteLine(" */");

			// Write generation notice
			WriteLine();
			WriteLine("/* This file was generated using VVKGen. */");
			WriteLine("/* This file should not be edited by hand. All edits will be lost on next generation. */");

			// Write comment
			WriteLine();
			WriteLine($"/* {comment} */");

			// Write standard using statements
			WriteLine();
			WriteLine("using System;");
			WriteLine();
		}
		#endregion // File-Level

		#region Blocks
		// Opens a new block context with the given header, increasing the indent level
		public BlockWriter PushBlock(string? header)
		{
			var b = new BlockWriter(this, header);
			BlockLevel += 1;
			_indentString += "\t";
			return b;
		}

		// Pops an indent level from the source
		public void PopBlock()
		{
			if (BlockLevel == 0) {
				throw new InvalidOperationException("Too many block pops in FileGenerator");
			}
			BlockLevel -= 1;
			_indentString = _indentString.Substring(0, _indentString.Length - 1);
		}
		#endregion // Blocks

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
