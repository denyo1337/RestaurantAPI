﻿using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Data.EfCore
{
    public class EfCoreDishRepository : EfCoreRepository<Dish,RestaurantDbContext>
    {
        public EfCoreDishRepository(RestaurantDbContext context): base(context)
        {

        }

        public override Task<Dish> Add(Dish entity)
        {
            return base.Add(entity);
        }

        public override async Task<Dish> Delete(int id)
        {
            var entity = await context.Dishes.Include(x=>x.Restaurant).FirstOrDefaultAsync(x=>x.RestaurantId ==id);

            if (entity == null)
            {
                return entity;
            }

            context.Dishes.RemoveRange(entity);

            await context.SaveChangesAsync();

            return entity;
        }

        public override async Task<List<Dish>> DeleteAll(int id)
        {

            var dishesOfRestaurant = await context.Dishes.ToListAsync();

            var dishes = dishesOfRestaurant.FindAll(x => x.RestaurantId == id);

            context.Dishes.RemoveRange(dishes);

            await context.SaveChangesAsync();

            return dishes;

        }

        public override async Task<Dish> Get(int id)
        {
            var result =  await context.Dishes.Include(x=>x.Restaurant).FirstOrDefaultAsync(x => x.Id == id);
            
            return result;
        }

        public override async Task<List<Dish>> GetAll()
        {
            var result = await context.Dishes.Include(x => x.Restaurant).ToListAsync();

            return result;
        }

        public override Task<Dish> Update(Dish entity)
        {
            return base.Update(entity);
        }
    }
}
