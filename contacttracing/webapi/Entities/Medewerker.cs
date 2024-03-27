using System.ComponentModel.DataAnnotations;

namespace webapi.Entities
{
    public class MedeWerker : Persoon
    {
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        public string Rol { get; set; }
        public Guid IdRestaurant { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
