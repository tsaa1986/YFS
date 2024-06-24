using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;

namespace YFS.Service.Interfaces
{ 
    public interface IMonoSyncedTransactionRepository
    {
        Task<MonoSyncedTransaction?> GetByTransactionIdAsync(string monobankTransactionId, int accountId);
        Task AddAsync(MonoSyncedTransaction msc);
        Task<bool> ExistsAsync(string monobankTransactionId,int accountId);
        Task<List<MonoSyncedTransaction>> GetAllSyncedTransactionsAsync();
        Task<MonoSyncedTransaction?> GetByIdAsync(int id);
    }
}
