using Microsoft.AspNetCore.Mvc;
using YFS.Core.Dtos;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<UserAccountDto>> GetMe(string userId);
        Task<ServiceResult<UserAccountDto>> RegisterUser(UserRegistrationDto userRegistration);
        Task<ServiceResult<string>> Authenticate(UserLoginDto user);
    }
}
