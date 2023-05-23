using System;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggered;

namespace YFS.Core.Models.Triggers
{
    public class AccountBalanceUpdateDateTrigger : IBeforeSaveTrigger<AccountBalance> 
    {
        public Task BeforeSave(ITriggerContext<AccountBalance> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Modified)
            {
                context.Entity.LastUpdateTime = DateTime.Now;
                
            }

            return Task.CompletedTask;
        }
    }
}
