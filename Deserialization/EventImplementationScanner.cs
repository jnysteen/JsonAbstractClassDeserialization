using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Deserialization.Models;

namespace Deserialization
{
    public static class EventImplementationScanner
    {
        public static Dictionary<string, Type> FindEventImplementations(Assembly[] assembliesWithEvents)
        {
            var typeMap = new Dictionary<string, Type>();
            var eventTypePropertyName = nameof(Event.EventType);

            try
            {
                foreach (var assemblyWithEvents in assembliesWithEvents)
                {
                    var allEventImplementations = assemblyWithEvents
                        .GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(Event)) && !t.IsAbstract);

                    foreach (var eventImpl in allEventImplementations)
                    {
                        var eventTypeProperty = eventImpl.GetProperty(eventTypePropertyName);
                        if (eventTypeProperty == null)
                            throw new ArgumentException($"Could not find the '{eventTypePropertyName}' property on the type");
                        var eventInstance = Activator.CreateInstance(eventImpl);
                        var eventTypeValue = eventTypeProperty.GetValue(eventInstance) as string;
                        if(string.IsNullOrWhiteSpace(eventTypeValue))
                            throw new ArgumentException($"The value of the '{eventTypePropertyName}' property was null or empty");
                        typeMap.Add(eventTypeValue, eventImpl);
                    }
                }
            }
            catch (Exception e)
            {
                throw new SerializationConfigurationException($"Event implementation discovery failed", e);
            }

            return typeMap;
        }
    }
}