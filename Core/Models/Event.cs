namespace Models
{
    public abstract class Event
    {
        public string EventId { get; set; }
        public abstract string EventType { get; }
    }
}