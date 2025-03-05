using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunlix.NET.Primitives
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public bool Equals(ValueObject? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return SatisfiesStructuralEquality(other);
        }

        public override bool Equals(object? obj)
            => obj is ValueObject other && Equals(other);

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(new HashCode(), (hash, component) =>
                {
                    hash.Add(component);
                    return hash;
                }).ToHashCode();
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
            => left is null ? right is null : left.Equals(right);

        public static bool operator !=(ValueObject? left, ValueObject? right)
            => !(left == right);

        protected abstract IEnumerable<object> GetEqualityComponents();

        private bool SatisfiesStructuralEquality(ValueObject other)
            => GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
}
