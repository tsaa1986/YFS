using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models.MonoIntegration;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface IMonoSyncedTransactionService
    {
        Task<ServiceResult<bool>> SyncTransactionFromStatements(string xToken, string userId, IEnumerable<MonoTransaction>? transactions);

        Task<ServiceResult<bool>> SaveSyncedTransaction(MonoTransaction mt, int operationId);
    }
}
