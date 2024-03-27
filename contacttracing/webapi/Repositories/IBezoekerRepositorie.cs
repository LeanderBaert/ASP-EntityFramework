using models.Bezoeker;

namespace webapi.Repositories
{
    public interface IBezoekerRepositorie
    {
        Task<ICollection<GetBezoekerModel>> GetAllBezoekersModels();
        Task<GetBezoekerModel> GetBezoekerModel(Guid id);
        Task<Guid> PostBezoekerModel(PostAndPutBezoekerModel bezoekerModel);
        Task<Guid?> PutBezoekerModel(Guid id, PostAndPutBezoekerModel bezoekerModel);
        Task DeleteBezoekerModel(Guid id);
    }
}
