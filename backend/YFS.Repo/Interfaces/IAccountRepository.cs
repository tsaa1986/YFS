using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IAccountRepository
    {
        Task CreateAccount(Account account);
        Task UpdateAccount(Account account);
        Task<IEnumerable<Account>> GetAccountsByGroup(int accountGroupId, string userId, bool trackChanges);
        Task<IEnumerable<Account>> GetOpenAccountsByUserId(string userId, bool trackChanges);
        Task<Account> GetAccount(int _accountId);
        Task<Account> GetAccountWithCurrency(int _accountId);
    }
}
