using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;

namespace EventSourcingDrinkAlcoholFun.DomainEvents
{
    public class AddedIngredientEvent : DomainEvent
    {
        public Ingredient Ingredient { get; init; }

        public AddedIngredientEvent(Ingredient ingredient,
            Guid id,int version) 
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
