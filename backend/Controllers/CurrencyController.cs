using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : BaseApiController
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService currencyService, IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _currencyService = currencyService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrencies()
        {
            var result = await _currencyService.GetCurrencies();

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
