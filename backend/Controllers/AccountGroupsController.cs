using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;
using YFS.Service.Services;

namespace YFS.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountGroupsController : BaseApiController
    {
        private readonly IAccountGroupsService _accountGroupsService;
        public AccountGroupsController(IAccountGroupsService accountGroupsService, 
            IRepositoryManager repository, IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {            
            _accountGroupsService = accountGroupsService;
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAccountGroupsForUser()
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var result = await _accountGroupsService.GetAccountGroupsForUser(userId);

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
        
        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateAccountGroupForUser([FromBody] AccountGroupDto accountGroup)
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var serviceResult = await _accountGroupsService.CreateAccountGroupForUser(accountGroup, userId);

            if (serviceResult.IsSuccess)
            {
                return Ok(serviceResult.Data);
            }
            else
            {
                return BadRequest(serviceResult.ErrorMessage);
            }
        }
 
        [HttpPut()]
        [Authorize]
        public async Task<IActionResult> UpdateGroupForUser([FromBody] AccountGroupDto accountGroup)
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var result = await _accountGroupsService.UpdateGroupForUser(accountGroup, userId);

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
