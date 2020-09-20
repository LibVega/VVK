/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen.Generate
{
	// Manages the context for a block in a generating C# source file
	public sealed class BlockWriter : IDisposable
	{
		#region Fields
		// The file generator this writer is operating on
		public readonly FileGenerator File;
		// The block depth that this writer is operating at
		public readonly uint BlockLevel;

		// If the block has been disposed
		public bool IsDisposed { get; private set; } = false;
		#endregion // Fields

		// Do not call directly, call from PushBlock() in here or FileGenerator only
		public BlockWriter(FileGenerator file, string? header)
		{
			File = file;
			BlockLevel = file.BlockLevel + 1;

			if (header is not null) {
				File.WriteLine(header);
			}
			File.WriteLine("{");
		}

		// Writes a line in the block
		public void WriteLine(string line)
		{
			if (BlockLevel != File.BlockLevel) {
				throw new InvalidOperationException("Cannot write to a block outside of it's context");
			}
			File.WriteLine(line);
		}

		// Writes a blank line to the file
		public void WriteLine()
		{
			if (BlockLevel != File.BlockLevel) {
				throw new InvalidOperationException("Cannot write to a block outside of it's context");
			}
			File.WriteLine();
		}

		// Opens up a new block context at this level
		public BlockWriter PushBlock(string? header)
		{
			if (BlockLevel != File.BlockLevel) {
				throw new InvalidOperationException("Cannot open a new block outside of the current block context");
			}
			return File.PushBlock(header);
		}

		#region IDisposable
		public void Dispose()
		{
			if (!IsDisposed) {
				if (File.BlockLevel != BlockLevel) {
					throw new InvalidOperationException("Out-of-order block pops in FileGenerator");
				}
				File.PopBlock();
				File.WriteLine("}");
			}
			IsDisposed = true;
		}
		#endregion // IDisposable
	}
}
