namespace Sunlix.NET.Primitives.Tests
{
    public class EnumerationTests
    {
        const int MaxNameLength = 255;

        #region Constructor
        [Fact]
        public void Constructor_should_throw_exception_when_value_is_negative()
        {
            Action act = () => new OrderStatus(-1, "Test name");

            act.Should().Throw<ArgumentException>()
                .WithMessage("Invalid enumeration value: '-1'. Expected a positive value. (Parameter 'value')");
        }

        [Theory]
        [MemberData(nameof(GetNullOrWhiteSpaceNames))]
        public void Constructor_should_throw_exception_when_name_is_null_or_whitespace(string? name)
        {
            Action act = () => new OrderStatus(0, name!);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Enumeration name cannot be null, empty, or contain only whitespace. (Parameter 'name')");
        }

        [Fact]
        public void Constructor_should_throw_exception_when_name_length_exceeds_maximum()
        {
            Action act = () => new OrderStatus(0, new string('A', MaxNameLength + 1));

            act.Should().Throw<ArgumentException>()
                .WithMessage($"Enumeration name 'AAAAAAAAAAAAAAAAAAAA...' exceeds the maximum allowed length of {MaxNameLength} characters. (Parameter 'name')");
        }
        #endregion

        #region Initialization, GetAll, Exists
        [Fact]
        public void Enumeration_should_initialize_correctly()
        {
            var sut = OrderStatus.Pending;

            sut.Value.Should().Be(1);
            sut.Name.Should().Be("Pending");
        }

        [Fact]
        public void GetAll_should_throw_exception_when_duplicate_value_exists()
        {
            Action act = () => OrderStatusWithDuplicateValue.GetAll();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Duplicate enumeration value detected: '1'. Value must be unique.");
        }

        [Fact]
        public void GetAll_should_throw_exception_when_duplicate_name_exists()
        {
            Action act = () => OrderStatusWithDuplicateName.GetAll();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Duplicate enumeration name detected: 'Pending'. Name must be unique.");
        }

        [Fact]
        public void GetAll_should_return_all_defined_enumerations()
        {
            var result = Enumeration<OrderStatus>.GetAll();

            result.Should().Contain([OrderStatus.Pending, OrderStatus.Shipped]);
            result.Should().HaveCount(2);
        }

        [Fact]
        public void Exists_should_return_true_when_value_exists()
        {
            var result = Enumeration<OrderStatus>.Exists(1);
            result.Should().BeTrue();
        }

        [Fact]
        public void Exists_should_return_false_when_value_does_not_exist()
        {
            var result = Enumeration<OrderStatus>.Exists(-1);
            result.Should().BeFalse();
        }

        [Fact]
        public void Exists_should_return_true_when_name_exists()
        {
            var result = Enumeration<OrderStatus>.Exists("Pending");
            result.Should().BeTrue();
        }

        [Fact]
        public void Exists_should_return_false_when_name_does_not_exist()
        {
            var result = Enumeration<OrderStatus>.Exists("Delivered");
            result.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(GetNullOrWhiteSpaceNames))]
        public void Exists_should_return_false_when_name_is_null_or_whitespace(string? name)
        {
            var result = Enumeration<OrderStatus>.Exists(name!);
            result.Should().BeFalse();
        }
        #endregion

        #region Get enumeration by value
        [Theory]
        [MemberData(nameof(GetValueOrderStatusTuples))]
        public void FromValue_should_return_correct_enumeration(int value, OrderStatus expextedResult)
        {
            var result = Enumeration<OrderStatus>.FromValue(value);
            result.Should().Be(expextedResult);
        }

        [Fact]
        public void FromValue_should_throw_exception_when_value_is_not_found()
        {
            Action act = () => Enumeration<OrderStatus>.FromValue(999);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Enumeration value '999' was not found. Ensure it exists before accessing.");
        }

        [Fact]
        public void FromValue_should_throw_exception_when_value_is_invalid()
        {
            Action act = () => Enumeration<OrderStatus>.FromValue(-1);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Invalid enumeration value: '-1'. Expected a positive value. (Parameter 'value')");
        }

        [Theory]
        [MemberData(nameof(GetValueOrderStatusTuples))]
        public void TryGetFromValue_should_set_correct_enumeration(int value, OrderStatus expectedEnumeration)
        {
            bool result = Enumeration<OrderStatus>.TryGetFromValue(value, out var enumeration);

            result.Should().BeTrue();
            enumeration.Should().Be(expectedEnumeration);
        }

        [Fact]
        public void TryGetFromValue_should_return_false_and_set_null_when_value_is_not_found()
        {
            bool result = Enumeration<OrderStatus>.TryGetFromValue(999, out var enumeration);

            result.Should().BeFalse();
            enumeration.Should().BeNull();
        }

