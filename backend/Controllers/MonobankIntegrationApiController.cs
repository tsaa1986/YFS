using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly IMonobankIntegrationApiService _monobankIntegrationApiService;
        public MonobankIntegrationApiController(ITokenService tokenService,
            IMonobankIntegrationApiService monobankIntegrationApiService,
            IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _tokenService = tokenService;
            _monobankIntegrationApiService = monobankIntegrationApiService;
        }

        [HttpGet("client-info")]
        [Authorize]
        public async Task<IActionResult> GetClientInfo()
        {
            try
            {
                string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);

                var tokenResult = await _tokenService.GetTokenByNameForUser("apiMonoBank", userId);
                if (!tokenResult.IsSuccess)
                {
                    return BadRequest("Failed to get API token for the user");
                }

                var clientInfoResult = await _monobankIntegrationApiService.GetClientInfo(tokenResult.Data.TokenValue);
                if (clientInfoResult.IsSuccess)
                {
                    return Ok(clientInfoResult.Data);
                }
                else
                {
                    return BadRequest("Failed to retrieve client info from Monobank API");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetClientInfo action: {ex}");

                return StatusCode(500, "An error occurred while processing your request");
            }
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

        [HttpGet("statements/{account}/{fromDate}/{toDate}")]
        [Authorize]
        public async Task<IActionResult> GetStatementsBetweenDates(string account, DateTime fromDate, DateTime toDate)
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);

            var tokenResult = await _tokenService.GetTokenByNameForUser("apiMonoBank", userId);
            if (!tokenResult.IsSuccess)
            {
                return BadRequest("Failed to get API token for the user");
            }

            var result = await _monobankIntegrationApiService.GetStatementsBetweenDates(tokenResult.Data.TokenValue, account, fromDate, toDate);

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
    }
}
