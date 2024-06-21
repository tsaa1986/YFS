using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;
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

        public Task CreateMccCategoryMappingAsync(MccCategoryMapping mcc)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMccCategoryMappingAsync(MccCategoryMapping mcc)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MccCategoryMapping>> GetAllMccsCategoryMappingAsync(bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public async Task<MccCategoryMapping?> GetCategoryMappingByMccCodeAsync(int mccCode) =>
            await FindByCondition(m => m.MccCode == mccCode, false).SingleOrDefaultAsync();

        public Task UpdateMccCategoryMappingAsync(MccCategoryMapping mcc)
        {
            throw new NotImplementedException();
        }
    }
}
