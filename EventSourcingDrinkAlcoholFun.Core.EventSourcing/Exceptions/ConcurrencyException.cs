using EventSourcingDrinkAlcoholFun.Domain.Events;
using System.Runtime.Serialization;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    [Serializable]
    internal class ConcurrencyException : Exception
    {
        private AggregateKey key;

        public ConcurrencyException()
        {
        }

        public ConcurrencyException(AggregateKey key)
        {
            this.key = key;
        }

        public ConcurrencyException(string? message) : base(message)
        {
        }

        public ConcurrencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}