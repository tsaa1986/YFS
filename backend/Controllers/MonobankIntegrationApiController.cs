using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Enums;
using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;
using YFS.Core.Utilities;
using YFS.Service.Interfaces;

namespace YFS.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonobankIntegrationApiController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMonoIntegrationApiService _monobankIntegrationApiService;
        public MonobankIntegrationApiController(ITokenService tokenService,
            IMonoIntegrationApiService monobankIntegrationApiService,
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
        
        [HttpPost("importAccounts")]
        [Authorize]
        public async Task<IActionResult> ImportMonoBankAccounts()
        {
            try
            {
                string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);

                var tokenResult = await _tokenService.GetTokenByNameForUser("apiMonoBank", userId);
                if (!tokenResult.IsSuccess)
                {
                    return BadRequest("Failed to get API token for the user");
                }
                var clientInfoResponse = await _monobankIntegrationApiService.GetClientInfo(tokenResult.Data.TokenValue);
                if (!clientInfoResponse.IsSuccess)
                {
                    return BadRequest("Failed to get ClientInfo from monobank api");
                }
                var importAccountResult = await _monobankIntegrationApiService.SyncAccounts(tokenResult.Data.TokenValue, userId, clientInfoResponse.Data);
                if (importAccountResult.IsSuccess)
                {
                    return Ok(importAccountResult.Data);
                }
                if (importAccountResult.IsNotFound)
                    return Ok(new { Message = "Warning: " + importAccountResult.ErrorMessage });
                else
                {
                    return BadRequest("Failed to synchronize Accounts");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetClientInfo action: {ex}");

                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("statements/{account}/{fromDate}/{toDate}")]
        [Authorize]
        public async Task<IActionResult> GetTransactions(string account, DateTime fromDate, DateTime toDate)
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);

            var tokenResult = await _tokenService.GetTokenByNameForUser("apiMonoBank", userId);
            if (!tokenResult.IsSuccess)
            {
                return BadRequest("Failed to get API token for the user");
            }

            var result = await _monobankIntegrationApiService.GetTransactions(tokenResult.Data.TokenValue, account, fromDate, toDate);

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
        [HttpPost("rules")]
        [Authorize]
        public async Task<IActionResult> AddRule([FromBody] MonoSyncRule rule)
        {
            try
            {
                string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);

                var tokenResult = await _tokenService.GetTokenByNameForUser("apiMonoBank", userId);
                if (!tokenResult.IsSuccess)
                {
                    return BadRequest("Failed to get API token for the user");
                }

                var addRuleResult = await _monobankIntegrationApiService.AddRuleAsync(rule);
                return Ok(addRuleResult.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddRule action: {ex}");

                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
