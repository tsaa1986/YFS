using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using YFS.Data.Models;

namespace YFS.Data.Repository
{
    public interface IAccountGroupRepository
    {
        // Task<IEnumerable<AccountGroup> GetAllByUser(int uderid);
        Task CreateAccountGroup(AccountGroup ac);
        Task CreateAccountGroupsDefaultForUser(string userid);        
    }
}
