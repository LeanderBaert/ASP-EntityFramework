using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models.Helper;

namespace models.Bezoeker
{
    public class BaseBezoekerModel : BasePersoonModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        [EmailAddress]
        public string EmailAdres { get; set; }
    }
}
