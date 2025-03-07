# Sunlix.NET.Primitives

[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%208.0%20%7C%209.0-blue)](https://dotnet.microsoft.com/en-us/)
[![NuGet](https://img.shields.io/nuget/v/Sunlix.NET.Primitives.svg)](https://www.nuget.org/packages/Sunlix.NET.Primitives/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Sunlix.NET.Primitives.svg)](https://www.nuget.org/packages/Sunlix.NET.Primitives/)
[![GitHub license](https://img.shields.io/github/license/Sunlix-Software/Sunlix.NET.Primitives.svg)](https://github.com/SunlixSoftware/Sunlix.NET.Primitives/blob/main/LICENSE)


**Sunlix.NET.Primitives** is a lightweight library providing a set of key types for structured domain modeling in C#. It includes `Entity`, `ValueObject`, `Enumeration`, `Unit`, and `Error`, which are particularly useful in domain-driven design (DDD).

## Unit

### Overview
`Unit` is a structure representing a unit type that holds no information. It is commonly used in functional programming as a replacement for `void` when a method needs to return a value but has no meaningful data to return.

### Features
- Serves as a placeholder return type in functional programming.
- Provides a single shared instance: `Unit.value`.
- All instances of `Unit` are considered equal.

### Usage
#### Basic Example
```csharp
Unit unit = Unit.value;
```

#### Equality Rules

| Expression                             | Result  | Comment                                       |
|----------------------------------------|---------|-----------------------------------------------|
| `Unit.value.Equals(Unit.value)`        | `true`  |`Units` are always equal                       |
| `Unit.value.Equals(null)`              | `false` |`Unit` is not equal to `null`                  |
| `Unit.value.Equals("unit")`            | `false` |`Unit` is not equal to object of different type|

#### Why Use `Unit` Instead of `void`?
In functional programming, functions should be composable, meaning their outputs can be used as inputs for other functions. However, functions that return `void` break this pattern because `void` is not a real type and cannot be passed as a value.

Consider the following example:

##### **Approach with `void` (breaks composition)**
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

##### **Approach with `Unit` (enables composition)**
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
- **Cannot be passed to another function**  
  `void` is not a value, but rather the absence of a value, so it cannot be used in composition. `Unit`, on the other hand, is a concrete value that can be passed around.
- **Cannot be used in generic functions**  
  For example, `Func<T, void>` does not exist, but `Func<T, Unit>` is a valid function signature.
- **Breaks functional style**  
  In functional programming, every function should return a value, even if it carries no information. `void` violates this principle, while `Unit` maintains it.

#### Key Benefits of using `Unit`
- **Allows function composition** – `Unit` enables treating all functions as first-class, even those that conceptually return `void`.
- **Prevents special cases** – Avoids needing separate handling for `void` methods in functional pipelines.
- **Works with generic types** – Can be used as a return type in generic functions where void is not allowe.

By using `Unit`, we ensure consistency in functional programming patterns while enabling better function composition and reuse.


---

## 🤝 Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

