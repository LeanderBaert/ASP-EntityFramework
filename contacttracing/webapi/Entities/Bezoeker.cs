using System.ComponentModel.DataAnnotations;

namespace webapi.Entities
{
    public class Bezoeker : Persoon
    {
        [Required]
        [StringLength(50, ErrorMessage = "Input is to long")]
        [EmailAddress]
        public string EmailAdres { get; set; }
    }
}
