namespace Sunlix.NET.Primitives.Tests
{
    public class UnitTests
    {
        [Fact]
        public void Units_should_always_be_equal()
        {
            var sut = Unit.value;
            var other = Unit.value;

            sut.Equals(other).Should().BeTrue();
        }

        [Fact]
        public void Units_should_always_be_equal_as_object()
        {
            var sut = Unit.value;
            var other = Unit.value;

            sut.Equals((object)other).Should().BeTrue();
        }

        [Fact]
        public void Equality_operator_should_always_return_true()
        {
            var left = Unit.value;
            var right = Unit.value;

            (left == right).Should().BeTrue();
        }

        [Fact]
        public void Inequality_operator_should_always_return_false()
        {
            var left = Unit.value;
            var right = Unit.value;

            (left != right).Should().BeFalse();
        }

        [Fact]
        public void Units_should_always_have_same_hash_code()
        {
            var sut = new Unit();
            var other = new Unit();
            sut.GetHashCode().Should().Be(other.GetHashCode());
        }

        [Fact]
        public void ToString_should_return_correct_value()
        {
            var unit = new Unit();
            unit.ToString().Should().Be("()");
        }
    }
}
