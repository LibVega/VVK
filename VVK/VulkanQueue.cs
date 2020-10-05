/*
 * MIT License - Copyright (c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at <https://opensource.org/licenses/MIT>.
 */

using System;

namespace VVK
{
	/// <summary>
	/// Wraps a VkQueue object at a higher level, providing an OOP approach to VkQueue resources and functions. Queue
	/// functions are thread-safe.
	/// </summary>
	public unsafe sealed partial class VulkanQueue
	{
		#region Fields
		/// <summary>
		/// The parent device that owns this queue.
		/// </summary>
		public readonly VulkanDevice Parent;
		/// <summary>
		/// The API handle for this queue.
		/// </summary>
		public readonly Vk.Queue Handle;
		/// <summary>
		/// The queue family index that this queue is from.
		/// </summary>
		public readonly uint FamilyIndex;
		/// <summary>
		/// The index of the queue within the queue family.
		/// </summary>
		public readonly uint QueueIndex;

		// Submission lock
		private readonly object _lock = new();
		#endregion // Fields

		/// <summary>
		/// Creates a new queue wrapper from an existing device, queue handle, and queue indices.
		/// </summary>
		/// <param name="parent">The device that owns the queue.</param>
		/// <param name="handle">The existing queue handle.</param>
		/// <param name="familyIdx">The queue family index.</param>
		/// <param name="queueIdx">The index within the queue family.</param>
		public VulkanQueue(VulkanDevice parent, Vk.Queue handle, uint familyIdx, uint queueIdx)
		{
			Parent = parent.Handle ? parent : throw new ArgumentNullException(nameof(parent), "Null parent device");
			Handle = handle ? handle : throw new ArgumentNullException(nameof(handle), "Null queue handle");
			FamilyIndex = familyIdx;
			QueueIndex = queueIdx;
		}

		/// <summary>
		/// Creates a new queue wrapper from an existing device and queue indices, and retrieves the queue handle
		/// associated with the indices.
		/// </summary>
		/// <param name="parent">The device that owns the queue.</param>
		/// <param name="familyIdx">The queue family index.</param>
		/// <param name="queueIdx">The index within the queue family.</param>
		public VulkanQueue(VulkanDevice parent, uint familyIdx, uint queueIdx)
		{
			Parent = parent.Handle ? parent : throw new ArgumentNullException(nameof(parent), "Null parent device");
			FamilyIndex = familyIdx;
			QueueIndex = queueIdx;

			Vk.Queue handle;
			parent.GetDeviceQueue(familyIdx, queueIdx, &handle);
			if (!handle) {
				throw new ArgumentException($"The device {parent} does not have a queue at [{familyIdx}:{queueIdx}]");
			}
			Handle = handle;
		}
	}
}
