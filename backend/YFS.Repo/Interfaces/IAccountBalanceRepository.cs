using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IAccountBalanceRepository
    {
        Task CreateAccountBalance(AccountBalance accountBalance);
        Task UpdateAccountBalance(AccountBalance accountBalance);
        Task<AccountBalance?> GetAccountBalance(int _accountId);
    }
}
