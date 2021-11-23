using EventSourcingDrinkAlcoholFun.Domain.Events;
using System.Runtime.Serialization;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    [Serializable]
    internal class AggregateNotFoundException : Exception
    {
        private AggregateKey id;

        public AggregateNotFoundException()
        {
        }

        public AggregateNotFoundException(AggregateKey id)
        {
            this.id = id;
        }

        public AggregateNotFoundException(string? message) : base(message)
        {
        }

        public AggregateNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AggregateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}