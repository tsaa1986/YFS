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
        //Task<ServiceResult<MonoSyncedTransaction>> SyncTransactionFromStatements(string xToken, string userId, int accountId, IEnumerable<MonoTransaction>? transactions);

        Task<ServiceResult<MonoSyncedTransaction>> SaveSyncedTransaction(MonoTransaction mt, int accountId, int operationId);
    }
}
