using EventSourcingDrinkAlcoholFun.Domain.Events;
using System.Runtime.Serialization;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    [Serializable]
    public class EventsOutOfOrderException : System.Exception
    {
        public EventsOutOfOrderException(AggregateKey id)
            : base(string.Format("Eventstore gave event for aggregate {0} out of order", id))
        {
        }
    }
}