using System.Reflection;
using System.Text.Json;

namespace Core.SystemTextJson
{
    public class SystemTextJsonEventSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SystemTextJsonEventSerializer(params Assembly[] assembliesWithEvents)
        {
            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.Converters.Add(new SystemTextJsonEventConverter(assembliesWithEvents));
        }
        public string SerializeToJson<T>(T instance)
        {
            var serialized = JsonSerializer.Serialize(instance, _jsonSerializerOptions);
            return serialized;
        }

        public T DeserializeJson<T>(string serialized)
        {
            var deserialized = JsonSerializer.Deserialize<T>(serialized, _jsonSerializerOptions);
            return deserialized;
        }
    }
}