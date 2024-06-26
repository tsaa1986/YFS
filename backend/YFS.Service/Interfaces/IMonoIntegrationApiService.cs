using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models.MonoIntegration;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
   public interface IMonoIntegrationApiService: IMonoSyncRulesService
    {
        /// <summary>
        /// Отримання виписки за час від {from} до {to} часу в секундах в форматі Unix time. 
        /// Максимальний час, за який можливо отримати виписку — 31 доба + 1 година (2682000 секунд).
        ///
        ///Обмеження на використання функції — не частіше ніж 1 раз на 60 секунд.
        /// </summary>
        /// <param name="xToken"> string Token для особистого доступу до API</param>
        /// <param name="accountId"> string Ідентифікатор рахунку або банки з переліків Statement list або 0 - дефолтний рахунок</param>
        /// <param name="fromDate">string Example: 1546304461 Початок часу виписки.</param>
        /// <param name="toDate">string Example: 1546306461 Останній час виписки (якщо відсутній, буде використовуватись поточний час).</param>
        /// <returns></returns>
        Task<ServiceResult<MonoClientInfoResponse>> GetClientInfo(string xToken);
        /// <summary>
        /// for getting accounts from monobank
        /// </summary>
        /// <param name="xToken">string Token для особистого доступу до API</param>
        /// <param name="account"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<MonoTransaction>>> GetTransactions(string xToken, string account, DateTime fromDate, DateTime toDate);
        /// <summary>
        /// this method for adding accounts from monobank
        /// </summary>
        /// <param name="xToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<AccountDto>>> SyncAccounts(string xToken, string userId, MonoClientInfoResponse clientInfoResponse);
        Task<ServiceResult<IEnumerable<MonoTransaction>>> SyncTransactionFromStatements(string xToken, string userId, string externalIdAccount, IEnumerable<MonoTransaction> transactions);
    }
}
