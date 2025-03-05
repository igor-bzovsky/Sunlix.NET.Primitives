using System;

namespace Sunlix.NET.Primitives
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>, IComparable, IComparable<Entity<TId>>
        where TId : IComparable<TId>
    {
        public virtual TId? Id { get; protected set; }

        protected Entity() { }
        protected Entity(TId id) => Id = id;

        public virtual bool Equals(Entity<TId>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return SatisfiesIdentifierEquality(other);
        }

        public override bool Equals(object? obj)
            => obj is Entity<TId> other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(GetType(), Id?.GetHashCode() ?? 0);

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
            => left is null ? right is null : left.Equals(right);

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
            => !(left == right);

        public virtual int CompareTo(Entity<TId>? other)
        {
            if (other is null) return 1;
            if (Id is null && other.Id is null) return 0;
            if (Id is null) return -1;
            if (other.Id is null) return 1;

            return Id.CompareTo(other.Id);
        }

        public virtual int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is not Entity<TId> other)
                throw new ArgumentException("Object type mismatch. Ensure the correct type is used.", nameof(obj));

            return CompareTo(other);
        }

        private bool SatisfiesIdentifierEquality(Entity<TId> other)
        {
            if (Id is null || Id.Equals(default)) return false;
            if (other.Id is null || other.Id.Equals(default)) return false;
            return Id.Equals(other.Id);
        }
    }
}
