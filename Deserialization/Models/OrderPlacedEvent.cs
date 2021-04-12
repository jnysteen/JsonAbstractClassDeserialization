using System.Collections.Generic;

namespace Deserialization.Models
{
    /// <summary>
    ///     A concrete implementation of an <see cref="Event"/>
    /// </summary>
    public class OrderPlacedEvent : OrderEvent
    {
        public override string EventType => "OrderPlaced";
        public List<OrderLine> OrderLines { get; set; }
    }

    public class OrderLine
    {
        public string ItemId { get; set; }
        public decimal Quantity { get; set; }
    }
}