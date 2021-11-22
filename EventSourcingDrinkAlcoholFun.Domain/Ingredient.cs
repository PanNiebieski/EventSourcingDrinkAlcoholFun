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
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Volume { get; set; }

        public decimal Price { get; set; }

        public ICollection<Drink> UseInDrinks { get; set; }
    }
}
