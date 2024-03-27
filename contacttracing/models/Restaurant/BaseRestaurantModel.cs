using models.AanwezigHeid;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public class BaseRestaurantModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        public string Name { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Input is to long")]
        public string Stijl { get; set; }
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
        public int AantalSterren { get; set; }
    }
}
