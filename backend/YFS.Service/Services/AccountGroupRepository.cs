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
        public async Task CreateAccountGroupsDefaultForUser(string userid) 
        {
            AccountGroup acFavorite = new AccountGroup
            {
                UserId = userid,
                AccountGroupNameEn = "Favorites",
                AccountGroupNameRu = "Избранное",
                AccountGroupNameUa = "Обрані"
            };

            AccountGroup ac = new AccountGroup
            {
                UserId = userid,
                AccountGroupNameEn = "Cash",
                AccountGroupNameRu = "Наличные",
                AccountGroupNameUa = "Готівка"
            };

            await CreateAsync(ac);// CreateAccountGroup(ac);
            await CreateAsync(acFavorite);
        }
        public Task<AccountGroup> GetAccountGroup(int accountGroupId, bool trackChanges)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<AccountGroup>> GetAccountGroupsForUser(string userId, bool trackChanges)
            => await FindByConditionAsync(c => c.UserId.Equals(userId), trackChanges).Result.OrderBy(c => c.GroupOrderBy).ToListAsync();
        public async Task UpdateAccountGroupForUser(AccountGroup accountGroup) => 
            await UpdateAsync(accountGroup);
    }
}
