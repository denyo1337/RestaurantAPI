using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Data;
using RestaurantAPI.Data.EfCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Expections;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly EfCoreRestaurantRepository _efCoreRestaurantRepository;

        public RestaurantService(RestaurantDbContext context, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService,IUserContextService userContextService,EfCoreRestaurantRepository efCoreRestaurantRepository)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _efCoreRestaurantRepository = efCoreRestaurantRepository;
        }

        public async Task<RestaurantDTO> GetById(int id)
        {
            
            var restaurant = await _efCoreRestaurantRepository.Get(id);
           
            var result = _mapper.Map<RestaurantDTO>(restaurant);

            return result;
        }
        public PageResult<RestaurantDTO> GetAll(RestaurantQuery query)
        {

            var baseQuery = _dbContext.Restaurants
               .Include(x => x.Address)
               .Include(x => x.Dishes)
               .Where(r => query.searchPhare == null || (r.Name.ToLower().Contains(query.searchPhare.ToLower()) || r.Description.ToLower().Contains(query.searchPhare.ToLower())));


            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                    {
                    {nameof(Restaurant.Name), r=>r.Name },
                    {nameof(Restaurant.Category), r=>r.Category},
                    {nameof(Restaurant.Description), r=>r.Description}
                    };

                var selectedColumn = columnsSelector[query.SortBy];
                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }


            var restaurants = baseQuery
               .Skip(query.pageSize*(query.pageNumber -1))
               .Take(query.pageSize)
               .ToList();

            var restaurantDtos = _mapper.Map<List<RestaurantDTO>>(restaurants);

            var totalItemCount = baseQuery.Count();

            var result = new PageResult<RestaurantDTO>(restaurantDtos,totalItemCount,query.pageSize,query.pageNumber);

            //zrób refaktor by niepotrzebna była validacja, lepiej wymusisz wartości domyślne !

            return result;
        }
        public async Task<int> CreateAsync(CreateRestaurantDTO dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);

            restaurant.CreatedById = _userContextService.GetUserId;

            await _efCoreRestaurantRepository.Add(restaurant);

            return restaurant.Id;
        }
        public async Task<Restaurant> Delete(int id)
        {
            /* _logger.LogWarning($"Restaurant with id: {id} DELETE action invoked ! Done by {_userContextService.User}");
             _logger.LogError($"Restaurant with id: {id} DELETE action invoked but failed! Done by {_userContextService.User}"); to na później
            */
            var user = _userContextService.User.IsInRole("Admin");
            if (!user)
                throw new ForbidException("Nie masz praw do usuwania danych");

            var restaurant = await _efCoreRestaurantRepository.Delete(id);
            
            return restaurant;
            
            
        }

        public async Task UpdateAsync(int id, UpdateRestaurantDTO dto)
        {

            var restaurant = _efCoreRestaurantRepository.Get(id).Result;
            if (restaurant == null)
                throw new NotFoundExpection("Restaurant not found");

            var userAu = _userContextService.User.IsInRole("Admin");

            if (!userAu)
                throw new ForbidException("Nie masz dostępu");

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;


            await _efCoreRestaurantRepository.Update(restaurant);

            
        }
    }
}
