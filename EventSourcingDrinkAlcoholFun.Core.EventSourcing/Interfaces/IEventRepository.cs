using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces
{
    public interface IEventRepository
    {
        Task Save<T>(T aggregate, int? expectedVersion = null) 
            where T : AggregateRoot;

        Task<T> Get<T>(AggregateKey aggregateId) where T : AggregateRoot;


        Task<List<RawEventRecord>> GetRawAllEvents();

        Task<List<T>> GetAll<T>() where T : AggregateRoot;

    }
}
