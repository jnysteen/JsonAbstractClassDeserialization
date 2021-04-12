using System.Reflection;
using Newtonsoft.Json;

namespace Deserialization.Newtonsoft
{
    public class NewtonsoftEventSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _jsonSettings;

        public NewtonsoftEventSerializer(params Assembly[] assembliesWithEvents)
        {
            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.Converters.Add(new NewtonsoftEventJsonConverter(assembliesWithEvents));
        }
        
        public string SerializeToJson<T>(T instance)
        {
            var serialized = JsonConvert.SerializeObject(instance);
            return serialized;
        }

        public T DeserializeJson<T>(string serialized)
        {
            var deserialized = JsonConvert.DeserializeObject<T>(serialized, _jsonSettings);
            return deserialized;
        }
    }
}