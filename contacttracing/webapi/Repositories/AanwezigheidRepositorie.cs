using models.Restaurant;
using models.Persoon;
using models;
using webapi.Entities;
using models.AanwezigHeid;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace webapi.Repositories
{
    public class AanwezigheidRepositorie : IAanwezigheidRepositorie
    {
        private readonly ContacttracingContext _context;
        private readonly UserManager<Persoon> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal? _user;

        public AanwezigheidRepositorie(ContacttracingContext context, UserManager<Persoon> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _user =  _httpContextAccessor.HttpContext.User;

        }

        public async Task<ICollection<GetAanwezigHeidModel>> GetAllAanwezigHeidModels()
        {
            ICollection<GetAanwezigHeidModel>? aanwezigHeidModels = await _context.Aanwezigheden
                .Select(x => new GetAanwezigHeidModel
                {
                    IdAanwezigheid = x.IdAanwezigheid,
                    Dag = x.Dag,
                    Shift = (models.Enums.EShift)x.Shift,
                    Persoon = new GetPersoonModel
                    {
                        FamilieNaam = x.Persoon.FamilieNaam,
                        TelefoonNr = x.Persoon.TelefoonNr,
                        Gemeente = x.Persoon.Gemeente,
                        HuisNr = x.Persoon.HuisNr,
                        Land = x.Persoon.Land,
                        PostCode = x.Persoon.PostCode,
                        Straat = x.Persoon.Straat,
                        VoorName = x.Persoon.VoorName,
                        IdPersoon= x.IdPersoon,
                        IdAanwezigheiden = x.Persoon.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList(),
                    },
                    Restaurant = new GetRestaurantModel
                    {
                        AantalSterren = x.Restaurant.AantalSterren,
                        Gemeente = x.Restaurant.Gemeente,
                        HuisNr = x.Restaurant.HuisNr,
                        Name = x.Restaurant.Name,
                        PostCode = x.Restaurant.PostCode,
                        Stijl = x.Restaurant.Stijl,
                        Straat = x.Restaurant.Straat,
                        IdRestaurant = x.IdRestaurant,
                        AanwezigHeden = x.Restaurant.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList(),
                        MedeWerkers = x.Restaurant.MedeWerkers.Select(m => m.Id).ToList()
                    }

                }).AsNoTracking()
                .ToListAsync();

            foreach (GetAanwezigHeidModel aanwezigHeidModel in aanwezigHeidModels)
            {
                Persoon? user = await _userManager.FindByIdAsync(aanwezigHeidModel.Persoon.IdPersoon.ToString());
                if (user != null)
                {
                    aanwezigHeidModel.UserRole = user.GetType().Name;
                }
            }

            if (aanwezigHeidModels == null) return new List<GetAanwezigHeidModel>();
            return aanwezigHeidModels;
        }

        public async Task<GetAanwezigHeidModel> GetAanwezigHeidModel(Guid id)
        {
            GetAanwezigHeidModel? aanwezigHeidModel = await _context.Aanwezigheden
                .Select(x => new GetAanwezigHeidModel
                {
                    IdAanwezigheid = x.IdAanwezigheid,
                    Dag = x.Dag,
                    Shift = (models.Enums.EShift)x.Shift,
                    Persoon = new models.Persoon.GetPersoonModel
                    {
                        FamilieNaam = x.Persoon.FamilieNaam,
                        TelefoonNr = x.Persoon.TelefoonNr,
                        Gemeente = x.Persoon.Gemeente,
                        HuisNr = x.Persoon.HuisNr,
                        Land = x.Persoon.Land,
                        PostCode = x.Persoon.PostCode,
                        Straat = x.Persoon.Straat,
                        VoorName = x.Persoon.VoorName,
                        IdPersoon = x.Persoon.Id,
                        IdAanwezigheiden = x.Persoon.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList(),
                    },
                    Restaurant = new GetRestaurantModel
                    {
                        AantalSterren = x.Restaurant.AantalSterren,
                        Gemeente = x.Restaurant.Gemeente,
                        HuisNr = x.Restaurant.HuisNr,
                        Name = x.Restaurant.Name,
                        PostCode = x.Restaurant.PostCode,
                        Stijl = x.Restaurant.Stijl,
                        Straat = x.Restaurant.Straat,
                        IdRestaurant = x.Restaurant.IdRestaurant,
                        AanwezigHeden = x.Restaurant.Aanwezigheden.Select(r => r.IdPersoon).ToList(),
                        MedeWerkers = x.Restaurant.MedeWerkers.Select(m => m.Id).ToList()
                    }

                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdAanwezigheid == id);

            if (aanwezigHeidModel == null) throw new Exception("NotFound");

            if (_user.Identity.Name != aanwezigHeidModel.Persoon.IdPersoon.ToString() && _user.IsInRole("Medewerker"))
            {
                throw new Exception("Forbid");
            }
            
            return aanwezigHeidModel;
        }

        public async Task<Guid> PostAanwezigHeidModel(PostAndPutAanwezigHeidModel aanwezigheidModel)
        {
            Aanwezigheid aanwezigheid = new Aanwezigheid
            {
                Dag = aanwezigheidModel.Dag,
                Shift = (Globals.EShift)aanwezigheidModel.Shift,
                IdPersoon = aanwezigheidModel.IdPersoon,
                IdRestaurant = aanwezigheidModel.IdRestaurant
            };

            _context.Aanwezigheden.Add(aanwezigheid);
            await _context.SaveChangesAsync();

            return aanwezigheid.IdAanwezigheid;
        }

        public async Task<Guid?> PutAanwezigHeidModel(Guid id, PostAndPutAanwezigHeidModel aanwezigheidModel)
        {
            Aanwezigheid? aanwezigheid = await _context.Aanwezigheden.FindAsync(id);
            if (aanwezigheid == null)return null;

            aanwezigheid.Dag = aanwezigheidModel.Dag;
            aanwezigheid.Shift = (Globals.EShift)aanwezigheidModel.Shift;
            aanwezigheid.IdPersoon = aanwezigheidModel.IdPersoon;
            aanwezigheid.IdRestaurant = aanwezigheid.IdRestaurant;

            await _context.SaveChangesAsync();

            return id;
        }

        public async Task DeleteAanwezigheidModel(Guid id)
        {
            Aanwezigheid? aanwezigheid = await _context.Aanwezigheden.FirstOrDefaultAsync(a => a.IdAanwezigheid == id);
            if (aanwezigheid != null)
            {
                _context.Aanwezigheden.Remove(aanwezigheid);
                await _context.SaveChangesAsync();
            }
        }
    }
}
