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

            await CreateAsync(acGroup);// CreateAccountGroup(ac);
            await CreateAsync(acGroup_internetmoney);
            await CreateAsync(acGroup_bank);
        }
        public async Task<AccountGroup> GetAccountGroup(int accountGroupId, bool trackChanges)
           => await FindByConditionAsync(c => c.AccountGroupId.Equals(accountGroupId), trackChanges).Result.SingleOrDefaultAsync();
        
        public async Task<IEnumerable<AccountGroup>> GetAccountGroupsForUser(string userId, bool trackChanges)
            => await FindByConditionAsync(c => c.UserId.Equals(userId), trackChanges).Result.OrderBy(c => c.GroupOrderBy).ToListAsync();
        
        public async Task UpdateAccountGroupForUser(AccountGroup accountGroup) => 
            await UpdateAsync(accountGroup);
    }
}
