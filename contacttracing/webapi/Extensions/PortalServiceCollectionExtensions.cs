using webapi.Repositories;

namespace webapi.Extensions
{
    public static class PortalServiceCollectionExtensions
    {
        public static IServiceCollection AddPortalServices(
            this IServiceCollection services, IConfiguration config) 
        {
            services.AddTransient<IRestaurantRepositorie, RestaurantRepositorie>();
            services.AddTransient<IAanwezigheidRepositorie, AanwezigheidRepositorie>();
            services.AddTransient<IMedewerkerRepositorie, MedewerkerRepositorie>();
            services.AddTransient<IBezoekerRepositorie, BezoekerRepositorie>();
            services.AddTransient<IPersoonRepositorie, PersoonRepositorie>();

            return services;
        }
    }
}
