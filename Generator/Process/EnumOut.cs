/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace Gen
{
	// The processed partner of EnumSpec
	public sealed class EnumOut
	{
		#region Fields
		// The spec object that this object was generated from
		public readonly EnumSpec Spec;
		#endregion // Fields

		private EnumOut(EnumSpec spec)
		{
			Spec = spec;
		}

		// Processes an EnumSpec object into an EnumOut object
		public static bool TryProcess(EnumSpec spec, out EnumOut? output)
		{
			output = null;

			return true;
		}
	}
}
