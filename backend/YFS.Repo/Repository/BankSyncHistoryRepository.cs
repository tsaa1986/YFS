using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Repo.Interfaces;

namespace YFS.Repo.Repository
{
    public class BankSyncHistoryRepository: RepositoryBase<BankSyncHistory>, IBankSyncHistoryRepository
    {
        public BankSyncHistoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task AddOrUpdateHistoryAsync(BankSyncHistory history)
        {
            var existingHistory = await RepositoryContext.BankSyncHistories
                                    .FirstOrDefaultAsync(h => h.Country == history.Country);

            if (existingHistory != null)
            {
                existingHistory.LastUpdated = DateTime.UtcNow;
            }
            else
            {
                RepositoryContext.BankSyncHistories.Add(history);
            }

            await RepositoryContext.SaveChangesAsync();
        }
    }
}
