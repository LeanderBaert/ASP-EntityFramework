using models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.MedeWerker
{
    public class BaseMedeWerkerModel : BasePersoonModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        public string Rol { get; set; }
    }
}
