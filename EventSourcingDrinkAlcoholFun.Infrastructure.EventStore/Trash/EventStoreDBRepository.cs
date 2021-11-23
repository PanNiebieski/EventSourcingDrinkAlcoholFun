using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.EventStore
{
    public class EventStoreDBRepository :
        IEventStoreDBRepository<Drink>
    {
        private readonly SqlLiteEventStore _eventStore;

        public EventStoreDBRepository
            (SqlLiteEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public  async Task Append(
            DomainEvent @event)
        {
            await _eventStore.Save(@event);
        }

        public async Task Append(AggregateKey key, int version, DomainEvent @event)
        {
            await _eventStore.Save(@event);
        }

        //public Task Append(AggregateKey key, 
        //    int version, DomainEvent @event, 
        //    CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<Drink> Find(Func<Drink?, object, Drink> when,
        //    AggregateKey key, 
        //    CancellationToken cancellationToken)
        //{
        //    var list = await _eventStore.ReadEventStream(key, 0);



        //    IEnumerable<DomainEvent> readresult = _eventStore;


        //}



    }


}