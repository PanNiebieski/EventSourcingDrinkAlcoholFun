using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.DomainEvents.Drinks
{
    public class RemovedIngredientEvent : DomainEvent
    {
        public Ingredient Ingredient { get; set; }

        public RemovedIngredientEvent(Ingredient ingredient,
            Guid id, int version)
            : base()
        {
            Ingredient = ingredient;
            Key_StreamId = AggregateKey.FromGuid(id);
            Version_SerialNumber = version;
        }

        public void Deconstruct(out Guid uniqueId,
            out Ingredient ingredient)
        {
            uniqueId = Key_StreamId.Id;
            ingredient = Ingredient;
        }
    }
}
