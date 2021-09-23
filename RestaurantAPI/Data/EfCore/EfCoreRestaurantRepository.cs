using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Migrations.Data;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RestaurantAPI.Data.EfCore
{
    public class EfCoreRestaurantRepository : EfCoreRepository<Restaurant, RestaurantDbContext>
    {
        public EfCoreRestaurantRepository(RestaurantDbContext context): base(context)
        {

        }

        public override async Task<Restaurant> Delete(int id)
        {

            var restaurant = await context.Set<Restaurant>().FindAsync(id);

            if (restaurant == null)
                return restaurant;

            context.Set<Restaurant>().Remove(restaurant);
            await context.SaveChangesAsync();

            return restaurant;
        }

        public override async Task<Restaurant> Get(int id)
        {
            
            return await context.Restaurants.Include(x => x.Address).Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<List<Restaurant>> GetAll()
        {
            return await context.Restaurants
               .Include(x => x.Address)
               .Include(x => x.Dishes)
               .ToListAsync();
        }
    }
}
