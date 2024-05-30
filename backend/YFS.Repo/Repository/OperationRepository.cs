using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YFS.Core.Enums;
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
        public async Task UpdateOperation(Operation operation) =>
            await UpdateAsync(operation);
        public async Task RemoveOperation(Operation operation) =>
            await RemoveAsync(operation);
        public async Task<IEnumerable<Operation>> GetOperationsForAccount(string languageCode, int accountId, bool trackChanges)
                => await FindByConditionAsync(op => ((op.AccountId == accountId)), trackChanges)
            .Result.Include(p => p.Account.AccountBalance).OrderByDescending(op => op.OperationDate).ToListAsync();
        public async Task<IEnumerable<Operation>> GetOperationsForAccountForPeriod(string languageCode, int accountId, DateTime startDate, DateTime endDate, bool trackChanges)
            => await FindByConditionAsync(op => ((op.AccountId == accountId) && (op.OperationDate >= startDate && op.OperationDate <= endDate) ), trackChanges)
            .Result.Include(p => p.Account.AccountBalance).OrderByDescending(op => op.OperationDate)
            .Include(op => op.OperationItems)
            .ThenInclude(oi => oi.Category.Translations.Where(t => t.LanguageCode == languageCode))
                .ToListAsync();


        public Task<IEnumerable<Operation>> GetOperationsForAccountGroupForPeriod(string languageCode, int accountGroupId, bool trackChanges)
        {
            throw new NotImplementedException();
        }
        //public async Task<IEnumerable<Operation>> GetLast10OperationsForAccount(string userId, int accountId, bool trackChanges)
        //    => await FindByConditionAsync(op => op.UserId.Equals(userId) && ((op.AccountId == accountId)), trackChanges).Result.OrderByDescending(op => op. OperationDate).Take(10).ToListAsync();
        public async Task<IEnumerable<Operation>> GetLast10OperationsForAccount(string languageCode, int accountId, bool trackChanges)
            => await FindByConditionAsync(op => ((op.AccountId == accountId)), trackChanges)
            .Result.Include(p => p.Account.AccountBalance).AsNoTracking()
            .Include(op => op.OperationItems)
                .ThenInclude(oi => oi.Category.Translations.Where(t => t.LanguageCode == languageCode))
                    .OrderByDescending(op => op.OperationDate).Take(10).ToListAsync();
        //=> await Task.Run(() => RepositoryContext.Operations.Where(op => (op.AccountId == accountId))
        //.Include(p => p.Account.AccountBalance)            
        //.AsNoTracking().Take(10));

        public async Task<Operation?> GetOperationById(string languageCode, int operationId, bool trackChanges)
           => await FindByConditionAsync(op => op.Id.Equals(operationId), trackChanges)
            .Result.AsNoTracking()
            .Include(p => p.Account.AccountBalance)
            .Include(op => op.OperationItems)
            .ThenInclude(oi => oi.Category.Translations.Where(t => t.LanguageCode == languageCode))
                .AsNoTracking()
                .SingleOrDefaultAsync();

        public async Task<Operation?> GetTransferOperationById(string languageCode, int transferOperationId)
            => await FindByConditionAsync(op => op.TransferOperationId.Equals(transferOperationId), false)
                .Result
                .Include(p => p.Account.AccountBalance)
                .Include(op => op.OperationItems)
                    .ThenInclude(oi => oi.Category.Translations.Where(t => t.LanguageCode == languageCode))
                    .AsNoTracking()
                    .SingleOrDefaultAsync();

        public async Task<Operation?> GetOperationByIdWithoutCategory(int operationId, bool trackChanges)
           => await FindByConditionAsync(op => op.Id.Equals(operationId), trackChanges)
            .Result.AsNoTracking()
            .Include(p => p.Account.AccountBalance)
            .Include(op => op.OperationItems)
            .SingleOrDefaultAsync();
    }
}
