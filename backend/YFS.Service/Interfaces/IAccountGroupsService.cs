using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Enums;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface IAccountGroupsService
    {
        Task<ServiceResult<IEnumerable<AccountGroupDto>>> GetAccountGroupsForUser(string userId, string languageCode);
        Task<ServiceResult<AccountGroupDto>> CreateAccountGroupForUser(AccountGroupDto accountGroup, string userId);
        Task<ServiceResult<AccountGroupDto>> UpdateGroupForUser(AccountGroupDto accountGroup, string userId);
    }
}
