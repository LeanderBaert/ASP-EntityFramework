using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.Restaurant
{
    public class GetRestaurantModel : PostAndPutRestaurantModel
    {
        public Guid IdRestaurant { get; set; }
    }
}
