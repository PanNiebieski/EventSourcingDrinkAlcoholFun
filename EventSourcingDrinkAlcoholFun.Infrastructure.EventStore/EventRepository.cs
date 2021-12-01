using EventSourcingDrinkAlcoholFun.Core.EventSourcing;
using EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces;
using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.EventStore
{
    public  class EventRepository : IEventRepository
    {
        private readonly IEventStore _eventStore;

        public EventRepository(IEventStore eventStore)
        {
            if (eventStore == null)
                throw new ArgumentNullException("eventStore");

            _eventStore = eventStore;

        }

        public async Task<T> Get<T>(AggregateKey aggregateId) 
            where T : AggregateRoot
        {
            return await LoadAggregate<T>(aggregateId);
        }


        public async Task Save<T>(T aggregate, int? expectedVersion = null) 
            where T : AggregateRoot
        {
            if (expectedVersion != null)
            {
                var check = await _eventStore.ReadEventStream(
                    aggregate.Key, expectedVersion.Value);

                if (check.Any())
                {
                    throw new ConcurrencyException(aggregate.Key);
                }

            }

            var i = 0;

            foreach (var @event in aggregate.GetUncommittedChanges())
            {
                await _eventStore.Save(@event);
            }

            aggregate.MarkChangesAsCommitted();

        }

        private async Task<T> LoadAggregate<T>(AggregateKey id) where T : AggregateRoot
        {
            var aggregate = AggregateFactory.CreateAggregate<T>();

            var events = await _eventStore.ReadEventStream(id, -1);
            if (!events.Any())
            {
                throw new AggregateNotFoundException(id);
            }
            
            aggregate.LoadFromHistory(events);
            return aggregate;
        }


        public async Task<List<RawEventRecord>> GetRawAllEvents()
        {
            return await _eventStore.GetRawAllEvents();
        }

        public async Task<List<T>> GetAll<T>() where T : AggregateRoot
        {

            List<T> result = new List<T>();

            var eventsPerAggregate = await _eventStore.ReadAllStreams();

            foreach (var item in eventsPerAggregate)
            {
                var aggregate = AggregateFactory.CreateAggregate<T>();
                aggregate.LoadFromHistory(item.Events);

                result.Add(aggregate);
            }

            return result;
        }
    }
}