        [Fact]
        public void TryGetFromValue_should_return_false_and_set_null_when_value_is_invalid()
        {
            bool result = Enumeration<OrderStatus>.TryGetFromValue(-1, out var enumeration);

            result.Should().BeFalse();
            enumeration.Should().BeNull();
        }
        #endregion

        #region Get enumeration by name
        [Theory]
        [MemberData(nameof(GetNameOrderStatusTuples))]
        public void FromName_should_return_correct_enumeration(string name, OrderStatus expectedEnumeration)
        {
            var enumeration = Enumeration<OrderStatus>.FromName(name);
            enumeration.Should().Be(expectedEnumeration);
        }

        [Fact]
        public void FromName_should_throw_exception_when_name_is_not_found()
        {
            Action act = () => Enumeration<OrderStatus>.FromName("Created");

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Enumeration name 'Created' was not found. Ensure it exists before accessing.");
        }

        [Theory]
        [MemberData(nameof(GetNullOrWhiteSpaceNames))]
        public void FromName_should_throw_exception_when_name_is_null_or_whitespace(string? name)
        {
            Action act = () => Enumeration<OrderStatus>.FromName(name!);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Enumeration name cannot be null, empty, or contain only whitespace. (Parameter 'name')");
        }

        [Theory]
        [MemberData(nameof(GetNameOrderStatusTuples))]
        public void TryGetFromName_should_set_correct_enumeration(string name, OrderStatus expectedEnumeration)
        {
            bool result = Enumeration<OrderStatus>.TryGetFromName(name, out var enumeration);

            result.Should().BeTrue();
            enumeration.Should().Be(expectedEnumeration);
        }

