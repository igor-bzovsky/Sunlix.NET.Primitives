# Sunlix.NET.Primitives
![Sunlix NET Primitives (1200 x 150 px )](https://github.com/user-attachments/assets/3f89d51d-633e-468e-843a-58526157ba02)





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
An **Entity** represents an object with a distinct and persistent identity, uniquely identified by an identifier rather than its attributes. Entities are designed to model domain concepts that require continuity over time, ensuring that their identity remains stable despite changes to their attributes.

**🔹 Purpose:** Entities enable the tracking and management of domain objects with unique identities, such as users, orders, or products, within the context of a domain model.

**🔹 Key Characteristics:** Identity-based equality, persistence, and mutability of attributes (while preserving the identifier).

### Aggregate Root
An **Aggregate Root** is an Entity that serves as the gateway to a cluster of related objects, known as an aggregate. It enforces consistency rules and coordinates changes, ensuring the integrity of the entire group in line with domain-driven design (DDD) principles.

- **Purpose:** Aggregate Roots ensure consistency and enforce domain rules within a bounded context, serving as the entry point for operations that involve multiple related objects.
- **Key Characteristics:** Single point of access, enforcement of invariants, and coordination of updates within the aggregate.

---

## 🤝 Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

