using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonobankIntegrationApiController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        public MonobankIntegrationApiController(ITokenService tokenService, IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _tokenService = tokenService;
        }

        [HttpGet("token")]
        [Authorize]
        public async Task<IActionResult> GetApiTokenForUser()
        {
             string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
             var result = await _tokenService.GetTokenByNameForUser("apiMonoBank", userId);

             if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                else if (result.IsNotFound)
                {
                    return NotFound(result.ErrorMessage);
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
        }
        
        [HttpPost("token")]
        [Authorize]
        public async Task<IActionResult> CreateApiTokenForUser([FromBody] ApiTokenDto apiToken)
        {
            apiToken.UserId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var result = await _tokenService.CreateToken(apiToken);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            else if (result.IsNotFound)
            {
                return NotFound(result.ErrorMessage);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
        
        [HttpPut("token")]
        [Authorize]
        public async Task<IActionResult> UpdateApiTokenForUser([FromBody] ApiTokenDto apiToken)
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            if (userId.Equals(apiToken.UserId)) {
                var result = await _tokenService.UpdateToken(apiToken);

                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                else if (result.IsNotFound)
                {
                    return NotFound(result.ErrorMessage);
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            } else
            {
                BadRequest("This token not for this user");
            }

            return NoContent();
        }
    }
}
