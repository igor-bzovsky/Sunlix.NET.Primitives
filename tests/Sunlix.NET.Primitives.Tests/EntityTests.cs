namespace Sunlix.NET.Primitives.Tests
{
    public class EntityTests
    {
        [Fact]
        public void Constructor_should_throw_exception_when_id_is_null()
        {
            Action act = () => new StringEntity(null!);
            act.Should().Throw<ArgumentNullException>().WithParameterName("id");
        }

        [Fact]
        public void Entities_with_same_id_should_be_equal()
        {
            var id = Guid.NewGuid();
            var entity1 = new GuidEntity(id);
            var entity2 = new GuidEntity(id);

            entity1.Should().Be(entity2);
            (entity1 == entity2).Should().BeTrue();
        }

        [Fact]
        public void Entities_with_different_id_should_not_be_equal()
        {
            var entity1 = new GuidEntity(Guid.NewGuid());
            var entity2 = new GuidEntity(Guid.NewGuid());

            entity1.Should().NotBe(entity2);
            (entity1 != entity2).Should().BeTrue();
        }

        [Fact]
        public void Entities_with_same_id_should_have_same_hash_code()
        {
            var id = Guid.NewGuid();
            var entity1 = new GuidEntity(id);
            var entity2 = new GuidEntity(id);

            entity1.GetHashCode().Should().Be(entity2.GetHashCode());
        }

        [Fact]
        public void Entities_with_different_id_should_have_different_hash_codes()
        {
            var entity1 = new GuidEntity(Guid.NewGuid());
            var entity2 = new GuidEntity(Guid.NewGuid());

            entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
        }

        private class GuidEntity : Entity<Guid>
        {
            public GuidEntity(Guid id) : base(id) { }
        }

        private class StringEntity : Entity<string>
        {
            public StringEntity(string id) : base(id) { }
        }
    }
}
