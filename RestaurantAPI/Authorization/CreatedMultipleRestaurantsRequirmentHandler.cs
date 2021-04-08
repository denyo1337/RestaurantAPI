using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Data;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirmentHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirment>
    {
        private readonly RestaurantDbContext dbContext;

        public CreatedMultipleRestaurantsRequirmentHandler(RestaurantDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirment requirement)
        {
            var userId = int.Parse(context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

             var createdCount =  dbContext.Restaurants.Count(r => r.CreatedById == userId);
            if(createdCount>= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);

            }
            return Task.CompletedTask;

        }
    }
}
