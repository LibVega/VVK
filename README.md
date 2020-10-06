# VVK

Low-overhead Vulkan API bindings for .NET 5 and C# 9.

The raw API bindings are generated from the `vk.xml` specification file published by Khronos. These bindings can be used directly, or with the hand-written wrapper classes that provide a thin OOP layer on the raw Vulkan API. The usage guide for both approaches can be found on [the Wiki](https://github.com/VegaLib/VVK/wikihttps://github.com/VegaLib/VVK/wiki).

The bindings use features new to C# 9, namely raw function pointers for faster function calls over unmanaged delegates. The code for the generator is available in this repo.

## Contributing

Contributions are welcome, particularly with improvements to the generator and the hand-written components of the bindings.

Updates to new Vulkan spec versions is easy, as long as Khronos doesn't make any changes to their xml spec structure. The primary authors of the library will try to stay on top of new spec versions.

## License

These bindings are used for the [Vega](https://github.com/VegaLib/Vega) graphics framework, but have no dependencies and can be incorporated into other projects. The code for the generator and the bindings are under the MIT license.
