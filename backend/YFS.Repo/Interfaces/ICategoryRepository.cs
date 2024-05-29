using YFS.Core.Enums;
using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface ICategoryRepository
    {
        Task CreateCategoryDefaultForUser(string userId);
        Task CreateCategoryForUser(Category category);
        Task UpdateCategoryForUser(Category category);
        Task<IEnumerable<Category>> GetCategoryForUser(string userId, string languageCode, bool trackChanges);
        //Task<Category> GetCategory(int categoryId, bool trackChanges);
    }
}
