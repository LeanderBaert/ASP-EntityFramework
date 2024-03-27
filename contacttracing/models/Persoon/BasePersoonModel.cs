using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.Helper
{
    public class BasePersoonModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        public string VoorName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        public string FamilieNaam { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        public string Straat { get; set; }
        [Required]
        public int HuisNr { get; set; }
        [Required]
        public int PostCode { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        public string Gemeente { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Input is to long")]
        public string Land { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Input is to long")]
        public string TelefoonNr { get; set; }
    }
}
