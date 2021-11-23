using EventSourcingDrinkAlcoholFun.Domain.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.DomainEvents.Drinks
{
    public class ReverseEvent : DomainEvent
    {
        public override int Type => 5;
        public new ReverseEventData Data { get; init; } = 
            new ReverseEventData();

        public ReverseEvent(List<DomainEvent> events,int steps,
            Guid id, int version): base()
        {
            Data.ReversedEvents = events;
            Key_StreamId = AggregateKey.FromGuid(id);
            Version_SerialNumber = version;
        }

        public void Deconstruct(out Guid uniqueId, 
            out List<DomainEvent> events)
        {
            uniqueId = Key_StreamId.Id;
            events = Data.ReversedEvents;
        }
    }

    public class ReverseEventData
    {
        [JsonConverter(typeof(ConcreteTypeConverter))]
        public List<DomainEvent> ReversedEvents { get; set; }

        public int Steps { get; set; }
    }
}
