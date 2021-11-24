namespace EventSourcingDrinkAlcoholFun.Infrastructure.DataAccess.Dapper.Repositories
{
    public class TempDrink
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public int Status { get; set; }

        public string ToWho { get; set; }

        public string CreatedAt { get; set; }
    }


}
