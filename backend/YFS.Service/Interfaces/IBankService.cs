using YFS.Core.Models;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface IBankService
    {
        Task<ServiceResult<Bank>> GetBankByGLMFO(int glmfo);
    }
}
