# Sunlix.NET.Primitives

**Sunlix.NET.Primitives** is a lightweight library providing fundamental types for object-oriented programming and domain-driven design (DDD). It includes essential types such as `Entity`, `ValueObject`, `Enumeration`, `Unit`, and `Error`, designed to enhance type safety and maintainability.

---

## 📌 Features
✔ **Entity<TId>** – Ensures consistent identity handling for domain entities.
✔ **ValueObject** – Immutable, equality-based objects with structural comparison.
✔ **Enumeration** – Type-safe alternative to traditional enums with extended functionality.
✔ **Unit** – Represents an absence of a meaningful result (functional programming).
✔ **Error** – Structured error representation with error codes and messages.

---

## 🔍 Entity vs. ValueObject

### **🆔 Entity<TId> (Has Identity)**
Entities represent objects with a **unique identity** that remains constant throughout their lifecycle. Even if an entity’s properties change, it is still the same entity as long as its **ID remains unchanged**.

#### **Key Characteristics:**
- Has a unique identifier (`Id`).
- Identity does not change even if attributes change.
- Compared based on identity (ID), not attribute values.
- Implements equality using `Equals` and `GetHashCode` based on `Id`.

#### **Example:**
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

---

### **🏷️ ValueObject (Defined by Attributes)**
Value objects represent **conceptual values** rather than distinct entities. Their identity is defined by the combination of their attribute values.

#### **Key Characteristics:**
- Does not have a unique identifier.
- Completely interchangeable if their values are the same.
- Compared by value (all attributes must be equal).
- Useful for representing concepts like monetary amounts, coordinates, or measurements.

#### **Example:**
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

---

## ⚖️ Equals Implementation
### **Why Implement IEquatable<T>?**
Both `Entity<TId>` and `ValueObject` implement `IEquatable<T>`, which provides optimized equality checks. This has several benefits:

1. **Improved Performance** – Avoids unnecessary boxing/unboxing.
2. **Type Safety** – Prevents incorrect comparisons between unrelated types.
3. **Better Integration with .NET Collections** – Works well with LINQ, `HashSet<T>`, and `Dictionary<TKey, TValue>`.

### **Entity Equals Implementation**
```csharp
public override bool Equals(object? obj)
{
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return EqualsCore((Entity<TId>)obj);
}
```

### **ValueObject Equals Implementation**
```csharp
private bool EqualsCore(ValueObject other)
    => this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
```

---

## 🔢 Enumeration
**Enumeration** is a base class for strongly-typed enumerations, offering more flexibility than built-in `enum` types.

#### **Example:**
```csharp
public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Pending = new(1, "Pending");
    public static readonly OrderStatus Shipped = new(2, "Shipped");
    public static readonly OrderStatus Delivered = new(3, "Delivered");
    
    private OrderStatus(int value, string displayName) : base(value, displayName) {}
}
```

---

## ❗ Error
**Error** represents structured error handling with error codes and messages.

#### **Example:**
```csharp
public static class Errors
{
    public static readonly Error NotFound = new("404", "Resource not found");
    public static readonly Error ValidationError = new("400", "Invalid input provided");
}
```

---

## 🏳️ Unit
**Unit** is a struct representing a "void" return type, commonly used in functional programming.

#### **Example:**
```csharp
public Task<Unit> SaveChangesAsync()
{
    return Task.FromResult(Unit.value);
}
```

---

## 🚀 Installation
You can install the package via NuGet:
```sh
dotnet add package Sunlix.NET.Primitives
```

---

## 📄 License
Sunlix.NET.Primitives is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## 🤝 Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

