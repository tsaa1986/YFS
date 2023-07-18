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

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseApiController
    {
        public AccountsController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAccountForUser([FromBody] AccountDto account)
        {
            var accountData = _mapper.Map<Account>(account);

            string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
            accountData.UserId = userid;
            accountData.AccountBalance = new AccountBalance { Balance = account.Balance };
            await _repository.Account.CreateAccount(accountData);

            if (account.Balance != 0)
            {
                accountData.Operations = new List<Operation>() { {
                    new Operation {
                        AccountId = account.Id,
                        UserId= userid,
                        OperationAmount = account.Balance,
                        OperationCurrencyId = account.CurrencyId,
                        CurrencyAmount = account.Balance,
                        Description = "openning account",
                        TypeOperation = account.Balance > 0 ? 2 : 1,
                        CategoryId = -2, 
                        OperationDate = account.OpeningDate,
                    } } };
            }
            await _repository.SaveAsync();

            var accountReturn = _mapper.Map<AccountDto>(accountData);
            return Ok(accountReturn);
        }

        [HttpGet("byId/{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountById(int accountId)
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var account = await _repository.Account.GetAccount(accountId);
                var accountDto = _mapper.Map<AccountDto>(account);
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{accountGroupId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountsByGroup(int accountGroupId)
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var accounts = await _repository.Account.GetAccountsByGroup(accountGroupId, userid, trackChanges: false);
                var accountDto = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("openAccountsByUserId")]
        [Authorize]
        public async Task<IActionResult> GetOpenAccountsByUserId()
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var accounts = await _repository.Account.GetOpenAccountsByUserId(userid, trackChanges: false);
                var accountDto = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountDto account)
        {
            //var accountData = HttpContext.Items["account"] as Account;
            string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var accountData = _mapper.Map<Account>(account);
            accountData.UserId= userid;
            _mapper.Map(account, accountData);
            await _repository.Account.UpdateAccount(accountData);
            await _repository.SaveAsync();
            Account updatedAccount = await _repository.Account.GetAccount(accountData.Id);
            var accountDto = _mapper.Map<AccountDto>(updatedAccount);
            return Ok(accountDto);
        }
    }
}
