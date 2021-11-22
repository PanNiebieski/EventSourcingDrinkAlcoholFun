using EventSourcingDrinkAlcoholFun.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Core
{
    public interface IIngredientRepository
    {
        Task<IReadOnlyList<Ingredient>> GetAllAsync();

        Task<Ingredient> GetByIdAsync(int id);

    }
}
