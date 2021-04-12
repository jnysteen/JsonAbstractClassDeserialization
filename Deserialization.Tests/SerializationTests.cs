using System.Collections.Generic;
using System.Reflection;
using Deserialization.Models;
using Deserialization.Newtonsoft;
using Deserialization.SystemTextJson;
using FluentAssertions;
using NUnit.Framework;

namespace Deserialization.Tests
{
    public class SerializationTests
    {
        [TestCaseSource(nameof(SerializerTestCases))]
        public void CanDeserializeToAbstractClass(IJsonSerializer serializer)
        {
            // Arrange
            var orderPlacedEvent = new OrderPlacedEvent
            {
                EventId = "event-123",
                OrderId = "order-123",
                OrderLines = new List<OrderLine>()
                {
                    new()
                    {
                        Quantity = 2m,
                        ItemId = "item-123"
                    }
                }
            };
            
            // Act 
            var serialized = serializer.SerializeToJson(orderPlacedEvent);
            var deserialized = serializer.DeserializeJson<Event>(serialized);

            // Assert
            deserialized.Should().NotBeNull("the deserialization should have produced something");
            deserialized.Should().BeOfType<OrderPlacedEvent>("the deserialized instance should be an OrderPlacedEvent");
            deserialized.Should().BeEquivalentTo(orderPlacedEvent);
        }
        
        static object[] SerializerTestCases =
        {
            new object[] { new NewtonsoftEventSerializer(Assembly.GetAssembly(typeof(Event))) },
            new object[] { new SystemTextJsonEventSerializer(Assembly.GetAssembly(typeof(Event))) },
        };
    }
}