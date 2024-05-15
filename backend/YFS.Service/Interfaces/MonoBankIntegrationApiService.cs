using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models.MonoIntegration;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
   public interface IMonobankIntegrationApiService
   {
        Task<ServiceResult<MonoClientInfoResponse>> GetClientInfo(string xToken);
   }
}
