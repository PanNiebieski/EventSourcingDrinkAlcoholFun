using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;
using EventSourcingDrinkAlcoholFun.DomainEvents;
using EventSourcingDrinkAlcoholFun.DomainEvents.Drinks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    public static class EventHelper
    {
        public static Drink When(this Drink? drink, DomainEvent @event)
        {
            return @event switch
            {
                EmptyGlassInitializedEvent (var uniqueId) =>
                    new Drink(uniqueId),

                AddedIngredientEvent (_,var ingredient) =>
                    drink!.AddIngredient(ingredient),

                RemovedIngredientEvent (_, var ingredient) =>
                    drink!.RemoveIngredient(ingredient),

                ToWhoChangedEvent(_, var towho) =>
                    drink!.ChangeToWho(towho),

                GlassDoneEvent => drink.GlassDone(),
            };
        }
    }
}