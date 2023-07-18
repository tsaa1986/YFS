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

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountMonthlyBalanceController : BaseApiController
    {
        public AccountMonthlyBalanceController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [HttpGet("{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountMonthlyBalanceByAccountId(int accountId)
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var accountMonthlyBalance = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceByAccountId(accountId, false);
                var accountMonthlyBalanceDto = _mapper.Map<IEnumerable<AccountMonthlyBalanceDto>>(accountMonthlyBalance);
                return Ok(accountMonthlyBalanceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }   
    }
}
