using EventSourcingDrinkAlcoholFun.Domain.Events;
using System.Runtime.Serialization;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    [Serializable]
    public class ConcurrencyException : System.Exception
    {
        public ConcurrencyException(AggregateKey id)
            : base(string.Format("A different version than expected was found in aggregate {0}", id))
        {
        }
    }
}