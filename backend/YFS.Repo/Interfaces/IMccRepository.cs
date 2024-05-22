using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IMccRepository
    {
        Task CreateMccAsync(MerchantCategoryCode mcc);
        Task<MerchantCategoryCode?> GetMccByIdAsync(string code, bool trackChanges);
        Task<IEnumerable<MerchantCategoryCode>> GetAllMccsAsync(bool trackChanges);
        Task UpdateMccAsync(MerchantCategoryCode mcc);
        Task DeleteMccAsync(MerchantCategoryCode mcc);
    }
}
