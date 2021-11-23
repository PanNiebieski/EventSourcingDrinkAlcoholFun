using System.Runtime.Serialization;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    [Serializable]
    internal class EventsOutOfOrderException : Exception
    {
        private object key;

        public EventsOutOfOrderException()
        {
        }

        public EventsOutOfOrderException(object key)
        {
            this.key = key;
        }

        public EventsOutOfOrderException(string? message) : base(message)
        {
        }

        public EventsOutOfOrderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EventsOutOfOrderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}