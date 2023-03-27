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
            await _repository.Account.CreateAccount(accountData);
            await _repository.SaveAsync();

            var accountReturn = _mapper.Map<AccountDto>(accountData);
            return Ok(accountReturn);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountDto account)
        {
            //var accountData = HttpContext.Items["account"] as Account;
            var accountData = _mapper.Map<Account>(account);
            _mapper.Map(account, accountData);
            await _repository.Account.UpdateAccount(accountData);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
