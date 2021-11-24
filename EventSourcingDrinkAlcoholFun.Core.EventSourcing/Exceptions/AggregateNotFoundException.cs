using EventSourcingDrinkAlcoholFun.Domain.Events;
using System.Runtime.Serialization;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    [Serializable]
    public class AggregateNotFoundException : System.Exception
    {
        public AggregateNotFoundException(AggregateKey id)
            : base(string.Format("Aggregate {0} was not found", id))
        {
        }
    }
}