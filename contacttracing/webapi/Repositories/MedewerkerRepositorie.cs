using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using models.Bezoeker;
using models.MedeWerker;
using System.Security.Claims;
using webapi.Entities;

namespace webapi.Repositories
{
    public class MedewerkerRepositorie : IMedewerkerRepositorie
    {
        private readonly ContacttracingContext _context;
        private readonly UserManager<MedeWerker> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal? _user;

        public MedewerkerRepositorie(ContacttracingContext context, UserManager<MedeWerker> userManager, RoleManager<Rol> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
        }

        public async Task<ICollection<GetMedeWerkerModel>> GetAllMedewerkerModels()
        {
            ICollection<GetMedeWerkerModel>? modewerkerModels = await _userManager.Users
                .Select(x => new GetMedeWerkerModel
                {
                    IdMedewerker = x.Id,
                    VoorName = x.VoorName,
                    FamilieNaam = x.FamilieNaam,
                    Gemeente = x.Gemeente,
                    HuisNr = x.HuisNr,
                    Land = x.Land,
                    PostCode = x.PostCode,
                    Straat = x.Straat,
                    TelefoonNr = x.TelefoonNr,
                    Rol = x.Rol,
                    IdRestaurant = x.IdRestaurant,
                    IdAanwezigHeden = x.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            if (modewerkerModels == null) return new List<GetMedeWerkerModel>();
            return modewerkerModels;
        }

        public async Task<GetMedeWerkerModel> GetMedewerkerModel(Guid id)
        {
            GetMedeWerkerModel? medeWerkerModel = await _userManager.Users
                .Select(x => new GetMedeWerkerModel
                {
                    IdMedewerker = x.Id,
                    VoorName = x.VoorName,
                    FamilieNaam = x.FamilieNaam,
                    Gemeente = x.Gemeente,
                    HuisNr = x.HuisNr,
                    Land = x.Land,
                    PostCode = x.PostCode,
                    Straat = x.Straat,
                    TelefoonNr = x.TelefoonNr,
                    Rol = x.Rol,
                    IdRestaurant = x.IdRestaurant,
                    IdAanwezigHeden = x.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdMedewerker == id);

            if (medeWerkerModel == null) throw new Exception("NotFound");

            
            return medeWerkerModel;
        }

        public async Task<Guid> PostMedewerkerModel(PostAndPutMedewerkerModel medeWerkerModel)
        {
            const string RoleName = "Medewerker";

            MedeWerker medewerker = new MedeWerker
            {
                VoorName = medeWerkerModel.VoorName,
                FamilieNaam = medeWerkerModel.FamilieNaam,
                Gemeente = medeWerkerModel.Gemeente,
                HuisNr = medeWerkerModel.HuisNr,
                Land = medeWerkerModel.Land,
                PostCode = medeWerkerModel.PostCode,
                Straat = medeWerkerModel.Straat,
                TelefoonNr = medeWerkerModel.TelefoonNr,
                Rol = medeWerkerModel.Rol,
                UserName = medeWerkerModel.VoorName + medeWerkerModel.FamilieNaam,
                IdRestaurant = medeWerkerModel.IdRestaurant
            };

            var hashedPassword = _userManager.PasswordHasher.HashPassword(medewerker, medeWerkerModel.Password);
            medewerker.PasswordHash = hashedPassword;

            IdentityResult result = await _userManager.CreateAsync(medewerker);
            if (!result.Succeeded)
            {
                return Guid.Empty;
            }

            //<<<---- Rol configuratie ---->>>
            Rol rol = new Rol
            {
                Name = RoleName,
                Description = "Medewerker met hooge machtiging"
            };

            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                await _roleManager.CreateAsync(rol);
            }
            rol = await _roleManager.FindByNameAsync(RoleName);

            await _context.UserRoles.AddAsync(new PersoonRol
            {
                UserId = medewerker.Id,
                RoleId = rol.Id
            });
            await _context.SaveChangesAsync();


            return medewerker.Id;
        }

        public async Task<Guid?> PutMedewerkerModel(Guid id, PostAndPutMedewerkerModel medeWerkerModel)
        {
            MedeWerker? medewerker = await _userManager.FindByIdAsync(id.ToString());
            if (medewerker == null) return null;

            medewerker.VoorName = medeWerkerModel.VoorName;
            medewerker.Rol = medeWerkerModel.Rol;
            medewerker.FamilieNaam = medeWerkerModel.FamilieNaam;
            medewerker.Gemeente = medeWerkerModel.Gemeente;
            medewerker.HuisNr = medeWerkerModel.HuisNr;
            medewerker.Land = medeWerkerModel.Land;
            medewerker.PostCode = medeWerkerModel.PostCode;
            medewerker.Straat = medeWerkerModel.Straat;
            medewerker.TelefoonNr = medeWerkerModel.TelefoonNr;

            var hashedPassword = _userManager.PasswordHasher.HashPassword(medewerker, medeWerkerModel.Password);
            medewerker.PasswordHash = hashedPassword;

            IdentityResult updateResult = await _userManager.UpdateAsync(medewerker);
            if (!updateResult.Succeeded) return null;

            return id;
        }

        public async Task DeleteMedewerkerModel(Guid id)
        {
            MedeWerker? medeWerker = await _userManager.FindByIdAsync(id.ToString());
            if (medeWerker != null)
            {
                IdentityResult deleteResult = await _userManager.DeleteAsync(medeWerker);
            }
        }

        public Task<Guid> PostMedewerkerModel(BaseMedeWerkerModel medeWerkerModel)
        {
            throw new NotImplementedException();
        }

        public Task<Guid?> PutMedewerkerModel(Guid id, BaseMedeWerkerModel medeWerkerModel)
        {
            throw new NotImplementedException();
        }
    }
}