namespace Sunlix.NET.Primitives.Tests
{
    public class EnumerationTests
    {
        const int MaxNameLength = 256;

        #region Constructor
        [Fact]
        public void Constructor_throws_exception_if_value_is_negative()
        {
            Action sut = () => new OrderStatus(-1, "Test name");

            sut.Should().Throw<ArgumentException>()
                .WithMessage("Value '-1' is invalid. Enumeration: OrderStatus");
        }

        [Theory]
        [MemberData(nameof(GetNullOrWhiteSpaceNames))]
        public void Constructor_throws_exception_if_name_is_null_or_whitespace(string? name)
        {
            Action act = () => new OrderStatus(0, name!);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Name should not be null or empty. Enumeration: OrderStatus");
        }

        [Fact]
        public void Constructor_throws_exception_if_name_length_exceeds_max_length()
        {
            Action act = () => new OrderStatus(0, new string('A', MaxNameLength));

            act.Should().Throw<ArgumentException>()
                .WithMessage("Name 'AAAAAAAAAAAAAAAAAAAA...' length exceeds maximum length. Enumeration: OrderStatus");
        }
        #endregion

        #region Initialization, GetAll, Exists
        [Fact]
        public void Enumeration_initializes_correctly()
        {
            var status = OrderStatus.Pending;

            status.Value.Should().Be(1);
            status.Name.Should().Be("Pending");
        }

        [Fact]
        public void Enumeration_throws_exception_if_duplicate_value_exists()
        {
            Action act = () => OrderStatusWithDuplicateValue.GetAll();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Duplicate value detected. Enumeration: OrderStatusWithDuplicateValue. Value: 1");
        }

        [Fact]
        public void Enumeration_throws_exception_if_duplicate_name_exists()
        {
            Action act = () => OrderStatusWithDuplicateName.GetAll();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Duplicate name detected. Enumeration: OrderStatusWithDuplicateName. Name: 'Pending'");
        }

        [Fact]
        public void GetAll_returns_all_defined_enumerations()
        {
            var allStatuses = Enumeration<OrderStatus>.GetAll();

            allStatuses.Should().Contain(new[] { OrderStatus.Pending, OrderStatus.Shipped });
            allStatuses.Should().HaveCount(2);
        }

        [Fact]
        public void Exists_returns_true_if_value_exists()
        {
            bool result = Enumeration<OrderStatus>.Exists(1);
            result.Should().BeTrue();
        }

        [Fact]
        public void Exists_returns_true_if_value_does_not_exist()
        {
            bool result = Enumeration<OrderStatus>.Exists(-1);
            result.Should().BeFalse();
        }

        [Fact]
        public void Exists_returns_true_if_name_exists()
        {
            bool result = Enumeration<OrderStatus>.Exists("Pending");
            result.Should().BeTrue();
        }

        [Fact]
        public void Exists_returns_true_if_name_does_not_exist()
        {
            bool result = Enumeration<OrderStatus>.Exists("Delivered");
            result.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(GetNullOrWhiteSpaceNames))]
        public void Exists_returns_false_if_name_is_null_or_whitespace(string? name)
        {
            bool result = Enumeration<OrderStatus>.Exists(name!);
            result.Should().BeFalse();
        }
        #endregion

        #region Get enumeration by value
        [Theory]
        [MemberData(nameof(GetValueOrderStatusTuples))]
        public void FromValue_returns_correct_enumeration(int value, OrderStatus orderStatus)
        {
            var status = Enumeration<OrderStatus>.FromValue(value);

            status.Should().Be(orderStatus);
        }

        [Fact]
        public void FromValue_throws_exception_if_value_is_not_found()
        {
            Action act = () => Enumeration<OrderStatus>.FromValue(999);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Value '999' is invalid. Enumeration: OrderStatus");
        }

        [Fact]
        public void FromValue_throws_exception_if_value_is_invalid()
        {
            Action act = () => Enumeration<OrderStatus>.FromValue(-1);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Value '-1' is invalid. Enumeration: OrderStatus");
        }

        [Theory]
        [MemberData(nameof(GetValueOrderStatusTuples))]
        public void TryGetFromValue_sets_correct_enumeration(int value, OrderStatus orderStatus)
        {
            bool result = Enumeration<OrderStatus>.TryGetFromValue(value, out var status);

            result.Should().BeTrue();
            status.Should().Be(orderStatus);
        }

        [Fact]
        public void TryGetFromValue_sets_null_if_value_is_not_found()
        {
            bool result = Enumeration<OrderStatus>.TryGetFromValue(999, out var status);

            result.Should().BeFalse();
            status.Should().BeNull();
        }

