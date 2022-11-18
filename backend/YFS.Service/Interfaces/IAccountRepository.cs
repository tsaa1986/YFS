using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IAccountRepository
    {
        Task CreateAccount(Account account);
        Task UpdateAccount(Account account);
        Task<IEnumerable<Account>> GetAccountsByGroup(int accountGroupId, bool trackChanges);
        Task<Account> GetAccount(Account account);
    }
}
