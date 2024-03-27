using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.AanwezigHeid
{
    public class PostAndPutAanwezigHeidModel : BaseAanwezigHeidModel
    {
        [Required]
        public Guid IdPersoon { get; set; }

        [Required]
        public Guid IdRestaurant { get; set; }
    }
}
