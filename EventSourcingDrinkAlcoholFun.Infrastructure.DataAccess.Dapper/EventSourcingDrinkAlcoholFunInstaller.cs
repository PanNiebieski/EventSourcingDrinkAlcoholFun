using Dapper;
using EventSourcingDrinkAlcoholFun.Core;
using EventSourcingDrinkAlcoholFun.Infrastructure.DataAccess.Dapper.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.DataAccess.Dapper
{
    public static class EventSourcingDrinkAlcoholFunInstaller
    {
        public static IServiceCollection AddDapperServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connection = configuration.
                GetConnectionString("EventSourcingDrinkAlcoholFunConnectionString");

            services.AddScoped<IDBContext, DBContext>
                (
                    (services) =>
                    {
                        var c =
                        new DBContext(connection);
                        return c;
                    }
                );


            services.AddScoped<IDrinkRepository, DrinkRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<ITableManager, TableManager>();

            SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
            SqlMapper.AddTypeHandler(DateTimeHandler.Default);

            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            return services;
        }
    }
}
