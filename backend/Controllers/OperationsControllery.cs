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
    public class OperationsController : BaseApiController
    {
        public OperationsController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [HttpPost("{targetAccountId}")]
        [Authorize]
        public async Task<IActionResult> CreateOperation([FromBody] OperationDto operation, int targetAccountId)
        {
            var operationData = _mapper.Map<Operation>(operation);

            string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
            operationData.UserId = userid;
            
            Account account = await _repository.Account.GetAccount(operationData.AccountId);
            
            operationData.OperationCurrencyId = account.CurrencyId;
            if (operationData.TypeOperation == 1)
                operationData.OperationAmount = -operationData.OperationAmount;
            if (operationData.TypeOperation == 2)
                operationData.OperationAmount = Math.Abs(operationData.OperationAmount);
            if (operationData.TypeOperation == 3)
                operationData.OperationAmount = -operationData.OperationAmount;

            operationData.CurrencyAmount = operationData.OperationAmount;
            operationData.Balance = account.Balance + operationData.CurrencyAmount;
            account.Balance = operationData.CurrencyAmount + account.Balance;

            await _repository.Operation.CreateOperation(operationData);
            await _repository.Account.UpdateAccount(account);
            await _repository.SaveAsync();

            var operationReturn = _mapper.Map<OperationDto>(operationData);

            int transferOperationIdFromAccount = 0;
            if (operationData.TypeOperation == 3) //transfer Money
            {
                if (targetAccountId > 0)
                {
                    Account accountTarget = await _repository.Account.GetAccount(targetAccountId);

                    Operation transferOperaitonData = new Operation { UserId = userid,
                        TypeOperation = operationData.TypeOperation,
                        AccountId = targetAccountId,
                        CategoryId = -1,
                        TransferOperationId = operationData.Id,
                        OperationDate = operationData.OperationDate,
                        OperationCurrencyId = operationData.OperationCurrencyId,
                        CurrencyAmount = Math.Abs(operationData.OperationAmount),
                        OperationAmount = Math.Abs(operationData.OperationAmount),
                        Balance = accountTarget.Balance + Math.Abs(operationData.OperationAmount)
                    };

                    accountTarget.Balance = Math.Abs(operationData.CurrencyAmount) + accountTarget.Balance;

                    await _repository.Operation.CreateOperation(transferOperaitonData);
                    await _repository.Account.UpdateAccount(accountTarget);
                    await _repository.SaveAsync();
                } 
            }          

            return Ok(operationReturn);
        }

        [HttpGet("{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountsByGroup(int accountId)
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var operations = await _repository.Operation.GetOperationsForAccount(userid, accountId, trackChanges: false);
                var operationsDto = _mapper.Map<IEnumerable<OperationDto>>(operations);
                return Ok(operationsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOperation([FromBody] OperationDto operation)
        {
            //var accountData = HttpContext.Items["account"] as Account;
            var operationData = _mapper.Map<Operation>(operation);
            _mapper.Map(operation, operationData);
            await _repository.Operation.UpdateOperation(operationData);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
