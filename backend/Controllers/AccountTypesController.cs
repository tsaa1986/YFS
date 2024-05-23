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
using YFS.Service.Services;
using Microsoft.Extensions.Logging;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypesController : BaseApiController
    {
        private readonly IAccountTypesService _accountTypesService;
        public AccountTypesController(IAccountTypesService accountTypesService,IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _accountTypesService = accountTypesService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAccountTypes(string language)
        {
            var result = await _accountTypesService.GetAccountTypes(language);

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
        //_logger.LogError($"Something went wrong in the {nameof(GetAccountTypes)} action {ex}");
    }
}
