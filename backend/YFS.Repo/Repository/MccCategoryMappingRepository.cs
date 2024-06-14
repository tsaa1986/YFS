using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class MccCategoryMappingRepository : RepositoryBase<MccCategoryMapping>, IMccCategoryMappingRepository
    {
        public MccCategoryMappingRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public Task CreateMccCategoryMappingAsync(MerchantCategoryCode mcc)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMccCategoryMappingAsync(MerchantCategoryCode mcc)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MerchantCategoryCode>> GetAllMccsCategoryMappingAsync(bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<MerchantCategoryCode?> GetCategoryMappingByMccCodeAsync(int mccCode)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMccCategoryMappingAsync(MerchantCategoryCode mcc)
        {
            throw new NotImplementedException();
        }
    }
}
