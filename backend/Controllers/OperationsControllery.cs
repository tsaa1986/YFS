using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YFS.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using YFS.Core.Dtos;
using Microsoft.Extensions.Logging;
using YFS.Core.Utilities;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : BaseApiController
    {
        private readonly IOperationsService _operationsService;
        public OperationsController(IOperationsService operationsService, IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _operationsService = operationsService;
        }
        [HttpPost("{targetAccountId}")]
        [Authorize]
        public async Task<IActionResult> CreateOperation([FromBody] OperationDto operation, int targetAccountId)
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var serviceResult = await _operationsService.CreateOperation(operation, targetAccountId, userId);

            if (serviceResult.IsSuccess)
            {
                return Ok(serviceResult.Data);
            }
            else
            {
                return BadRequest(serviceResult.ErrorMessage);
            }
        }        
        [HttpGet("period/{accountId:int}/{startDate:datetime}/{endDate:datetime}")]
        [Authorize]
        public async Task<IActionResult> GetOperationsAccountForPeriod(int accountId, DateTime startDate, DateTime endDate)
        {
            var result = await _operationsService.GetOperationsAccountForPeriod(accountId, startDate, endDate);

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
        [HttpGet("last10/{accountId:int}")]
        [Authorize]
        public async Task<IActionResult> GetLast10OperationsAccount(int accountId)
        {
            var result = await _operationsService.GetLast10OperationsAccount(accountId);

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
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOperation([FromBody] OperationDto operationDto)
        {
            var result = await _operationsService.UpdateOperation(operationDto);

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
        [HttpDelete("{operationId:int}")]
        [Authorize]
        public async Task<IActionResult> RemoveOperation(int operationId)
        {
            var result = await _operationsService.RemoveOperation(operationId);

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
        [HttpDelete("transfer/{operationId:int}")]
        [Authorize]
        public async Task<ActionResult> RemoveTransferOperation(int operationId)
        {
            var result = await _operationsService.RemoveTransferOperation(operationId);

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


