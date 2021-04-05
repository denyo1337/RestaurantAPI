using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public class DishService:IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDTO dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dishEntity);
            _dbContext.SaveChanges();

            return dishEntity.Id;

        }
        public DishDTO GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            

            var dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == dishId);
            if(dish==null || dish.RestaurantId !=restaurantId)
            {
                throw new NotFoundExpection("Dish not found");
            }

            var dishDTO = _mapper.Map<DishDTO>(dish);

            return dishDTO;
        }
        
        public List<DishDTO> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);


            var dishDtos = _mapper.Map<List<DishDTO>>(restaurant.Dishes);

            return dishDtos;
        }
        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _dbContext.RemoveRange(restaurant.Dishes);
            _dbContext.SaveChanges();

        }
        public void RemoveById(int restaurantId, int dishId) //works fine
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(x => x.Id == dishId);
            
            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();


        }
        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant == null) 
                throw new NotFoundExpection("Restaurant not found");
            
            return restaurant;
        }
    }
}
