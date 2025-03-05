namespace Sunlix.NET.Primitives.Tests
{
    public class ErrorTests
    {
        [Fact]
        public void Error_should_be_initialized_correctly()
        {
            var sut = new Error("404", "Resource not found");

            sut.Code.Should().Be("404");
            sut.Message.Should().Be("Resource not found");
        }

        [Fact]
        public void ToString_should_return_correct_value()
        {
            var sut = new Error("404", "Resource not found");
            var result = sut.ToString();

            result.Should().Be("404: Resource not found");
        }
    }
}
