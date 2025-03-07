# Sunlix.NET.Primitives

[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%208.0%20%7C%209.0-blue)](https://dotnet.microsoft.com/en-us/)
[![NuGet](https://img.shields.io/nuget/v/Sunlix.NET.Primitives.svg)](https://www.nuget.org/packages/Sunlix.NET.Primitives/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Sunlix.NET.Primitives.svg)](https://www.nuget.org/packages/Sunlix.NET.Primitives/)
[![GitHub license](https://img.shields.io/github/license/Sunlix-Software/Sunlix.NET.Primitives.svg)](https://github.com/SunlixSoftware/Sunlix.NET.Primitives/blob/main/LICENSE)


**Sunlix.NET.Primitives** is a lightweight library providing a set of key types for structured domain modeling in C#. It includes `Entity`, `ValueObject`, `Enumeration`, `Unit`, and `Error`, which are particularly useful in domain-driven design (DDD).


## Features
✔ **Entity** – **Ensures consistent identity handling for domain entities.** An `Entity` is an object that has a distinct identity rather than being defined by its attributes. In domain-driven design (DDD), entities are central concepts that must be uniquely identifiable within the system.

✔ **Value Object** – **An immutable object with structural comparison.** A `ValueObject` is an immutable object that represents a concept but has no unique identity. Two value objects are equal if their properties are equal.

✔ **Enumeration** – **An alternative to traditional enums that provides more flexibility.** Unlike enums, which only represent a fixed set of named values, `Enumeration` allows each value to have additional properties and behavior. This eliminates the need for scattered switch statements, centralizes related logic, and makes the model easier to extend without modifying existing code, following the Open-Closed Principle (OCP).

✔ **Unit** – **Represents an absence of a meaningful result (functional programming).** In functional programming, `Unit` is used as a return type to indicate that a function executes an action but does not return a meaningful value (like `void`).

✔ **Error** – **Structured error representation with error codes and messages.** Instead of using exceptions, `Error` enables structured error handling by encapsulating meaningful details.


## Entity vs. Value Object

### **Entity (identifier equality)**
Entities represent objects with a **unique identity** that remains constant throughout their lifecycle. Even if an entity’s properties change, it is still the same entity as long as its **ID remains unchanged**.

#### 📌 **Key Characteristics:**
- Has a unique identifier (`Id`).
- Identity does not change even if properties change.
- Compared based on identity (`Id`), not properties values.
- Implements equality using `Equals` and `GetHashCode` based on `Id`.

#### 📝 **Example:**
```csharp
public class User : Entity<Guid>
{
    public string Name { get; }
    public User(Guid id, string name) : base(id) => Name = name;
}

var user1 = new User(Guid.NewGuid(), "Alice");
var user2 = new User(Guid.NewGuid(), "Alice");
Console.WriteLine(user1 == user2); // false, since IDs are different
```


### **Value Object (structural equality)**
Value objects represent **conceptual values** rather than distinct entities. Their identity is defined by the combination of their properties values.

#### 📌 **Key Characteristics:**
- Does not have a unique identifier.
- Completely interchangeable if their values are the same.
- Compared by value (all compared properties must be equal).
- Useful for representing concepts like monetary amounts, coordinates, or measurements.

#### 📝 **Example:**
```csharp
public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}

var money1 = new Money(100, "USD");
var money2 = new Money(100, "USD");
var money3 = new Money(50, "USD");

Console.WriteLine(money1 == money2); // true, as values are identical
Console.WriteLine(money1 == money3); // false, since Amount is different
```


## Equals Implementation
### **Why Implement IEquatable<T>?**
Both `Entity<TId>` and `ValueObject` implement `IEquatable<T>`, which provides optimized equality checks. This has several benefits:

- **Improved Performance** – Avoids unnecessary boxing/unboxing.
- **Type Safety** – Prevents incorrect comparisons between unrelated types.
- **Better Integration with .NET Collections** – Works well with LINQ, `HashSet<T>`, and `Dictionary<TKey, TValue>`.

#### 📝 **Entity Equals Implementation:**
```csharp
public override bool Equals(object? obj)
{
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return EqualsCore((Entity<TId>)obj);
}
```

#### 📝 **Value Object Equals Implementation:**
```csharp
private bool EqualsCore(ValueObject other)
    => this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
```

## Enumeration
**Enumeration** is a more flexible alternative to standard C# enums. Unlike enums, which only represent a fixed set of named values, `Enumeration` allows defining additional properties and methods, such as retrieving values dynamically or encapsulating specific logic.
#### 📝 **Example:**
```csharp
public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Pending = new(1, "Pending");
    public static readonly OrderStatus Shipped = new(2, "Shipped");
    public static readonly OrderStatus Delivered = new(3, "Delivered");
    
    private OrderStatus(int value, string displayName) : base(value, displayName) {}
}
```
### The Problem with Using `enum` and `switch`
When using a standard `enum`, we often rely on `switch` statements, which become harder to maintain as new values are added.
#### 📝 **Example:**
```csharp
public enum SubscriptionPlan
{
    Free,
    Premium,
    Enterprise
}

public class SubscriptionService
{
    public decimal GetMonthlyFee(SubscriptionPlan plan)
    {
        switch (plan)
        {
            case SubscriptionPlan.Free:
                return 0m;
            case SubscriptionPlan.Premium:
                return 9.99m;
            case SubscriptionPlan.Enterprise:
                return 49.99m;
            default:
                throw new ArgumentOutOfRangeException(nameof(plan), "Unknown plan");
        }
    }
}
```
#### ❌ Drawbacks of Using `enum`
- **Scattered logic** – The `GetMonthlyFee` method relies on `switch` instead of being encapsulated in `SubscriptionPlan` itself.
- **Violates the Open-Closed Principle (OCP)** – Adding a new subscription plan requires modifying multiple switch statements across the codebase.
#### ✅ Using Enumeration to Eliminate `switch`.
Instead of relying on `switch`, we encapsulate logic directly within `SubscriptionPlan`.
#### 📝 **Example:**
```csharp
public abstract class SubscriptionPlan : Enumeration
{
    public static readonly SubscriptionPlan Free = new FreePlan();
    public static readonly SubscriptionPlan Premium = new PremiumPlan();
    public static readonly SubscriptionPlan Enterprise = new EnterprisePlan();

    protected SubscriptionPlan(int value, string displayName) : base(value, displayName) { }

    public abstract decimal MonthlyFee { get; }
    public abstract int MaxUsers { get; }

    private class FreePlan : SubscriptionPlan
    {
        public FreePlan() : base(0, "Free") { }

        public override decimal MonthlyFee => 0m;
        public override int MaxUsers => 1;
    }

    private class PremiumPlan : SubscriptionPlan
    {
        public PremiumPlan() : base(1, "Premium") { }

        public override decimal MonthlyFee => 9.99m;
        public override int MaxUsers => 5;
    }

    private class EnterprisePlan : SubscriptionPlan
    {
        public EnterprisePlan() : base(2, "Enterprise") { }

        public override decimal MonthlyFee => 49.99m;
        public override int MaxUsers => 100;
    }
}
```

## Error
**Error** represents structured error handling with error codes and messages.

#### 📝 **Example:**
```csharp
public static class Errors
{
    public static readonly Error NotFound = new("404", "Resource not found");
    public static readonly Error ValidationError = new("400", "Invalid input provided");
}
```

## Unit

### Overview
`Unit` represents a unit type that holds no information. It is commonly used in functional programming as a replacement for `void` when a method needs to return a value but has no meaningful data to return.

### Features
- Serves as a placeholder return type in functional programming.
- Provides a single shared instance: `Unit.value`.
- All instances of `Unit` are considered equal.

### Usage
#### Basic Example
```csharp
Unit unit = Unit.value;
```

#### Equality Comparison
| Expression | Result |
|------------|--------|
| `Unit.value == Unit.value` | `true` |
| `Unit.value.Equals(Unit.value)` | `true` |
| `Unit.value.Equals(null)` | `false` |
| `Unit.value.GetHashCode()` | `0` |

### API Reference
#### Fields
- `Unit.value` - A single shared instance of the `Unit` type.

#### Methods
##### `bool Equals(Unit other)`
Determines whether the specified `Unit` is equal to the current instance.
- **Returns:** Always `true`.

##### `bool Equals(object obj)`
Determines whether the specified object is equal to the current `Unit` instance.
- **Returns:** `true` if the object is an instance of `Unit`, otherwise `false`.

##### `int GetHashCode()`
Returns a hash code for this instance.
- **Returns:** Always `0` since all instances of `Unit` are considered equal.

##### `string ToString()`
Returns a string representation of the `Unit` type.
- **Returns:** `"()"`.

### Additional Information
`Unit` is useful in scenarios where an operation must return a value, but no actual data is necessary. This is particularly common in functional programming paradigms.

#### Example
##### Using `Unit` to Chain Functions
In functional programming, functions should be composable, meaning their outputs can be used as inputs for other functions. However, functions that return `void` break this pattern because `void` is not a real type and cannot be passed as a value.

Consider the following example:

##### **Incorrect Approach (`void` breaks composition)**
```csharp
void LogMessage(string message)
{
    Console.WriteLine($"[LOG]: {message}");
}

void FinalStep()
{
    Console.WriteLine("Final step executed.");
}

void Process(string message)
{
    LogMessage(message);
    FinalStep(); // Simply calling the next function
}

Process("Starting process");
```

Here, `LogMessage` does not return a value, and `FinalStep` is called separately. If we needed to pass the result of one function to another, **we could not do that**, because `void` cannot be used as an argument.

##### **Correct Approach (`Unit` enables composition)**
```csharp
Func<string, Unit> logMessage = message =>
{
    Console.WriteLine($"[LOG]: {message}");
    return Unit.value;
};

Func<Unit, Unit> finalStep = _ =>
{
    Console.WriteLine("Final step executed.");
    return Unit.value;
};

// Functional composition: the result of logMessage is passed to finalStep
Func<string, Unit> process = message => finalStep(logMessage(message));

process("Starting process");
```

#### Why `void` Doesn't Work?
1. **`void` cannot be passed to another function**  
   - `void` is not a value, but rather the absence of a value, so it cannot be used in composition.
   - `Unit`, on the other hand, is a concrete value that can be passed around.

2. **Cannot be used in generic (`generic`) functions**  
   - For example, `Func<T, void>` does not exist, but `Func<T, Unit>` is a valid function signature.

3. **Breaks functional style**  
   - In functional programming, every function should **return a value**, even if it carries no information.
   - `void` violates this principle, while `Unit` maintains it.

#### Key Benefits
- **Allows function composition:** `Unit` enables treating all functions as first-class, even those that conceptually return `void`.
- **Prevents special cases:** Avoids needing separate handling for `void` methods in functional pipelines.
- **Supports generic programming:** Can be used as a return type in generics where `void` is not allowed.

By using `Unit`, we ensure consistency in functional programming patterns while enabling better function composition and reuse.


---

## 🚀 Installation
You can install the package via NuGet:
```sh
dotnet add package Sunlix.NET.Primitives
```

## 📄 License
Sunlix.NET.Primitives is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## 🤝 Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

