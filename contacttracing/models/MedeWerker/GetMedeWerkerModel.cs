using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.MedeWerker
{
    public class GetMedeWerkerModel : BaseMedeWerkerModel
    {
        [Required]
        public Guid IdRestaurant { get; set; }
        public Guid IdMedewerker { get; set; }
        public ICollection<Guid> IdAanwezigHeden { get; set; }
    }
}
