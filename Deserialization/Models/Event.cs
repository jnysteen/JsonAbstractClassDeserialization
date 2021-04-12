namespace Deserialization.Models
{
    /// <summary>
    ///     An abstract base class for events
    /// </summary>
    public abstract class Event
    {
        /// <summary>
        ///     A concrete property
        /// </summary>
        public string EventId { get; set; }
        
        /// <summary>
        ///     An abstract property
        /// </summary>
        public abstract string EventType { get; }
    }
}