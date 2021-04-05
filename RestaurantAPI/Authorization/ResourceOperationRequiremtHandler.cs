﻿using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequiremtHandler : AuthorizationHandler<ResourceOperationRequiremt, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequiremt requirement, Restaurant restaurant)
        {
            if(requirement.ResourceOperation == ResourceOperation.Create || requirement.ResourceOperation == ResourceOperation.Read)
            {
                context.Succeed(requirement);

            }
           var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if(restaurant.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;

        }
    }
}
