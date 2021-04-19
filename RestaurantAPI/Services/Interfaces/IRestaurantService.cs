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
        int Create(CreateRestaurantDTO dto);
        void Delete(int id);
        void Update(int id, UpdateRestaurantDTO dto);


    }
}
