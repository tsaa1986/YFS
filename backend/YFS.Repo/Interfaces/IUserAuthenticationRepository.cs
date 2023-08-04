using Microsoft.AspNetCore.Identity;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface IUserAuthenticationRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userForRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto loginDto);
        Task<string> CreateTokenAsync();
        Task<string> GetUserId(UserLoginDto loginDto);
        Task<User> GetUserAccountById(string _userId);
    }
}
