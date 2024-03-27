using models;
using models.Restaurant;

namespace webapi.Repositories
{
    public interface IRestaurantRepositorie
    {
        Task<ICollection<GetRestaurantModel>> GetAllRestaurantModels();
        Task<GetRestaurantModel> GetRestaurantModel(Guid id);
        Task<Guid> PostRestaurantModel(BaseRestaurantModel model);
        Task<Guid?> PutRestaurantModel(Guid id, BaseRestaurantModel model);
        Task DeleteRestaurantModel(Guid id);
    }
}