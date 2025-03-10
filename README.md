# Sunlix.NET.Primitives
![Sunlix NET Primitives (1200 x 150 px ) (2)](https://github.com/user-attachments/assets/335bae96-9c6d-4930-b74a-effb06bb1896)

[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%208.0%20%7C%209.0-blue)](https://dotnet.microsoft.com/en-us/)
[![NuGet](https://img.shields.io/nuget/v/Sunlix.NET.Primitives.svg)](https://www.nuget.org/packages/Sunlix.NET.Primitives/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Sunlix.NET.Primitives.svg)](https://www.nuget.org/packages/Sunlix.NET.Primitives/)
[![GitHub license](https://img.shields.io/github/license/Sunlix-Software/Sunlix.NET.Primitives.svg)](https://github.com/SunlixSoftware/Sunlix.NET.Primitives/blob/main/LICENSE)


**Sunlix.NET.Primitives** is a lightweight library providing a set of key types for structured domain modeling in C#. It includes core abstractions such as *Entity*, *Value Object*, *Aggregate Root*, and *Domain Events*, essential for implementing domain-driven design (DDD) principles. Additionally, it provides *Enumeration* for defining strongly-typed sets of named values and *Error* for structured error handling. The library is designed to be extensible, intuitive, and efficient, making it suitable for a wide range of applications where well-structured domain models are needed.

## Contents
- [Installation](#installation)
- [Overview of Core Concepts](#overview-of-core-concepts)
- [Usage](#usage)
  - [Entity](#entity)

## Installation
#### NuGet Package  
The package is available on NuGet. You can install it using the following command:

```sh
dotnet add package Sunlix.NET.Primitives
```
#### Installing from Source (GitHub)  
If you prefer to install the library directly from the source code, you can clone the repository and reference the project in your solution:

```sh
git clone https://github.com/Sunlix-Software/Sunlix.NET.Primitives.git
```
Then, add a project reference in your .csproj file:
```sh
<ItemGroup>
    <ProjectReference Include="..\Sunlix.NET.Primitives\src\Sunlix.NET.Primitives.csproj" />
</ItemGroup>
```
#### Minimum Requirements  
Compatible with **.NET 6**, **.NET 8**, and **.NET 9**.  
The library has no external dependencies and can be used in any .NET application.

##  Overview of Core Concepts


##  Usage
This section covers the fundamental base classes and structures used in Sunlix.NET.Primitives, along with usage examples and important implementation nuances.

### Entity
An **Entity** represents an object with a distinct and persistent identity, uniquely identified by an identifier rather than its attributes. Entities are designed to model domain concepts that require continuity over time, ensuring that their identity remains stable despite changes to their attributes. Entities enable the tracking and management of domain objects with unique identities, such as users, orders, or products, within the context of a domain model.

#### 🔑 Key Characteristics
🔹 **Identity-based equality** – Two entities are equal if they share the same `Id`.  
🔹 **Persistence** – Entity identity remains constant throughout its lifecycle.  
🔹 **Mutable attributes** – The values of an entity's attributes may change, but its identity does not.

**📝 Example:**
```csharp
public class Order : Entity<int>
{
    public virtual Money? TotalAmount { get; private set; }

    private Order() { }
    public Order(int id, Money? totalAmount) : base(id)
    {
        TotalAmount = totalAmount;
    }

    public void UpdateTotalAmount(Money totalAmount)
    {
        if (totalAmount is null)
            throw new ArgumentNullException(nameof(totalAmount), "Total amount cannot be null.");
        TotalAmount = newAmount;
    }
}
```
> **📄 Note:** *The `Entity<TId>` class includes a parameterless protected constructor and a virtual `Id` property to support compatibility with Object-Relational Mappers (ORMs) like NHibernate, which require a [parameterless constructor](https://nhibernate.info/doc/nhibernate-reference/persistent-classes.html#persistent-classes-poco-constructor) for entity instantiation. Although Entity Framework Core supports [parameterized constructors](https://learn.microsoft.com/en-us/ef/core/modeling/constructors), it still requires navigation properties to be virtual to enable [proxy-based lazy loading](https://learn.microsoft.com/en-us/ef/core/querying/related-data/lazy).*
>
> *This design choice introduces a trade-off where persistence concerns slightly leak into the domain model, but it enables seamless integration with the ORM frameworks.* 

#### ⚖️ Equality Rules
Entities follow identifier equality rather than structural equality.

| Scenario | Explanation | Result |
|----------|-------------|----------------------------|
| Same object reference | *Both variables point to the same object in memory, so they are inherently equal.* | ✅ |
| Same type, same Id | *Entities of the same type with matching Id values are considered equal, regardless of attributes.* | ✅ |
| Same type, different Id | *Entities of the same type but with different Id values are not equal.* | ❌ |
| Different type | *Entities must be of the same type to be considered equal, even if their Id values match.* | ❌ |
| Other entity is null | *An Entity cannot be equal to null; equals returns false for null comparisons.* | ❌ |
| Default Id (null or default(TId)) | *Entities with default Id values (e.g., null or 0) are not considered equal, as default Id typically indicates an uninitialized state.* | ❌ |

> **⚠️ Warning:** *if one entity inherits from another and they share the same Id, they will still not be considered equal.*
**📝 Example:**
```csharp
// Same object reference
var order1 = new Order(id: 1, totalAmount: new Money(10, "USD"));
var order2 = order1;
order1.Equals(order2);                                                        // ✅ True

// Same type, same Id
var order1 = new Order(id: 1, totalAmount: new Money(10, "USD"));
var order2 = new Order(id: 1, totalAmount: new Money(20, "USD"));
order1.Equals(order2);                                                        // ✅ True

// Same type, different Id
var order1 = new Order(id: 1, totalAmount: new Money(10, "USD"));
var order2 = new Order(id: 2, totalAmount: new Money(20, "USD"));
order1.Equals(order2);                                                        // ❌ False

// Different type
var order = new Order(id: 1, totalAmount: new Money(10, "USD"));
var invoice = new Invoice(id: 1, totalAmount: new Money(20, "USD"));
order.Equals(invoice);                                                        // ❌ False

// Other entity is null
var order1 = new Order(id: 1, totalAmount: new Money(10, "USD"));
Order order2 = null;
order1.Equals(order2);                                                        // ❌ False

// Default Id (null or default(TId))
var order1 = new Order();
var order2 = new Order();
order1.Equals(order2);                                                        // ❌ False
```
---

## 🤝 Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

