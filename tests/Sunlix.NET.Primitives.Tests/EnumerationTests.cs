namespace Sunlix.NET.Primitives.Tests
{
    public class EnumerationTests
    {
        [Fact]
        public void Enumeration_should_retrieve_by_value()
        {
            var status = Enumeration.GetByValue<OrderStatus>(2);

            status.Should().Be(OrderStatus.Shipped);
        }

        [Fact]
        public void Enumeration_should_retrieve_by_display_name()
        {
            var status = Enumeration.GetByDisplayName<OrderStatus>("Delivered");

            status.Should().Be(OrderStatus.Delivered);
        }

        [Fact]
        public void Enumeration_fields_with_same_values_should_be_equal()
        {
            var statusPending = Enumeration.GetByDisplayName<OrderStatus>("Pending");
            var statusPendingTestCopy = Enumeration.GetByDisplayName<OrderStatus>("Pending Test Copy");

            statusPending.Should().Be(statusPendingTestCopy);
            (statusPending == statusPendingTestCopy).Should().BeTrue();
        }

        [Fact]
        public void Enumeration_fields_with_different_values_should_not_be_equal()
        {
            var statusPending = Enumeration.GetByValue<OrderStatus>(1);
            var statusShipped = Enumeration.GetByValue<OrderStatus>(2);

            statusPending.Should().NotBe(statusShipped);
            (statusPending != statusShipped).Should().BeTrue();
        }

        [Fact]
        public void Enumeration_fields_with_same_values_should_have_same_hash_code()
        {
            var statusPending = Enumeration.GetByDisplayName<OrderStatus>("Pending");
            var statusPendingTestCopy = Enumeration.GetByDisplayName<OrderStatus>("Pending Test Copy");

            statusPending.GetHashCode().Should().Be(statusPendingTestCopy.GetHashCode());
        }

        [Fact]
        public void Enumeration_fields_with_different_values_should_have_different_hash_codes()
        {
            var statusPending = Enumeration.GetByValue<OrderStatus>(1);
            var statusShipped = Enumeration.GetByValue<OrderStatus>(2);

            statusPending.GetHashCode().Should().NotBe(statusShipped.GetHashCode());
        }

        private class OrderStatus : Enumeration
        {
            public static readonly OrderStatus Pending = new(1, "Pending");
            public static readonly OrderStatus PendingTestCopy = new(1, "Pending Test Copy");
            public static readonly OrderStatus Shipped = new(2, "Shipped");
            public static readonly OrderStatus Delivered = new(3, "Delivered");

            private OrderStatus(int value, string displayName) : base(value, displayName) { }
        }
    }
}
