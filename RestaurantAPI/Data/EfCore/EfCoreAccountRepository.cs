using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Data.EfCore
{
    public class EfCoreAccountRepository : EfCoreRepository<User, RestaurantDbContext>
    {
        public EfCoreAccountRepository(RestaurantDbContext context) : base(context)
        {
        }
        public async Task RegisterUser(User dto)
        {
            context.Set<User>().Add(dto);
            await context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmail(LoginDTO dto)
        {
            return await context.Users
                .Include(x=>x.Role)
                .FirstOrDefaultAsync(x => x.Email == dto.Email);
        }
    }
}
