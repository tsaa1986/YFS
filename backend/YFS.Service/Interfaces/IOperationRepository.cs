using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IOperationRepository
    {
        Task CreateOperation(Operation transaction);
        Task UpdateOperation(Operation transaction);
        Task<IEnumerable<Operation>> GetOperationsForAccount(string userId, int accountId, bool trackChanges);
        Task<IEnumerable<Operation>> GetOperationsForAccountGroup(string userId, int accountGroupId, bool trackChanges);
    }
}
