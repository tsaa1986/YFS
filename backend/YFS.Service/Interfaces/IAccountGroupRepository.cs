using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IAccountGroupRepository
    {
        // Task<IEnumerable<AccountGroup> GetAllByUser(int uderid);
        Task CreateAccountGroup(AccountGroup ac);
        Task CreateAccountGroupsDefaultForUser(string userid);        
    }
}
