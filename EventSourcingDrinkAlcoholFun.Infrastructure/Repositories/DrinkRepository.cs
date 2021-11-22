using EventSourcingDrinkAlcoholFun.Core;
using EventSourcingDrinkAlcoholFun.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure
{
    public class DrinkRepository :
        BaseRepository<Drink>, IDrinkRepository
    {
        public DrinkRepository(DrinkContext dbContext) 
            : base(dbContext)
        {
        }


        public new async Task<IReadOnlyList<Drink>> GetAllAsync()
        {
            return await _dbContext.Set<Drink>()
                .Include(x => x.Ingredients).ToListAsync();
        }

        public new async Task<Drink> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Drink>().FindAsync(id);
        }
    }
}
