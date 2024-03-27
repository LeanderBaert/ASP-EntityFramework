using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace models.Users
{
    public class PostAuthenticeerResponseModel
    {
        public Guid Id { get; set; }
        public string Voornaam { get; set; }
        public string Familienaam { get; set;}
        public string Gebruikersnaam { get; set; }
        public string JwtToken { get; set; }
        public ICollection<string> Roles { get; set; }

        [JsonIgnore] // refresh token is only for htttp cookies
        public string RefreshToken { get; set; }
    }
}
