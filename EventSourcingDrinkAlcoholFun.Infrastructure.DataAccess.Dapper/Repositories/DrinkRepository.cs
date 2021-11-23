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
    public class DrinkRepository : IDrinkRepository
    {
        private IDBContext _dbContext;

        public DrinkRepository(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Drink> AddAsync(Drink entity)
        {
            using var connection = new SqliteConnection
                 (_dbContext.ConnectionString);

            var q = @"INSERT INTO Drinks 
                (Status, CreatedAt, UniqueId,
                ToWho)
                VALUES(@Status, @CreatedAt, 
                @UniqueId, @ToWho);

                SELECT seq From sqlite_sequence Where Name='Drinks'";

            var r1= await connection
                .QueryAsync<int>(q, entity);


            foreach (var item in entity.Ingredients)
            {
                var q1 = @"INSERT INTO IngredientsInGlassDrink 
                (DrinkId, IngredientId, CreatedAt)
                VALUES(@DrinkId, @IngredientId, @CreatedAt);

                SELECT seq From sqlite_sequence Where Name='IngredientsInGlassDrink'";

                var r2 = await connection.QueryAsync<int>(q1, 
                    new
                {
                        @DrinkId = entity.Id,
                        @IngredientId = item.Id,
                        @CreatedAt = item.CreatedAt
                });

            }

            entity.Id = r1.FirstOrDefault();
            return entity;
        }

        public async Task DeleteAsync(Drink entity)
        {
            using var connection = 
                new SqliteConnection(_dbContext.ConnectionString);

            var q = "DELETE FROM Drinks WHERE Id=@Id;";

    
            var result = await connection.QueryAsync<int>(q,
            new
            {
                @Id = entity.Id
            });
        }

        public async Task<IReadOnlyList<Drink>> GetAllAsync()
        {
            using var connection = new SqliteConnection
                (_dbContext.ConnectionString);

            var q = @$"SELECT
                Id,Status,CreatedAt,UniqueId,ToWho FROM Drinks";

            var r = await connection.QueryAsync<Drink>(q);

            return r.ToList().AsReadOnly();
        }

        public async Task<Drink> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection
                (_dbContext.ConnectionString);

            var q = @$"SELECT
                Id,Status,CreatedAt,UniqueId,ToWho FROM Drinks
                WHERE Id = @Id";

            var r = await connection.
                QueryFirstOrDefaultAsync<Drink>(q,new { @Id = id});

            return r;
        }

        public async Task<Drink> GetByUniqueIdAsync(Guid id)
        {
            using var connection = new SqliteConnection
                (_dbContext.ConnectionString);

            var q = @$"SELECT
                Id,Status,CreatedAt,UniqueId,ToWho FROM Drinks
                WHERE UniqueId = @UId";

            var r = await connection.
                QueryFirstOrDefaultAsync<Drink>
                (q, new { @UId = id.ToString()});

            return r;
        }

        public async Task UpdateAsync(Drink entity)
        {
            using var connection = new SqliteConnection
                 (_dbContext.ConnectionString);

            var q = @"UPDATE Drinks
                SET Status = @Status, ToWho = @ToWho
                WHERE UniqueId = @UniqueId;";

            var r1 = await connection
                .ExecuteAsync(q,
                new 
                { 
                    @Status = entity.Status,
                    @ToWho = entity.ToWho,
                    @UniqueId = entity.UniqueId
                });

            var q1 = @$"SELECT
                Id,DrinkId,IngredientId,CreatedAt 
                FROM IngredientsInGlassDrink WHERE DrinkId = @DId";

            var r = await connection.QueryAsync<IngredientsInGlassDrink>
                (q, new { @DId = entity.Id});

            List<IngredientsInGlassDrink> todelete = 
                new List<IngredientsInGlassDrink>();


            foreach (var ingredient in entity.Ingredients)
            {
                if (ingredient.ToInsert)
                {
                    var q2 = @"INSERT INTO IngredientsInGlassDrink 
                    (DrinkId, IngredientId, CreatedAt)
                    VALUES(@DrinkId, @IngredientId, @CreatedAt);

                    SELECT seq From sqlite_sequence Where Name='IngredientsInGlassDrink'";

                    var r2 = await connection.QueryAsync<int>(q2,
                        new
                        {
                                @DrinkId = entity.Id,
                                @IngredientId = ingredient.Id,
                                @CreatedAt = DateTimeOffset.UtcNow
                        });
                }

                foreach (var dbin in r)
                {
                    if (dbin.IngredientId != ingredient.Id )
                    {
                        todelete.Add(
                            new IngredientsInGlassDrink()
                            {
                                Id = dbin.Id,
                                IngredientId = dbin.IngredientId,
                                DrinkId = entity.Id
                            });
                    }
                }
            }

            foreach (var item in todelete)
            {
                var q3 = "DELETE FROM Drinks WHERE Id=@Id;";

                await connection.ExecuteAsync(q3,
                new
                {
                    @Id = item.Id,
                });

            }         

        }
    }
}
