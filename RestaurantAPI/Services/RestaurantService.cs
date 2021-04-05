﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Expections;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace RestaurantAPI.Services
{
    public class RestaurantService:IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext context, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService,IUserContextService userContextService)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public RestaurantDTO GetById(int id)
        {
            
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .FirstOrDefault(x => x.Id == id);
            if (restaurant == null)
                throw new NotFoundExpection("Restaurant not found");

            var result = _mapper.Map<RestaurantDTO>(restaurant);
            return result;
        }
        public IEnumerable<RestaurantDTO> GetAll()
        {
            var restaurants = _dbContext.Restaurants
               .Include(x => x.Address)
               .Include(x => x.Dishes)
               .ToList();

            var restaurantDtos = _mapper.Map<List<RestaurantDTO>>(restaurants);
            return restaurantDtos;
        }
        public int Create(CreateRestaurantDTO dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);

            restaurant.CreatedById = _userContextService.GetUserId;

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }
        public void Delete(int id)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE action invoked ! Done by {_userContextService.User}");
            _logger.LogError($"Restaurant with id: {id} DELETE action invoked but failed! Done by {_userContextService.User}");

            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (restaurant == null)
                throw new NotFoundExpection("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequiremt(ResourceOperation.Delete)).Result;


            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
            

        }

        public void Update(int id, UpdateRestaurantDTO dto)
        {
            
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);
            if (restaurant == null)
                throw new NotFoundExpection("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User,restaurant,new ResourceOperationRequiremt(ResourceOperation.Upadte)).Result;
            if(!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            
            _dbContext.SaveChanges();

            
           


            
        }
    }
}
