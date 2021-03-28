namespace Core.Models
{
    public abstract class OrderEvent : Event
    {
        public string OrderId { get; set; }
    }
}