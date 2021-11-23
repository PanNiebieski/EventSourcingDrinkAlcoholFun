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
        public override int Type => 0;
        public EmptyGlassInitializedEvent
            (Guid uniqueId) : base()
        {
            Key_StreamId = AggregateKey.FromGuid(uniqueId);
            TimeStamp = DateTimeOffset.Now;
        }

        public void Deconstruct(out Guid uniqueId)
        {
            uniqueId = Key_StreamId.Id;
        }
    }
}
