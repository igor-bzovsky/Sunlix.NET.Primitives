using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sunlix.NET.Primitives.Serialization.Json.Converters.Newtonsoft.Json
{
    public class EnumerationSTJConverter<T> : JsonConverter<T> where T : Enumeration
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var displayName = reader.GetString();
            if (displayName == null)
                throw new JsonException($"Invalid enumeration value for {typeof(T).Name}.");

            return Enumeration.GetByDisplayName<T>(displayName);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.DisplayName);
        }
    }
}
