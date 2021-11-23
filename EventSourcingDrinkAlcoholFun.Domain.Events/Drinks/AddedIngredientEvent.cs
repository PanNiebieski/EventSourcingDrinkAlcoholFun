using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Domain.Events;

namespace EventSourcingDrinkAlcoholFun.DomainEvents
{
    public class AddedIngredientEvent : DomainEvent
    {
        public override int Type => 1;
        public new AddedIngredientEventData Data { get; init; } 
              = new AddedIngredientEventData();

        public AddedIngredientEvent(Ingredient ingredient,
            Guid id,int version) 
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

    public class AddedIngredientEventData : DomainEventData
    {
        public Ingredient Ingredient { get; set; }
    }
}