        [Fact]
        public void TryGetFromName_should_return_false_and_set_null_when_name_is_not_found()
        {
            bool result = Enumeration<OrderStatus>.TryGetFromName("Created", out var enumeration);

            result.Should().BeFalse();
            enumeration.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(GetNullOrWhiteSpaceNames))]
        public void TryGetFromName_should_return_false_and_set_null_when_name_is_null_or_whitespace(string? name)
        {
            bool result = Enumeration<OrderStatus>.TryGetFromName(name!, out var enumeration);

            result.Should().BeFalse();
            enumeration.Should().BeNull();
        }

        [Fact]
        public void TryGetFromName_should_return_false_and_set_null_when_name_length_exceeds_maximum()
        {
            bool result = Enumeration<OrderStatus>.TryGetFromName(new string('A', MaxNameLength), out var enumeration);

            result.Should().BeFalse();
            enumeration.Should().BeNull();
        }
        #endregion

        #region Equality
        [Fact]
        public void Enumeration_should_be_equal_to_same_instance()
        {
            OrderStatus.Pending
                .Equals(OrderStatus.Pending)
                .Should().BeTrue();
        }

        [Fact]
        public void Enumeration_should_be_equal_to_instance_with_same_type_and_value()
        {
            OrderStatus.Pending
                .Equals(new OrderStatus(1, "Pending"))
                .Should().BeTrue();
        }

        [Fact]
        public void Enumeration_should_not_be_equal_to_instance_with_different_value()
        {
            OrderStatus.Pending
                .Equals(new OrderStatus(2, "Pending"))
                .Should().BeFalse();
        }

        [Fact]
        public void Enumeration_should_not_be_equal_to_null()
        {
            OrderStatus.Pending
                .Equals(null)
                .Should().BeFalse();
        }

        [Fact]
        public void Enumeration_should_be_equal_to_same_instance_as_object()
        {
            OrderStatus.Pending
                .Equals((object)OrderStatus.Pending)
                .Should().BeTrue();
        }

        [Fact]
        public void Enumeration_should_be_equal_to_object_with_same_type_and_value()
        {
            OrderStatus.Pending
                .Equals((object)(new OrderStatus(1, "Pending")))
                .Should().BeTrue();
        }

        [Fact]
        public void Enumeration_should_not_be_equal_to_object_with_different_value()
        {
            OrderStatus.Pending
                .Equals((object)(new OrderStatus(2, "Pending")))
                .Should().BeFalse();
        }

        [Fact]
        public void Enumeration_should_not_be_equal_to_different_enumeration_type()
        {
            OrderStatus.Pending
                .Equals(new InvoiceStatus(1, "Pending"))
                .Should().BeFalse();
        }

        [Fact]
        public void Enumeration_should_not_be_equal_to_null_object()
        {
            OrderStatus.Pending
                .Equals((object?)null)
                .Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_should_return_true_for_instances_with_same_value()
        {
            (OrderStatus.Pending == new OrderStatus(1, "Pending"))
                .Should().BeTrue();
        }
        #endregion

        #region Operators
        [Fact]
        public void Inequality_operator_should_return_false_for_instances_with_same_value()
        {
            (OrderStatus.Pending != new OrderStatus(1, "Pending"))
                .Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_should_return_false_for_instances_with_different_values()
        {
            (OrderStatus.Pending == OrderStatus.Shipped)
                .Should().BeFalse();
        }

        [Fact]
        public void Inequality_operator_should_return_true_for_instances_with_different_values()
        {
            (OrderStatus.Pending != OrderStatus.Shipped)
                .Should().BeTrue();
        }

        [Fact]
        public void Equality_operator_should_return_true_when_both_operands_are_null()
        {
            OrderStatus left = null!;
            OrderStatus right = null!;
            (left == right).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_should_return_false_when_both_operands_are_null()
        {
            OrderStatus left = null!;
            OrderStatus right = null!;
            (left != right).Should().BeFalse();
        }

        [Fact]
        public void Equality_operator_should_return_false_when_one_operand_is_null()
        {
            OrderStatus? left = null!;
            (left == OrderStatus.Pending).Should().BeFalse();
            (OrderStatus.Pending == left).Should().BeFalse();
        }

        [Fact]
        public void Inequality_operator_should_return_true_when_one_operand_is_null()
        {
            OrderStatus? left = null!;
            (left != OrderStatus.Pending).Should().BeTrue();
            (OrderStatus.Pending != left).Should().BeTrue();
        }
        #endregion

        #region GetHashCode
        [Fact]
        public void Enumerations_with_same_value_should_have_same_hash_code()
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

        [Fact]
        public void GetHashCode_should_return_consistent_value()
        {
            var sut = OrderStatus.Pending;
            var hash1 = sut.GetHashCode();
            var hash2 = sut.GetHashCode();

            hash1.Should().Be(hash2);
        }
        #endregion

        #region Compare
        [Fact]
        public void CompareTo_should_return_zero_when_values_are_equal()
        {
            OrderStatus.Pending
                .CompareTo(new OrderStatus(1, "Pending"))
                .Should().Be(0);
        }

        [Fact]
        public void CompareTo_should_return_negative_when_compared_to_higher_value()
        {
            OrderStatus.Pending
                .CompareTo(OrderStatus.Shipped)
                .Should().BeNegative();
        }

        [Fact]
        public void CompareTo_should_return_positive_when_compared_to_lower_value()
        {
            OrderStatus.Shipped
                .CompareTo(OrderStatus.Pending)
                .Should().BePositive();
        }

        [Fact]
        public void CompareTo_should_return_positive_when_other_is_null()
        {
            OrderStatus? nullEnumeration = null;
            OrderStatus.Pending
                .CompareTo(nullEnumeration)
                .Should().BePositive();
        }

        [Fact]
        public void CompareTo_object_should_return_zero_when_values_are_equal()
        {
            OrderStatus.Pending
                .CompareTo((object)(new OrderStatus(1, "Pending")))
                .Should().Be(0);
        }

        [Fact]
        public void CompareTo_object_should_return_negative_when_compared_to_higher_value()
        {
            OrderStatus.Pending
                .CompareTo((object)OrderStatus.Shipped)
                .Should().BeNegative();
        }

        [Fact]
        public void CompareTo_object_should_return_positive_when_compared_to_lower_value()
        {
            OrderStatus.Shipped
                .CompareTo((object)OrderStatus.Pending)
                .Should().BePositive();
        }

        [Fact]
        public void CompareTo_object_should_return_positive_when_other_is_null()
        {
            object? nullEnumeration = null;
            OrderStatus.Pending
                .CompareTo(nullEnumeration)
                .Should().BePositive();
        }

        [Fact]
        public void CompareTo_object_should_throw_exception_when_compared_to_different_enumeration_type()
        {
            var sut = OrderStatus.Pending;
            var other = InvoiceStatus.Pending;

            Action act = () => sut.CompareTo(other);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Object type mismatch. Ensure the correct type is used. (Parameter 'obj')");
        }

        [Fact]
        public void CompareTo_object_should_throw_exception_when_compared_to_non_enumeration_type()
        {
            var sut = OrderStatus.Pending;
            string other = "Invoice status";

            Action act = () => sut.CompareTo(other);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Object type mismatch. Ensure the correct type is used. (Parameter 'obj')");
        }
        #endregion

        [Fact]
        public void ToString_should_return_enumeration_name()
        {
            var sut = OrderStatus.Pending;
            string result = sut.ToString();

            result.Should().Be(sut.Name);
        }

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
            public static readonly InvoiceStatus Shipped = new InvoiceStatus(2, nameof(Shipped));

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
