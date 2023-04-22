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
        public async Task UpdateOperation(Operation operation) =>
            await UpdateAsync(operation);
        public async Task RemoveOperation(Operation operation) =>
            await RemoveAsync(operation);
        public async Task<IEnumerable<Operation>> GetOperationsForAccount(string userId, int accountId, bool trackChanges)
                => await FindByConditionAsync(op => op.UserId.Equals(userId) && ((op.AccountId == accountId)), trackChanges).Result.OrderByDescending(op => op.OperationDate).ToListAsync();

        public async Task<IEnumerable<Operation>> GetOperationsForAccountForPeriod(string userId, int accountId, DateTime startDate, DateTime endDate, bool trackChanges)
            => await FindByConditionAsync(op => ((op.AccountId == accountId) && (op.OperationDate >= startDate && op.OperationDate <= endDate) ), trackChanges).Result.OrderByDescending(op => op.OperationDate).ToListAsync();

        public Task<IEnumerable<Operation>> GetOperationsForAccountGroupForPeriod(string userId, int accountGroupId, bool trackChanges)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Operation>> GetLast10OperationsForAccount(string userId, int accountId, bool trackChanges)
            => await FindByConditionAsync(op => op.UserId.Equals(userId) && ((op.AccountId == accountId)), trackChanges).Result.OrderByDescending(op => op. OperationDate).Take(10).ToListAsync();

        public async Task<Operation?> GetOperationById(int operationId)
            => await FindByConditionAsync(op => op.Id.Equals(operationId), false).Result.SingleOrDefaultAsync();
    }
}
