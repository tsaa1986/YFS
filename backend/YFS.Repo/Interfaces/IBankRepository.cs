using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IBankRepository
    {
        Task UpdateBanksAsync(IEnumerable<Bank> updatedBanks);
        Task<Bank?> GetBankByGLMFO(int kodGLMFO);
    }
}
