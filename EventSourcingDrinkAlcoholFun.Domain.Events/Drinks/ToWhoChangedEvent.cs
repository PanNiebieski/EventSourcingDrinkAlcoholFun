using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.DomainEvents.Drinks
{
    public class ToWhoChangedEvent : DomainEvent
    {
        public string ToWho { get; set; }

        public ToWhoChangedEvent(string toWho,
    Guid id, int version)
    : base()
        {
            ToWho = toWho;
            Key_StreamId = AggregateKey.FromGuid(id);
            Version_SerialNumber = version;
        }

        public void Deconstruct(out Guid uniqueId, out string toWho)
        {
            uniqueId = Key_StreamId.Id;
            toWho = ToWho;
        }
    }
}
