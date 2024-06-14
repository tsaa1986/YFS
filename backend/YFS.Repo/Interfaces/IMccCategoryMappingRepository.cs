using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IMccCategoryMappingRepository
    {
        Task CreateMccCategoryMappingAsync(MerchantCategoryCode mcc);
        Task<IEnumerable<MerchantCategoryCode>> GetAllMccsCategoryMappingAsync(bool trackChanges);
        Task<MerchantCategoryCode?> GetCategoryMappingByMccCodeAsync(int mccCode);
        Task UpdateMccCategoryMappingAsync(MerchantCategoryCode mcc);
        Task DeleteMccCategoryMappingAsync(MerchantCategoryCode mcc);
    }
}
