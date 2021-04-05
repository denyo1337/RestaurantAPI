using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Models
{
    public class UpdateRestaurantDTO
    {
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public bool HasDelivery { get; set; }

    }
}
