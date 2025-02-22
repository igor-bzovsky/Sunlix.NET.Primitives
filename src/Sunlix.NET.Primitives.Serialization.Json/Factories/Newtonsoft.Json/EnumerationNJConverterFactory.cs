using Newtonsoft.Json;
using Sunlix.NET.Primitives.Serialization.Json.Converters.Newtonsoft.Json;

namespace Sunlix.NET.Primitives.Serialization.Json.Factories.Newtonsoft.Json
{
    public class EnumerationNJConverterFactory : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType.IsSubclassOf(typeof(Enumeration));

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var converterType = typeof(EnumerationNJConverter<>).MakeGenericType(objectType);
            var converter = (JsonConverter?)Activator.CreateInstance(converterType);
            return converter?.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null) return;
            var converterType = typeof(EnumerationNJConverter<>).MakeGenericType(value.GetType());
            var converter = (JsonConverter?)Activator.CreateInstance(converterType);
            converter?.WriteJson(writer, value, serializer);
        }
    }
}
