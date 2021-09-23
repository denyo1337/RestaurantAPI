using RestaurantAPI.Models;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDTO dto);
        Task<string> GenerateJwt(LoginDTO dto);
    }
}
