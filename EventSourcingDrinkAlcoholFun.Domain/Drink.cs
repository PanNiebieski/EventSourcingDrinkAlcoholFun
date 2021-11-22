using System.ComponentModel.DataAnnotations;

namespace EventSourcingDrinkAlcoholFun.Domain
{

    public class Drink : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public DrinkStatus Status { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public Drink()
        {
            Ingredients = new List<Ingredient>();
            Status = DrinkStatus.Creating;
        }
    }

    public enum DrinkStatus
    {
        Creating = 0,
        Done = 1,
    }
}