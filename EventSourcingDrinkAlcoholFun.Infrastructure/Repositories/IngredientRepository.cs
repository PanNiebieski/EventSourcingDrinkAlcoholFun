using EventSourcingDrinkAlcoholFun.Core;
using EventSourcingDrinkAlcoholFun.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure.Repositories
{
    public  class IngredientRepository : IIngredientRepository
    {

        protected readonly DrinkContext _dbContext;


        public IngredientRepository(DrinkContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IReadOnlyList<Ingredient>> GetAllAsync()
        {
            return await _dbContext.Set<Ingredient>().ToListAsync();
        }

        public async Task<Ingredient> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Ingredient>().FindAsync(id);
        }
    }
}
