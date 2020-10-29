/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

using System;

namespace Vk
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
	public sealed class ResultException : Exception
	{
		/// <summary>
		/// The result code that generated the exception.
		/// </summary>
		public readonly Vk.Result Result;
		/// <summary>
		/// An additional user message explaining the call failure.
		/// </summary>
		public readonly string UserMessage;

		/// <summary>
		/// Create a new exception for the given result.
		/// </summary>
		/// <param name="result">The result code.</param>
		public ResultException(Vk.Result result) :
			base($"Vulkan call failed with code {result}")
		{
			Result = result;
			UserMessage = String.Empty;
		}

		/// <summary>
		/// Create a new exception for the given result and function name.
		/// </summary>
		/// <param name="result">The result code.</param>
		/// <param name="userMsg">A message further explaining the failure.</param>
		public ResultException(Vk.Result result, string userMsg) :
			base($"{userMsg} (code={result})")
		{
			Result = result;
			UserMessage = userMsg;
		}
	}
}
