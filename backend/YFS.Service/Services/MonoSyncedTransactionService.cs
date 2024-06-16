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
    public class MonoSyncedTransactionService: BaseService
    {
        public MonoSyncedTransactionService(IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseService> logger,
            LanguageScopedService languageService)
            : base(repository, mapper, logger, languageService)
        {
        }
        public async Task<ServiceResult<bool>> SyncTransactionFromStatements(string xToken, string userId, IEnumerable<MonoTransaction>? transactions)
        {
            if (transactions == null)
            {
                return ServiceResult<bool>.Error("No transactions to sync.");
            }

            foreach (var transaction in transactions)
            {
                if (transaction == null)
                {
                    continue; // Skip null transactions
                }

                // Check if the transaction already exists in the MonoSyncedTransaction table
                //var existingTransaction = await _monoSyncedTransactionRepository.GetByTransactionIdAsync(transaction.Id);
                var existingTransaction = await _repository.MonoSyncedTransaction.GetByTransactionIdAsync(transaction.Id);

                if (existingTransaction != null)
                {
                    continue; // Skip already synced transactions
                }

                // Your logic to process the transaction

                // Save the successful sync status in the MonoSyncedTransaction table
                var syncedTransaction = new MonoSyncedTransaction
                {
                    //MonobankTransactionId = transaction.Id,
                    //UserId = userId,
                    // other fields
                };

                await _repository.MonoSyncedTransaction.AddAsync(syncedTransaction);
                await _repository.SaveAsync();
            }

            return ServiceResult<bool>.Success(true);
        }
    }
}
