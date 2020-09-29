/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

using System;

namespace VVK
{
	/// <summary>
	/// Exception thrown by <see cref="Vk.InstanceFunctionTable"/> and <see cref="Vk.DeviceFunctionTable"/> when an
	/// extension function is called that has not been loaded by the table.
	/// </summary>
	public sealed class FunctionNotLoadedException : Exception
	{
		/// <summary>
		/// The name of the function that was called and was not loaded.
		/// </summary>
		public readonly string FunctionName;

		internal FunctionNotLoadedException(string func) :
			base($"Attempt to call function '{func}' which has not been loaded")
		{
			FunctionName = func;
		}
	}
}
