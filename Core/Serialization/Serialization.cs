using System.Reflection;
using Newtonsoft.Json;

namespace Models
{
    public static class Serialization
    {
        public static string SerializeToJson<T>(T instance)
        {
            var serialized = JsonConvert.SerializeObject(instance);
            return serialized;
        }

        public static T DeserializeJson<T>(string serialized)
        {
            JsonConverter[] converters = { new EventJsonConverter(Assembly.GetAssembly(typeof(Event)))};
            var jsonSettings = new JsonSerializerSettings()
            {
                Converters = converters
            };
            
            var deserialized = JsonConvert.DeserializeObject<T>(serialized,jsonSettings);
            
            return deserialized;
        }
    }
}