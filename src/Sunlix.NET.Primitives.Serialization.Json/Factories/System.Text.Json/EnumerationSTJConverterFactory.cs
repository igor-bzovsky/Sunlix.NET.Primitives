using Sunlix.NET.Primitives.Serialization.Json.Converters.Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sunlix.NET.Primitives.Serialization.Json.Factories.System.Text.Json
{
    public class EnumerationSTJConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert.IsSubclassOf(typeof(Enumeration));

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(EnumerationSTJConverter<>).MakeGenericType(typeToConvert);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }
    }
}
