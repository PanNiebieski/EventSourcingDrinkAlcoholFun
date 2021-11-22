using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Domain
{
    public class IngredientsInGlassDrink : AuditableEntity
    {
        public int Id { get; set; }

        public int DrinkId { get; set; }

        public int IngredientId { get; set; }

        public Drink Drink { get; private set; }

        public Ingredient Ingredient { get; private set; }
    }
}
