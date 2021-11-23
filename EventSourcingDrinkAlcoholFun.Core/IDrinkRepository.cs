using EventSourcingDrinkAlcoholFun.Domain;

namespace EventSourcingDrinkAlcoholFun.Core
{
    public interface IDrinkRepository : IAsyncRepository<Drink>
    {
        Task<Drink> GetByUniqueIdAsync(Guid id);
    }
}