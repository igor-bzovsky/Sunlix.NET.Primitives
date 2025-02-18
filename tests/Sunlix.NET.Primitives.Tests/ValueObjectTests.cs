namespace Sunlix.NET.Primitives.Tests
{
    public class ValueObjectTests
    {
        [Fact]
        public void ValueObjects_with_same_values_should_be_equal()
        {
            var money1 = new Money(100, "USD");
            var money2 = new Money(100, "USD");

            money1.Should().Be(money2);
            (money1 == money2).Should().BeTrue();
        }

        [Fact]
        public void ValueObjects_with_different_values_should_not_be_equal()
        {
            var money1 = new Money(100, "USD");
            var money2 = new Money(50, "USD");

            money1.Should().NotBe(money2);
            (money1 != money2).Should().BeTrue();
        }

        [Fact]
        public void ValueObjects_with_same_values_should_have_same_hash_code()
        {
            var money1 = new Money(100, "USD");
            var money2 = new Money(100, "USD");

            money1.GetHashCode().Should().Be(money2.GetHashCode());
        }

        [Fact]
        public void ValueObjects_with_different_values_should_have_different_hash_codes()
        {
            var money1 = new Money(100, "USD");
            var money2 = new Money(50, "USD");

            money1.GetHashCode().Should().NotBe(money2.GetHashCode());
        }

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
    }
}
