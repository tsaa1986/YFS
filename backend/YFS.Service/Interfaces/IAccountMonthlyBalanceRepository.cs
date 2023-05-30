using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IAccountMonthlyBalanceRepository
    {
        Task CreateAccountMonthlyBalance(AccountMonthlyBalance accountMonthlyBalance);
        Task UpdateAccountMonthlyBalance(AccountMonthlyBalance accountMonthlyBalance);
        Task<AccountMonthlyBalance?> GetAccountMonthlyBalanceById(int _id);
        Task<List<AccountMonthlyBalance?>> GetAccountMonthlyBalanceAfterOperation(Operation _operation, bool trackChanges);
        Task<List<AccountMonthlyBalance?>> GetAccountMonthlyBalanceBeforeOperation(Operation _operation, bool trackChanges);
        Task<AccountMonthlyBalance?> CheckAccountMonthlyBalance(Operation _operation, bool trackChanges);
    }
}
