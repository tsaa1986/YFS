
using Microsoft.EntityFrameworkCore;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountBalanceRepository : RepositoryBase<AccountBalance>, IAccountBalanceRepository
    {
        public AccountBalanceRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateAccountBalance(AccountBalance accountBalance) =>
            await CreateAsync(accountBalance);

        public async Task UpdateAccountBalance(AccountBalance accountBalance) => 
            await UpdateAsync(accountBalance);

        public async Task<AccountBalance?> GetAccountBalance(int _accountId) =>
            await FindByConditionAsync(c => c.Id.Equals(_accountId), false)
                .Result
                //.AsNoTracking()
                .SingleOrDefaultAsync();
    }
}
