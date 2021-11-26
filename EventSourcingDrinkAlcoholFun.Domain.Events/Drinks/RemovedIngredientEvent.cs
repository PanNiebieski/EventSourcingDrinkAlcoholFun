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
        public override int Type => 1;
        public new RemovedIngredientEventData Data { get; init; } 
            = new RemovedIngredientEventData();

        public RemovedIngredientEvent(Ingredient ingredient,
            Guid id, int version)
            : base()
        {
            Data.Ingredient = ingredient;
            Key_StreamId = AggregateKey.FromGuid(id);
            Version_SerialNumber = version;
        }

        public void Deconstruct(out Guid uniqueId,
            out Ingredient ingredient)
        {
            uniqueId = Key_StreamId.Id;
            ingredient = Data.Ingredient;
        }
    }

    //TODO: Change to IngredientId
    public class RemovedIngredientEventData : DomainEventData
    {
        public Ingredient Ingredient { get; set; }

        //public int IngredientId { get; set; }
    }
}
