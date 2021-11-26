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

            var r1 = await connection
                .QueryAsync<int>(q, entity);

            entity.Id = r1.FirstOrDefault();

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
                d.Id,d.Status,d.CreatedAt,d.UniqueId,d.ToWho
                FROM Drinks as d";

            var r = await connection.QueryAsync<Drink>(q);

            var q2 = @$"SELECT
                A.Id, A.DrinkId, A.IngredientId, A.CreatedAt,
                I.Id,
                I.Name ,
                I.Volume ,
                I.Price,
                I.CreatedAt ,
                d.Id,
                d.Status AS 'Drink.Status' ,
                d.CreatedAt AS 'Drink.CreatedA',
                d.UniqueId AS 'Drink.UniqueId',
                d.ToWho AS 'Drink.ToWho'
                FROM IngredientsInGlassDrink as A
                INNER JOIN Ingredients as I ON I.Id = A.IngredientId
                INNER JOIN Drinks as d ON d.Id = A.DrinkId";

            var r2 = await connection.QueryAsync<IngredientsInGlassDrink
                , Ingredient, Drink, IngredientsInGlassDrink>(q2, (iifd, i, d) =>
                  {
                      iifd.Ingredient = i;
                      iifd.Drink = d;

                      return iifd;
                  });

            var ingredientsInGlassDrinks = r2.ToList();
            var drinks = r.ToList();

            foreach (var drink in drinks)
            {
                foreach (var iigd in ingredientsInGlassDrinks)
                {
                    if (drink.Id == iigd.DrinkId)
                    {
                        drink.Ingredients.Add(iigd.Ingredient);
                    }
                }
            }

            return drinks.AsReadOnly();


            //var q = @$"SELECT
            //    d.Id,d.Status,d.CreatedAt,d.UniqueId,d.ToWho, 
            //    C.*,I.* 
            //    FROM Drinks as d
            //    INNER JOIN IngredientsInGlassDrink as C ON d.Id = C.DrinkId
            //    INNER JOIN Ingredients as I ON I.Id = C.IngredientId";

            //var r = await connection.QueryAsync<Drink,
            //    IngredientsInGlassDrink, Ingredient, Drink>(q,
            //    (d, iifd, i) =>
            //    {

            //        return d;
            //    }
            //);
        }

        public async Task<Drink> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection
                (_dbContext.ConnectionString);

            var q = @$"SELECT
                d.*
                FROM Drinks as d
                WHERE Id = @Id";

            var r = await connection.QueryFirstOrDefaultAsync
                <Drink>
                (q, new { @Id = id });

            if (r == null)
                return r;

            var ina = await GetIngredientsInGlassDrink(connection);

            foreach (var item in ina)
            {
                if (item.DrinkId == r.Id)
                {
                    r.Ingredients.Add(item.Ingredient);
                }
            }

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
                (q, new { @UId = id.ToString() });

            if (r == null)
                return r;

            var ina = await GetIngredientsInGlassDrink(connection);

            foreach (var item in ina)
            {
                if (item.DrinkId == r.Id)
                {
                    r.Ingredients.Add(item.Ingredient);
                }
            }

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


            }


            foreach (var item in DeleteFlags.Get())
            {
                var q3 = "DELETE FROM IngredientsInGlassDrink WHERE DrinkId=@DId AND IngredientId= @IId ;";

                if (item.drinkid == entity.Id)
                {
                    await connection.ExecuteAsync(q3,
                    new
                    {
                        @DId = item.drinkid,
                        @IId = item.ingredientid
                    });
                    DeleteFlags.Clear(entity.Id);
                }
            }
   
        }

        //var r = await GetIngredientsInGlassDrink(connection);

        //List<IngredientsInGlassDrink> todelete =
        //    new List<IngredientsInGlassDrink>();

        //foreach (var dbin in r)
        //{
        //    if (dbin.DrinkId == entity.Id)
        //    {
        //        entity.Ingredients.Remove(dbin.Ingredient);
        //    }

        //}


        //var t = 0;

        //foreach (var ingredient in entity.Ingredients)
        //{
        //    foreach (var dbin in r)
        //    {
        //        if (dbin.IngredientId != ingredient.Id && !ingredient.ToInsert)
        //        {
        //            todelete.Add(
        //                new IngredientsInGlassDrink()
        //                {
        //                    Id = dbin.Id,
        //                    IngredientId = dbin.IngredientId,
        //                    DrinkId = entity.Id
        //                });

        //            var q3 = "DELETE FROM IngredientsInGlassDrink WHERE Id=@Id;";

        //            await connection.ExecuteAsync(q3,
        //            new
        //            {
        //                @Id = dbin.Id,
        //            });
        //        }
        //    }
        //}


        private async Task<List<IngredientsInGlassDrink>> GetIngredientsInGlassDrink(SqliteConnection connection)
        {
            var q2 = @$"SELECT
                A.Id, A.DrinkId, A.IngredientId, A.CreatedAt,
                I.Id,
                I.Name ,
                I.Volume ,
                I.Price,
                I.CreatedAt ,
                d.Id,
                d.Status AS 'Drink.Status' ,
                d.CreatedAt AS 'Drink.CreatedA',
                d.UniqueId AS 'Drink.UniqueId',
                d.ToWho AS 'Drink.ToWho'
                FROM IngredientsInGlassDrink as A
                INNER JOIN Ingredients as I ON I.Id = A.IngredientId
                INNER JOIN Drinks as d ON d.Id = A.DrinkId";

            var r2 = await connection.QueryAsync<IngredientsInGlassDrink
                , Ingredient, Drink, IngredientsInGlassDrink>(q2, (iifd, i, d) =>
                {
                    iifd.Ingredient = i;
                    iifd.Drink = d;

                    return iifd;
                });

            var ingredientsInGlassDrinks = r2.ToList();

            return ingredientsInGlassDrinks;
        }


    }




    //public class TempDrink
    //{
    //    public int Id { get; set; }

    //    public string UniqueId { get; set; }

    //    public int Status { get; set; }

    //    public string ToWho { get; set; }

    //    public string CreatedAt { get; set; }
    //}
}
