using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
    public class NotFoundExpection : Exception
    {
        public NotFoundExpection(string msg):base(msg)
        {

        }
    }
}
