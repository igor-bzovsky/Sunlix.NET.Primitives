using Newtonsoft.Json;

namespace Sunlix.NET.Primitives.Serialization.Json.Converters.Newtonsoft.Json
{
    public class EnumerationNJConverter<T> : JsonConverter<T> where T : Enumeration
    {
        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var displayName = reader.Value?.ToString();
            if (displayName == null)
                throw new JsonSerializationException($"Invalid enumeration value for {typeof(T).Name}.");

            return Enumeration.GetByDisplayName<T>(displayName);
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            writer.WriteValue(value.DisplayName);
        }
    }
}
