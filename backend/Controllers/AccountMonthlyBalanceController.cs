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
using YFS.Core.Models.Triggers;
using System.Collections;
using YFS.Service.Services;
using Microsoft.Extensions.Logging;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountMonthlyBalanceController : BaseApiController
    {
        private readonly IAccountMonthlyBalanceService _accountMonthlyBalanceService;
        public AccountMonthlyBalanceController(IAccountMonthlyBalanceService accountMonthlyBalanceService, IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _accountMonthlyBalanceService = accountMonthlyBalanceService;
        }

        [HttpGet("{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountMonthlyBalanceByAccountId(int accountId)
        {
            var result = await _accountMonthlyBalanceService.GetAccountMonthlyBalanceByAccountId(accountId);

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
