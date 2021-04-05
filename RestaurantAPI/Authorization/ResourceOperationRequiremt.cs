using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Upadte,
        Delete
    }
    public class ResourceOperationRequiremt:IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation { get; }
        public ResourceOperationRequiremt(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
       
        
    }
}
