using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using webapi.Entities;

namespace webapi.Entities
{
    public class PersoonRol : IdentityUserRole<Guid>
    {
        public virtual Persoon Persoon { get; set; } 
        public virtual Rol Role { get; set; }
    }
}
