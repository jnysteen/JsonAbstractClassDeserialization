using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Deserialization.Models;

namespace Deserialization.SystemTextJson
{
    /// <summary>
    ///     Strongly inspired by https://stackoverflow.com/questions/58074304/is-polymorphic-deserialization-possible-in-system-text-json
    /// </summary>
    public class SystemTextJsonEventConverter : JsonConverter<Event>
    {
        private readonly Dictionary<string, Type> _typeMapping;
        
        public SystemTextJsonEventConverter(params Assembly[] assembliesWithEvents)
        {
            _typeMapping = EventImplementationScanner.FindEventImplementations(assembliesWithEvents);
        }
        
        public override Event Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                var eventTypePropertyName = nameof(Event.EventType);
                if (!jsonDocument.RootElement.TryGetProperty(eventTypePropertyName, out var typeProperty))
                    throw new JsonException($"Could not find type discriminator property '{eventTypePropertyName}' in JSON");

                var typePropertyValue = typeProperty.GetString();
                if(string.IsNullOrEmpty(typePropertyValue))
                    throw new JsonException("The type discriminator property was null or empty");
                
                if(!_typeMapping.TryGetValue(typePropertyValue, out var knownType)) 
                    throw new JsonException($"Type '{typePropertyValue}' from the JSON is unknown");

                var jsonObject = jsonDocument.RootElement.GetRawText();
                var result = (Event) JsonSerializer.Deserialize(jsonObject, knownType);

                return result;
            }
        }

        public override void Write(Utf8JsonWriter writer, Event value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object) value, options);
        }
    }
}