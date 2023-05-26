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
using System.Linq;

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
            Task<AccountMonthlyBalance> accountMonthlyBalance = GetAccountMonthlyBalance(account, operationData);

            account.AccountBalance.Balance = account.AccountBalance.Balance + operationData.OperationAmount;            

            await _repository.Operation.CreateOperation(operationData);
            await _repository.Account.UpdateAccount(account);
            await _repository.SaveAsync();

            List<Operation> listOperationReturn = new List<Operation>();
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
                    };

                    Task<AccountMonthlyBalance> accountTargetMonthlyBalance = GetAccountMonthlyBalance(accountTarget, transferOperaitonData);

                    accountTarget.AccountBalance.Balance = Math.Abs(operationData.CurrencyAmount) + accountTarget.AccountBalance.Balance;

                    await _repository.Operation.CreateOperation(transferOperaitonData);
                    await _repository.Account.UpdateAccount(accountTarget);
                    await _repository.SaveAsync();
                    listOperationReturn.Add(transferOperaitonData);
                } 
            }

            var operationReturn = _mapper.Map<List<OperationDto>>(listOperationReturn);

            return Ok(operationReturn);
        }

        private async Task<AccountMonthlyBalance> GetAccountMonthlyBalance(Account _account, Operation _operation)
        {
            int month = _operation.OperationDate.Month;
            int year = _operation.OperationDate.Year;
            AccountMonthlyBalance accountMonthlyBalance = null;

            accountMonthlyBalance = _account.AccountsMonthlyBalance.FirstOrDefault<AccountMonthlyBalance>(amb => (amb.AccountId == _account.Id && amb.MonthNumber == month && amb.YearNumber == year));
                    //.FirstOrDefault<AccountMonthlyBalance>();    
            
            if (accountMonthlyBalance == null) {
                await AddAccountMonthlyBalance(_account, _operation);
            }
            else
            {
                await ChangeAccountMonthlyBalance(_account, _operation, accountMonthlyBalance);
            }

            return accountMonthlyBalance;
        }

        private async Task<bool> AddAccountMonthlyBalance(Account _account, Operation _operation)
        {
            decimal MonthCreadit = 0;
            decimal MonthDebit = 0;
            AccountMonthlyBalance accountMonthlyBalance = null;

            if (_operation.CurrencyAmount> 0)
            {
                MonthDebit = _operation.CurrencyAmount;
            } else MonthCreadit = _operation.CurrencyAmount;

            if ((_operation.OperationDate.Month == DateTime.Now.Month && _operation.OperationDate.Year == DateTime.Now.Year))
            {
                accountMonthlyBalance = new AccountMonthlyBalance
                {
                    OpeningMonthBalance = _account.AccountBalance.Balance,
                    ClosingMonthBalance = _account.AccountBalance.Balance + MonthDebit + MonthCreadit,
                    MonthNumber = _operation.OperationDate.Month,
                    YearNumber = _operation.OperationDate.Year,
                    MonthCredit = MonthCreadit,
                    MonthDebit = MonthDebit
                };
            }
            

            _account.AccountsMonthlyBalance.Add(accountMonthlyBalance);
            return true;
        }
        private async Task<bool> ChangeAccountMonthlyBalance(Account _account, Operation _operation, AccountMonthlyBalance _accountMonthlyBalance)
        {
            decimal MonthCreadit = 0;
            decimal MonthDebit = 0;
            //AccountMonthlyBalance accountMonthlyBalance = null;

            if (_operation.CurrencyAmount > 0)
            {
                MonthDebit = _operation.CurrencyAmount;
            }
            else MonthCreadit = _operation.CurrencyAmount;

            _accountMonthlyBalance.ClosingMonthBalance = _account.AccountBalance.Balance + MonthDebit + MonthCreadit;
            _accountMonthlyBalance.MonthDebit = _accountMonthlyBalance.MonthDebit + MonthDebit;
            _accountMonthlyBalance.MonthCredit = _accountMonthlyBalance.MonthCredit + MonthCreadit;

            //_account.AccountsMonthlyBalance..Add(_accountMonthlyBalance);
            return true;
        }

        [HttpGet("period/{accountId:int}/{startDate:datetime}/{endDate:datetime}")]
        [Authorize]
        public async Task<IActionResult> GetOperationsAccountForPeriod(int accountId, DateTime startDate, DateTime endDate)
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var operations = await _repository.Operation.GetOperationsForAccountForPeriod(accountId, startDate, endDate, trackChanges: false);
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
                var operations = await _repository.Operation.GetLast10OperationsForAccount(accountId, trackChanges: false);
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

                            //targetAccount.Balance = targetAccount.Balance - temp_CurrencyIncomeAmount;
                            //withdrawAccount.Balance = withdrawAccount.Balance - temp_CurrencyAmount;
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
                            //targetAccount.Balance = targetAccount.Balance - temp_CurrencyIncomeAmount;
                           // withdrawAccount.Balance = withdrawAccount.Balance - temp_CurrencyAmount;
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

                        //AccountMonthlyBalance accountMonthlyBalance1 = operationData.Account.AccountsMonthlyBalance.FirstOrDefault<AccountMonthlyBalance>(amb => (amb.MonthNumber == operationData.OperationDate.Month && amb.YearNumber == operationData.OperationDate.Year));

                        AccountMonthlyBalance accountMonthlyBalance = await GetAccountMonthlyBalance(targetAccount, operationData);

                        //targetAccount.AccountBalance.Balance = targetAccount.AccountBalance.Balance - temp_CurrencyAmount;
                        operationData.Account.AccountBalance.Balance = operationData.Account.AccountBalance.Balance - operationData.CurrencyAmount;                        
                        //operationData.Account.AccountBalance.Balance = operationData.Account.AccountBalance.Balance - temp_CurrencyAmount;
                        //targetAccount.AccountBalance.Balance = targetAccount.AccountBalance.Balance - temp_CurrencyAmount;
                        await _repository.AccountBalance.UpdateAccountBalance(operationData.Account.AccountBalance);
                        await _repository.Operation.RemoveOperation(operationData);
                        //await _repository.Account.UpdateAccount(targetAccount);

                        await _repository.SaveAsync();

                        List<Account> accountList = new List<Account>();
                        accountList.Add(targetAccount);

                        //var accountData = _mapper.Map<Account>(targetAccount);
                        var accountResult = _mapper.Map<List<Account>>(accountList);
                        //var accountReturn = _mapper.Map<AccountDto>(accountData);

                        


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
