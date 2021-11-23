using System.ComponentModel.DataAnnotations;

namespace EventSourcingDrinkAlcoholFun.Domain
{



    public enum DrinkStatus
    {
        Creating = 0,
        Done = 1,
    }


    public class Drink : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public DrinkStatus Status { get; set; }

        public string ToWho { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public Drink()
        {
            Ingredients = new List<Ingredient>();
            Status = DrinkStatus.Creating;
            UniqueId = Guid.NewGuid();
        }


        public Drink(Guid uniqueId)
        {
            Ingredients = new List<Ingredient>();
            Status = DrinkStatus.Creating;
            UniqueId = uniqueId;
        }




    }



    //public record Drink([property: Key]int Id, 
    //    DrinkStatus Status,
    //    string ToWho) 
    //    : AuditableEntity(DateTimeOffset.UtcNow)
    //{
    //    public ICollection<Ingredient> Ingredients { get; init; } 
    //        = new List<Ingredient>();
    //    public Guid UniqueId { get; init; } = Guid.NewGuid();
    //};


}