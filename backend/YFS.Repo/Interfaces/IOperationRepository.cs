using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IOperationRepository
    {
        Task CreateOperation(Operation operation);
        Task UpdateOperation(Operation operation);
        Task RemoveOperation(Operation operation);
        Task<Operation?> GetOperationById(int operationId, bool trackChanges);
        Task<Operation?> GetTransferOperationById(int transferOperationId);
        Task<IEnumerable<Operation>> GetOperationsForAccountForPeriod(int accountId, DateTime startDate, DateTime endDate, bool trackChanges);
        Task<IEnumerable<Operation>> GetLast10OperationsForAccount(int accountId, bool trackChanges);
        Task<IEnumerable<Operation>> GetOperationsForAccountGroupForPeriod(int accountGroupId, bool trackChanges);
    }
}
