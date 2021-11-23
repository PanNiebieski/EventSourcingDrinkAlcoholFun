using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.EventStore
{
    public interface IEventStoreDBRepository<TEnity> 
        where TEnity : notnull
    {

        //Task<TEnity> Find(Func<TEnity?, object, TEnity> when,
        //    AggregateKey key);

        Task Append(
            DomainEvent @event);
    }
}
