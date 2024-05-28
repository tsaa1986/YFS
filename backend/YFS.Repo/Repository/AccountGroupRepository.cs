using Microsoft.EntityFrameworkCore;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountGroupRepository : RepositoryBase<AccountGroup>, IAccountGroupRepository
    {
        public AccountGroupRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
           
        }

        public async Task CreateAccountGroupForUser(AccountGroup accountGroup) =>
            await CreateAsync(accountGroup);
        public async Task CreateAccountGroupsDefaultForUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var cashGroup = new AccountGroup { UserId = userId };
                cashGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Cash", LanguageCode = "en" });
                cashGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Наличные", LanguageCode = "ru" });
                cashGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Готівка", LanguageCode = "ua" });
                await CreateAsync(cashGroup);

                var bankGroup = new AccountGroup { UserId = userId };
                bankGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Bank", LanguageCode = "en" });
                bankGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Банковские", LanguageCode = "ru" });
                bankGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Банківські", LanguageCode = "ua" });
                await CreateAsync(bankGroup);

                var internetGroup = new AccountGroup { UserId = userId };
                internetGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Internet", LanguageCode = "en" });
                internetGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Интернет", LanguageCode = "ru" });
                internetGroup.Translations.Add(new AccountGroupTranslation { AccountGroupName = "Інтернет", LanguageCode = "ua" });
                await CreateAsync(internetGroup);
            }
        }
        public async Task<AccountGroup> GetAccountGroup(int accountGroupId, bool trackChanges)
        {
            var query = await FindByConditionAsync(c => c.AccountGroupId == accountGroupId, trackChanges);
            return await query.Include(c => c.Translations).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<AccountGroup>> GetAccountGroupsForUser(string userId, bool trackChanges)
        {
            var accountGroupsQuery = await FindByConditionAsync(c => c.UserId.Equals(userId), trackChanges);
            var accountGroups = await accountGroupsQuery
                                    .Include(c => c.Translations)
                                    .ToListAsync();
            return accountGroups.OrderBy(c => c.GroupOrderBy);
        }
        public async Task UpdateAccountGroupForUser(AccountGroup accountGroup) => 
            await UpdateAsync(accountGroup);
    }
}
