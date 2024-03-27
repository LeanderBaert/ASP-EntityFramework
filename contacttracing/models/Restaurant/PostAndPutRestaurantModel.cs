using models.AanwezigHeid;
using models.MedeWerker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.Restaurant
{
    public class PostAndPutRestaurantModel : BaseRestaurantModel
    {
        [Required]
        public ICollection<Guid> AanwezigHeden { get; set; }
        [Required]
        public ICollection<Guid> MedeWerkers { get; set; }
    }
}
