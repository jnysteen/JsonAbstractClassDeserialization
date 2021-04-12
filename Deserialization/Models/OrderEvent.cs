namespace Deserialization.Models
{
    /// <summary>
    ///     The base class for Order events
    /// </summary>
    public abstract class OrderEvent : Event
    {
        public string OrderId { get; set; }
    }
}