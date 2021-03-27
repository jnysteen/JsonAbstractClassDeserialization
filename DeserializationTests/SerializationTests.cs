using System.Collections.Generic;
using FluentAssertions;
using Models;
using NUnit.Framework;

namespace DeserializationTests
{
    public class SerializationTests
    {
        [Test]
        public void CanDeserializeToAbstractClass()
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
            var serialized = Serialization.SerializeToJson(orderPlacedEvent);
            var deserialized = Serialization.DeserializeJson<Event>(serialized);

            // Assert
            deserialized.Should().NotBeNull("the deserialization should have produced something");
            deserialized.Should().BeOfType<OrderPlacedEvent>("the deserialized instance should be an OrderPlacedEvent");
            deserialized.Should().BeEquivalentTo(orderPlacedEvent);
        }
    }
}