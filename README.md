# Sunlix.NET.Primitives
![Favicon (128 x 128)](https://github.com/user-attachments/assets/5a06a848-9786-4119-8a0a-3dd950039253)

[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%208.0%20%7C%209.0-blue)](https://dotnet.microsoft.com/en-us/)
[![NuGet](https://img.shields.io/nuget/v/Sunlix.NET.Primitives.svg)](https://www.nuget.org/packages/Sunlix.NET.Primitives/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Sunlix.NET.Primitives.svg)](https://www.nuget.org/packages/Sunlix.NET.Primitives/)
[![GitHub license](https://img.shields.io/github/license/Sunlix-Software/Sunlix.NET.Primitives.svg)](https://github.com/SunlixSoftware/Sunlix.NET.Primitives/blob/main/LICENSE)


**Sunlix.NET.Primitives** is a lightweight library providing a set of key types for structured domain modeling in C#. It includes core abstractions such as *Entity*, *ValueObject*, *AggregateRoot*, and *Domain Events*, essential for implementing domain-driven design (DDD) principles.

## Contents
- [Installation](#installation)
- [Overview of Core Concepts](#overview-of-core-concepts)

## Installation
#### NuGet Package  
The package is available on NuGet. You can install it using the following command:

```sh
dotnet add package Sunlix.NET.Primitives
```  
#### Minimum Requirements  
Compatible with **.NET 6**, **.NET 8**, and **.NET 9**.  

The library has no external dependencies and can be used in any .NET application.

## Overview of Core Concepts

### Entity
An **Entity** represents an object with a distinct identity that persists over time. Entities are compared based on their identifier rather than their attributes. Entities let you track objects—like customers, orders, or products—over time, ensuring continuity even as their details evolve.

---

## 🤝 Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

