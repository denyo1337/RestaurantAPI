using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Data.EfCore;
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
        private readonly EfCoreDishRepository _dishRepository;
        private readonly EfCoreRestaurantRepository _restaurantRepository;

        public DishService(RestaurantDbContext dbContext, IMapper mapper,EfCoreDishRepository dishRepository,EfCoreRestaurantRepository restaurantRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _dishRepository = dishRepository;
            _restaurantRepository = restaurantRepository;
        }
        public int CreateDish(int restaurantId, CreateDishDTO dto)
        {
            var restaurant = _restaurantRepository.Get(restaurantId).Result;
            if (restaurant == null)
                throw new NotFoundExpection($"Restauracja o id:{restaurantId} nie istnieje ");


                var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;

             var dishid =_dishRepository.Add(dishEntity);

            return dishid.Id;

        }
        public DishDTO GetById(int restaurantId, int dishId)
        {
           
            var dish = _dishRepository.Get(dishId).Result ;

            if(dish==null || dish.Restaurant.Id !=restaurantId)
                throw new NotFoundExpection("Dish not found or restaurantId is not valid");  
            
            
            var dishDTO = _mapper.Map<DishDTO>(dish);
            
            return dishDTO;
        }
        
        public async Task<List<DishDTO>> GetAll(int restaurantId)
        {
            var dishes = await _dishRepository.GetAllDishesFromRestaurant(restaurantId);
          

            var dishDtos = _mapper.Map<List<DishDTO>>(dishes);

            if (dishDtos.Count <= 0)
                throw new NotFoundExpection("Pusta lista");

            return dishDtos;
        }
        public void RemoveAll(int restaurantId)
        {
            var restaurant = _dishRepository.DeleteAll(restaurantId).Result;
            if (restaurant == null)
                throw new NotFoundExpection($"Restauracja o id:{restaurantId} nie istnieje ");


        }
        public void RemoveById(int restaurantId, int dishId) //works fine
        {
            var restaurant = _dishRepository.DeleteById(restaurantId, dishId).Result;
            if (restaurant == null)
                throw new NotFoundExpection($"Restauracja o id:{restaurantId} nie istnieje ");

        }
        /*private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant == null) 
                throw new NotFoundExpection("Restaurant not found");
            
            return restaurant;
        }
        */
    }
}
