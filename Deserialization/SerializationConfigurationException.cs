using System;
using System.Runtime.Serialization;

namespace Deserialization
{
    public class SerializationConfigurationException : Exception
    {
        public SerializationConfigurationException()
        {
        }

        protected SerializationConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SerializationConfigurationException(string message) : base(message)
        {
        }

        public SerializationConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}