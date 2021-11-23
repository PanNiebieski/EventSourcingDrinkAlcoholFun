using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;
using EventSourcingDrinkAlcoholFun.DomainEvents;
using EventSourcingDrinkAlcoholFun.DomainEvents.Drinks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    public static class EventHExtensionMethods
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

                ToWhoChangedEvent(_, var towhoAftER,_) =>
                    drink!.ChangeToWho(towhoAftER),

                GlassDoneEvent => drink.GlassDone(),

                ReverseEvent (_,var events) => drink!
                .ReversEvents(events),
            };
        }

        private static Drink ReversEvents(this Drink drink, List<DomainEvent> events)
        {
            foreach (var e in events)
            {
                drink.DidNotWhen(e);
            }

            return drink;
        }

        private static Drink When(this Drink drink, List<DomainEvent> events)
        {
            foreach (var e in events)
            {
                drink.When(e);
            }

            return drink;
        }

        public static Drink DidNotWhen(this Drink? drink, DomainEvent @event)
        {
            return @event switch
            {
                EmptyGlassInitializedEvent(var uniqueId) =>
                    null,

                AddedIngredientEvent(_, var ingredient) =>
                    drink!.RemoveIngredient(ingredient),

                RemovedIngredientEvent(_, var ingredient) =>
                    drink!.AddIngredient(ingredient),

                ToWhoChangedEvent(_, _, var towhoBefore) =>
                    drink!.ChangeToWho(towhoBefore),

                GlassDoneEvent => drink.GlassDone(),
                ReverseEvent(_, var events) => drink!.When(events),
            };

        }
    }
}