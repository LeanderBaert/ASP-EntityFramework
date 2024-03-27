using System.ComponentModel.DataAnnotations;

namespace webapi.Entities
{
    public class Restaurant
    {
        public Guid IdRestaurant { get; set; }
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
        public virtual ICollection<Aanwezigheid> Aanwezigheden { get; set; }
        public virtual ICollection<MedeWerker> MedeWerkers { get; set; }
    }
}
