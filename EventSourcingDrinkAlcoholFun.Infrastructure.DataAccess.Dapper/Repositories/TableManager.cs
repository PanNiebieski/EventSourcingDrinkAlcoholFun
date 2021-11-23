using Dapper;
using EventSourcingDrinkAlcoholFun.Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.DataAccess.Dapper.Repositories
{
    public class TableManager : ITableManager
    {
        private IDBContext _dbContext;

        public TableManager(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task DeleteAllRecordsInTables()
        {
            using var connection = new SqliteConnection
                (_dbContext.ConnectionString);

            string sql = "DELETE FROM IngredientsInGlassDrink;" +
                "DELETE FROM Drinks;" +
                "DELETE FROM sqlite_sequence WHERE name='Drinks'" +
                "OR name='IngredientsInGlassDrink';";

            await connection.ExecuteAsync(sql);
        }
    }
}
