using models.AanwezigHeid;

namespace webapi.Repositories
{
    public interface IAanwezigheidRepositorie
    {
        Task<ICollection<GetAanwezigHeidModel>> GetAllAanwezigHeidModels();
        Task<GetAanwezigHeidModel> GetAanwezigHeidModel(Guid id);
        Task<Guid> PostAanwezigHeidModel(PostAndPutAanwezigHeidModel aanwezigheidModel);
        Task<Guid?> PutAanwezigHeidModel(Guid id, PostAndPutAanwezigHeidModel aanwezigheidModel);
        Task DeleteAanwezigheidModel(Guid id);
    }
}