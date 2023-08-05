using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : BaseApiController
    {
        public CurrencyController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [HttpGet()]
        public async Task<IActionResult> GetCurrencies()
        {
            try
            {
                var currency = await _repository.Currency.GetCurrencies(trackChanges: false);
                var currencyDto = _mapper.Map<IEnumerable<CurrencyDto>>(currency);
                return Ok(currencyDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
