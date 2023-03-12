using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using YFS.Service.Interfaces;
using YFS.Core.Dtos;
using System.Collections.Generic;
using YFS.Core.Models;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypesController : BaseApiController
    {
        public AccountTypesController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountTypes()
        {
            try
            {
                var accountTypes = await _repository.AccountType.GetAllAccountTypes(trackChanges: false);
                var accountTypeDto = _mapper.Map<IEnumerable<AccountTypeDto>>(accountTypes);
                return Ok(accountTypeDto);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong in the {nameof(GetAccountTypes)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
