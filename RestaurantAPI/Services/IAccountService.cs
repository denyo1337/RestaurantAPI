using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDTO dto);
        string GenerateJwt(LoginDTO dto);
    }
}
