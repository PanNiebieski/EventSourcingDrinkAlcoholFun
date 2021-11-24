using EventSourcingDrinkAlcoholFun.Domain.Events;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces
{
    public class AggregatePerDomainEvent
    {
        public AggregatePerDomainEvent()
        {
            Events = new();
        }

        public List<DomainEvent> Events { get; set; }

        public AggregateKey AggregateId { get; set; }
    }
}
