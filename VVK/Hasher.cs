/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.CompilerServices;

namespace VVK
{
	// Hash code calculations
	internal unsafe static class Hasher
	{
		// Loops over a sequence of bytes to calculate a hash
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int HashBytes(void* data, uint size)
		{
			uint count = size / 8, rem = size % 8;
			ulong hash = 0xDEADBEEF0D15EA5E;
			for (uint i = 0; i < count; ++i) {
				hash ^= ((ulong*)data)[i];
			}
			for (uint i = 0; i < rem; ++i) {
				hash ^= ((byte*)data)[size - rem + i];
			}
			return (int)(hash & 0xFFFFFFFF) ^ (int)(hash >> 32);
		}
	}
}
