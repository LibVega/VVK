﻿/*
 * MIT License - Copyright(c) 2020 Sean Moss
 * This file is subject to the terms and conditions of the MIT License, the text of which can be found in the 'LICENSE'
 * file at the root of this repository, or online at<https://opensource.org/licenses/MIT>.
 */

/// This file was generated by VVKGen. Edits to this file will be lost on next generation.

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace Vk
{

public unsafe partial struct Instance : IHandleType<Instance>
{
	public static readonly Instance Null = new();

	private readonly Handle<Instance> _handle;
	readonly Handle<Instance> IHandleType<Instance>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Instance 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Instance t) && (t._handle == _handle);
	readonly bool IEquatable<Instance>.Equals(Instance other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Instance> (in Instance handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Instance l, Instance r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Instance l, Instance r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Instance handle) => handle._handle.IsValid;
}

public unsafe partial struct PhysicalDevice : IHandleType<PhysicalDevice>
{
	public static readonly PhysicalDevice Null = new();

	private readonly Handle<PhysicalDevice> _handle;
	readonly Handle<PhysicalDevice> IHandleType<PhysicalDevice>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[PhysicalDevice 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is PhysicalDevice t) && (t._handle == _handle);
	readonly bool IEquatable<PhysicalDevice>.Equals(PhysicalDevice other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<PhysicalDevice> (in PhysicalDevice handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (PhysicalDevice l, PhysicalDevice r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (PhysicalDevice l, PhysicalDevice r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (PhysicalDevice handle) => handle._handle.IsValid;
}

public unsafe partial struct Device : IHandleType<Device>
{
	public static readonly Device Null = new();

	private readonly Handle<Device> _handle;
	readonly Handle<Device> IHandleType<Device>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Device 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Device t) && (t._handle == _handle);
	readonly bool IEquatable<Device>.Equals(Device other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Device> (in Device handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Device l, Device r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Device l, Device r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Device handle) => handle._handle.IsValid;
}

public unsafe partial struct Queue : IHandleType<Queue>
{
	public static readonly Queue Null = new();

	private readonly Handle<Queue> _handle;
	readonly Handle<Queue> IHandleType<Queue>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Queue 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Queue t) && (t._handle == _handle);
	readonly bool IEquatable<Queue>.Equals(Queue other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Queue> (in Queue handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Queue l, Queue r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Queue l, Queue r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Queue handle) => handle._handle.IsValid;
}

public unsafe partial struct CommandBuffer : IHandleType<CommandBuffer>
{
	public static readonly CommandBuffer Null = new();

	private readonly Handle<CommandBuffer> _handle;
	readonly Handle<CommandBuffer> IHandleType<CommandBuffer>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[CommandBuffer 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is CommandBuffer t) && (t._handle == _handle);
	readonly bool IEquatable<CommandBuffer>.Equals(CommandBuffer other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<CommandBuffer> (in CommandBuffer handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (CommandBuffer l, CommandBuffer r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (CommandBuffer l, CommandBuffer r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (CommandBuffer handle) => handle._handle.IsValid;
}

public unsafe partial struct DeviceMemory : IHandleType<DeviceMemory>
{
	public static readonly DeviceMemory Null = new();

	private readonly Handle<DeviceMemory> _handle;
	readonly Handle<DeviceMemory> IHandleType<DeviceMemory>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[DeviceMemory 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is DeviceMemory t) && (t._handle == _handle);
	readonly bool IEquatable<DeviceMemory>.Equals(DeviceMemory other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<DeviceMemory> (in DeviceMemory handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (DeviceMemory l, DeviceMemory r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (DeviceMemory l, DeviceMemory r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (DeviceMemory handle) => handle._handle.IsValid;
}

public unsafe partial struct CommandPool : IHandleType<CommandPool>
{
	public static readonly CommandPool Null = new();

	private readonly Handle<CommandPool> _handle;
	readonly Handle<CommandPool> IHandleType<CommandPool>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[CommandPool 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is CommandPool t) && (t._handle == _handle);
	readonly bool IEquatable<CommandPool>.Equals(CommandPool other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<CommandPool> (in CommandPool handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (CommandPool l, CommandPool r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (CommandPool l, CommandPool r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (CommandPool handle) => handle._handle.IsValid;
}

public unsafe partial struct Buffer : IHandleType<Buffer>
{
	public static readonly Buffer Null = new();

	private readonly Handle<Buffer> _handle;
	readonly Handle<Buffer> IHandleType<Buffer>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Buffer 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Buffer t) && (t._handle == _handle);
	readonly bool IEquatable<Buffer>.Equals(Buffer other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Buffer> (in Buffer handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Buffer l, Buffer r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Buffer l, Buffer r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Buffer handle) => handle._handle.IsValid;
}

public unsafe partial struct BufferView : IHandleType<BufferView>
{
	public static readonly BufferView Null = new();

	private readonly Handle<BufferView> _handle;
	readonly Handle<BufferView> IHandleType<BufferView>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[BufferView 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is BufferView t) && (t._handle == _handle);
	readonly bool IEquatable<BufferView>.Equals(BufferView other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<BufferView> (in BufferView handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (BufferView l, BufferView r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (BufferView l, BufferView r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (BufferView handle) => handle._handle.IsValid;
}

public unsafe partial struct Image : IHandleType<Image>
{
	public static readonly Image Null = new();

	private readonly Handle<Image> _handle;
	readonly Handle<Image> IHandleType<Image>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Image 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Image t) && (t._handle == _handle);
	readonly bool IEquatable<Image>.Equals(Image other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Image> (in Image handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Image l, Image r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Image l, Image r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Image handle) => handle._handle.IsValid;
}

public unsafe partial struct ImageView : IHandleType<ImageView>
{
	public static readonly ImageView Null = new();

	private readonly Handle<ImageView> _handle;
	readonly Handle<ImageView> IHandleType<ImageView>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[ImageView 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is ImageView t) && (t._handle == _handle);
	readonly bool IEquatable<ImageView>.Equals(ImageView other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<ImageView> (in ImageView handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (ImageView l, ImageView r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (ImageView l, ImageView r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (ImageView handle) => handle._handle.IsValid;
}

public unsafe partial struct ShaderModule : IHandleType<ShaderModule>
{
	public static readonly ShaderModule Null = new();

	private readonly Handle<ShaderModule> _handle;
	readonly Handle<ShaderModule> IHandleType<ShaderModule>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[ShaderModule 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is ShaderModule t) && (t._handle == _handle);
	readonly bool IEquatable<ShaderModule>.Equals(ShaderModule other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<ShaderModule> (in ShaderModule handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (ShaderModule l, ShaderModule r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (ShaderModule l, ShaderModule r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (ShaderModule handle) => handle._handle.IsValid;
}

public unsafe partial struct Pipeline : IHandleType<Pipeline>
{
	public static readonly Pipeline Null = new();

	private readonly Handle<Pipeline> _handle;
	readonly Handle<Pipeline> IHandleType<Pipeline>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Pipeline 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Pipeline t) && (t._handle == _handle);
	readonly bool IEquatable<Pipeline>.Equals(Pipeline other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Pipeline> (in Pipeline handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Pipeline l, Pipeline r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Pipeline l, Pipeline r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Pipeline handle) => handle._handle.IsValid;
}

public unsafe partial struct PipelineLayout : IHandleType<PipelineLayout>
{
	public static readonly PipelineLayout Null = new();

	private readonly Handle<PipelineLayout> _handle;
	readonly Handle<PipelineLayout> IHandleType<PipelineLayout>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[PipelineLayout 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is PipelineLayout t) && (t._handle == _handle);
	readonly bool IEquatable<PipelineLayout>.Equals(PipelineLayout other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<PipelineLayout> (in PipelineLayout handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (PipelineLayout l, PipelineLayout r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (PipelineLayout l, PipelineLayout r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (PipelineLayout handle) => handle._handle.IsValid;
}

public unsafe partial struct Sampler : IHandleType<Sampler>
{
	public static readonly Sampler Null = new();

	private readonly Handle<Sampler> _handle;
	readonly Handle<Sampler> IHandleType<Sampler>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Sampler 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Sampler t) && (t._handle == _handle);
	readonly bool IEquatable<Sampler>.Equals(Sampler other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Sampler> (in Sampler handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Sampler l, Sampler r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Sampler l, Sampler r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Sampler handle) => handle._handle.IsValid;
}

public unsafe partial struct DescriptorSet : IHandleType<DescriptorSet>
{
	public static readonly DescriptorSet Null = new();

	private readonly Handle<DescriptorSet> _handle;
	readonly Handle<DescriptorSet> IHandleType<DescriptorSet>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[DescriptorSet 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is DescriptorSet t) && (t._handle == _handle);
	readonly bool IEquatable<DescriptorSet>.Equals(DescriptorSet other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<DescriptorSet> (in DescriptorSet handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (DescriptorSet l, DescriptorSet r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (DescriptorSet l, DescriptorSet r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (DescriptorSet handle) => handle._handle.IsValid;
}

public unsafe partial struct DescriptorSetLayout : IHandleType<DescriptorSetLayout>
{
	public static readonly DescriptorSetLayout Null = new();

	private readonly Handle<DescriptorSetLayout> _handle;
	readonly Handle<DescriptorSetLayout> IHandleType<DescriptorSetLayout>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[DescriptorSetLayout 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is DescriptorSetLayout t) && (t._handle == _handle);
	readonly bool IEquatable<DescriptorSetLayout>.Equals(DescriptorSetLayout other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<DescriptorSetLayout> (in DescriptorSetLayout handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (DescriptorSetLayout l, DescriptorSetLayout r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (DescriptorSetLayout l, DescriptorSetLayout r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (DescriptorSetLayout handle) => handle._handle.IsValid;
}

public unsafe partial struct DescriptorPool : IHandleType<DescriptorPool>
{
	public static readonly DescriptorPool Null = new();

	private readonly Handle<DescriptorPool> _handle;
	readonly Handle<DescriptorPool> IHandleType<DescriptorPool>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[DescriptorPool 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is DescriptorPool t) && (t._handle == _handle);
	readonly bool IEquatable<DescriptorPool>.Equals(DescriptorPool other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<DescriptorPool> (in DescriptorPool handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (DescriptorPool l, DescriptorPool r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (DescriptorPool l, DescriptorPool r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (DescriptorPool handle) => handle._handle.IsValid;
}

public unsafe partial struct Fence : IHandleType<Fence>
{
	public static readonly Fence Null = new();

	private readonly Handle<Fence> _handle;
	readonly Handle<Fence> IHandleType<Fence>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Fence 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Fence t) && (t._handle == _handle);
	readonly bool IEquatable<Fence>.Equals(Fence other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Fence> (in Fence handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Fence l, Fence r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Fence l, Fence r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Fence handle) => handle._handle.IsValid;
}

public unsafe partial struct Semaphore : IHandleType<Semaphore>
{
	public static readonly Semaphore Null = new();

	private readonly Handle<Semaphore> _handle;
	readonly Handle<Semaphore> IHandleType<Semaphore>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Semaphore 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Semaphore t) && (t._handle == _handle);
	readonly bool IEquatable<Semaphore>.Equals(Semaphore other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Semaphore> (in Semaphore handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Semaphore l, Semaphore r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Semaphore l, Semaphore r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Semaphore handle) => handle._handle.IsValid;
}

public unsafe partial struct Event : IHandleType<Event>
{
	public static readonly Event Null = new();

	private readonly Handle<Event> _handle;
	readonly Handle<Event> IHandleType<Event>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Event 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Event t) && (t._handle == _handle);
	readonly bool IEquatable<Event>.Equals(Event other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Event> (in Event handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Event l, Event r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Event l, Event r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Event handle) => handle._handle.IsValid;
}

public unsafe partial struct QueryPool : IHandleType<QueryPool>
{
	public static readonly QueryPool Null = new();

	private readonly Handle<QueryPool> _handle;
	readonly Handle<QueryPool> IHandleType<QueryPool>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[QueryPool 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is QueryPool t) && (t._handle == _handle);
	readonly bool IEquatable<QueryPool>.Equals(QueryPool other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<QueryPool> (in QueryPool handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (QueryPool l, QueryPool r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (QueryPool l, QueryPool r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (QueryPool handle) => handle._handle.IsValid;
}

public unsafe partial struct Framebuffer : IHandleType<Framebuffer>
{
	public static readonly Framebuffer Null = new();

	private readonly Handle<Framebuffer> _handle;
	readonly Handle<Framebuffer> IHandleType<Framebuffer>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[Framebuffer 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is Framebuffer t) && (t._handle == _handle);
	readonly bool IEquatable<Framebuffer>.Equals(Framebuffer other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<Framebuffer> (in Framebuffer handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (Framebuffer l, Framebuffer r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (Framebuffer l, Framebuffer r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (Framebuffer handle) => handle._handle.IsValid;
}

public unsafe partial struct RenderPass : IHandleType<RenderPass>
{
	public static readonly RenderPass Null = new();

	private readonly Handle<RenderPass> _handle;
	readonly Handle<RenderPass> IHandleType<RenderPass>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[RenderPass 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is RenderPass t) && (t._handle == _handle);
	readonly bool IEquatable<RenderPass>.Equals(RenderPass other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<RenderPass> (in RenderPass handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (RenderPass l, RenderPass r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (RenderPass l, RenderPass r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (RenderPass handle) => handle._handle.IsValid;
}

public unsafe partial struct PipelineCache : IHandleType<PipelineCache>
{
	public static readonly PipelineCache Null = new();

	private readonly Handle<PipelineCache> _handle;
	readonly Handle<PipelineCache> IHandleType<PipelineCache>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[PipelineCache 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is PipelineCache t) && (t._handle == _handle);
	readonly bool IEquatable<PipelineCache>.Equals(PipelineCache other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<PipelineCache> (in PipelineCache handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (PipelineCache l, PipelineCache r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (PipelineCache l, PipelineCache r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (PipelineCache handle) => handle._handle.IsValid;
}

public unsafe partial struct DescriptorUpdateTemplate : IHandleType<DescriptorUpdateTemplate>
{
	public static readonly DescriptorUpdateTemplate Null = new();

	private readonly Handle<DescriptorUpdateTemplate> _handle;
	readonly Handle<DescriptorUpdateTemplate> IHandleType<DescriptorUpdateTemplate>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[DescriptorUpdateTemplate 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is DescriptorUpdateTemplate t) && (t._handle == _handle);
	readonly bool IEquatable<DescriptorUpdateTemplate>.Equals(DescriptorUpdateTemplate other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<DescriptorUpdateTemplate> (in DescriptorUpdateTemplate handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (DescriptorUpdateTemplate l, DescriptorUpdateTemplate r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (DescriptorUpdateTemplate l, DescriptorUpdateTemplate r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (DescriptorUpdateTemplate handle) => handle._handle.IsValid;
}

public unsafe partial struct SamplerYcbcrConversion : IHandleType<SamplerYcbcrConversion>
{
	public static readonly SamplerYcbcrConversion Null = new();

	private readonly Handle<SamplerYcbcrConversion> _handle;
	readonly Handle<SamplerYcbcrConversion> IHandleType<SamplerYcbcrConversion>.Handle => _handle;

	public override readonly int GetHashCode() => _handle.GetHashCode();
	public override readonly string? ToString() => $"[SamplerYcbcrConversion 0x{(ulong)_handle:X16}]";
	public override readonly bool Equals(object? o) => (o is SamplerYcbcrConversion t) && (t._handle == _handle);
	readonly bool IEquatable<SamplerYcbcrConversion>.Equals(SamplerYcbcrConversion other) => other._handle == _handle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Vk.Handle<SamplerYcbcrConversion> (in SamplerYcbcrConversion handle) => handle._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator == (SamplerYcbcrConversion l, SamplerYcbcrConversion r) => l._handle == r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator != (SamplerYcbcrConversion l, SamplerYcbcrConversion r) => l._handle != r._handle;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool (SamplerYcbcrConversion handle) => handle._handle.IsValid;
}

} // namespace Vk
