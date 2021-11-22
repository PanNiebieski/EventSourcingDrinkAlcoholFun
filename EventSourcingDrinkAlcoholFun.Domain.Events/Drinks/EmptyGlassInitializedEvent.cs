using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.DomainEvents
{
    public class EmptyGlassInitializedEvent : DomainEvent
    {
        public EmptyGlassInitializedEvent
            (Guid uniqueId) : base()
        {
            Key_StreamId = AggregateKey.FromGuid(uniqueId);
            TimeStamp = DateTimeOffset.Now;
        }
    }
}
