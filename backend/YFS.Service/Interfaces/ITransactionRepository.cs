using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface ITransactionRepository
    {
        Task CreateTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionForAccount(string userId, int accountId, bool trackChanges);
        Task<IEnumerable<Transaction>> GetTransactionForAccountGroup(string userId, int accountGroupId, bool trackChanges);
    }
}
