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

            Ingredient i1 = new Ingredient()
            {
                Id = 1,
                Name = "Vodka",
                Volume = 40,
                Price = 5.2m,
            };

            Ingredient i2 = new Ingredient()
            {
                Id = 2,
                Name = "Sprite",
                Volume = 300,
                Price = 5.2m,
            };

            Ingredient i3 = new Ingredient()
            {
                Id = 3,
                Name = "Coca-Cola",
                Volume = 300,
                Price = 6.0m,
            };

            Ingredient i4 = new Ingredient()
            {
                Id = 4,
                Name = "Kahlua Coffee Liqueur",
                Volume = 20,
                Price = 2.1m,
            };

            Ingredient i5 = new Ingredient()
            {
                Id = 5,
                Name = "Ice Cubes",
                Volume = 30,
                Price = 0.1m,
            };

            Ingredient i6 = new Ingredient()
            {
                Id = 6,
                Name = "Orange Juice",
                Volume = 80,
                Price = 1.0m,
            };

            Ingredient i7 = new Ingredient()
            {
                Id = 7,
                Name = "Cream",
                Volume = 20,
                Price = 0.5m,
            };

            Ingredient i8 = new Ingredient()
            {
                Id = 8,
                Name = "Peach Liqueur",
                Volume = 40,
                Price = 3.8m,
            };

            Ingredient i9 = new Ingredient()
            {
                Id = 9,
                Name = "Cranberry Juice",
                Volume = 80,
                Price = 1.8m,
            };


            Ingredient i10 = new Ingredient()
            {
                Id = 10,
                Name = "Blue Curacao Liqueur",
                Volume = 40,
                Price = 3.8m,
            };

            Ingredient i11 = new Ingredient()
            {
                Id = 11,
                Name = "Lemon Juice",
                Volume = 20,
                Price = 0.6m,
            };

            p.Add(i1);
            p.Add(i2); p.Add(i3);
            p.Add(i4); p.Add(i5);
            p.Add(i6); p.Add(i7);
            p.Add(i8); p.Add(i9);
            p.Add(i10); p.Add(i11);

            foreach (var item in p)
            {
                item.CreatedAt = DateTime.Now;
            }

            return p;

        }
    }
}
