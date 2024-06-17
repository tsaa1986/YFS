using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IMccCategoryMappingRepository
    {
        Task CreateMccCategoryMappingAsync(MccCategoryMapping mcc);
        Task<IEnumerable<MccCategoryMapping>> GetAllMccsCategoryMappingAsync(bool trackChanges);
        Task<MccCategoryMapping?> GetCategoryMappingByMccCodeAsync(int mccCode);
        Task UpdateMccCategoryMappingAsync(MccCategoryMapping mcc);
        Task DeleteMccCategoryMappingAsync(MccCategoryMapping mcc);
    }
}
