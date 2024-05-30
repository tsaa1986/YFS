using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IOperationRepository
    {
        Task CreateOperation(Operation operation);
        Task UpdateOperation(Operation operation);
        Task RemoveOperation(Operation operation);
        Task<Operation?> GetOperationByIdWithoutCategory(int operationId, bool trackChanges);
        Task<Operation?> GetOperationById(string languageCode,int operationId, bool trackChanges);
        Task<Operation?> GetTransferOperationById(string languageCode, int transferOperationId);
        Task<IEnumerable<Operation>> GetOperationsForAccountForPeriod(string languageCode, int accountId, DateTime startDate, DateTime endDate, bool trackChanges);
        Task<IEnumerable<Operation>> GetLast10OperationsForAccount(string languageCode, int accountId, bool trackChanges);
        Task<IEnumerable<Operation>> GetOperationsForAccountGroupForPeriod(string languageCode, int accountGroupId, bool trackChanges);
    }
}
