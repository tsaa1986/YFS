using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using YFS.Data.Dtos;


namespace YFS.Data.Repository
{
    public interface IUserAuthenticationRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userForRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto loginDto);
        Task<string> CreateTokenAsync();
        Task<string> GetUserId(UserLoginDto loginDto);
    }
}
