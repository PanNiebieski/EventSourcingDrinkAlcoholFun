using Dapper;
using EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.EventStore
{
    public class EventTableManager : IEventTableManager
    {
        private IEventStoreSQLiteContext _eventstoreContext;

        public EventTableManager(IEventStoreSQLiteContext context)
        {
            _eventstoreContext = context;
        }

        public async Task DeleteAllRecordsInTables()
        {
            using var connection = new SqliteConnection
                (_eventstoreContext.ConnectionString);

            string sql = "DELETE FROM EventsTable;" +
                "DELETE FROM sqlite_sequence WHERE name='EventsTable'";

            await connection.ExecuteAsync(sql);
        }
    }
}
