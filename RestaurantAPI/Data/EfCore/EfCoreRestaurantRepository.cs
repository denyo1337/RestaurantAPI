using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Data.EfCore
{
    public class EfCoreRestaurantRepository : EfCoreRepository<Restaurant, RestaurantDbContext>
    {
        public EfCoreRestaurantRepository(RestaurantDbContext context): base(context)
        {

        }

        public override async Task<Restaurant> Get(int id)
        {
            
            return await context.Restaurants.Include(x => x.Address).Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
