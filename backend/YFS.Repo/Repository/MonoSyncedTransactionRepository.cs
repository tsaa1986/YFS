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
        public async Task AddAsync(MonoSyncedTransaction mst) =>
            await CreateAsync(mst);
        public async Task<MonoSyncedTransaction?> ExistsAsync(string monobankTransactionId, int accountId) =>
            await FindByCondition(m => m.MonoTransactionId == monobankTransactionId && m.AccountId == accountId, false).SingleOrDefaultAsync() ?? null;

        public async Task<List<MonoSyncedTransaction>> GetAllSyncedTransactionsAsync() =>
            await FindAll(false).ToListAsync();

        public async Task<MonoSyncedTransaction?> GetByIdAsync(int id) =>
                await FindByCondition(m => m.Id == id, false).SingleOrDefaultAsync() ?? null;

        public async Task<MonoSyncedTransaction?> GetByTransactionIdAsync(string monobankTransactionId, int accountId) =>
                await FindByCondition(m => m.MonoTransactionId == monobankTransactionId && m.AccountId == accountId, false).SingleOrDefaultAsync() ?? null;

    }
}