        [Fact]
        public void TryGetFromValue_sets_null_if_value_is_invalid()
        {
            bool result = Enumeration<OrderStatus>.TryGetFromValue(-1, out var status);

            result.Should().BeFalse();
            status.Should().BeNull();
        }
        #endregion

        #region Get enumeration by name
        [Theory]
        [MemberData(nameof(GetNameOrderStatusTuples))]
        public void FromName_returns_correct_enumeration(string name, OrderStatus orderStatus)
        {
            var status = Enumeration<OrderStatus>.FromName(name);

            status.Should().Be(orderStatus);
        }

        [Fact]
        public void FromName_throws_exception_if_name_is_not_found()
        {
            Action act = () => Enumeration<OrderStatus>.FromName("Created");

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Name 'Created' is invalid. Enumeration: OrderStatus");
        }

        [Theory]
        [MemberData(nameof(GetNullOrWhiteSpaceNames))]
        public void FromName_throws_exception_if_name_is_null_or_whitespace(string? name)
        {
            Action act = () => Enumeration<OrderStatus>.FromName(name!);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Name should not be null or empty. Enumeration: OrderStatus");
        }

        [Theory]
        [MemberData(nameof(GetNameOrderStatusTuples))]
        public void TryGetFromName_sets_correct_enumeration(string name, OrderStatus orderStatus)
        {
            bool result = Enumeration<OrderStatus>.TryGetFromName(name, out var status);

            result.Should().BeTrue();
            status.Should().Be(orderStatus);
        }

