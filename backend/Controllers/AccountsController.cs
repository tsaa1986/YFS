using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService,IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAccountForUser([FromBody] AccountDto account)
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var serviceResult = await _accountService.CreateAccountForUser(account, userId);

            if (serviceResult.IsSuccess)
            {
                return Ok(serviceResult.Data);
            }
            else
            {
                return BadRequest(serviceResult.ErrorMessage);
            }
        }

        [HttpGet("byId/{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountById(int accountId)
        {
            var result = await _accountService.GetAccountById(accountId);

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

        [HttpGet("{accountGroupId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountsByGroup(int accountGroupId)
        {
            string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var result = await _accountService.GetAccountsByGroup(accountGroupId, userid, trackChanges: false);

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
        
        [HttpGet("openAccountsByUserId")]
        [Authorize]
        public async Task<IActionResult> GetOpenAccountsByUserId()
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var result = await _accountService.GetOpenAccountsByUserId(userId, trackChanges: false);

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

        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountDto account)
        {
            var result = await _accountService.UpdateAccount(account);

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
