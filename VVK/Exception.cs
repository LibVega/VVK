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

	/// <summary>
	/// Exception that is thrown for <see cref="Vk.Result"/> error codes.
	/// </summary>
	public sealed class VulkanResultException : Exception
	{
		/// <summary>
		/// The result code that generated the exception.
		/// </summary>
		public readonly Vk.Result Result;
		/// <summary>
		/// The name of the Vulkan API function that generated the result.
		/// </summary>
		public readonly string FunctionName;
		/// <summary>
		/// The caller name that generated the result exception.
		/// </summary>
		public readonly string CallerName;
		/// <summary>
		/// The source line that generated the result exception.
		/// </summary>
		public readonly uint CallerLine;

		internal VulkanResultException(Vk.Result result, string func, string name, uint line) :
			base($"Call '{func}' failed with code {result} at [{name}:{line}]")
		{
			Result = result;
			FunctionName = func;
			CallerName = name;
			CallerLine = line;
		}
	}
}
