using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;
using EventSourcingDrinkAlcoholFun.DomainEvents;
using EventSourcingDrinkAlcoholFun.DomainEvents.Drinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    public class DrinkAggregate : AggregateRoot
    {
        public Drink Drink { get; set; }

        protected override void ApplyChange(DomainEvent @event)
        {
            Drink = Drink.When(@event);
        }

        public void RemovedIngredient(Guid id,Ingredient ingredient)
        {
            ApplyChangeBase(new RemovedIngredientEvent
                (ingredient, id,Version_SerialNumber), true);
        }


        public void AddedIngredient(Guid id, Ingredient ingredient)
        {
            ApplyChangeBase(new AddedIngredientEvent
                (ingredient, id, Version_SerialNumber), true);
        }

        public void ToWhoChanged(Guid id, string after,string before)
        {
            ApplyChangeBase(new ToWhoChangedEvent
                (after, before, id, Version_SerialNumber), true);
        }

        public void GlassDone(Guid id)
        {
            ApplyChangeBase(new GlassDoneEvent(id, Version_SerialNumber), true);
        }

        public DrinkAggregate(Drink cc)
        {
            var c = new EmptyGlassInitializedEvent(cc.UniqueId);
            Drink = cc;
            ApplyChangeBase(c,true);
        }

        public DrinkAggregate()
        {

        }
    }
}
