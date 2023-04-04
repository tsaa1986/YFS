
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateAccount(Account account) =>
            await CreateAsync(account);

        public Task<Account> GetAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Account>> GetAccountsByGroup(int accountGroupId, string userId, bool trackChanges) =>
            await FindByConditionAsync(c => c.AccountGroupId.Equals(accountGroupId), trackChanges);
        public async Task<IEnumerable<Account>> GetAccountsByFavorites(string userId, bool trackChanges) =>
            await FindByConditionAsync(c => c.Favorites.Equals(1) && c.UserId.Equals(userId), trackChanges);
        public async Task UpdateAccount(Account account) => 
            await UpdateAsync(account);
    }
}
