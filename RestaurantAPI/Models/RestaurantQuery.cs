using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Models
{
    public class RestaurantQuery
    {
        public string searchPhare { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }             
    }
}
