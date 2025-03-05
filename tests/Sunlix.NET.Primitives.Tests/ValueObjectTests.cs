using static Sunlix.NET.Primitives.Tests.EntityTests;

namespace Sunlix.NET.Primitives.Tests
{
    public class ValueObjectTests
    {
        #region Equality
        [Fact]
        public void ValueObject_should_not_be_equal_to_null()
        {
            var sut = new Money(100, "USD");
            sut.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void ValueObject_should_be_equal_to_itself()
        {
            var sut = new Money(100, "USD");
            sut.Equals(sut).Should().BeTrue();
        }

        [Fact]
        public void ValueObjects_of_different_types_should_not_be_equal()
        {
            var sut = new Money(100, "USD");
            var other = new Currency("US Dollar", "USD");

            sut.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void ValueObjects_with_different_values_should_not_be_equal()
        {
            var sut = new Money(100, "USD");
            var other = new Money(10, "USD");

            sut.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void ValueObjects_with_same_values_should_be_equal()
        {
            var sut = new Money(100, "USD");
            var other = new Money(100, "USD");

            sut.Equals(other).Should().BeTrue();
        }

        [Fact]
        public void ValueObject_should_not_be_equal_to_null_object()
        {
            var sut = new Money(100, "USD");
            sut.Equals((object?)null).Should().BeFalse();
        }

        [Fact]
        public void ValueObjects_of_different_types_should_not_be_equal_as_objects()
        {
            var sut = new Money(100, "USD");
            var other = new Currency("US Dollar", "USD");

            sut.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void ValueObjects_with_different_values_should_not_be_equal_as_objects()
        {
            var sut = new Money(100, "USD");
            var other = new Money(10, "USD");

            sut.Equals((object)other).Should().BeFalse();
        }

        [Fact]
        public void ValueObjects_with_same_values_should_be_equal_as_objects()
        {
            var sut = new Money(100, "USD");
            var other = new Money(100, "USD");

            sut.Equals((object)other).Should().BeTrue();
        }
        #endregion

        #region Operators
        [Fact]
        public void Equality_operator_should_return_true_when_both_value_objects_are_null()
        {
            ValueObject? left = null;
            ValueObject? right = null;

            (left == right).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_should_return_false_when_both_value_objects_are_null()
        {
            Order? left = null;
            Order? right = null;

            (left != right).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_should_return_false_when_one_value_object_is_null()
        {
            var sut = new Money(100, "USD");
            (sut == null).Should().BeFalse();
            (null == sut).Should().BeFalse();
        }

        [Fact]
        public void Inequality_operator_should_return_true_when_one_value_object_is_null()
        {
            var sut = new Money(100, "USD");
            (sut != null).Should().BeTrue();
            (null != sut).Should().BeTrue();
        }

        [Fact]
        public void Equality_operator_should_return_true_when_value_objects_are_equal()
        {
            var sut = new Money(100, "USD");
            var other = new Money(100, "USD");
            (sut == other).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_should_return_false_when_value_objects_are_equal()
        {
            var sut = new Money(100, "USD");
            var other = new Money(100, "USD");
            (sut != other).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_should_return_false_when_value_objects_are_not_equal()
        {
            var sut = new Money(100, "USD");
            var other = new Money(10, "USD");
            (sut == other).Should().BeFalse();
        }

        [Fact]
        public void Inequality_operator_should_return_true_when_value_objects_are_not_equal()
        {
            var sut = new Money(100, "USD");
            var other = new Money(10, "USD");
            (sut != other).Should().BeTrue();
        }
        #endregion

        #region GetHashCode
        [Fact]
        public void ValueObjects_with_same_values_should_have_same_hash_code()
        {
            var sut = new Money(100, "USD");
            var other = new Money(100, "USD");

            sut.GetHashCode().Should().Be(other.GetHashCode());
        }

        [Fact]
        public void ValueObjects_of_different_types_should_have_different_hash_codes()
        {
            var sut = new Money(100, "USD");
            var other = new Currency("US Dollar", "USD");

            sut.GetHashCode().Should().NotBe(other.GetHashCode());
        }

        [Fact]
        public void ValueObjects_with_different_values_should_have_different_hash_codes()
        {
            var sut = new Money(100, "USD");
            var other = new Money(10, "USD");

            sut.GetHashCode().Should().NotBe(other.GetHashCode());
        }

        [Fact]
        public void GetHashCode_should_return_consistent_value()
        {
            var sut = new Money(100, "USD");
            var hash1 = sut.GetHashCode();
            var hash2 = sut.GetHashCode();

            hash1.Should().Be(hash2);
        }

        [Fact]
        public void GetHashCode_should_handle_null_values()
        {
            var sut = new Money(100, null!);

            Action act = () => sut.GetHashCode();
            act.Should().NotThrow();
        }
        #endregion


        private class Money : ValueObject
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

        private class Currency : ValueObject
        {
            public string Name { get; }
            public string Code { get; }

            public Currency(string name, string code)
            {
                Name = name;
                Code = code;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Name;
                yield return Code;
            }
        }
    }
}
