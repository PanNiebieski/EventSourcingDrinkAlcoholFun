using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Domain
{
    //public record IngredientsInGlassDrink
    //    (int Id, int DrinkId, int IngredientId,
    //    Drink Drink, Ingredient Ingredient) :
    //    AuditableEntity(DateTimeOffset.UtcNow);


    public class IngredientsInGlassDrink : AuditableEntity
    {
        public int Id { get; set; }

        public int DrinkId { get; set; }

        public int IngredientId { get; set; }

        public Drink Drink { get;  set; }

        public Ingredient Ingredient { get;  set; }
    }
}
