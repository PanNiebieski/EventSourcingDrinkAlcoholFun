using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.EventStore
{
    public interface IEventStoreSQLiteContext
    {
        string ConnectionString { get; }
    }

    public class EventStoreSQLiteContext : IEventStoreSQLiteContext
    {
        public EventStoreSQLiteContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
    }
}
