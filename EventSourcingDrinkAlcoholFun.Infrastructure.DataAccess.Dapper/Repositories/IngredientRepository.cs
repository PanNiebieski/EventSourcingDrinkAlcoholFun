using Dapper;
using EventSourcingDrinkAlcoholFun.Core;
using EventSourcingDrinkAlcoholFun.Domain;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.DataAccess.Dapper.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private IDBContext _dbContext;

        public IngredientRepository(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Ingredient>> GetAllAsync()
        {
            using var connection = new SqliteConnection
                (_dbContext.ConnectionString);

            var q = @$"SELECT
                Id,Name,Volume,Price,CreatedAt FROM Ingredients";

            var r = await connection.QueryAsync<Ingredient>(q);

            return r.ToList().AsReadOnly();
        }

        public async Task<Ingredient> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection
                (_dbContext.ConnectionString);

            var q = @$"SELECT
                Id,Name,Volume,Price,CreatedAt FROM Ingredients
                WHERE Id = @Id";

            var r = await connection.
                QueryFirstOrDefaultAsync<Ingredient>(q, new { @Id = id });

            return r;
        }
    }
}
