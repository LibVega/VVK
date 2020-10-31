/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

using System;

namespace Vk
{
	// Extensions to Vk.ClearColorValue
	public unsafe partial struct ClearColorValue
	{
		public float FR { readonly get => Float32[0]; set => Float32[0] = value; }
		public float FG { readonly get => Float32[1]; set => Float32[1] = value; }
		public float FB { readonly get => Float32[2]; set => Float32[2] = value; }
		public float FA { readonly get => Float32[3]; set => Float32[3] = value; }

		public int IR { readonly get => Int32[0]; set => Int32[0] = value; }
		public int IG { readonly get => Int32[1]; set => Int32[1] = value; }
		public int IB { readonly get => Int32[2]; set => Int32[2] = value; }
		public int IA { readonly get => Int32[3]; set => Int32[3] = value; }

		public uint UR { readonly get => Uint32[0]; set => Uint32[0] = value; }
		public uint UG { readonly get => Uint32[1]; set => Uint32[1] = value; }
		public uint UB { readonly get => Uint32[2]; set => Uint32[2] = value; }
		public uint UA { readonly get => Uint32[3]; set => Uint32[3] = value; }

		public ClearColorValue(float r, float g, float b, float a)
		{
			Float32[0] = r; Float32[1] = g; Float32[2] = b; Float32[3] = a;
		}

		public ClearColorValue(int r, int g, int b, int a)
		{
			Int32[0] = r; Int32[1] = g; Int32[2] = b; Int32[3] = a;
		}

		public ClearColorValue(uint r, uint g, uint b, uint a)
		{
			Uint32[0] = r; Uint32[1] = g; Uint32[2] = b; Uint32[3] = a;
		}
	}

	// Extensions to Vk.ClearValue
	public unsafe partial struct ClearValue
	{
		public ClearValue(in Vk.ClearColorValue value)
		{
			DepthStencil = default;
			Color = value;
		}

		public ClearValue(in Vk.ClearDepthStencilValue value)
		{
			Color = default;
			DepthStencil = value;
		}
	}
}
