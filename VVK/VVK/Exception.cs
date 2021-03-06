﻿/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

using System;

namespace Vulkan.VVK
{
	/// <summary>
	/// Exception that is thrown for <see cref="VkResult"/> error codes.
	/// </summary>
	public sealed class ResultException : Exception
	{
		/// <summary>
		/// The result code that generated the exception.
		/// </summary>
		public readonly VkResult Result;
		/// <summary>
		/// An additional user message explaining the call failure.
		/// </summary>
		public readonly string UserMessage;

		/// <summary>
		/// Create a new exception for the given result.
		/// </summary>
		/// <param name="result">The result code.</param>
		public ResultException(VkResult result) :
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
		public ResultException(VkResult result, string userMsg) :
			base($"{userMsg} (code={result})")
		{
			Result = result;
			UserMessage = userMsg;
		}
	}

	/// <summary>
	/// Exception that is thrown when attempting to call an API function that was not loaded.
	/// </summary>
	public sealed class FunctionNotLoadedException : Exception
	{
		/// <summary>
		/// The name of the function that was called without being loaded.
		/// </summary>
		public readonly string Function;

		internal FunctionNotLoadedException(string fn)
			: base($"Attempt to call null function '{fn}'")
		{
			Function = fn;
		}
	}
}
