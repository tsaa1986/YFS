using Microsoft.EntityFrameworkCore;
using System;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class MccRepository : RepositoryBase<MerchantCategoryCode>, IMccRepository
    {
        public MccRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateMccAsync(MerchantCategoryCode mcc) =>
            await CreateAsync(mcc);
        public async Task DeleteMccAsync(MerchantCategoryCode mcc) =>
            await RemoveAsync(mcc);
        public async Task<IEnumerable<MerchantCategoryCode>> GetAllMccsAsync(bool trackChanges) =>
            await FindAllAsync(trackChanges).Result.OrderBy(m => m.Code).ToListAsync();
        public async Task<MerchantCategoryCode?> GetMccByIdAsync(string code, bool trackChanges) =>
            await FindByConditionAsync(m => m.Code.Equals(code), trackChanges).Result.SingleOrDefaultAsync();
        public async Task UpdateMccAsync(MerchantCategoryCode mcc) =>
            await UpdateAsync(mcc);
    }
}
