using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models.MonoIntegration;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
   public interface IMonobankIntegrationApiService
   {
        Task<ServiceResult<MonoClientInfoResponse>> GetClientInfo(string xToken);
        Task<ServiceResult<IEnumerable<MonoStatement>>> GetStatementsBetweenDates(string xToken, string account, DateTime fromDate, DateTime toDate);
        /// <summary>
        /// this method for adding accounts from monobank
        /// </summary>
        /// <param name="xToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<AccountDto>>> SynchronizeAccounts(string xToken, string userId);
   }
}
