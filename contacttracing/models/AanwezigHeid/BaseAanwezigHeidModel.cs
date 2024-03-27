using models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.AanwezigHeid
{
    public class BaseAanwezigHeidModel
    {
        [Required]
        public EShift Shift { get; set; }
        [Required]
        public DateTime Dag { get; set; }
    }
}
