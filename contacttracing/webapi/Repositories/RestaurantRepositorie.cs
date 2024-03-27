using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using models;
using models.Restaurant;
using System.Linq;
using System.Security.Claims;
using webapi.Entities;

namespace webapi.Repositories
{
    public class RestaurantRepositorie : IRestaurantRepositorie
    {
        private readonly ContacttracingContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal? _user;

        public RestaurantRepositorie(ContacttracingContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
        }

        public async Task<ICollection<GetRestaurantModel>> GetAllRestaurantModels()
        {
            ICollection<GetRestaurantModel>? restaurantModel = await _context.Restaurants
                .Select(x => new GetRestaurantModel
                {
                    IdRestaurant = x.IdRestaurant,
                    Name = x.Name,
                    Stijl = x.Stijl,
                    Straat= x.Straat,
                    HuisNr= x.HuisNr,
                    PostCode= x.PostCode,
                    Gemeente= x.Gemeente,
                    AantalSterren= x.AantalSterren,
                    AanwezigHeden = x.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList(),
                    MedeWerkers = x.MedeWerkers.Select(a => a.Id).ToList(),
                }).AsNoTracking()
                .ToListAsync();

            if(restaurantModel == null) return new List<GetRestaurantModel>();
            return restaurantModel;
        }

        public async Task<GetRestaurantModel> GetRestaurantModel(Guid id)
        {
            ICollection<string> persoonIdsOfRestaurentAanwezigheden
                = await _context.Restaurants
                   .Where(x => x.IdRestaurant == id)
                   .SelectMany(x => x.Aanwezigheden.Select(a => a.IdPersoon.ToString().ToLower()))
                   .ToListAsync();

            GetRestaurantModel? restaurantModel = await _context.Restaurants
                .Select(x => new GetRestaurantModel
                {
                    IdRestaurant = x.IdRestaurant,
                    Name = x.Name,
                    Stijl = x.Stijl,
                    Straat = x.Straat,
                    HuisNr = x.HuisNr,
                    PostCode = x.PostCode,
                    Gemeente = x.Gemeente,
                    AantalSterren = x.AantalSterren,
                    AanwezigHeden = x.Aanwezigheden.Select(a => a.IdAanwezigheid).ToList(),
                    MedeWerkers = x.MedeWerkers.Select(a => a.Id).ToList(),
                }
                ).AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdRestaurant == id);

            if (restaurantModel == null) throw new Exception("NotFound");

            if (!persoonIdsOfRestaurentAanwezigheden.Contains(_user.Identity.Name) && !_user.IsInRole("Medewerker"))
            {
                throw new Exception("Forbid");
            }

            
            return restaurantModel;
        }

        public async Task<Guid> PostRestaurantModel(BaseRestaurantModel restaurantModel)
        {
            Restaurant restaurant = new Restaurant
            {
                Name = restaurantModel.Name,
                Stijl = restaurantModel.Stijl,
                Straat = restaurantModel.Straat,
                HuisNr = restaurantModel.HuisNr,
                PostCode = restaurantModel.PostCode,
                Gemeente = restaurantModel.Gemeente,
                AantalSterren = restaurantModel.AantalSterren,
            };

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return restaurant.IdRestaurant;
        }

        public async Task<Guid?> PutRestaurantModel(Guid id, BaseRestaurantModel restaurantModel)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)return null;


            restaurant.Name = restaurantModel.Name;
            restaurant.Stijl = restaurantModel.Stijl;
            restaurant.Straat = restaurantModel.Straat;
            restaurant.HuisNr = restaurantModel.HuisNr;
            restaurant.PostCode = restaurantModel.PostCode;
            restaurant.Gemeente = restaurantModel.Gemeente;
            restaurant.AantalSterren = restaurantModel.AantalSterren;

            await _context.SaveChangesAsync();

            return id;
        }

        public async Task DeleteRestaurantModel(Guid id)
        {
            Restaurant? restaurantEntity = await _context.Restaurants.FindAsync(id);
            if (restaurantEntity != null)
            {
                _context.Restaurants.Remove(restaurantEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
