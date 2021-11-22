using EventSourcingDrinkAlcoholFun.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.Repositories
{
    public class TableManager : ITableManager
    {
        protected readonly DrinkContext _dbContext;


        public TableManager(DrinkContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteAllRecordsInTables()
        {
            string sql = "DELETE FROM IngredientsInGlassDrink;" +
                "DELETE FROM Drinks;" +
                "DELETE FROM sqlite_sequence;";

            await _dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
