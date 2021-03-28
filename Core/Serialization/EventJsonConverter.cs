using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Serialization
{
    public class EventJsonConverter : JsonConverter
    {
        private readonly Dictionary<string, Type> _typeMapping;

        public EventJsonConverter(params Assembly[] assembliesWithEvents)
        {
            _typeMapping = CreateEventTypeMap(assembliesWithEvents);
        }

        private Dictionary<string, Type> CreateEventTypeMap(Assembly[] assembliesWithEvents)
        {
            var typeMap = new Dictionary<string, Type>();

            foreach (var assemblyWithEvents in assembliesWithEvents)
            {
                var allEventImplementations = assemblyWithEvents
                    .GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(Event)) && !t.IsAbstract);

                foreach (var eventImpl in allEventImplementations)
                {
                    var eventTypeProperty = eventImpl.GetProperty(nameof(Event.EventType));
                    var eventInstance = Activator.CreateInstance(eventImpl);
                    var eventTypeValue = eventTypeProperty.GetValue(eventInstance) as string; // TODO throw if anything here is null
                    _typeMapping.Add(eventTypeValue, eventImpl);
                }
            }

            return typeMap;
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