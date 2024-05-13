using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IApiTokenRepository
    {
        Task AddToken(ApiToken newToken);
        Task UpdateToken(ApiToken newToken);
        Task<ApiToken?> GetApiToken(string tokenName);
    }
}
