using Dapper;
using EventSourcingDrinkAlcoholFun.Domain.Events;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.EventStore
{
    public class SqlLiteEventStore : IEventStore
    {
        private IEventStoreSQLiteContext _eventstoreContext;

        public SqlLiteEventStore(IEventStoreSQLiteContext context)
        {
            _eventstoreContext = context;
        }

        public async Task<List<DomainEvent>> ReadEventStream
            (AggregateKey aggregateId, int fromVersion)
        {
            using var connection = new SqliteConnection
                (_eventstoreContext.ConnectionString);

            try
            {
                var r = await connection.QueryAsync<RawEventRecord>
                (@"SELECT Id,Key_StreamId, Value_Data, AssemblyQualifiedName_Type, Version_SerialNumber,TimeStamp FROM EventsTable
                    WHERE Key_StreamId = @aggregateId and Version_SerialNumber > @Version;", new
                {
                    @aggregateId = aggregateId.Id,
                    @Version = fromVersion
                });

                List<DomainEvent> de = new List<DomainEvent>();

                foreach (var item in r)
                {
                    Assembly asm = typeof(DomainEvent).Assembly;
                    Type type = TypeRecon.ReconstructType(item.AssemblyQualifiedName_Type, true, asm);

                    var domain = JsonConvert.
                        DeserializeObject(item.Value_Data, type);

                    de.Add(domain as DomainEvent);
                }

                return de;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Save(DomainEvent @event)
        {
            using var connection = new SqliteConnection
                (_eventstoreContext.ConnectionString);


            try
            {
                var q = @"INSERT INTO EventsTable(Key_StreamId, Value_Data,
                    AssemblyQualifiedName_Type
                    ,Version_SerialNumber,TimeStamp)
                    VALUES (@Key, @Value, @AssemblyQualifiedName,@Version,
                    @Time);";


                var result = await connection.ExecuteAsync(q, new
                    {
                        @Key = @event.Key_StreamId.Id,
                        @Value = JsonConvert.SerializeObject(@event),
                        @AssemblyQualifiedName = @event.GetType().AssemblyQualifiedName,
                        @Version = @event.Version_SerialNumber,
                        @Time = @event.TimeStamp,
                }
                );

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<List<RawEventRecord>> GetRawAllEvents()
        {
            using var connection = new SqliteConnection
                (_eventstoreContext.ConnectionString);

            try
            {
                var r = await connection.QueryAsync<RawEventRecord>
                (@"SELECT Id,Key_StreamId, Value_Data, AssemblyQualifiedName_Type, Version_SerialNumber,TimeStamp FROM EventsTable
                    ");

                return r.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

         }

        public async Task<List<AggregatePerDomainEvent>> ReadAllStreams()
        {
            using var connection = new SqliteConnection
                (_eventstoreContext.ConnectionString);

            try
            {
                var r = await connection.QueryAsync<RawEventRecord>
                (@"SELECT Id,Key_StreamId, Value_Data, AssemblyQualifiedName_Type, Version_SerialNumber,TimeStamp FROM EventsTable
                   ", new
                {
  
                });

                List<AggregatePerDomainEvent> de = new List<AggregatePerDomainEvent>();
                var groupedbyKey = r.GroupBy(k => k.Key_StreamId);
                
                foreach (var listevents in groupedbyKey)
                {
                    AggregatePerDomainEvent e = new();
                    e.AggregateId = AggregateKey.FromString(listevents.Key);

                    foreach (var item in listevents)
                    {
                        Assembly asm = typeof(DomainEvent).Assembly;
                        Type type = TypeRecon.ReconstructType(item.AssemblyQualifiedName_Type, true, asm);

                        var domain = JsonConvert.
                            DeserializeObject(item.Value_Data, type);

                        e.Events.Add(domain as DomainEvent);
                    }

                    de.Add(e);
                }

                return de;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }


}
