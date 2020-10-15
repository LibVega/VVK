/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Runtime.CompilerServices;

namespace VVK
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
		public static bool IsError(this Vk.Result result) => (int)result < 0;

		/// <summary>
		/// Gets if the result value represents an error.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="copy">The copied result value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsError(this Vk.Result result, out Vk.Result copy)
		{
			copy = result;
			return (int)result < 0;
		}

		/// <summary>
		/// Gets if the result is <see cref="Vk.Result.Success"/>.
		/// </summary>
		/// <param name="result">The result to check.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSuccess(this Vk.Result result) => result == Vk.Result.Success;

		/// <summary>
		/// Gets if the result is <see cref="Vk.Result.Success"/>.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="copy">The copied result value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSuccess(this Vk.Result result, out Vk.Result copy)
		{
			copy = result;
			return result == Vk.Result.Success;
		}

		/// <summary>
		/// Gets if the result is a status value, either <see cref="Vk.Result.Success"/> or some other non-error value.
		/// </summary>
		/// <param name="result">The result to check.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsStatus(this Vk.Result result) => (int)result >= 0;

		/// <summary>
		/// Gets if the result is a status value, either <see cref="Vk.Result.Success"/> or some other non-error value.
		/// </summary>
		/// <param name="result">The result to check.</param>
		/// <param name="copy">The copied result value.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsStatus(this Vk.Result result, out Vk.Result copy)
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
		public static void Throw(this Vk.Result result, string? message = null)
		{
			if (result != Vk.Result.Success) {
				if (message is null) throw new ResultException(result);
				else throw new ResultException(result, message);
			}
		}
	}
}
