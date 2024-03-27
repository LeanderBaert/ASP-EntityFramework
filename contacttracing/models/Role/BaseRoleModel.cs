using System.ComponentModel.DataAnnotations;

namespace models.Role
{
    public class BaseRoleModel
    {
        public string Id { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        [Required (ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cant be longer then 50 char")]
        public string Name { get; set; }
        
    }
}
