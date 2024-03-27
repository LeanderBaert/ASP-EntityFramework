using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using models.Bezoeker;
using models.Restaurant;
using models.AanwezigHeid;
using System.IO;
using webapi.Entities;
using System.Security.Claims;

namespace webapi.Repositories
{
    public class BezoekerRepositorie : IBezoekerRepositorie
    {
        private readonly ContacttracingContext _context;
        private readonly UserManager<Bezoeker> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal? _user;

        public BezoekerRepositorie(ContacttracingContext context, UserManager<Bezoeker> userManager, RoleManager<Rol> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
        }

        public async Task<ICollection<GetBezoekerModel>> GetAllBezoekersModels()
        {
            ICollection<GetBezoekerModel>? bezoekerModels = await _userManager.Users
                .Select(x => new GetBezoekerModel
                {
                    IdBezoeker = x.Id,
                    VoorName = x.VoorName,
                    EmailAdres = x.EmailAdres,
                    FamilieNaam = x.FamilieNaam,
                    Gemeente = x.Gemeente,
                    HuisNr = x.HuisNr,
                    Land = x.Land,
                    PostCode = x.PostCode,
                    Straat = x.Straat,
                    TelefoonNr = x.TelefoonNr,
                    IdAanwezigHeden = x.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList(),
                })
                .AsNoTracking()
                .ToListAsync();

            if (bezoekerModels == null) return new List<GetBezoekerModel>();
            return bezoekerModels;
        }

        public async Task<GetBezoekerModel> GetBezoekerModel(Guid id)
        {
            GetBezoekerModel? bezoekerModel = await _userManager.Users
                .Select(x => new GetBezoekerModel
                {
                    IdBezoeker = x.Id,
                    VoorName = x.VoorName,
                    EmailAdres = x.EmailAdres,
                    FamilieNaam = x.FamilieNaam,
                    Gemeente = x.Gemeente,
                    HuisNr = x.HuisNr,
                    Land = x.Land,
                    PostCode = x.PostCode,
                    Straat = x.Straat,
                    TelefoonNr = x.TelefoonNr,
                    IdAanwezigHeden = x.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdBezoeker == id);

            if (bezoekerModel == null) throw new Exception("NotFound");

            if (_user.Identity.Name != bezoekerModel.IdBezoeker.ToString() && !_user.IsInRole("Medewerker"))
            {
                throw new Exception("Forbid");
            }

            
            return bezoekerModel;
        }

        public async Task<Guid> PostBezoekerModel(PostAndPutBezoekerModel bezoekerModel)
        {
            const string RoleName = "Bezoeker";

            Bezoeker bezoeker = new Bezoeker
            {
                VoorName = bezoekerModel.VoorName,
                EmailAdres = bezoekerModel.EmailAdres,
                FamilieNaam = bezoekerModel.FamilieNaam,
                Gemeente = bezoekerModel.Gemeente,
                HuisNr = bezoekerModel.HuisNr,
                Land = bezoekerModel.Land,
                PostCode = bezoekerModel.PostCode,
                Straat = bezoekerModel.Straat,
                TelefoonNr = bezoekerModel.TelefoonNr,
                UserName = bezoekerModel.VoorName + bezoekerModel.FamilieNaam,
                PasswordHash = bezoekerModel.Password
            };

            var hashedPassword = _userManager.PasswordHasher.HashPassword(bezoeker, bezoekerModel.Password);
            bezoeker.PasswordHash = hashedPassword;


            IdentityResult result =  await _userManager.CreateAsync(bezoeker);
            if (!result.Succeeded)
            {
                return Guid.Empty;
            }

            //<<<---- Rol configuratie ---->>>
            Rol rol = new Rol
            {
                Name = RoleName,
                Description = "Bezoeker met lage machtiging"
            };

            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                await _roleManager.CreateAsync(rol);
            }
            rol = await _roleManager.FindByNameAsync(RoleName);

            await _context.UserRoles.AddAsync(new PersoonRol
            {
                UserId = bezoeker.Id,
                RoleId = rol.Id
            });
            await _context.SaveChangesAsync();


            return bezoeker.Id;
        }

        public async Task<Guid?> PutBezoekerModel(Guid id, PostAndPutBezoekerModel bezoekerModel)
        {
            Bezoeker? bezoeker = await _userManager.FindByIdAsync(id.ToString());
            if (bezoeker == null) return null;

            bezoeker.VoorName = bezoekerModel.VoorName;
            bezoeker.EmailAdres = bezoekerModel.EmailAdres;
            bezoeker.FamilieNaam = bezoekerModel.FamilieNaam;
            bezoeker.Gemeente = bezoekerModel.Gemeente;
            bezoeker.HuisNr = bezoekerModel.HuisNr;
            bezoeker.Land = bezoekerModel.Land;
            bezoeker.PostCode = bezoekerModel.PostCode;
            bezoeker.Straat = bezoekerModel.Straat;
            bezoeker.TelefoonNr = bezoekerModel.TelefoonNr;

            var hashedPassword = _userManager.PasswordHasher.HashPassword(bezoeker, bezoekerModel.Password);
            bezoeker.PasswordHash = hashedPassword;

            IdentityResult updateResult = await _userManager.UpdateAsync(bezoeker);
            if (!updateResult.Succeeded) return null;

            return id;
        }

        public async Task DeleteBezoekerModel(Guid id)
        {
            Bezoeker? bezoeker = await _userManager.FindByIdAsync(id.ToString());
            if (bezoeker != null)
            {
                IdentityResult deleteResult = await _userManager.DeleteAsync(bezoeker);
            }
        }
    }
}
