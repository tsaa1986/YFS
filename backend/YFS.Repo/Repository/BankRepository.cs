using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class BankRepository : RepositoryBase<Bank>, IBankRepository
    {
        public BankRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Bank?> GetBankByGLMFO(int kodGLMFO) =>
            await FindByConditionAsync(b => b.GLMFO == kodGLMFO, false)
                    .Result.SingleOrDefaultAsync();

        public async Task UpdateBanksAsync(IEnumerable<Bank> updatedBanks)
        {
            foreach (var updatedBank in updatedBanks)
            {
                // Correctly using await to ensure that the operation completes and returns the result
                var bank = await RepositoryContext.Banks.FindAsync(updatedBank.GLMFO);
                if (bank != null)
                {
                    // Directly using the result, which is now properly awaited
                    RepositoryContext.Entry(bank).CurrentValues.SetValues(updatedBank);
                }
                else
                {
                    // Adding asynchronously where appropriate
                    await RepositoryContext.Banks.AddAsync(updatedBank);
                }
            }
        }
    }
}
