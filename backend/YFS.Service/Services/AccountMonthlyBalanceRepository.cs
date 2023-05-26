
using Microsoft.EntityFrameworkCore;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountMonthlyBalanceRepository : RepositoryBase<AccountMonthlyBalance>, IAccountMonthlyBalanceRepository
    {
        public AccountMonthlyBalanceRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateAccountMonthlyBalance(AccountMonthlyBalance accountMonthlyBalance) =>
            await CreateAsync(accountMonthlyBalance);

        public async Task UpdateAccountMonthlyBalance(AccountMonthlyBalance accountMonthlyBalance) =>
            await UpdateAsync(accountMonthlyBalance); 
    }
}
