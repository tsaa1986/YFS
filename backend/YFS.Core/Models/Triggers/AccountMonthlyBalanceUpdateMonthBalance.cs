using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;

namespace YFS.Core.Models.Triggers
{
    public class AccountMonthlyBalanceUpdateMonthBalance : IBeforeSaveTrigger<AccountMonthlyBalance>
    {
       /* readonly RepositoryContext _applicationDbContext;

        public AccountMonthlyBalanceUpdateMonthBalance(DbContext dbContext)
        {
            this._applicationDbContext = dbContext;
        }*/
        public Task BeforeSave(ITriggerContext<AccountMonthlyBalance> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Added)
            {
                //context.Entity.OpeningMonthBalance = 111;

            }

            return Task.CompletedTask;
        }
    }
}
