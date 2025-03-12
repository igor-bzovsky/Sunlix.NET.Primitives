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
  - [Entity&lt;TId&gt;](#entitytid)
    - [Equality Rules](#equality-rules)
    - [Comparison Rules](#comparison-rules)
  - [AggregateRoot](#aggregateroot)
  - [IDomainEvent](#idomainevent)

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
This section covers the fundamental base classes and structures used in **Sunlix.NET.Primitives**, along with usage examples and key aspects of implementation.

### Entity&lt;TId&gt;
An **Entity** represents an object with a distinct and persistent identity, uniquely identified by an identifier rather than its attributes. Entities are designed to model domain concepts that require continuity over time, ensuring that their identity remains stable despite changes to their attributes. Entities enable the tracking and management of domain objects with unique identities, such as users, orders, or products, within the context of a domain model.

**Key Characteristics:**
- **Identity-based equality** – Two entities are equal if they have the same identifier.  
- **Persistence** – Entity identity remains constant throughout its lifecycle.  
- **Mutable attributes** – The values of an entity's attributes may change, but its identity does not.


<details>
  <summary>Sample Entity Definition</summary>

  ### Sample Entity Definition
  Entities are implemented as subclasses of `Entity<TId>`, an abstract generic class that enforces identity-based equality. The example below shows how to implement an entity with a defined identifier type.
  
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
  
  **📌 Implementation guidelines:**
  
  🔹 **Use POCO (Plain Old CLR Object) Classes**  
When implementing an entity, it is best to use a POCO (Plain Old CLR Object) class. A POCO class is a simple C# class that does not inherit from any framework-specific base classes and does not rely and does not include any attributes or code tied to a specific ORM framework.  This means that entities should follow the [Persistence Ignorance](https://deviq.com/persistence-ignorance/) and [Infrastructure Ignorance principles](https://ayende.com/blog/3137/infrastructure-ignorance).
  > **📄 Note:** *The `Entity<TId>` class includes a parameterless protected constructor and a virtual `Id` property to support compatibility with Object-Relational Mappers (ORMs) like NHibernate, which [require a parameterless constructor](https://nhibernate.info/doc/nhibernate-reference/persistent-classes.html#persistent-classes-poco-constructor) for entity instantiation. Although Entity Framework Core [supports parameterized constructors](https://learn.microsoft.com/en-us/ef/core/modeling/constructors), it still requires navigation properties to be virtual to enable [proxy-based lazy loading](https://learn.microsoft.com/en-us/ef/core/querying/related-data/lazy).*
  >
  > *This design choice introduces a trade-off where persistence concerns slightly leak into the domain model, but it enables seamless integration with the ORM frameworks. The entity itself does not rely on any external dependencies.*

  
  🔹 **Use a Meaningful Identifier Type.** The identifier type (TId) should be clear and appropriate for the domain.

  🔹 jnln
</details>

---
---
---
### Equality Rules
Entities follow identifier equality rather than structural equality, this means that two entities are considered equal if their IDs are equal.

| Scenario                                      | Explanation                                                                                                                               | Result        |
|-----------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------|---------------|
| Same object reference                         | *Both variables point to the same object in memory, so they are inherently equal.*                                                        |True           |
| Same type, same Id                            | *Entities of the same type with matching Id values are considered equal, regardless of attributes.*                                       |True           |
| Same type, have different Id values           | *Entities of the same type but with different Id values are not equal.*                                                                   |False          |
| Same type, other Id is null or default        | *Entity with a valid Id is not considered equal to the entity with a default Id (e.g., null or 0).*                                       |False          |
| Same type, both Id values are null or default | *Entities with default Id values (e.g., null or 0) are not considered equal, as a default Id typically indicates an uninitialized state.* |False          |
| Different runtime types*                      | *Entities must be of the same type to be considered equal, even if their Id values match.*                                                |False          |
| Other object is not `Entity<TId>`             | *An entity can only be equal to another entity.*                                                                                          |False          |
| Other entity is null                          | *An Entity cannot be equal to null; equals returns false for null comparisons.*                                                           |False          |
| Both entities are null                        | *Equality operator `==` return true when both entities are null*                                                                          |True           |

> **📄 Note:** *if one entity inherits from another and they have the same Id, they will still not be considered equal.*

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

// Other object is not Entity<TId>
var order = new Order(id: 1, totalAmount: new Money(10, "USD"));
var notEntity = "notEntity";
order.Equals(invoice);                                                        // ❌ False

// Other entity is null
var order1 = new Order(id: 1, totalAmount: new Money(10, "USD"));
Order order2 = null;
order1.Equals(order2);                                                        // ❌ False

// Same type, one has null or default Id
var order1 = new Order(id: 1, totalAmount: new Money(10, "USD"));
var order2 = new Order();
order1.Equals(order2);                                                        // ❌ False

// Same type, both have null or default Id
var order1 = new Order();
var order2 = new Order();
order1.Equals(order2);                                                        // ❌ False
```
**🛠 Key Aspects of Implementation:**
- Implements `IEquatable<Entity<TId>` interface to improve type safety and performance (*by allowing generic collections to call `Equals(Entity<TId>)` directly, avoiding the overhead of virtual method calls through `Equals(object)`*).  
- Overridden `Equals(object)` method delegates to `Equals(Entity<TId>)`, ensuring consistent results.
- The `==` and `!=` operators are overloaded to align with `Equals` methods, ensuring that entity comparisons behave consistently across different usage contexts.
- Overridden `GetHashCode()` method calculates the hash code based on the runtime type (*using `GetType()`*) and `Id` and is consistent with `Equals` methods and the equality operator.  
- Strict type checking (*using `GetType()`*) ensures that entities of different types, even within the same inheritance hierarchy, are never considered equal, preventing logical inconsistencies in equality operations.  
- Respects all equality rules (*reflexivity*, *symmetry*, *transitivity*, *consistency*, *null handling*).  

**📖 References:**
- **Microsoft Docs**: [IEquatable&lt;T&gt; Interface](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1?view=net-9.0)  
- **Microsoft Docs**: [How to define value equality for a class or struct](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type)

### Comparison Rules
Entities support ordering and sorting based on their identifiers.

| Scenario                                      | Explanation                                                                                              | Result                         |
|-----------------------------------------------|----------------------------------------------------------------------------------------------------------|--------------------------------|
| Same type, same Id                            |*Entities with identical Id values are considered equal in comparison.*                                   | Zero                           |
| Same type, Id is greater than other Id        |*Entity with a greater Id is considered larger.*                                                          | Positive&nbsp;value            |
| Same type, Id is smaller than other Id        |*Entity with a smaller Id is considered smaller.*                                                         | Negative&nbsp;value            |
| Same type, other Id is null or default        |*Entity with a default Id (e.g., null or 0) is considered smaller than an entity with a valid Id.*        | Positive&nbsp;value            |
| Same type, both Id values are null or default |*Two entities with the same default Id (e.g., null or 0) are considered equal in comparison.*             | Zero                           |
| Different runtime types                       |*Entities of different types are ordered lexicographically by their runtime type names.*                  | Ordered by type* (see note)    |
| Other object is not Entity<TId>               |*Comparison is only defined between entities. Comparing an entity with a non-entity throws an exception.* | `ArgumentException`            |
| Other entity is null                          |*Any entity is always greater than null.*                                                                 | Positive&nbsp;value            |

> **📄 Note:** *When comparing entities of different runtime types, they are ordered lexicographically based on their type names. This ensures a consistent ordering without causing exceptions in sorting scenarios (e.g., `SortedSet<T>` or `List<T>.Sort()`). The ordering is implemented using ordinal string comparison as follows:*
> ```
> private int CompareByType(Type thisType, Type otherType)
>    => StringComparer.Ordinal.Compare(thisType.Name, otherType.Name);
> ``` 

> **📄 Warning:** *Comparison of two entities with null or default Id values returns 0, even though `Equals` returns false. This is because `CompareTo` is designed for sorting, not for determining entity equality*.

**📝 Example:**
```csharp

// Same type, same Id
var order1 = new Order(1);
var order2 = new Order(1);
order1.CompareTo(order2);                                 //  0️⃣ Zero

// Same type, Id is greater than other Id
var order1 = new Order(2);
var order2 = new Order(1);
order1.CompareTo(order2);                                 //  ➕ Positive value

// Same type, Id is smaller than other Id
var order1 = new Order(1);
var order2 = new Order(2);
order1.CompareTo(order2);                                 //  ➖ Negative value

// Same type, other Id is null or default
var order1 = new Order(1);
var order2 = new Order();
order1.CompareTo(order2);                                 //  ➕ Positive value

// Same type, both Id values are null or default
var order1 = new Order(1);
var order2 = new Order();
order1.CompareTo(order2);                                 //  0️⃣ Zero

// Different runtime types
var order = new Order(1);
var invoice = new Invoice(1);
order.CompareTo(invoice);                                 //  ➕ Positive value (compared lexicographically by type names)

// Other object is not `Entity<TId>`
var order = new Order(1);
var notEntity = "notEntity";
order.CompareTo(notEntity);                               //  ❗ ArgumentException

// Same type, both Id values are null or default
var order1 = new Order(1);
Order order2 = null;
order1.CompareTo(order2);                                 //  ➕ Positive value
```
**🛠 Key Aspects of Implementation:**
- Implements `IComparable<Entity<TId>` interface to provide a strongly typed comparison method for ordering members of a generic collection object.
- Implements `IComparable` interface to provide a generalized type-specific comparison method.
- Overridden `CompareTo(object)` method delegates to `CompareTo(Entity<TId)`, ensuring consistent results.
- Overridden `CompareTo(object)` method ensures type safety, throwing an `ArgumentException` if the object is not an `Entity<TId>`.
- When comparing two entities with null or default Id, `CompareTo` returns 0, even though `Equals` returns false. This behavior ensures stable sorting while maintaining strict identity comparison.
- Entities of different runtime types are ordered lexicographically by their type names ensuring a deterministic sort order.


## AggregateRoot
## IDomainEvent
---

## 🤝 Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

