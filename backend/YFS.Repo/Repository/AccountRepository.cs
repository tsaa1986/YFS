﻿
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateAccount(Account account) =>
            await CreateAsync(account);

       // public async Task <Account> GetAccount(int _accountId) =>
       //     await FindByConditionAsync(c => c.Id.Equals(_accountId), false);

        public async Task<IEnumerable<Account>> GetAccountsByGroup(int accountGroupId, 
            string userId, 
            bool trackChanges) =>
                FindByCondition(c => c.AccountGroupId.Equals(accountGroupId) 
                    && c.AccountIsEnabled.Equals(1), trackChanges);
        public async Task UpdateAccount(Account account) => 
            await UpdateAsync(account);
        public async Task<IEnumerable<Account>> GetOpenAccountsByUserId(string userId, bool trackChanges) =>
            await FindByCondition(c => c.AccountIsEnabled.Equals(1) && c.UserId.Equals(userId), trackChanges).OrderByDescending(c => c.Favorites).Include(c => c.Currency)
            .Include(p => p.AccountBalance)
            .Include(amb => amb.AccountsMonthlyBalance)
            .ToListAsync();
        public async Task<Account?> GetAccount(int _accountId, bool trackChanges) =>
            await FindByCondition(a => a.Id.Equals(_accountId), trackChanges)
                .Include(p => p.AccountBalance)
                .SingleOrDefaultAsync();

        public async Task<Account?> GetAccountWithCurrency(int _accountId, bool trackChanges) =>
            await FindByCondition(a => a.Id.Equals(_accountId), trackChanges)
                .Include(c => c.Currency)
                .Include(p => p.AccountBalance)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Account>> GetAccountsByUserId(string userId, bool trackChanges) =>
            await FindByCondition(c => c.UserId.Equals(userId), trackChanges)
                .OrderByDescending(c => c.Favorites)
                .Include(c => c.Currency)
                .Include(p => p.AccountBalance)
                .Include(amb => amb.AccountsMonthlyBalance)
                .ToListAsync();

        public async Task<Account?> GetExternalAccountById(string externalAccountId, string userId, bool trackChanges) =>
            await FindByCondition(c => c.UserId.Equals(userId) && c.ExternalId.Equals(externalAccountId), trackChanges)
                .Include(c => c.Currency)
                .Include(p => p.AccountBalance)
                .Include(amb => amb.AccountsMonthlyBalance).SingleOrDefaultAsync();
    }
}
