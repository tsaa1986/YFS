
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

        public async Task<IEnumerable<AccountMonthlyBalance?>> GetAccountMonthlyBalanceAfterOperation(Operation _operation, bool trackChanges)
        {
            DateTime nextMonthOperationDate = _operation.OperationDate.AddMonths(1);
            DateTime dtSearchMonth = new DateTime(nextMonthOperationDate.Year, nextMonthOperationDate.Month, 1);
            var res = await FindByConditionAsync(amb => (amb.AccountId == _operation.AccountId) &&
                (amb.StartDateOfMonth >= dtSearchMonth), trackChanges)
                .Result.OrderByDescending(amb => amb.StartDateOfMonth).ToListAsync();
            return res;
        }


        public async Task<AccountMonthlyBalance?> GetAccountMonthlyBalanceBeforeOperation(Operation _operation, bool trackChanges) 
        {
            var res = await FindByConditionAsync(amb => (amb.AccountId == _operation.AccountId) &&
               (amb.StartDateOfMonth < _operation.OperationDate), trackChanges).Result.FirstOrDefaultAsync();

            return res;
        }

   

        public async Task<AccountMonthlyBalance?> CheckAccountMonthlyBalance(Operation _operation, bool trackChages) =>
            await FindByConditionAsync(amb => (amb.AccountId == _operation.AccountId) &&
            (amb.StartDateOfMonth.Month == _operation.OperationDate.Date.Month) && (amb.StartDateOfMonth.Year == _operation.OperationDate.Date.Year), trackChages)
            .Result.SingleOrDefaultAsync();

        public async Task<AccountMonthlyBalance?> GetAccountMonthlyBalanceById(int _id) => 
            await FindByConditionAsync(acb => acb.Id.Equals(_id), false)
                .Result
                .SingleOrDefaultAsync();
    }
}
