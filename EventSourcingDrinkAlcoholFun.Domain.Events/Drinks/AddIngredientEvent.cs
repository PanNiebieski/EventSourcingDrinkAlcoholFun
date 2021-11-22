using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;

namespace EventSourcingDrinkAlcoholFun.DomainEvents
{
    public class AddIngredientEvent : DomainEvent
    {
        public Ingredient Ingredient { get; set; }

        public AddIngredientEvent(Ingredient ingredient,
            Guid id,int version) 
            : base()
        {
            Ingredient = ingredient;
            Key_StreamId = AggregateKey.FromGuid(id);
            Version_SerialNumber = version;
        }
    }
}
