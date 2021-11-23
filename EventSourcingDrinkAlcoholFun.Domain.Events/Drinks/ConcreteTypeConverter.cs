using EventSourcingDrinkAlcoholFun.Domain.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventSourcingDrinkAlcoholFun.DomainEvents.Drinks
{
    public class ConcreteTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, 
            Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jArray = JArray.Load(reader);
            List<DomainEvent> domainEvents = new List<DomainEvent>();

            foreach (var item in jArray)
            {

                var type = ((int)item["Type"]);

                DomainEvent d = null;
                switch (type)
                {
                    case 0:
                        d = item.ToObject<EmptyGlassInitializedEvent>();
                        break;
                    case 1:
                        d = item.ToObject<AddedIngredientEvent>();
                        break;
                    case 3:
                        d = item.ToObject<RemovedIngredientEvent>();
                        break;
                    case 5:
                        d = item.ToObject<ReverseEvent>();
                        break;
                    case 2:
                        d = item.ToObject<ToWhoChangedEvent>();
                        break;
                    case 4:
                        d = item.ToObject<GlassDoneEvent>();
                        break;
                }

                domainEvents.Add(d);
            }

            return domainEvents;
        }

        public override void WriteJson(JsonWriter writer,
            object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}