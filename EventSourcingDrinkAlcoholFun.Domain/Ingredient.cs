using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Domain
{
    public class Ingredient : AuditableEntity
    {

        public Ingredient()
        {

        }
        public Ingredient(int v1, string v2, int v3, decimal v4)
        {
            Id = v1;
            Name = v2;
            Volume = v3;
            Price = v4;
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Volume { get; set; }

        public decimal Price { get; set; }

 
        [JsonIgnore]
        public ICollection<Drink> UseInDrinks { get; set; }
    }


    //public record Ingredient
    //    ([property:Key]int Id, string Name, int Volume,
    //    decimal Price) : AuditableEntity(DateTimeOffset.UtcNow)
    //{
    //    public ICollection<Drink> UseInDrinks { get; init; }
    //        = new List<Drink>();
    //}
}
