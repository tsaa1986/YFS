using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IAccountTypeRepository
    {
        Task<IEnumerable<AccountType>> GetAllAccountTypes(bool trackChanges);
    }
}
