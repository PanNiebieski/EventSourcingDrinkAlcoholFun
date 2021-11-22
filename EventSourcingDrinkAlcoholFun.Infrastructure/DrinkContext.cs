using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Infrastructure.DummyData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingDrinkAlcoholFun.Infrastructure
{
    public class DrinkContext : DbContext
    {
        public DrinkContext(DbContextOptions<DrinkContext> options)
    : base(options)
        {
            this.Database.EnsureCreated();
            
        }

        public DbSet<Drink> Drinks { get; set; }

        public DbSet<Ingredient> Ingredient { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        //entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        
            modelBuilder.
                ApplyConfigurationsFromAssembly
                (typeof(DrinkContext).Assembly);

            modelBuilder.Entity<Drink>().ToTable("Drinks");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients");

            modelBuilder.Entity<Drink>()
            .HasMany<Ingredient>(s => s.Ingredients)
            .WithMany(c => c.UseInDrinks)
            .UsingEntity<IngredientsInGlassDrink>(
                    x => x.HasOne(x => x.Ingredient)
                    .WithMany().HasForeignKey(x => x.IngredientId),
                    x => x.HasOne(x => x.Drink)
                   .WithMany().HasForeignKey(x => x.DrinkId)
            .OnDelete(DeleteBehavior.Cascade));

            foreach (var item in DummyIngredients.Get())
            {
                modelBuilder.Entity<Ingredient>().HasData(item);
            }

          
        }
    }
}
