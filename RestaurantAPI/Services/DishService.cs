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
       
        private readonly IMapper _mapper;
        private readonly EfCoreDishRepository _dishRepository;
        private readonly EfCoreRestaurantRepository _restaurantRepository;

        public DishService(IMapper mapper,EfCoreDishRepository dishRepository,EfCoreRestaurantRepository restaurantRepository)
        {
           
            _mapper = mapper;
            _dishRepository = dishRepository;
            _restaurantRepository = restaurantRepository;
        }
        public async Task<int> CreateDish(int restaurantId, CreateDishDTO dto)
        {
            var restaurant = await _restaurantRepository.Get(restaurantId);
            if (restaurant == null)
                throw new NotFoundExpection($"Restauracja o id:{restaurantId} nie istnieje ");

            var dishEntity = _mapper.Map<Dish>(dto);
            dishEntity.RestaurantId = restaurantId;
            var dishid =_dishRepository.Add(dishEntity);

            return dishid.Id;
        }
        public async Task<DishDTO> GetById(int restaurantId, int dishId)
        {
            var dish = await _dishRepository.Get(dishId) ;

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
        public async Task RemoveAll(int restaurantId)
        {
            var restaurant = await _dishRepository.DeleteAll(restaurantId);
            if (restaurant == null)
                throw new NotFoundExpection($"Restauracja o id:{restaurantId} nie istnieje lub nie posiada dań");

        }
        public async Task RemoveById(int restaurantId, int dishId) //works fine
        {
            var restaurant = await _dishRepository.DeleteById(restaurantId, dishId);
            if (restaurant == null)
                throw new NotFoundExpection($"Restauracja o id:{restaurantId} nie istnieje ");
        }
    }
}
