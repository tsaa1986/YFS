using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Repo.Repository
{
    public class MonoSyncedTransactionRepository : RepositoryBase<MonoSyncedTransaction>, IMonoSyncedTransactionRepository
    {
        public MonoSyncedTransactionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task AddAsync(MonoSyncedTransaction monoSyncedTransaction) =>
            await CreateAsync(monoSyncedTransaction);
        public async Task<bool> ExistsAsync(string monobankTransactionId)
        {
            var result = await FindByConditionAsync(m => m.MonoTransactionId == monobankTransactionId, false)
                .Result.SingleOrDefaultAsync();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<MonoSyncedTransaction>> GetAllSyncedTransactionsAsync() =>
            await FindAllAsync(false).Result.ToListAsync();

        public async Task<MonoSyncedTransaction?> GetByIdAsync(int id) =>
                await FindByConditionAsync(m => m.Id == id, false)
                    .Result.SingleOrDefaultAsync() ?? null;

        public async Task<MonoSyncedTransaction?> GetByTransactionIdAsync(string monobankTransactionId) =>
                await FindByConditionAsync(m => m.MonoTransactionId == monobankTransactionId, false)
                    .Result.SingleOrDefaultAsync() ?? null;

    }
}
