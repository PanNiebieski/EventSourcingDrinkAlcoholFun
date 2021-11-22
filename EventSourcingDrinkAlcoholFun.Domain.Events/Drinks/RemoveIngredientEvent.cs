using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.DomainEvents.Drinks
{
    public class RemoveIngredientEvent : DomainEvent
    {
        public Ingredient Ingredient { get; set; }

        public RemoveIngredientEvent(Ingredient ingredient,
            Guid id, int version)
            : base()
        {
            Ingredient = ingredient;
            Key_StreamId = AggregateKey.FromGuid(id);
            Version_SerialNumber = version;
        }
    }
}
