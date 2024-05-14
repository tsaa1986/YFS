using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface ITokenService
    {
        Task<ServiceResult<IEnumerable<ApiTokenDto>>> GetTokensForUser(string userId);
        Task<ServiceResult<ApiTokenDto>> GetTokenByNameForUser(string tokenName, string userId);
        Task<ServiceResult<ApiTokenDto>> CreateToken(ApiTokenDto token);
        Task<ServiceResult<ApiTokenDto>> UpdateToken(ApiTokenDto token);
    }
}
