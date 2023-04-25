﻿using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IOperationRepository
    {
        Task CreateOperation(Operation operation);
        Task UpdateOperation(Operation operation);
        Task RemoveOperation(Operation operation);
        Task<Operation?> GetOperationById(int operationId);
        Task<Operation?> GetTransferOperationById(int transferOperationId);
        Task<IEnumerable<Operation>> GetOperationsForAccountForPeriod(string userId, int accountId, DateTime startDate, DateTime endDate, bool trackChanges);
        Task<IEnumerable<Operation>> GetLast10OperationsForAccount(string userId, int accountId, bool trackChanges);
        Task<IEnumerable<Operation>> GetOperationsForAccountGroupForPeriod(string userId, int accountGroupId, bool trackChanges);
    }
}
