namespace EventSourcingDrinkAlcoholFun.Domain
{
    public static class HelperDrink
    {
        public static Drink AddIngredient
            (this Drink drink, Ingredient ingredient)
        {
            ingredient.ToInsert = true;
            drink.Ingredients.Add(ingredient);
            return drink;
        }

        public static Drink RemoveIngredient
            (this Drink drink, Ingredient ingredient)
        {
            drink.Ingredients.Remove(ingredient);
            DeleteFlags.AddDeleteFlag(drink.Id, ingredient.Id);
            return drink;
        }

        public static Drink ChangeToWho(this Drink drink,
            string towho)
        {
            drink.ToWho = towho;
            return drink;
        }

        public static Drink GlassDone
            (this Drink drink)
        {
            drink.Status = DrinkStatus.Done;
            return drink;
        }

        public static Drink GlassUnDone
    (this Drink drink)
        {
            drink.Status = DrinkStatus.Creating;
            return drink;
        }
    }




}