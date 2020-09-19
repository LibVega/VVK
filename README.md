# VVK

Low-overhead Vulkan API bindings for .NET 5 and C# 9.

The bindings are auto-generated from the `vk.xml` standard definition, using a hand-crafted generator for maximum control. The generated code takes advantage of new high-performance options in C# 9.0, including unmanaged function pointers and `SkipLocalsInitAttribute`.

These bindings are used for the [Vega](https://github.com/VegaLib/Vega) graphics framework, but have no dependencies and can be freely incorporated into any other projects. The code for the generator and the bindings are under the MIT license.
