/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// Represents a block within a generating source file, for automatically closing and updating the generation 
	// context
	public sealed class SourceBlock : IDisposable
	{
		#region Fields
		// The file that this block is in
		public readonly SourceFile File;
		// The block depth for this block
		public readonly uint Depth;
		#endregion // Fields

		public SourceBlock(SourceFile file)
		{
			File = file;
			Depth = file.BlockDepth;
		}

		public SourceBlock PushBlock(string? header)
		{
			if (header is not null) {
				File.WriteLine(header);
			}
			File.WriteLine("{");

			File.PushBlock(Depth);
			return new(File);
		}

		public void WriteLine(string line)
		{
			if (File.BlockDepth != Depth) {
				throw new InvalidOperationException("Cannot write to block the current depth");
			}
			File.WriteLine(line);
		}

		public void WriteLine() => File.WriteLine();

		#region IDisposable
		public void Dispose()
		{
			File.PopBlock(Depth);
		}
		#endregion // IDisposable
	}
}
