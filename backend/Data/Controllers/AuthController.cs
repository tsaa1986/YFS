using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YFS.Data.Dtos;
using YFS.Data.Repository;
using YFS.Filters.ActionFilters;

namespace YFS.Data.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        public AuthController(IRepositoryManager repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

        [HttpPost("sign-up")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {

            var userResult = await _repository.UserAuthentication.RegisterUserAsync(userRegistration);
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
