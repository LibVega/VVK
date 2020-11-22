﻿/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.IO;
using System.Text;

namespace Gen
{
	public sealed class SourceFile : IDisposable
	{
		#region Fields
		// The relative path of the file within the output directory
		public readonly string RelativePath;
		// The full path to the file
		public string FullPath => Path.Combine(ArgParse.OutputPath, RelativePath);

		// Indent values
		public uint BlockDepth { get; private set; } = 0;
		private string _indentStr = "";

		// The file stream
		private readonly StreamWriter _file;
		#endregion // Fields

		public SourceFile(string relPath)
		{
			RelativePath = relPath;

			// Open file
			var file = File.Open(FullPath, FileMode.Create, FileAccess.Write, FileShare.None);
			_file = new StreamWriter(file, Encoding.UTF8, leaveOpen: false);

			// Write header
			foreach (var line in STANDARD_HEADER) {
				_file.WriteLine(line);
			}
		}

		public void WriteLine(string line)
		{
			_file.Write(_indentStr);
			_file.WriteLine(line);
		}

		public void WriteLine() => _file.WriteLine();

		// Only call when there are no active blocks
		public SourceBlock PushBlock(string? header)
		{
			if (BlockDepth != 0) {
				throw new InvalidOperationException("A block is already active in the source file");
			}

			if (header is not null) {
				_file.WriteLine(header);
			}
			_file.WriteLine("{");

			PushBlock(0);
			return new(this);
		}

		// Only call this from SourceBlock
		public void PushBlock(uint depth)
		{
			if (depth != BlockDepth) {
				throw new InvalidOperationException("Cannot push a new source block at the current depth");
			}

			BlockDepth += 1;
			_indentStr = new string('\t', (int)BlockDepth);
		}

		// Only call this from SourceBlock
		public void PopBlock(uint depth)
		{
			if (BlockDepth != depth) {
				throw new InvalidOperationException("Mismatch in block depth of popped block");
			}

			BlockDepth -= 1;
			_indentStr = new string('\t', (int)BlockDepth);
		}

		public void Dispose()
		{
			if (BlockDepth != 0) {
				throw new InvalidOperationException("Attempting to close a source file that has open blocks");
			}

			_file.WriteLine();
			_file.WriteLine("} // namespace Vulkan");
			_file.WriteLine();
			_file.Flush();
			_file.Close();
			_file.Dispose();
		}

		private static readonly string[] STANDARD_HEADER = new string[] {
			"/*",
			" * MIT License - Copyright(c) 2020 Sean Moss",
			" * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'",
			" * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.",
			" */",
			"",
			"/// This file was generated by VVKGen. Edits to this file will be lost on next generation.",
			"",
			"using System;",
			"using System.Runtime.InteropServices;",
			"using System.Runtime.CompilerServices;",
			"",
			"namespace Vulkan",
			"{",
			""
		};
	}
}