using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using YFS.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksSyncController : BaseApiController
    {
        private readonly IBanksSyncService _banksSyncService;
        public BanksSyncController(IBanksSyncService banksSyncService, IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _banksSyncService = banksSyncService;
        }

        [HttpPost("sync/{country}")]
        public async Task<IActionResult> SyncBanks(string country)
        {
            try
            {
                var result = await _banksSyncService.SyncBanksAsync(country);
                return Ok(new { message = "Synchronization successful", details = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during synchronization", error = ex.Message });
            }
        }
    }
}
