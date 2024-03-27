using models.Helper;
using models.Persoon;
using models.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.AanwezigHeid
{
    public class GetAanwezigHeidModel : BaseAanwezigHeidModel
    {
        public Guid IdAanwezigheid { get; set; }
        public GetPersoonModel Persoon { get; set; }
        public GetRestaurantModel Restaurant { get; set; }
        public string UserRole { get; set; }


    }
}
