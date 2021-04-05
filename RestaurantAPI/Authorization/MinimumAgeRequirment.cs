using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirment:IAuthorizationRequirement //marker
    {
        public int MinimumAge { get;}
        public MinimumAgeRequirment(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
