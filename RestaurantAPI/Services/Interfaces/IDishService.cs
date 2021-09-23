using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        Task<int> CreateDish(int restaurantId, CreateDishDTO dto);
        Task<DishDTO> GetById(int restaurantId, int dishId);
        Task<List<DishDTO>> GetAll(int restaurantId);
        Task RemoveAll(int restaurantId);
        Task RemoveById(int restaurantId, int dishId);
    }
}
