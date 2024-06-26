using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models.MonoIntegration;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class MonoSyncedTransactionService: BaseService, IMonoSyncedTransactionService
    {
        public MonoSyncedTransactionService(IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseService> logger,
            LanguageScopedService languageService)
            : base(repository, mapper, logger, languageService)
        {
        }
        public async Task<ServiceResult<MonoSyncedTransaction>> SaveSyncedTransaction(MonoTransaction mt, int accountId, int operationId)
        {
            //var operation = await _repository.Operation.GetOperationById("en",operationId, false); // Get Operation entity

            var syncedTransaction = new MonoSyncedTransaction
            {
                MonoTransactionId = mt.Id,
                AccountId = accountId,
                OperationId = operationId,
                SyncedOn = DateTime.UtcNow
            };

            await _repository.MonoSyncedTransaction.AddAsync(syncedTransaction); // Add to repository
            await _repository.SaveAsync(); // Save changes

            return ServiceResult<MonoSyncedTransaction>.Success(syncedTransaction);
        }
    }
}
