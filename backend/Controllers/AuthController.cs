using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Service.Filters.ActionFilters;
using YFS.Service.Interfaces;

namespace YFS.Data.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        public AuthController(IRepositoryManager repository, 
            IMapper mapper) 
            : base(repository, mapper)
        {

        }

        /*
        private async Task<IActionResult> CreateAdminAccount()
        {
            const string adminUser = "Admin";
            const string adminPassword = "Secret123$";

            UserRegistrationDto user = new UserRegistrationDto {
                FirstName = "Administrator",
                LastName = "",
                UserName = adminUser,
                Password = adminPassword,
                Email = "admin@admin.com",
                PhoneNumber = ""};
            /*UserLoginDto user = new UserLoginDto
            {
                UserName = adminUser,
                Password = adminPassword
            };*/

          /*  var userResult = await _repository.UserAuthentication.RegisterUserAsync(user);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);

        }*/

        [HttpPost("sign-up")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await _repository.UserAuthentication.RegisterUserAsync(userRegistration);

            if (userResult.Succeeded)
            {
                UserLoginDto user = new UserLoginDto { UserName = userRegistration.UserName, Password = userRegistration.Password };

                var Token = await _repository.UserAuthentication.ValidateUserAsync(user);
                string user_id = _repository.UserAuthentication.GetUserId(user).Result;


                await _repository.AccountGroup.CreateAccountGroupsDefaultForUser(user_id);
                await _repository.SaveAsync();
            }


            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);          
        }

        [HttpPost("sign-in")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto user)
        {
            return !await _repository.UserAuthentication.ValidateUserAsync(user)
                ? Unauthorized()
                : Ok(new { Token = await _repository.UserAuthentication.CreateTokenAsync() });
        }
    }
}
