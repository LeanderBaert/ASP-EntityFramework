using System.ComponentModel.DataAnnotations;
using webapi.Globals;

namespace webapi.Entities
{
    public class Aanwezigheid
    {
        public Guid IdAanwezigheid { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Incorrect Format")]
        public EShift Shift { get; set; }
        [Required]
        public DateTime Dag { get; set; }
        public Guid IdPersoon { get; set; }
        public Guid IdRestaurant { get; set; }
        public virtual Persoon Persoon { get; set; }    
        public virtual Restaurant Restaurant { get; set; }
    }
}
