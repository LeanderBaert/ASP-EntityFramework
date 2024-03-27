using models.MedeWerker;

namespace webapi.Repositories
{
    public interface IMedewerkerRepositorie
    {
        Task<ICollection<GetMedeWerkerModel>> GetAllMedewerkerModels();
        Task<GetMedeWerkerModel> GetMedewerkerModel(Guid id);
        Task<Guid> PostMedewerkerModel(PostAndPutMedewerkerModel medeWerkerModel);
        Task<Guid?> PutMedewerkerModel(Guid id, PostAndPutMedewerkerModel medeWerkerModel);
        Task DeleteMedewerkerModel(Guid id);
    }
}
