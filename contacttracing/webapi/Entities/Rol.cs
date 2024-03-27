using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace webapi.Entities
{
    public class Rol : IdentityRole<Guid>
    {
        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Incorrect Format")]
        public string Description { get; set; }
        public virtual ICollection<PersoonRol> PersoonRolen { get; set; }
    }
}
