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
using System.Security.Principal;

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

            List<Operation> listOperationReturn= new List<Operation>();
            listOperationReturn.Add(operationData);         

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
                    listOperationReturn.Add(transferOperaitonData);
                } 
            }

            var operationReturn = _mapper.Map<List<OperationDto>>(listOperationReturn);

            return Ok(operationReturn);
        }

        [HttpGet("period/{accountId:int}/{startDate:datetime}/{endDate:datetime}")]
        [Authorize]
        public async Task<IActionResult> GetOperationsAccountForPeriod(int accountId, DateTime startDate, DateTime endDate)
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var operations = await _repository.Operation.GetOperationsForAccountForPeriod(userid, accountId, startDate, endDate, trackChanges: false);
                var operationsDto = _mapper.Map<IEnumerable<OperationDto>>(operations);
                return Ok(operationsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("last10/{accountId:int}")]
        [Authorize]
        public async Task<IActionResult> GetLast10OperationsAccount(int accountId)
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var operations = await _repository.Operation.GetLast10OperationsForAccount(userid, accountId, trackChanges: false);
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

        [HttpDelete("{operationId:int}")]
        [Authorize]
        public async Task<ActionResult> RemoveOperation(int operationId)
        {
            try
            {
                var operationData = await _repository.Operation.GetOperationById(operationId);

                if (operationData == null)
                {
                    return NotFound($"Operation with Id = {operationId} not found");
                }
                //check if transfer before delete operation, remove child/parent operation
                //change ballance account after remove operation
                Account withdrawAccount, targetAccount = null;

                var operationWithdrawData = (Operation)null;
                var operationIncomeData = (Operation)null;

                if (operationData.CategoryId == -1)
                {                
                    if (operationData.TransferOperationId > 0)
                    {                       
                        operationWithdrawData = await _repository.Operation.GetOperationById(operationData.TransferOperationId);
                        operationIncomeData = operationData;

                        if (operationWithdrawData != null)
                        {
                            withdrawAccount = await _repository.Account.GetAccount(operationWithdrawData.AccountId);
                            targetAccount = await _repository.Account.GetAccount(operationIncomeData.AccountId);
                            decimal temp_CurrencyAmount = operationWithdrawData.CurrencyAmount;
                            decimal temp_CurrencyIncomeAmount = operationIncomeData.CurrencyAmount;

                            targetAccount.Balance = targetAccount.Balance - temp_CurrencyIncomeAmount;
                            withdrawAccount.Balance = withdrawAccount.Balance - temp_CurrencyAmount;
                        }
                        else return NotFound($"Operation with Id = {operationData.TransferOperationId} not found");
                    }
                    else
                    {
                        operationWithdrawData = operationData;
                        operationIncomeData = await _repository.Operation.GetTransferOperationById(operationWithdrawData.Id);

                        if (operationIncomeData != null)
                        {
                            withdrawAccount = await _repository.Account.GetAccount(operationWithdrawData.AccountId);
                            targetAccount = await _repository.Account.GetAccount(operationIncomeData.AccountId);
                            decimal temp_CurrencyAmount = operationWithdrawData.CurrencyAmount;
                            decimal temp_CurrencyIncomeAmount = operationIncomeData.CurrencyAmount;
                            targetAccount.Balance = targetAccount.Balance - temp_CurrencyIncomeAmount;
                            withdrawAccount.Balance = withdrawAccount.Balance - temp_CurrencyAmount;
                        }
                        else return NotFound($"Operation with Id = {operationData.TransferOperationId} not found");
                    }

                    await _repository.Operation.RemoveOperation(operationWithdrawData);
                    await _repository.Operation.RemoveOperation(operationIncomeData);                    

                    await _repository.Account.UpdateAccount(targetAccount);
                    await _repository.Account.UpdateAccount(withdrawAccount);

                    List<Account> accountList = new List<Account>();
                    accountList.Add(targetAccount);
                    accountList.Add(withdrawAccount);

                    var accountResult = _mapper.Map<List<Account>>(accountList);

                    await _repository.SaveAsync();

                    return Ok(accountResult);
                }
                else
                {
                    if (operationData != null) { 
                        targetAccount = await _repository.Account.GetAccount(operationData.AccountId);
                        decimal temp_CurrencyAmount = operationData.CurrencyAmount;
                        targetAccount.Balance = targetAccount.Balance - temp_CurrencyAmount;
                        await _repository.Operation.RemoveOperation(operationData);
                        await _repository.Account.UpdateAccount(targetAccount);

                        List<Account> accountList = new List<Account>();
                        accountList.Add(targetAccount);

                        //var accountData = _mapper.Map<Account>(targetAccount);
                        var accountResult = _mapper.Map<List<Account>>(accountList);
                        //var accountReturn = _mapper.Map<AccountDto>(accountData);

                        await _repository.SaveAsync();


                        return Ok(accountResult);
                    }
                }
            }
            catch (Exception ex)
            {
                return  BadRequest(ex.Message);
            }
                return Ok();
        }

        private async Task<Account> ChangeAccountBalance(Account account, int operationId, int transferOperationId)
        {
            //Account accountTarget = await _repository.Account.GetAccount(targetAccountId);
            return null;
        }
    }

}
