using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces
{
    public interface IEventStore
    {
        Task Save(DomainEvent @event);

        Task<List<DomainEvent>> ReadEventStream(AggregateKey aggregateId,
            int fromVersion);

        Task<List<RawEventRecord>> GetRawAllEvents();

        Task<List<AggregatePerDomainEvent>> ReadAllStreams();

    }
}
