using Sunlix.NET.Primitives.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sunlix.NET.Primitives
{
    public abstract class Enumeration<T> : IComparable<Enumeration<T>>, IEquatable<Enumeration<T>> where T : Enumeration<T>
    {
        private const int MaxNameLength = 255;

        private static readonly Lazy<IReadOnlyList<T>> EnumerationsLazy = new(ValidateAndGetEnumerations);

        private static readonly Lazy<Dictionary<int, T>> EnumerationValuesLazy = new(()
            => EnumerationsLazy.Value.ToDictionary(x => x.Value));

        private static readonly Lazy<Dictionary<string, T>> EnumerationNamesLazy = new(()
            => EnumerationsLazy.Value.ToDictionary(x => x.Name));

        private static Dictionary<int, T> EnumerationValues => EnumerationValuesLazy.Value;
        private static Dictionary<string, T> EnumerationNames => EnumerationNamesLazy.Value;

        public int Value { get; init; }
        public string Name { get; init; }

        protected Enumeration(int value, string name)
        {
            Validate(value, name);
            Value = value;
            Name = name;
        }

        public override string ToString() => Name;

        public static T FromValue(int value)
        {
            if (value < 0)
                throw new ArgumentException(ExceptionMessages.Enumeration<T>.InvalidValue(value));

            return TryGetFromValue(value, out T? enumeration)
                ? enumeration!
                : throw new InvalidOperationException(ExceptionMessages.Enumeration<T>.InvalidValue(value));
        }

        public static bool TryGetFromValue(int value, out T? enumeration)
            => EnumerationValues.TryGetValue(value, out enumeration);

        public static T FromName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionMessages.Enumeration<T>.NullOrEmptyName());

            return TryGetFromName(name, out T? enumeration)
                ? enumeration!
                : throw new InvalidOperationException(ExceptionMessages.Enumeration<T>.InvalidName(name));
        }

        public static bool TryGetFromName(string name, out T? enumeration)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                enumeration = null;
                return false;
            }
            return EnumerationNames.TryGetValue(name, out enumeration);
        }

        public static bool Exists(int value) 
            => EnumerationValues.ContainsKey(value);

        public static bool Exists(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return EnumerationNames.ContainsKey(name);
        }

        public static IEnumerable<T> GetAll() => EnumerationValues.Values;

        public virtual bool Equals(Enumeration<T>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj) => obj is Enumeration<T> other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(this.GetType(), Value);

        public int CompareTo(Enumeration<T>? other) => other is null ? 1 : Value.CompareTo(other.Value);

        public static bool operator ==(Enumeration<T> left, Enumeration<T> right)
            => left is null ? right is null : left.Equals(right);

        public static bool operator !=(Enumeration<T> left, Enumeration<T> right) => !(left == right);

        private static IReadOnlyList<T> LoadAllEnumerations()
        {
            var enumerationType = typeof(T);
            return enumerationType
                .GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.DeclaredOnly)
                .Where(f => enumerationType.IsAssignableFrom(f.FieldType))
                .Select(f => f.GetValue(null))
                .Cast<T>()
                .ToList().AsReadOnly();
        }

        private static IReadOnlyList<T> ValidateAndGetEnumerations()
        {
            var enumerationsList = LoadAllEnumerations();
            if (enumerationsList.GroupBy(x => x.Value).FirstOrDefault(g => g.Count() > 1) is { } duplicateValue)
                throw new InvalidOperationException(ExceptionMessages.Enumeration<T>.DuplicateValue(duplicateValue.Key));

            if (enumerationsList.GroupBy(x => x.Name).FirstOrDefault(g => g.Count() > 1) is { } duplicateName)
                throw new InvalidOperationException(ExceptionMessages.Enumeration<T>.DuplicateName(duplicateName.Key));

            return enumerationsList;
        }

        #region Validation
        private static void Validate(int value, string name)
        {
            if (value < 0)
                throw new ArgumentException(ExceptionMessages.Enumeration<T>.InvalidValue(value));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(ExceptionMessages.Enumeration<T>.NullOrEmptyName());

            if (name.Length > MaxNameLength)
                throw new ArgumentException(ExceptionMessages.Enumeration<T>.NameLengthExceeded(name));
        }
        #endregion
    }
}
