using System.Collections.Generic;

namespace Deserialization.Models
{
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