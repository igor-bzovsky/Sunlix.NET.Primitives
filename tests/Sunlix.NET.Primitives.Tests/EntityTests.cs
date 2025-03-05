namespace Sunlix.NET.Primitives.Tests
{
    public class EntityTests
    {
        #region Constructor
        [Fact]
        public void Constructor_should_initialize_id_correctly()
        {
            var sut = new Order(1);
            sut.Id.Should().Be(1);
        }

        [Fact]
        public void Constructor_should_allow_null_id()
        {
            var sut = new User(null!);
            sut.Id.Should().BeNull();
        }
        #endregion

        #region Equality
        [Fact]
        public void Entities_with_same_id_should_be_equal()
        {
            var sut = new Order(1);
            var other = new Order(1);

            sut.Equals(other).Should().BeTrue();
        }

        [Fact]
        public void Entities_with_different_id_should_not_be_equal()
        {
            var sut = new Order(1);
            var other = new Order(2);

            sut.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Entity_should_be_equal_to_itself()
        {
            var sut = new Order(1);
            sut.Equals(sut).Should().BeTrue();
        }

        [Fact]
        public void Entity_should_not_be_equal_to_null()
        {
            var sut = new Order(1);
            sut.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void Entities_of_different_types_should_not_be_equal()
        {
            var sut = new Order(1);
            var other = new Invoice(1);

            sut.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Entities_with_null_id_should_not_be_equal()
        {
            var sut = new User(null!);
            var other = new User(null!);

            sut.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Entity_with_null_id_should_not_be_equal_to_non_null_id()
        {
            var sut = new User("user1");
            var other = new User(null!);

            sut.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Entity_should_be_equal_to_itself_as_object()
        {
            var sut = new Order(1);
            sut.Equals((object)sut).Should().BeTrue();
        }

        [Fact]
        public void Entities_with_same_id_should_be_equal_as_object()
        {
            var sut = new Order(1);
            var other = new Order(1);

            sut.Equals((object)other).Should().BeTrue();
        }

        [Fact]
        public void Entities_with_different_id_should_not_be_equal_as_object()
        {
            var sut = new Order(1);
            var other = new Order(2);

            sut.Equals((object)other).Should().BeFalse();
        }

        [Fact]
        public void Entity_should_not_be_equal_to_null_object()
        {
            var sut = new Order(1);
            sut.Equals((object?)null).Should().BeFalse();
        }

        [Fact]
        public void Entities_of_different_types_should_not_be_equal_as_object()
        {
            var sut = new Order(1);
            var other = new Invoice(1);

            sut.Equals((object)other).Should().BeFalse();
        }

        [Fact]
        public void Entity_should_not_be_equal_to_different_type_object()
        {
            var sut = new Order(1);
            var other = "not an entity";

            sut.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Entity_with_null_id_should_not_be_equal_to_other_null_id_as_object()
        {
            var sut = new User(null!);
            var other = new User(null!);

            sut.Equals((object)other).Should().BeFalse();
        }

        [Fact]
        public void Entity_with_null_id_should_not_be_equal_to_non_null_id_as_object()
        {
            var sut = new User("user1");
            var other = new User(null!);

            sut.Equals((object)other).Should().BeFalse();
        }
        #endregion

        #region Operators
        [Fact]
        public void Equality_operator_should_return_true_when_ids_are_equal()
        {
            var left = new Order(1);
            var right = new Order(1);

            (left == right).Should().BeTrue();
        }

        [Fact]
        public void Equality_operator_should_return_false_when_ids_are_different()
        {
            var left = new Order(1);
            var right = new Order(2);

            (left == right).Should().BeFalse();
        }

        [Fact]
        public void Inequality_operator_should_return_true_when_ids_are_different()
        {
            var left = new Order(1);
            var right = new Order(2);

            (left != right).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_should_return_false_when_ids_are_equal()
        {
            var left = new Order(1);
            var right = new Order(1);

            (left != right).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_should_return_false_when_one_entity_is_null()
        {
            var sut = new Order(1);
            (sut == null).Should().BeFalse();
            (null == sut).Should().BeFalse();
        }

        [Fact]
        public void Inequality_operator_should_return_true_when_one_entity_is_null()
        {
            var sut = new Order(1);
            (sut != null).Should().BeTrue();
            (null != sut).Should().BeTrue();
        }

        [Fact]
        public void Equality_operator_should_return_true_when_both_entities_are_null()
        {
            Order? left = null;
            Order? right = null;

            (left == right).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_should_return_false_when_both_entities_are_null()
        {
            Order? left = null;
            Order? right = null;

            (left != right).Should().BeFalse();
        }
        #endregion

        #region GetHashCode
        [Fact]
        public void Entities_with_same_id_should_have_same_hash_code()
        {
            var sut = new Order(1);
            var other = new Order(1);

            sut.GetHashCode().Should().Be(other.GetHashCode());
        }

        [Fact]
        public void Entities_of_different_types_with_same_id_should_have_different_hash_codes()
        {
            var sut = new Order(1);
            var other = new Invoice(1);

            sut.GetHashCode().Should().NotBe(other.GetHashCode());
        }

        [Fact]
        public void Entities_with_different_id_should_have_different_hash_codes()
        {
            var sut = new Order(1);
            var other = new Order(2);

            sut.GetHashCode().Should().NotBe(other.GetHashCode());
        }

        [Fact]
        public void GetHashCode_should_return_consistent_value()
        {
            var sut = new Order(1);
            var hash1 = sut.GetHashCode();
            var hash2 = sut.GetHashCode();

            hash1.Should().Be(hash2);
        }

        [Fact]
        public void GetHashCode_should_handle_null_id()
        {
            var sut = new User(null!);

            Action act = () => sut.GetHashCode();
            act.Should().NotThrow();
        }
        #endregion

        #region Compare
        [Fact]
        public void Entities_with_same_id_should_be_considered_equal_in_comparison()
        {
            var sut = new Order(1);
            var other = new Order(1);

            sut.CompareTo(other).Should().Be(0);
        }

        [Fact]
        public void Entity_with_lower_id_should_be_less_than_entity_with_higher_id()
        {
            var sut = new Order(1);
            var other = new Order(2);

            sut.CompareTo(other).Should().BeNegative();
        }

        [Fact]
        public void Entity_with_higher_id_should_be_greater_than_entity_with_lower_id()
        {
            var sut = new Order(2);
            var other = new Order(1);

            sut.CompareTo(other).Should().BePositive();
        }

        [Fact]
        public void Entity_should_be_greater_than_null()
        {
            var sut = new Order(1);
            sut.CompareTo(null).Should().Be(1);
        }

        [Fact]
        public void Entity_with_null_id_should_be_less_than_entity_with_non_null_id()
        {
            var sut = new User(null!);
            var other = new User("user1");

            sut.CompareTo(other).Should().Be(-1);
        }

        [Fact]
        public void Entity_with_non_null_id_should_be_greater_than_entity_with_null_id()
        {
            var sut = new User("user1");
            var other = new User(null!);

            sut.CompareTo(other).Should().Be(1);
        }

        [Fact]
        public void Entities_with_both_null_ids_should_be_considered_equal_in_comparison()
        {
            var sut = new User(null!);
            var other = new User(null!);

            sut.CompareTo(other).Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(GetComparableEntitiesWithResults))]
        public void Entities_of_different_types_should_be_comparable(Order sut, Invoice other, int result)
        {
            sut.CompareTo(other).Should().Be(result);
        }

        [Fact]
        public void CompareTo_object_should_return_zero_when_entities_have_same_id()
        {
            var sut = new Order(1);
            var other = new Order(1);

            sut.CompareTo((object)other).Should().Be(0);
        }

        [Fact]
        public void CompareTo_object_should_return_negative_when_entity_has_lower_id()
        {
            var sut = new Order(1);
            var other = new Order(2);

            sut.CompareTo((object)other).Should().BeNegative();
        }

        [Fact]
        public void CompareTo_object_should_return_positive_when_entity_has_higher_id()
        {
            var sut = new Order(2);
            var other = new Order(1);

            sut.CompareTo((object)other).Should().BePositive();
        }

        [Fact]
        public void CompareTo_object_should_return_positive_when_other_is_null()
        {
            var sut = new Order(1);
            sut.CompareTo((object?)null).Should().Be(1);
        }

        [Fact]
        public void CompareTo_object_should_return_negative_when_entity_id_is_null()
        {
            var sut = new User(null!);
            var other = new User("user1");

            sut.CompareTo((object)other).Should().Be(-1);
        }

        [Fact]
        public void CompareTo_object_should_return_positive_when_other_entity_id_is_null()
        {
            var sut = new User("user1");
            var other = new User(null!);

            sut.CompareTo((object)other).Should().Be(1);
        }

        [Fact]
        public void CompareTo_object_should_return_zero_when_both_entities_have_null_id()
        {
            var sut = new User(null!);
            var other = new User(null!);

            sut.CompareTo((object)other).Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(GetComparableEntitiesWithResults))]
        public void Entities_of_different_types_should_be_comparable_as_objects(Order sut, Invoice other, int result)
        {
            sut.CompareTo((object)other).Should().Be(result);
        }

        [Fact]
        public void CompareTo_object_should_throw_exception_when_compared_to_non_entity_type()
        {
            var sut = new Order(1);
            var other = "Invoice";

            Action act = () => sut.CompareTo(other);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Object type mismatch. Ensure the correct type is used. (Parameter 'obj')");
        }
        #endregion


        public class Order : Entity<int>
        {
            public Order(int id) : base(id) { }
        }

        public class Invoice : Entity<int>
        {
            public Invoice(int id) : base(id) { }
        }

        public class User : Entity<string>
        {
            public User(string id) : base(id) { }
        }
        public static IEnumerable<object[]> GetComparableEntitiesWithResults()
        {
            yield return new object[] { new Order(1), new Invoice(2), -1 };
            yield return new object[] { new Order(1), new Invoice(1), 0 };
            yield return new object[] { new Order(2), new Invoice(1), 1 };
        }
    }
}
