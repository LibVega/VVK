# VVK

[![License](https://img.shields.io/badge/License-MIT-green)](https://github.com/LibVega/VVK/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/VVK)](https://www.nuget.org/packages/VVK/)
[![Build Status](https://travis-ci.com/LibVega/VVK.svg?branch=master)](https://travis-ci.com/LibVega/VVK)

Low-overhead Vulkan API bindings for .NET 5 and C# 9.

The API provides a low level, nearly one-to-one mapping to the C API, including all of the extension types and functions. Very little infrastructure is built on top of the raw types, except where required by C# (such as function tables). The API uses features new to C# 9 to maximize the performance of the library - namely raw function pointers for faster function calls over unmanaged delegates.

The majority of the API is generated from the `vk.xml` specification file released by Khronos. A few hand written utility types and extensions are present as well. The library can be easily updated by simply regerating against the latest specification file, and fixing any small issues that may arise with new spec versions. The source code for the generator is in this repo.

A full usage guide, and other notes about the API and generator, can be found on [the Wiki](https://github.com/LibVega/VVK/wiki).

## Contributing

Contributions are welcome, particularly with improvements to the generator and the hand-written components of the bindings.

Updates to new Vulkan spec versions is easy, as long as Khronos doesn't make any changes to their xml spec structure. The primary authors of the library will try to stay on top of new spec versions.

## License

The code for the generator and the bindings are under the MIT license.

## Quick Usage Guide

This is an abridged version of the usage guide found on the Wiki.

### Naming

* All library types are in the `Vulkan` namespace.
* Vulkan API types have unchanged names.
* All constant values are in the `VkConstants` static class.
* Additional utility types are in the `Vulkan` and `Vulkan.VVK` namespaces.

### Typing

* All enum and bitmask types are `enum`s in the API.
* All composite types are `struct`s.
* Handles are split into two types:
  * `VulkanHandle<VkTYPE>` for the raw object handles (pointers)
  * `VkTYPE` for composite handle objects, which hold the handle parent object and related references, in addition to the raw handle. They also have the functions associated with the specific handle type. These are class types.
*  There are special types for native strings (`VVK.NativeString`) and fixed strings (those extending `VVK.IFixedString`).
* All struct types have a static `New()` function used to setup their fields properly. This is important for "typed" structs (that start with a `VkStructureType` field), as this sets up their `sType` field correctly. Struct constructors with parameters will also set the `sType` field to the correct value. ***It is important to remember to use `New()` or a constructor with parameters, or always manually init the type.***

### Functions

* Functions are placed into function tables.
* Functions are loaded with `vkGetInstanceProcAddr` and `vkGetDeviceProcAddr`.
* Functions in the tables take raw pointers, but there are additionally functions in the handle types that take `in`, `ref`, and `out` variables.
* Global functions are in `VkInstance`, and `InstanceFunctionTable`.
