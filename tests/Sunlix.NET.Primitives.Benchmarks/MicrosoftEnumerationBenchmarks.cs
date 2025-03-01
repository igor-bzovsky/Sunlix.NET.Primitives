using BenchmarkDotNet.Attributes;

namespace Sunlix.NET.Primitives.Benchmarks
{
    public class MicrosoftEnumerationBenchmarks
    {
        public class OrderStatus : MicrosoftEnumeration
        {
            public static readonly OrderStatus Pending = new(1, nameof(Pending));
            public static readonly OrderStatus Shipped = new(2, nameof(Shipped));
            public static readonly OrderStatus Delivered = new(3, nameof(Delivered));

            public OrderStatus(int value, string name) : base(value, name) { }
        }

        [Benchmark]
        public IReadOnlyList<OrderStatus> GetAll_Microsoft()
            => MicrosoftEnumeration.GetAll<OrderStatus>().ToList();
    }
}
