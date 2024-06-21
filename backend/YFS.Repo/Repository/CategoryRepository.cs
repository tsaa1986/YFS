using Microsoft.EntityFrameworkCore;
using YFS.Core.Enums;
using YFS.Core.Models;
using YFS.Core.Utilities;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
           
        }

        public async Task CreateCategoryForUser(Category category) =>
            await CreateAsync(category);
        public async Task CreateCategoryDefaultForUser(string userid) 
        {/*
            AccountGroup acGroup = new AccountGroup
            {
                UserId = userid,
                AccountGroupNameEn = "Cash",
                AccountGroupNameRu = "Наличные",
                AccountGroupNameUa = "Готівка"
            };
            AccountGroup acGroup_bank = new AccountGroup
            {
                UserId = userid,
                AccountGroupNameEn = "Bank accounts",
                AccountGroupNameRu = "Банковские счета",
                AccountGroupNameUa = "Банківські рахунки"
            };
            AccountGroup acGroup_internetmoney = new AccountGroup
            {
                UserId = userid,
                AccountGroupNameEn = "Internet accounts",
                AccountGroupNameRu = "Интернет счета",
                AccountGroupNameUa = "Інтернет рахунки"
            };

            await this.CreateAsync(acGroup); 
            await this.CreateAsync(acGroup_internetmoney);
            await this.CreateAsync(acGroup_bank);*/
        }
        public Task<Category> GetCategory(int accountGroupId, bool trackChanges)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Category>> GetCategoryForUser(string userId, string languageCode, bool trackChanges)
        {
            var categoriesQuery = FindByCondition(c => (c.UserId.Equals(userId)) || (c.UserId == null), trackChanges);
            var categories = await categoriesQuery
                .Include(c => c.Translations)
                .ToListAsync();

            return categories.OrderBy(c => c.Translations.FirstOrDefault(t => t.LanguageCode == languageCode)?.Name);
        }
        public async Task UpdateCategoryForUser(Category category) => 
            await UpdateAsync(category);

        public Task UpdateAccountGroupForUser(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAccountGroupsForUser(string userId, bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
