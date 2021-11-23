using EventSourcingDrinkAlcoholFun.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.DummyData
{
    public class DummyIngredients
    {
        public static List<Ingredient> Get()
        {
            List<Ingredient> p = new List<Ingredient>();

            Ingredient i1 = new 
                (1, "Vodka",
                40, 5.2m);

            Ingredient i2 = new 
                (2, "Sprite",
                300, 5.2m);

            Ingredient i3 =
                new 
                (3, "Coca-Cola",
                300, 6.0m);

            Ingredient i4 = new 
                (4, "Kahlua Coffee Liqueur",
                20, 2.1m);

            Ingredient i5 =
                new 
                (5, "Ice Cubes",
                30, 0.1m);

            Ingredient i6 = new 
                (6, "Orange Juice",
                80, 1.0m);

            Ingredient i7 = new 
                (7, "Cream", 20,
                0.5m);


            Ingredient i8 = new 
                (8, "Peach Liqueur", 
                40, 3.8m);

            Ingredient i9 = new 
                (9, "Cranberry Juice", 
                80, 1.8m);

            Ingredient i10 = new 
                (10, "Blue Curacao Liqueur",
                40, 3.8m);

            Ingredient i11 = new
                Ingredient(11, "Lemon Juice",
                20, 0.6m);


            p.Add(i1);
            p.Add(i2); p.Add(i3);
            p.Add(i4); p.Add(i5);
            p.Add(i6); p.Add(i7);
            p.Add(i8); p.Add(i9);
            p.Add(i10); p.Add(i11);

            return p;

        }
    }
}
