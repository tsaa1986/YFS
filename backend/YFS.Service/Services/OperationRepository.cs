using Microsoft.EntityFrameworkCore;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class OperationRepository : RepositoryBase<Operation>, IOperationRepository
    {
        public OperationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
        public async Task CreateOperation(Operation operation) =>
            await CreateAsync(operation);

        //public async Task<IEnumerable<AccountGroup>> GetAccountGroupsForUser(string userId, bool trackChanges)
        //    => await FindByConditionAsync(c => c.UserId.Equals(userId), trackChanges).Result.OrderBy(c => c.GroupOrderBy).ToListAsync();
        //public async Task UpdateAccountGroupForUser(AccountGroup accountGroup) => 
        //    await UpdateAsync(accountGroup);
        //review sleect account from group
        public async Task<IEnumerable<Operation>> GetOperationsForAccountGroup(string userId, int accountGroupId, bool trackChanges)
                => await FindByConditionAsync(op => op.UserId.Equals(userId) && ((op.AccountId == 0)), trackChanges).Result.OrderBy(op => op.OperationDate).ToListAsync();

        public async Task UpdateOperation(Operation operation) =>
            await UpdateAsync(operation);

        public async Task<IEnumerable<Operation>> GetOperationsForAccount(string userId, int accountId, bool trackChanges)
                => await FindByConditionAsync(op => op.UserId.Equals(userId) && ((op.AccountId == accountId)), trackChanges).Result.OrderBy(op => op.OperationDate).ToListAsync();
    }
}
