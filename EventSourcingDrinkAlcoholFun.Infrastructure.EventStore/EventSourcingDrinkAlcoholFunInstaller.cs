using EventSourcingDrinkAlcoholFun.Core.EventSourcing.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.EventStore
{
    public static class EventSourcingDrinkAlcoholFunInstaller
    {

        public static IServiceCollection AddEventSourcing
            (this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.
                GetConnectionString("EventStoreSQLiteConnectionString");

            services.AddScoped<IEventStoreSQLiteContext, EventStoreSQLiteContext>
                (
                    (services) =>
                    {
                        var c =
                        new EventStoreSQLiteContext(connection);
                        return c;
                    }
                );

            services.AddScoped<IEventStore, SqlLiteEventStore>();
            services.AddScoped<IEventRepository, EventRepository>();


            return services;
        }
    }
}
