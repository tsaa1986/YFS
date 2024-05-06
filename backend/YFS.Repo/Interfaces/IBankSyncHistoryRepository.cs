using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models;

namespace YFS.Repo.Interfaces
{
    public interface IBankSyncHistoryRepository
    {
        Task AddOrUpdateHistoryAsync(BankSyncHistory history);
    }
}
