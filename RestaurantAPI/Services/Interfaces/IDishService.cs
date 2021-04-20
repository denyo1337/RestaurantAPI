using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int CreateDish(int restaurantId, CreateDishDTO dto);
        DishDTO GetById(int restaurantId, int dishId);
        List<DishDTO> GetAll(int restaurantId);
        void RemoveAll(int restaurantId);
        void RemoveById(int restaurantId, int dishId);
    }
}
