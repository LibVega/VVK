/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.CompilerServices;

namespace Vulkan
{
	/// <summary>
	/// Contains utility functionality for working with <see cref="Vk.Result"/> values.
	/// </summary>
	public static class ResultUtils
	{
		/// <summary>
		/// Gets if the result value represents an error.
		/// </summary>
		/// <param name="result">The result to check.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsError(this VkResult result) => (int)result < 0;

		/// <summary>
		/// Gets if the result value represents an error.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="copy">The copied result value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsError(this VkResult result, out VkResult copy)
		{
			copy = result;
			return (int)result < 0;
		}

		/// <summary>
		/// Gets if the result is <see cref="Vk.Result.Success"/>.
		/// </summary>
		/// <param name="result">The result to check.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSuccess(this VkResult result) => result == VkResult.Success;

		/// <summary>
		/// Gets if the result is <see cref="Vk.Result.Success"/>.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="copy">The copied result value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSuccess(this VkResult result, out VkResult copy)
		{
			copy = result;
			return result == VkResult.Success;
		}

		/// <summary>
		/// Gets if the result is a status value, either <see cref="Vk.Result.Success"/> or some other non-error value.
		/// </summary>
		/// <param name="result">The result to check.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsStatus(this VkResult result) => (int)result >= 0;

		/// <summary>
		/// Gets if the result is a status value, either <see cref="Vk.Result.Success"/> or some other non-error value.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="copy">The copied result value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsStatus(this VkResult result, out VkResult copy)
		{
			copy = result;
			return (int)result >= 0;
		}

		/// <summary>
		/// Throws a <see cref="ResultException"/> if the code is anything except <see cref="Vk.Result.Success"/>.
		/// </summary>
		/// <param name="result">The result to throw for.</param>
		/// <param name="message">The optional user message to add to the exception.</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Throw(this VkResult result, string? message = null)
		{
			if (result != VkResult.Success) {
				if (message is null) throw new VVK.ResultException(result);
				else throw new VVK.ResultException(result, message);
			}
		}
	}
}
