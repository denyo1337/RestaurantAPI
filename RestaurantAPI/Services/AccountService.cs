using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Entities;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Expections;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using RestaurantAPI.Data;
using RestaurantAPI.Data.EfCore;

namespace RestaurantAPI.Services
{
    public class AccountService:IAccountService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly EfCoreAccountRepository _efCoreAccountRepository;
        
        public AccountService(IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, EfCoreAccountRepository efCoreAccountRepository)
        {
           
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _efCoreAccountRepository = efCoreAccountRepository;
        }

        public async Task RegisterUser(RegisterUserDTO dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            await _efCoreAccountRepository.RegisterUser(newUser);
        }
        public async Task<string> GenerateJwt(LoginDTO dto)
        {
            var user = await _efCoreAccountRepository.GetUserByEmail(dto);

            if (user == null)
                throw new NotFoundExpection("Invalid user name or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if(result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid user name or password");
            

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Email}"),
                new Claim(ClaimTypes.Role,$"{user.Role.Name}"),
            };

            if(!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(new Claim("Nationality", user.Nationality));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();



            return tokenHandler.WriteToken(token);

        }
    }
}
