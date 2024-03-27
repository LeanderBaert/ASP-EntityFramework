using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.Bezoeker
{
    public class PostAndPutBezoekerModel : BaseBezoekerModel
    {
        [Required]
        [StringLength(16, MinimumLength = 9, ErrorMessage = "Input must be between 9 and 16 characters long")]
        public string Password { get; set; }
    }
}
