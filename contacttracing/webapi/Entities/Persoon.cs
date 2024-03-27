using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace webapi.Entities
{
    public class Persoon : IdentityUser<Guid>
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
        public virtual ICollection<PersoonRol> PersoonRolen { get; set; }
        public virtual ICollection<Aanwezigheid> Aanwezigheden { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
