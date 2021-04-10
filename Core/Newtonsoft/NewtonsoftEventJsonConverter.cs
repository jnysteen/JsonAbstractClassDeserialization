using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Newtonsoft
{
    public class NewtonsoftEventJsonConverter : JsonConverter
    {
        private readonly Dictionary<string, Type> _typeMapping;

        public NewtonsoftEventJsonConverter(params Assembly[] assembliesWithEvents)
        {
            _typeMapping = EventImplementationScanner.FindEventImplementations(assembliesWithEvents);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Event));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            var discriminator = jObject[nameof(Event.EventType)]?.Value<string>();
            if (discriminator == null)
                return null;

            var canRecognizeType = _typeMapping.TryGetValue(discriminator, out var typeToDeserializeTo);
            if (!canRecognizeType)
                return null;
            
            return jObject.ToObject(typeToDeserializeTo, serializer);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}