        [Fact]
        public void TryGetFromName_sets_null_if_not_found()
        {
            bool result = Enumeration<OrderStatus>.TryGetFromName("Created", out var status);

            result.Should().BeFalse();
            status.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(GetNullOrWhiteSpaceNames))]
        public void TryGetFromName_sets_null_if_name_is_null_or_whitespace(string? name)
        {
            bool result = Enumeration<OrderStatus>.TryGetFromName(name!, out var status);

            result.Should().BeFalse();
            status.Should().BeNull();
        }

        [Fact]
        public void TryGetFromName_sets_null_if_name_length_exceeds_max_length()
        {
            bool result = Enumeration<OrderStatus>.TryGetFromName(new string('A', MaxNameLength), out var status);

            result.Should().BeFalse();
            status.Should().BeNull();
        }
        #endregion

        #region Equality
        [Fact]
        public void Enumerations_with_equal_references_should_be_equal()
        {
            OrderStatus.Pending
                .Equals(OrderStatus.Pending)
                .Should().BeTrue();
        }

        [Fact]
        public void Enumerations_with_same_type_and_value_should_be_equal()
        {
            OrderStatus.Pending
                .Equals(new OrderStatus(1, "Pending"))
                .Should().BeTrue();
        }

        [Fact]
        public void Enumerations_with_different_values_should_not_be_equal()
        {
            OrderStatus.Pending
                .Equals(new OrderStatus(2, "Pending"))
                .Should().BeFalse();
        }

        [Fact]
        public void Equals_null_returns_false()
        {
            OrderStatus nullStatus = null!;
            OrderStatus.Pending
                .Equals(nullStatus)
                .Should().BeFalse();
        }

        [Fact]
        public void Enumeration_and_object_with_equal_references_should_be_equal()
        {
            OrderStatus.Pending
                .Equals((object)OrderStatus.Pending)
                .Should().BeTrue();
        }

        [Fact]
        public void Enumeration_and_object_with_same_type_and_value_should_be_equal()
        {
            OrderStatus.Pending
                .Equals((object)(new OrderStatus(1, "Pending")))
                .Should().BeTrue();
        }

        [Fact]
        public void Enumeration_and_object_with_different_values_should_not_be_equal()
        {
            OrderStatus.Pending
                .Equals((object)(new OrderStatus(2, "Pending")))
                .Should().BeFalse();
        }

        [Fact]
        public void Enumerations_with_different_types_should_not_be_equal()
        {
            OrderStatus.Pending
                .Equals(new InvoiceStatus(1, "Pending"))
                .Should().BeFalse();
        }

        [Fact]
        public void Equals_object_null_returns_false()
        {
            object nullStatus = null!;
            OrderStatus.Pending
                .Equals(nullStatus)
                .Should().BeFalse();
        }

        [Fact]
        public void Operator_Equals_returns_true_for_different_instances_with_same_value()
        {
            (OrderStatus.Pending == new OrderStatus(1, "Pending"))
                .Should().BeTrue();
        }

        [Fact]
        public void Operator_NotEquals_returns_false_for_different_instances_with_same_value()
        {
            (OrderStatus.Pending != new OrderStatus(1, "Pending"))
                .Should().BeFalse();
        }

        [Fact]
        public void Operator_Equals_returns_false_for_different_values()
        {
            (OrderStatus.Pending == OrderStatus.Shipped)
                .Should().BeFalse();
        }

        [Fact]
        public void Operator_NotEquals_returns_true_for_different_values()
        {
            (OrderStatus.Pending != OrderStatus.Shipped)
                .Should().BeTrue();
        }

        [Fact]
        public void Operator_Equals_returns_true_if_both_operands_are_null()
        {
            OrderStatus left = null!;
            OrderStatus right = null!;
            (left == right).Should().BeTrue();
        }

        [Fact]
        public void Operator_NotEquals_returns_false_if_both_operands_are_null()
        {
            OrderStatus left = null!;
            OrderStatus right = null!;
            (left != right).Should().BeFalse();
        }

        [Fact]
        public void Operator_Equals_returns_false_if_one_operand_is_null()
        {
            OrderStatus? left = null!;
            (left == OrderStatus.Pending).Should().BeFalse();
            (OrderStatus.Pending == left).Should().BeFalse();
        }

        [Fact]
        public void Operator_NotEquals_returns_true_if_one_operand_is_null()
        {
            OrderStatus? left = null!;
            (left != OrderStatus.Pending).Should().BeTrue();
            (OrderStatus.Pending != left).Should().BeTrue();
        }
        #endregion

        #region Hash code
        [Fact]
        public void Enumerations_with_same_values_should_have_same_hash_code()
        {
            OrderStatus.Pending.GetHashCode()
                .Should().Be(new OrderStatus(1, "Pending")
                .GetHashCode());
        }

        [Fact]
        public void Enumerations_with_different_values_should_have_different_hash_codes()
        {
            OrderStatus.Pending.GetHashCode()
                .Should().NotBe(new OrderStatus(2, "Pending")
                .GetHashCode());
        }
        #endregion

        #region Compare
        [Fact]
        public void CompareTo_same_value_returns_zero()
        {
            OrderStatus.Pending
                .CompareTo(new OrderStatus(1, "Pending"))
                .Should().Be(0);
        }

        [Fact]
        public void CompareTo_lower_value_returns_negative()
        {
            OrderStatus.Pending
                .CompareTo(OrderStatus.Shipped)
                .Should().BeNegative();
        }

        [Fact]
        public void CompareTo_higher_value_returns_positive()
        {
            OrderStatus.Shipped
                .CompareTo(OrderStatus.Pending)
                .Should().BePositive();
        }

        [Fact]
        public void CompareTo_null_returns_positive()
        {
            OrderStatus.Pending
                .CompareTo(null)
                .Should().BePositive();
        }
        #endregion

        public static IEnumerable<object[]> GetNullOrWhiteSpaceNames()
        {
            yield return new object[] { null! };
            yield return new object[] { "" };
            yield return new object[] { "  " };
        }

        public static IEnumerable<object[]> GetValueOrderStatusTuples()
        {
            yield return new object[] { 1, OrderStatus.Pending };
            yield return new object[] { 2, OrderStatus.Shipped };
        }

        public static IEnumerable<object[]> GetNameOrderStatusTuples()
        {
            yield return new object[] { "Pending", OrderStatus.Pending };
            yield return new object[] { "Shipped", OrderStatus.Shipped };
        }


        public class OrderStatus : Enumeration<OrderStatus>
        {
            public static readonly OrderStatus Pending = new OrderStatus(1, nameof(Pending));
            public static readonly OrderStatus Shipped = new OrderStatus(2, nameof(Shipped));

            public OrderStatus(int value, string name) : base(value, name) { }
        }

        public class InvoiceStatus : Enumeration<InvoiceStatus>
        {
            public static readonly InvoiceStatus Pending = new InvoiceStatus(1, nameof(Pending));

            public InvoiceStatus(int value, string name) : base(value, name) { }
        }

        public class OrderStatusWithDuplicateValue : Enumeration<OrderStatusWithDuplicateValue>
        {
            public static readonly OrderStatusWithDuplicateValue Pending = new OrderStatusWithDuplicateValue(1, nameof(Pending));
            public static readonly OrderStatusWithDuplicateValue Shipped = new OrderStatusWithDuplicateValue(1, nameof(Shipped));

            public OrderStatusWithDuplicateValue(int value, string name) : base(value, name) { }
        }

        public class OrderStatusWithDuplicateName : Enumeration<OrderStatusWithDuplicateName>
        {
            public static readonly OrderStatusWithDuplicateName Pending = new OrderStatusWithDuplicateName(1, nameof(Pending));
            public static readonly OrderStatusWithDuplicateName Shipped = new OrderStatusWithDuplicateName(2, nameof(Pending));

            public OrderStatusWithDuplicateName(int value, string name) : base(value, name) { }
        }
    }
}
