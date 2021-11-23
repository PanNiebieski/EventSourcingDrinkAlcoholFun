namespace EventSourcingDrinkAlcoholFun.Domain
{
    public static class HelperDrink
    {
        public static Drink AddIngredient
            (this Drink drink, Ingredient ingredient)
        {
            drink.Ingredients.Add(ingredient);
            return drink;
        }

        public static Drink RemoveIngredient
            (this Drink drink, Ingredient ingredient)
        {
            drink.Ingredients.Remove(ingredient);
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
    }




}