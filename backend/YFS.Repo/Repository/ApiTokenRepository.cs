using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class ApiTokenRepository : RepositoryBase<ApiToken>, IApiTokenRepository
    {
        public ApiTokenRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task AddToken(ApiToken newToken) =>
            await CreateAsync(newToken);
        public async Task<ApiToken?> GetApiToken(string tokenName, string userId) => 
            await FindByConditionAsync(a => a.Name.Equals(tokenName) && a.UserId.Equals(userId), false)
                .Result.SingleOrDefaultAsync() ?? throw new InvalidOperationException("ApiToken not found");
        public async Task UpdateToken(ApiToken newToken) => await UpdateAsync(newToken);
    }
}
