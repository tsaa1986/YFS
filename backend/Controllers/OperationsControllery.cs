using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using YFS.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using YFS.Core.Models;
using YFS.Core.Dtos;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : BaseApiController
    {
        private readonly IOperationsService _operationsService;
        public OperationsController(IOperationsService operationsService, IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
            _operationsService = operationsService;
        }
        public enum OperationType
        {
            Expense = 1,
            Income = 2,
            Transfer  = 3
        }
        [HttpPost("{targetAccountId}")]
        [Authorize]
        public async Task<IActionResult> CreateOperation([FromBody] OperationDto operation, int targetAccountId)
        {
            var operationData = _mapper.Map<Operation>(operation);

            try
            {
                Account account = null;
                Account accountTarget = null;
                var accountMonthlyBalance = (AccountMonthlyBalance)null;
                Operation transferOperaitonData = null;

                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                operationData.UserId = userid;
                string transferWithdrawDescription = "";

                if ((operationData.AccountId == targetAccountId) && (operationData.CategoryId == -1))
                    return BadRequest("Target Account must be not equal Withdraw Account");

                account = await _repository.Account.GetAccount(operationData.AccountId);
                
                operationData.OperationCurrencyId = account.CurrencyId;

                if ((OperationType)operationData.TypeOperation == OperationType.Expense)
                    operationData.OperationAmount = -operationData.OperationAmount;
                if ((OperationType)operationData.TypeOperation == OperationType.Income)
                    operationData.OperationAmount = Math.Abs(operationData.OperationAmount);
                if ((OperationType)operationData.TypeOperation == OperationType.Transfer)
                {
                    accountTarget = await _repository.Account.GetAccount(targetAccountId);
                    operationData.OperationAmount = -operationData.OperationAmount;
                    operationData.Description = operationData.Description + " [to " + accountTarget.Name + "]";
                    transferWithdrawDescription = " [from " + account.Name + "]";
                }

                operationData.CurrencyAmount = operationData.OperationAmount;

                accountMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationData, false);
                var listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationData, false);
                //var updatedAccountCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;

                if (accountMonthlyBalance == null)
                {
                    await AddAccountMonthlyBalance(account, operationData);
                }
                else
                {
                    List<AccountMonthlyBalance> listAccountMonthly = new List<AccountMonthlyBalance>();
                    listAccountMonthly.Add(accountMonthlyBalance);

                    ChangeAccountMonthlyBalance(operationData, listAccountMonthly, false);
                }

                ChangeAccountMonthlyBalance(operationData, listAccountMonthlyBalanceAfterOperationMonth, false);

                account.AccountBalance.Balance += operationData.OperationAmount;

                await _repository.Operation.CreateOperation(operationData);
                await _repository.Account.UpdateAccount(account);
                await _repository.SaveAsync();

                var operationReturnData = await _repository.Operation.GetOperationById(operationData.Id, false);

                var operationDataReturnDto = _mapper.Map<OperationDto>(operationReturnData);

                List<OperationDto> listOperationReturn = new List<OperationDto>();
                listOperationReturn.Add(operationDataReturnDto);

                if (operationData.TypeOperation == 3) //transfer Money
                {
                    if (targetAccountId > 0)
                    {
                        var accountTargetMonthlyBalance = (AccountMonthlyBalance)null;
                        var listAccountTargetMonthlyBalanceAfterOperationMonth = (IEnumerable<AccountMonthlyBalance>)null;
                        //accountTarget = await _repository.Account.GetAccount(targetAccountId);

                        transferOperaitonData = new Operation
                        {
                            UserId = userid,
                            TypeOperation = operationData.TypeOperation,
                            AccountId = targetAccountId,
                            CategoryId = -1,
                            TransferOperationId = operationData.Id,
                            OperationDate = operationData.OperationDate,
                            OperationCurrencyId = operationData.OperationCurrencyId,
                            CurrencyAmount = Math.Abs(operationData.OperationAmount),
                            OperationAmount = Math.Abs(operationData.OperationAmount),
                        };

                        accountTargetMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(transferOperaitonData, false);
                        listAccountTargetMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(transferOperaitonData, false);


                        if (accountTargetMonthlyBalance == null)
                        {
                            await AddAccountMonthlyBalance(accountTarget, transferOperaitonData);
                        }
                        else
                        {
                            List<AccountMonthlyBalance> listAccountMonthly = new List<AccountMonthlyBalance>() { accountTargetMonthlyBalance };
                            ChangeAccountMonthlyBalance(transferOperaitonData, (IEnumerable<AccountMonthlyBalance>)listAccountMonthly, false);
                        }

                        ChangeAccountMonthlyBalance(transferOperaitonData, listAccountTargetMonthlyBalanceAfterOperationMonth, false);
                        accountTarget.AccountBalance.Balance += Math.Abs(operationData.CurrencyAmount);
                        transferOperaitonData.Description = transferWithdrawDescription;
                        await _repository.Account.UpdateAccount(accountTarget);
                        await _repository.Operation.CreateOperation(transferOperaitonData);
                        await _repository.SaveAsync();

                        transferOperaitonData = await _repository.Operation.GetOperationById(transferOperaitonData.Id, false);
                        var operationTransferDataReturnDto = _mapper.Map<OperationDto>(transferOperaitonData);

                        listOperationReturn.Add(operationTransferDataReturnDto);
                    }
                }

                var operationReturn = _mapper.Map<List<OperationDto>>(listOperationReturn);

                return Ok(operationReturn);
            }
            catch (Exception e)
            {
                if ((operationData.Id > 0) && (operationData.TypeOperation == 3))
                {
                    var errorObject = new
                    {
                        ErrorMessage = e.Message,
                        operationId = operationData.Id
                    };
                    return BadRequest(errorObject);
                }
                else return BadRequest(e.Message);
            }
        }

        private async Task<AccountMonthlyBalance> AddAccountMonthlyBalance(Account _account, Operation _operation)
        {
            decimal MonthCreadit = 0;
            decimal MonthDebit = 0;
            AccountMonthlyBalance accountMonthlyBalance = null;

            if (_operation.CurrencyAmount > 0)
            {
                MonthDebit = _operation.CurrencyAmount;
            } else MonthCreadit = _operation.CurrencyAmount;

            if ((_operation.OperationDate.Month == DateTime.Now.Month && _operation.OperationDate.Year == DateTime.Now.Year))
            {
                accountMonthlyBalance = new AccountMonthlyBalance
                {
                    OpeningMonthBalance = _account.AccountBalance.Balance,
                    ClosingMonthBalance = _account.AccountBalance.Balance + MonthDebit + MonthCreadit,
                    StartDateOfMonth = new DateTime(_operation.OperationDate.Year, _operation.OperationDate.Month, 1),
                    MonthNumber = _operation.OperationDate.Month,
                    YearNumber = _operation.OperationDate.Year,
                    MonthCredit = MonthCreadit,
                    MonthDebit = MonthDebit
                };
            }
            else
            {
                var accountMonthlyBalanceBeforeOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceBeforeOperation(_operation, false);
                decimal openningBalance = 0;
                if (accountMonthlyBalanceBeforeOperationMonth != null)
                {
                    openningBalance = accountMonthlyBalanceBeforeOperationMonth.ClosingMonthBalance;
                }
                else
                {
                    //calculated operation before !!!
                }

                accountMonthlyBalance = new AccountMonthlyBalance
                {
                    OpeningMonthBalance = openningBalance,
                    ClosingMonthBalance = openningBalance + MonthDebit + MonthCreadit,
                    StartDateOfMonth = new DateTime(_operation.OperationDate.Year, _operation.OperationDate.Month, 1),
                    MonthNumber = _operation.OperationDate.Month,
                    YearNumber = _operation.OperationDate.Year,
                    MonthCredit = MonthCreadit,
                    MonthDebit = MonthDebit
                };
            }


            _account.AccountsMonthlyBalance.Add(accountMonthlyBalance);
            return accountMonthlyBalance;
        }
        private IEnumerable<AccountMonthlyBalance> ChangeAccountMonthlyBalance(Operation _operation, IEnumerable<AccountMonthlyBalance> _accountMonthlyBalances, Boolean _removeOperation)
        {
            decimal MonthCreadit = 0;
            decimal MonthDebit = 0;

            if (_operation.CurrencyAmount > 0)
            {
                if (_removeOperation == true)
                {
                    MonthDebit = -_operation.CurrencyAmount;
                }
                else { MonthDebit = _operation.CurrencyAmount; }
            }
            else
            {
                if (_removeOperation == true)
                {
                    MonthCreadit = -_operation.CurrencyAmount;
                }
                else
                {
                    MonthCreadit = _operation.CurrencyAmount;
                }
            }

            if (_accountMonthlyBalances.Count() > 0)
            {
                foreach (AccountMonthlyBalance amb in _accountMonthlyBalances)
                {
                    DateTime monthOperation = new DateTime(_operation.OperationDate.Year, _operation.OperationDate.Month, 1);
                    amb.ClosingMonthBalance = amb.ClosingMonthBalance + MonthDebit + MonthCreadit;

                    if ((_operation.OperationDate.Year == amb.StartDateOfMonth.Year) && (_operation.OperationDate.Month == amb.StartDateOfMonth.Month))
                    {
                        amb.MonthDebit = amb.MonthDebit + MonthDebit;
                        amb.MonthCredit = amb.MonthCredit + MonthCreadit;
                    }
                    else
                    {
                        amb.OpeningMonthBalance = amb.OpeningMonthBalance + MonthDebit + MonthCreadit;
                    }

                    _repository.AccountMonthlyBalance.UpdateAccountMonthlyBalance(amb);
                }
            }

            return _accountMonthlyBalances;
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
        public async Task<IActionResult> UpdateOperation([FromBody] OperationDto operationDto)
        {
            var result = await _operationsService.UpdateOperation(operationDto);
            return result;
        }

        [HttpDelete("{operationId:int}")]
        [Authorize]
        public async Task<ActionResult> RemoveOperation(int operationId)
        {
            try
            {
                var operationData = await _repository.Operation.GetOperationById(operationId, false);

                if (operationData == null)
                {
                    return NotFound($"Operation with Id = {operationId} not found");
                }
                //check if transfer before delete operation, remove child/parent operation
                //change ballance account after remove operation
                Account accountWithdraw = null;
                Account accountTarget = operationData.Account;

                AccountMonthlyBalance accountWithdrawCurrentMonthlyBalance = null;
                AccountMonthlyBalance accountTargetCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationData, false);
                var updatedAccountTargetCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;
                var updatedAccountWithdrawCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;

                var operationWithdrawData = (Operation)null;
                var operationIncomeData = (Operation)null;
                var listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationData, false);
                var listAccountWithdrawMonthlyBalanceAfterOperationMonth = (IEnumerable<AccountMonthlyBalance>)null;
                var updatedAccountTargetMonthlyBalancesAfter = (IEnumerable<AccountMonthlyBalance>)null;
                var updatedAccountWithdrawMonthlyBalancesAfter = (IEnumerable<AccountMonthlyBalance>)null;
                List<Operation> listOperationReturn = new List<Operation>();

                if (operationData.CategoryId == -1)
                {
                    if (operationData.TransferOperationId > 0)
                    {
                        operationIncomeData = operationData;
                        operationWithdrawData = await _repository.Operation.GetOperationById(operationData.TransferOperationId, false);

                        if (operationWithdrawData != null)
                        {
                            accountWithdraw = operationWithdrawData.Account;
                            accountTarget = operationIncomeData.Account;
                        }
                        else return NotFound($"Operation with Id = {operationData.TransferOperationId} not found");
                    }
                    else
                    {
                        operationWithdrawData = operationData;
                        operationIncomeData = await _repository.Operation.GetTransferOperationById(operationWithdrawData.Id);

                        if (operationIncomeData != null)
                        {
                            accountWithdraw = operationWithdrawData.Account;
                            accountTarget = operationIncomeData.Account;
                        }
                        else return NotFound($"Operation with Id = {operationData.TransferOperationId} not found");
                    }

                    accountTarget.AccountBalance.Balance = accountTarget.AccountBalance.Balance - operationIncomeData.CurrencyAmount;
                    accountWithdraw.AccountBalance.Balance = accountWithdraw.AccountBalance.Balance - operationWithdrawData.CurrencyAmount;

                    accountWithdrawCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationWithdrawData, false);
                    accountTargetCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationIncomeData, false);

                    listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationIncomeData, false);
                    listAccountWithdrawMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationWithdrawData, false);

                    #region update balance current month of operation
                    List<AccountMonthlyBalance> listAccountMonthly = new List<AccountMonthlyBalance>();
                    List<AccountMonthlyBalance> listAccountWithdrawMonthly = new List<AccountMonthlyBalance>();
                    listAccountMonthly.Add(accountTargetCurrentMonthlyBalance);
                    listAccountWithdrawMonthly.Add(accountWithdrawCurrentMonthlyBalance);

                    updatedAccountTargetCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationIncomeData, (IEnumerable<AccountMonthlyBalance>)listAccountMonthly, true);
                    updatedAccountWithdrawCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationWithdrawData, (IEnumerable<AccountMonthlyBalance>)listAccountWithdrawMonthly, true);

                    //if (updatedAccountTargetCurrentMonthlyBalance.Count() > 0)
                    //    foreach (AccountMonthlyBalance a in updatedAccountTargetCurrentMonthlyBalance) { _repository.AccountMonthlyBalance.UpdateAccountMonthlyBalance(a); }
                    //if (updatedAccountWithdrawCurrentMonthlyBalance.Count() > 0)
                    //    foreach (AccountMonthlyBalance a in updatedAccountWithdrawCurrentMonthlyBalance) { _repository.AccountMonthlyBalance.UpdateAccountMonthlyBalance(a); }
                    #endregion

                    if (listAccountMonthlyBalanceAfterOperationMonth.Count() > 0)
                        updatedAccountTargetMonthlyBalancesAfter = ChangeAccountMonthlyBalance(operationIncomeData, listAccountMonthlyBalanceAfterOperationMonth, true);
                    if (listAccountWithdrawMonthlyBalanceAfterOperationMonth.Count() > 0)
                        updatedAccountWithdrawMonthlyBalancesAfter = ChangeAccountMonthlyBalance(operationWithdrawData, listAccountWithdrawMonthlyBalanceAfterOperationMonth, true);

                    await _repository.AccountBalance.UpdateAccountBalance(operationWithdrawData.Account.AccountBalance);
                    await _repository.AccountBalance.UpdateAccountBalance(operationIncomeData.Account.AccountBalance);

                    await _repository.Operation.RemoveOperation(operationWithdrawData);
                    await _repository.Operation.RemoveOperation(operationIncomeData);

                    listOperationReturn.Add(operationWithdrawData);
                    listOperationReturn.Add(operationIncomeData);
                }
                else
                {
                    accountTarget.AccountBalance.Balance = accountTarget.AccountBalance.Balance - operationData.CurrencyAmount;

                    if (listAccountMonthlyBalanceAfterOperationMonth.Count() > 0)
                        updatedAccountTargetMonthlyBalancesAfter = ChangeAccountMonthlyBalance(operationData, listAccountMonthlyBalanceAfterOperationMonth, true);

                    List<AccountMonthlyBalance> listAccountMonthlyBalance = new List<AccountMonthlyBalance>();
                    listAccountMonthlyBalance.Add(accountTargetCurrentMonthlyBalance);
                    updatedAccountTargetCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationData, (IEnumerable<AccountMonthlyBalance>)listAccountMonthlyBalance, true);

                    await _repository.Operation.RemoveOperation(operationData);
                    await _repository.AccountBalance.UpdateAccountBalance(accountTarget.AccountBalance);

                    listOperationReturn.Add(operationData);
                }

                await _repository.SaveAsync();
                List<Account> accountList = new List<Account>();
                accountList.Add(accountTarget);
                if (accountWithdraw != null)
                    accountList.Add(accountWithdraw);

                var operationReturn = _mapper.Map<List<OperationDto>>(listOperationReturn);

                return Ok(operationReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("transfer/{operationId:int}")]
        [Authorize]
        public async Task<ActionResult> RemoveTransferOperation(int operationId)
        {
            var result = await _operationsService.RemoveTransferOperation(operationId);
            return result;
        }
    }
 }


