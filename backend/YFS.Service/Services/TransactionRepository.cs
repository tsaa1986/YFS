using Microsoft.EntityFrameworkCore;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
        public async Task CreateTransaction(Transaction transaction) =>
            await CreateAsync(transaction);

        //public async Task<IEnumerable<AccountGroup>> GetAccountGroupsForUser(string userId, bool trackChanges)
        //    => await FindByConditionAsync(c => c.UserId.Equals(userId), trackChanges).Result.OrderBy(c => c.GroupOrderBy).ToListAsync();
        //public async Task UpdateAccountGroupForUser(AccountGroup accountGroup) => 
        //    await UpdateAsync(accountGroup);
        //review sleect account from group
        public async Task<IEnumerable<Transaction>> GetTransactionForAccountGroup(string userId, int accountGroupId, bool trackChanges)
                => await FindByConditionAsync(tr => tr.UserId.Equals(userId) && ((tr.WithdrawFromAccountId == 0) || (tr.TargetAccountId == 0)), trackChanges).Result.OrderBy(tr => tr.TransactionDate).ToListAsync();

        public async Task UpdateTransaction(Transaction transaction) =>
            await UpdateAsync(transaction);

        public async Task<IEnumerable<Transaction>> GetTransactionForAccount(string userId, int accountId, bool trackChanges)
                => await FindByConditionAsync(tr => tr.UserId.Equals(userId) && ((tr.WithdrawFromAccountId == accountId) || (tr.TargetAccountId == accountId)), trackChanges).Result.OrderBy(tr => tr.TransactionDate).ToListAsync();
    }
}
