
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
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

       // public async Task <Account> GetAccount(int _accountId) =>
       //     await FindByConditionAsync(c => c.Id.Equals(_accountId), false);

        public async Task<IEnumerable<Account>> GetAccountsByGroup(int accountGroupId, string userId, bool trackChanges) =>
            await FindByConditionAsync(c => c.AccountGroupId.Equals(accountGroupId) && c.AccountStatus.Equals(1), trackChanges);
        public async Task UpdateAccount(Account account) => 
            await UpdateAsync(account);
        public async Task<IEnumerable<Account>> GetOpenAccountsByUserId(string userId, bool trackChanges) =>
            await FindByConditionAsync(c => c.AccountStatus.Equals(1) && c.UserId.Equals(userId), trackChanges)        
            .Result
            .OrderByDescending(c => c.Favorites)
            .Include(p => p.AccountBalance)
            .Include(amb => amb.AccountsMonthlyBalance)
            .ToListAsync();
        public async Task<Account?> GetAccount(int _accountId) =>
            await FindByConditionAsync(c => c.Id.Equals(_accountId), false)
                .Result
                .Include(p => p.AccountBalance)
                .SingleOrDefaultAsync();

    }
}
