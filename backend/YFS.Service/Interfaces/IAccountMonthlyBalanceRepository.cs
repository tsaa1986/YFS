using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IAccountMonthlyBalanceRepository
    {
        Task CreateAccountMonthlyBalance(AccountMonthlyBalance accountMonthlyBalance);
        Task UpdateAccountMonthlyBalance(AccountMonthlyBalance accountMonthlyBalance);
    }
}
