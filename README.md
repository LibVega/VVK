# VVK

[![License](https://img.shields.io/badge/License-MIT-green)](https://github.com/VegaLib/VVK/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/VVK)](https://www.nuget.org/packages/VVK/)

Low-overhead Vulkan API bindings for .NET 5 and C# 9.

The API provides a low level, nearly one-to-one mapping to the C API, including all of the extension types and functions. Very little infrastructure is built on top of the raw types, except where required by C# (such as function tables). The API uses features new to C# 9 to maximize the performance of the library - namely raw function pointers for faster function calls over unmanaged delegates.

The majority of the API is generated from the `vk.xml` specification file released by Khronos. A few hand written utility types and extensions are present as well. The library can be easily updated by simply regerating against the latest specification file, and fixing any small issues that may arise with new spec versions. The source code for the generator is in this repo.

A full usage guide, and other notes about the API and generator, can be found on [the Wiki](https://github.com/VegaLib/VVK/wiki).

## Contributing

Contributions are welcome, particularly with improvements to the generator and the hand-written components of the bindings.

Updates to new Vulkan spec versions is easy, as long as Khronos doesn't make any changes to their xml spec structure. The primary authors of the library will try to stay on top of new spec versions.

## License

The code for the generator and the bindings are under the MIT license.

## Quick Usage Guide

This is an abridged version of the usage guide found on the Wiki.

* Naming
  * All types are in the `Vk` namespace. Types in extension vendors are further divided into namespaces (e.g. `Vk.KHR`, `Vk.EXT`, `Vk.NV`, ...).
  * All constant values are in the `Vk.Constants` static class.
* Typing
  * All enum and bitmask types are `enum`s in the API.
  * All composite types are `struct`s.
  * Functions are in `InstanceFunctionTable` and `DeviceFunctionTable`.
  * Handles are split into two types:
    * `Vk.Handle<Vk.TYPE>` for the raw object handles (pointers)
    * `Vk.TYPE` for composite handle objects, which hold the handle parent object and related references, in addition to the raw handle. They also have the functions associated with the specific handle type.
  *  There are special types for native strings (`Vk.NativeString`) and fixed strings (`Vk.FixedString`).
  * Typed structs (those that start with a `VkStructureType` field) have special static `New()` and `Init()` functions that should be used to ensure that the fields are setup correctly. ***This is very important to remember.***
* Functions
  * Functions are placed into function tables.
  * Functions are loaded with `vkGetInstanceProcAddr` and `vkGetDeviceProcAddr`.
  * Functions in the tables take raw pointers, but there are additionally functions in the handle types that take `in` and `out` variables.
  * Global functions are in `Vk.Instance`, and `Vk.InstanceFunctionTable`.
