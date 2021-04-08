using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Migrations.Data

{
    public interface IEntity
    {
        int Id { get; set; }
    }
}
