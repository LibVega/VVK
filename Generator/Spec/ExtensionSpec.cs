/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// Loaded specification for a <extension> node
	public sealed class ExtensionSpec
	{
		#region Fields
		// The name of the extension
		public readonly string Name;
		// The extension number
		public readonly uint Number;
		// The extension version
		public readonly uint Version;
		#endregion // Fields

		public ExtensionSpec(string name, uint number, uint version)
		{
			Name = name;
			Number = number;
			Version = version;
		}
	}
}
