using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Enums;
using YFS.Core.Models;

namespace YFS.Service.Interfaces
{ 
    public interface IAccountGroupRepository
    {
        Task CreateAccountGroupsDefaultForUser(string userId);
        Task CreateAccountGroupForUser(AccountGroup accountGroup);
        Task UpdateAccountGroupForUser(AccountGroup accountGroup);
        Task<IEnumerable<AccountGroup>> GetAccountGroupsForUser(string userId, string languageCode,bool trackChanges);
        Task<AccountGroup> GetAccountGroup(int accountGroupId, bool trackChanges);
    }
}
