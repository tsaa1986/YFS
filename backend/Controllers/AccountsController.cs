﻿using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;

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
        public async Task<IActionResult> CreateAccountForUser([FromBody] AccountDto account)
        {
            var accountData = _mapper.Map<Account>(account);

            await _repository.Account.CreateAccount(accountData);
            await _repository.SaveAsync();

            var accountReturn = _mapper.Map<AccountDto>(accountData);
            return Ok(accountReturn);
        }

        [HttpGet("{accountGroupId}")]
        public async Task<IActionResult> GetAccountsByGroup(int accountGroupId)
        {
            try
            {
                var accounts = await _repository.Account.GetAccountsByGroup(accountGroupId, trackChanges: false);
                var accountDto = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
