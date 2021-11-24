using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Domain
{
    public class Ingredient : AuditableEntity,
        IEquatable<Ingredient>
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
        [NotMapped]
        public bool ToInsert { get; set; }




        [JsonIgnore]
        public ICollection<Drink> UseInDrinks { get; set; }

        public bool Equals(Ingredient? other)
        {
            Ingredient mod = other as Ingredient;

            if (mod != null)
            {
                return Id == mod.Id;
            }
            return base.Equals(other);
        }
    }


    //public record Ingredient
    //    ([property:Key]int Id, string Name, int Volume,
    //    decimal Price) : AuditableEntity(DateTimeOffset.UtcNow)
    //{
    //    public ICollection<Drink> UseInDrinks { get; init; }
    //        = new List<Drink>();
    //}
}
