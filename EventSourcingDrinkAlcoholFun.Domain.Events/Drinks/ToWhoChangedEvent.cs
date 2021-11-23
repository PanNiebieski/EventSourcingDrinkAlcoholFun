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
        public new ToWhoChangedEventData Data { get; init; } 
            = new ToWhoChangedEventData();

        public override int Type => 2;

        public ToWhoChangedEvent(string toWhoAfter, string toWhoBefore,
    Guid id, int version)
    : base()
        {
            Data.ToWhoAfter = toWhoAfter;
            Data.ToWhoBefore = toWhoBefore;
            Key_StreamId = AggregateKey.FromGuid(id);
            Version_SerialNumber = version;
        }

        public void Deconstruct(out Guid uniqueId, out string toWhoAfter,
            out string toWhoBefore)
        {
            uniqueId = Key_StreamId.Id;
            toWhoAfter = Data.ToWhoAfter;
            toWhoBefore = Data.ToWhoBefore;
        }
    }

    public class ToWhoChangedEventData : DomainEventData
    {
        public string ToWhoAfter { get; set; }

        public string ToWhoBefore { get; set; }
    }

}
