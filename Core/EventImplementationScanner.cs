using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Models;

namespace Core
{
    public static class EventImplementationScanner
    {
        public static Dictionary<string, Type> FindEventImplementations(Assembly[] assembliesWithEvents)
        {
            var typeMap = new Dictionary<string, Type>();

            foreach (var assemblyWithEvents in assembliesWithEvents)
            {
                var allEventImplementations = assemblyWithEvents
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Event)) && !t.IsAbstract);

                foreach (var eventImpl in allEventImplementations)
                {
                    var eventTypeProperty = eventImpl.GetProperty(nameof(Event.EventType));
                    // TODO throw if anything here is null
                    var eventInstance = Activator.CreateInstance(eventImpl);
                    var eventTypeValue = eventTypeProperty.GetValue(eventInstance) as string;
                    // TODO throw if duplicates are detected
                    typeMap.Add(eventTypeValue, eventImpl);
                }
            }

            return typeMap;
        }
    }
}