using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        public OperationsService(IRepositoryManager repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<IActionResult> UpdateOperation(OperationDto operationDto)
        {
            try
            {
                var operation = _mapper.Map<Operation>(operationDto);

                var existingOperation = await _repository.Operation.GetOperationById(operation.Id, trackChanges: true);

                if (existingOperation == null)
                {
                    return new NotFoundObjectResult($"Operation with Id = {operation.Id} not found");
                }

                _mapper.Map(operation, existingOperation);

                await _repository.Operation.UpdateOperation(existingOperation);
                await _repository.SaveAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        private async Task<AccountMonthlyBalance> AddAccountMonthlyBalance(Account _account, Operation operation)
        {
            decimal monthCredit = 0;
            decimal monthDebit = 0;

            if (operation.CurrencyAmount > 0)
            {
                monthDebit = operation.CurrencyAmount;
            }
            else
            {
                monthCredit = operation.CurrencyAmount;
            }

            decimal openingBalance = CalculateOpeningBalance(_account, operation);

            var accountMonthlyBalance = new AccountMonthlyBalance
            {
                OpeningMonthBalance = openingBalance,
                ClosingMonthBalance = openingBalance + monthDebit + monthCredit,
                StartDateOfMonth = new DateTime(operation.OperationDate.Year, operation.OperationDate.Month, 1),
                MonthNumber = operation.OperationDate.Month,
                YearNumber = operation.OperationDate.Year,
                MonthCredit = monthCredit,
                MonthDebit = monthDebit
            };

            AddMonthlyBalanceToAccount(_account, accountMonthlyBalance);

            return accountMonthlyBalance;
        }
    
        private decimal CalculateOpeningBalance(Account account, Operation _operation)
        {
            var accountMonthlyBalanceBeforeOperationMonth = _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceBeforeOperation(_operation, false);
            decimal openingBalance = 0;

            if (accountMonthlyBalanceBeforeOperationMonth.Result != null)
            {
                openingBalance = accountMonthlyBalanceBeforeOperationMonth.Result.ClosingMonthBalance;
            }
            else
            {
                openingBalance = account.AccountBalance.Balance;
            }

            return openingBalance;
        }
        private void AddMonthlyBalanceToAccount(Account account, AccountMonthlyBalance accountMonthlyBalance)
        {
            lock (account.AccountsMonthlyBalance)
            {
                account.AccountsMonthlyBalance.Add(accountMonthlyBalance);
            }
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

        private async Task ChangeAccountBalanceNew(Operation operation, decimal amount)
        {
            if (amount != 0)
            {
                var account = operation.Account;
                account.AccountBalance.Balance -= amount;
                await _repository.AccountBalance.UpdateAccountBalance(account.AccountBalance);
            }
        }
        private async Task ChangeAccountMonthlyBalances(Operation operation, IEnumerable<AccountMonthlyBalance> balances, bool isDebit)
        {
            if (balances.Any())
            {
                var amount = isDebit ? -operation.CurrencyAmount : operation.CurrencyAmount;

                foreach (var balance in balances)
                {
                    balance.ClosingMonthBalance += amount;
                    if (operation.OperationDate.Year == balance.StartDateOfMonth.Year &&
                        operation.OperationDate.Month == balance.StartDateOfMonth.Month)
                    {
                        if (isDebit)
                        {
                            balance.MonthDebit += amount;
                        }
                        else
                        {
                            balance.MonthCredit -= amount;
                        }
                    }
                    else
                    {
                        balance.OpeningMonthBalance += amount;
                    }

                    await _repository.AccountMonthlyBalance.UpdateAccountMonthlyBalance(balance);
                }
            }
        }
        public async Task<ActionResult> RemoveTransferOperation(int operationId)
        {
            try
            {
                var operationData = await _repository.Operation.GetOperationByIdWithoutCategory(operationId, false);

                if (operationData == null)
                {
                    return new NotFoundObjectResult($"Operation with Id = {operationId} not found");
                }
                Account accountWithdraw = null;
                Account accountTarget = operationData.Account;
                var operationWithdrawData = (Operation)null;
                var operationIncomeData = (Operation)null;
                List<Operation> listOperationReturn = new List<Operation>();
                AccountMonthlyBalance accountWithdrawCurrentMonthlyBalance = null;
                AccountMonthlyBalance accountTargetCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationData, false);
                var updatedAccountTargetCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;
                var updatedAccountWithdrawCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;
                var listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationData, false);
                var listAccountWithdrawMonthlyBalanceAfterOperationMonth = (IEnumerable<AccountMonthlyBalance>)null;
                var updatedAccountTargetMonthlyBalancesAfter = (IEnumerable<AccountMonthlyBalance>)null;
                var updatedAccountWithdrawMonthlyBalancesAfter = (IEnumerable<AccountMonthlyBalance>)null;

                if (operationData.CategoryId == -1)
                {
                    if (operationData.TransferOperationId > 0)
                    {
                        operationIncomeData = operationData;
                        operationWithdrawData = await _repository.Operation.GetOperationByIdWithoutCategory(operationIncomeData.TransferOperationId, false);

                        if (operationWithdrawData != null)
                        {
                            accountWithdraw = operationWithdrawData.Account;
                            accountTarget = operationIncomeData.Account;
                        }
                        else return new NotFoundObjectResult($"Operation with Id = {operationIncomeData.TransferOperationId} not found");
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
                        else return new NotFoundObjectResult($"Operation with Id = {operationData.TransferOperationId} not found");
                    }

                    accountWithdrawCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationWithdrawData, false);
                    accountTargetCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationIncomeData, false);

                    listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationIncomeData, false);
                    listAccountWithdrawMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationWithdrawData, false);

                    #region update balance current month of operation
                    List<AccountMonthlyBalance> listAccountMonthly = new List<AccountMonthlyBalance>();
                    List<AccountMonthlyBalance> listAccountWithdrawMonthly = new List<AccountMonthlyBalance>();
                    listAccountMonthly.Add(accountTargetCurrentMonthlyBalance);
                    listAccountWithdrawMonthly.Add(accountWithdrawCurrentMonthlyBalance);
                    
                    updatedAccountTargetCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationIncomeData, listAccountMonthly, true);
                    updatedAccountWithdrawCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationWithdrawData, listAccountWithdrawMonthly, true);                    
                    #endregion

                    await ChangeAccountBalanceNew(operationWithdrawData, operationWithdrawData.CurrencyAmount);
                    await ChangeAccountBalanceNew(operationIncomeData, operationIncomeData.CurrencyAmount);

                    await ChangeAccountMonthlyBalances(operationIncomeData, listAccountMonthlyBalanceAfterOperationMonth, false);
                    await ChangeAccountMonthlyBalances(operationWithdrawData, listAccountWithdrawMonthlyBalanceAfterOperationMonth, true);

                    await _repository.Operation.RemoveOperation(operationIncomeData);
                    await _repository.Operation.RemoveOperation(operationWithdrawData);


                    listOperationReturn.Add(operationWithdrawData);
                    listOperationReturn.Add(operationIncomeData);

                    await _repository.SaveAsync();

                    var operationReturn = _mapper.Map<List<OperationDto>>(listOperationReturn);
                    return new OkObjectResult(operationReturn);
                } 
                else 
                {
                    return new NotFoundObjectResult($"Not transfer operation = {operationId}");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


    }
}
