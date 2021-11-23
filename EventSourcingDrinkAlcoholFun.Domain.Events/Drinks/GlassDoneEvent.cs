using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.DomainEvents.Drinks
{
    public class GlassDoneEvent : DomainEvent
    {
        public GlassDoneEvent(Guid id, int version)
            : base()
        {
            Key_StreamId = AggregateKey.FromGuid(id);
            Version_SerialNumber = version;
        }


    }
}
