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
    {/*
        private readonly IOperationRepository _operationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountMonthlyBalanceRepository _accountMonthlyBalanceRepository;
        private readonly IAccountBalanceRepository _accountBalanceRepository;
        public OperationsService(
            IOperationRepository operationRepository,
            IAccountRepository accountRepository,
            IAccountMonthlyBalanceRepository accountMonthlyBalanceRepository,
            IAccountBalanceRepository accountBalanceRepository)
        {
            _operationRepository = operationRepository;
            _accountRepository = accountRepository;
            _accountMonthlyBalanceRepository = accountMonthlyBalanceRepository;
            _accountBalanceRepository = accountBalanceRepository;
        }*/
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        public OperationsService(IMapper mapper, IRepositoryManager repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<ActionResult> RemoveTransferOperation(int operationId)
        {
            try
            {
                var operationData = await _repository.Operation.GetOperationById(operationId, false);

                if (operationData == null)
                {
                    return NotFoundObjectResult($"Operation with Id = {operationId} not found");
                }
                //check if transfer before delete operation, remove child/parent operation
                //change ballance account after remove operation
                Account accountWithdraw = null;
                Account accountTarget = operationData.Account;

                AccountMonthlyBalance accountWithdrawCurrentMonthlyBalance = null;
                AccountMonthlyBalance accountTargetCurrentMonthlyBalance = await _repository.AccountMonthlyBalanceRepository.CheckAccountMonthlyBalance(operationData, false);
                var updatedAccountTargetCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;
                var updatedAccountWithdrawCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;

                var operationWithdrawData = (Operation)null;
                var operationIncomeData = (Operation)null;
                var listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalanceRepository.GetAccountMonthlyBalanceAfterOperation(operationData, false);
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

                    accountWithdrawCurrentMonthlyBalance = await _repository.AccountMonthlyBalanceRepository.CheckAccountMonthlyBalance(operationWithdrawData, false);
                    accountTargetCurrentMonthlyBalance = await _repository.AccountMonthlyBalanceRepository.CheckAccountMonthlyBalance(operationIncomeData, false);

                    listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationIncomeData, false);
                    listAccountWithdrawMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationWithdrawData, false);

                    #region update balance current month of operation
                    List<AccountMonthlyBalance> listAccountMonthly = new List<AccountMonthlyBalance>();
                    List<AccountMonthlyBalance> listAccountWithdrawMonthly = new List<AccountMonthlyBalance>();
                    listAccountMonthly.Add(accountTargetCurrentMonthlyBalance);
                    listAccountWithdrawMonthly.Add(accountWithdrawCurrentMonthlyBalance);

                    updatedAccountTargetCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationIncomeData, (IEnumerable<AccountMonthlyBalance>)listAccountMonthly, true);
                    updatedAccountWithdrawCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationWithdrawData, (IEnumerable<AccountMonthlyBalance>)listAccountWithdrawMonthly, true);

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
                return BadRequestObjectResult(ex.Message);
            }
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

        private async Task<AccountMonthlyBalance> AddAccountMonthlyBalance(Account _account, Operation _operation)
        {
            decimal MonthCreadit = 0;
            decimal MonthDebit = 0;
            AccountMonthlyBalance accountMonthlyBalance = null;

            if (_operation.CurrencyAmount > 0)
            {
                MonthDebit = _operation.CurrencyAmount;
            }
            else MonthCreadit = _operation.CurrencyAmount;

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
                var accountMonthlyBalanceBeforeOperationMonth = _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceBeforeOperation(_operation, false);
                decimal openningBalance = 0;
                if (accountMonthlyBalanceBeforeOperationMonth.Result != null)
                {
                    openningBalance = accountMonthlyBalanceBeforeOperationMonth.Result.ClosingMonthBalance;
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

    }
}
