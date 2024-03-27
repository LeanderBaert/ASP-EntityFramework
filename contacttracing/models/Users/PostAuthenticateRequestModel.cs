using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.Users
{
    public class PostAuthenticateRequestModel
    {
        [Required(ErrorMessage ="Gebruikernaam is verplicht")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        public string Password { get; set; }
    }
}
