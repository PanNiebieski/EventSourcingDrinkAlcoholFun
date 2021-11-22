using EventSourcingDrinkAlcoholFun.Core;
using EventSourcingDrinkAlcoholFun.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure
{
    public static class EventSourcingDrinkAlcoholFunInstaller
    {

        public static IServiceCollection AddEventSourcingDrinkAlcoholFunEFServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DrinkContext>(options =>
                options.UseSqlite(configuration.
                GetConnectionString
                ("EventSourcingDrinkAlcoholFunConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IDrinkRepository, DrinkRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<ITableManager, TableManager>();

            return services;
        }
    }
}
