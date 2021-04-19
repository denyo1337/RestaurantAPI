using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        Task<RestaurantDTO> GetById(int id);
        PageResult<RestaurantDTO> GetAll(RestaurantQuery query);
        Task<int> CreateAsync(CreateRestaurantDTO dto);
        Task<Restaurant> Delete(int id);
        Task UpdateAsync(int id, UpdateRestaurantDTO dto);


    }
}
