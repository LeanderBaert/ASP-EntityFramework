using models.Users;

namespace webapi.Repositories
{
    public interface IPersoonRepositorie
    {
        Task<PostAuthenticeerResponseModel> Authenticate(PostAuthenticateRequestModel postAuthenticateRequestModel, string ipAddress);
        Task<PostAuthenticeerResponseModel> RenewToken(string token, string ipAddress);
        Task DeactivateToken(string token, string ipAddress);
    }
}
