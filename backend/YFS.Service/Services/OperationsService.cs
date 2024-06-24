using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YFS.Core.Dtos;
using YFS.Core.Enums;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class OperationsService : BaseService, IOperationsService
    {
        public OperationsService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger, LanguageScopedService languageService) 
            : base(repository, mapper, logger, languageService)
        {
        }
        public enum OperationType
        {
            Expense = 1,
            Income = 2,
            Transfer = 3
        }
        public async Task<ServiceResult<IEnumerable<OperationDto>>> CreateOperation(OperationDto operation, int targetAccountId, string userId)
        {
            var operationData = _mapper.Map<Operation>(operation);
            try
            {
                Account account = null!;
                Account accountTarget = null!;
                var accountMonthlyBalance = (AccountMonthlyBalance)null!;
                Operation transferOperationData = null!;
                operationData.UserId = userId;
                string transferWithdrawDescription = "";

                if ((operationData.AccountId == targetAccountId) && operationData.OperationItems.Any(item => item.CategoryId == -1))
                    return ServiceResult<IEnumerable<OperationDto>>.Error("Target Account must be not equal Withdraw Account");

                account = await _repository.Account.GetAccount(operationData.AccountId, true);

                foreach (var item in operationData.OperationItems)
                {
                    if ((OperationType)operationData.TypeOperation == OperationType.Expense)
                        item.OperationAmount = -item.OperationAmount;
                    if ((OperationType)operationData.TypeOperation == OperationType.Income)
                        item.OperationAmount = Math.Abs(item.OperationAmount);
                    if ((OperationType)operationData.TypeOperation == OperationType.Transfer)
                    {
                        accountTarget = await _repository.Account.GetAccount(targetAccountId, true);
                        item.OperationAmount = -item.OperationAmount;
                        operationData.Description += " [to " + accountTarget.Name + "]";
                        transferWithdrawDescription = " [from " + account.Name + "]";
                    }

                    item.CurrencyAmount = item.OperationAmount;
                }

                operationData.TotalCurrencyAmount = operationData.OperationItems.Sum(item => item.CurrencyAmount);

                accountMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationData, true);
                var listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationData, false);

                if (accountMonthlyBalance == null)
                {
                    accountMonthlyBalance = await AddAccountMonthlyBalance(account, operationData);
                }
                else
                {
                    List<AccountMonthlyBalance> listAccountMonthly = new List<AccountMonthlyBalance>();
                    listAccountMonthly.Add(accountMonthlyBalance);

                    var ambList = ChangeAccountMonthlyBalance(operationData, listAccountMonthly, true);
                    foreach (AccountMonthlyBalance amb in ambList)
                    {
                        
                        await _repository.AccountMonthlyBalance.UpdateAccountMonthlyBalance(amb);
                    }
                }

                if (listAccountMonthlyBalanceAfterOperationMonth.Count() > 0)
                {
                    var ambList = ChangeAccountMonthlyBalance(operationData, listAccountMonthlyBalanceAfterOperationMonth, false);

                    foreach (AccountMonthlyBalance amb in ambList)
                    {
                        await _repository.AccountMonthlyBalance.UpdateAccountMonthlyBalance(amb);
                    }
                }

                account.AccountBalance.Balance += operationData.TotalCurrencyAmount;

                await _repository.Account.UpdateAccount(account);
                await _repository.Operation.CreateOperation(operationData);            
                await _repository.SaveAsync();

                if (operationData.Id == 0)
                {
                    return ServiceResult<IEnumerable<OperationDto>>.Error("Failed to generate operation ID");
                }

                var operationReturnData = await _repository.Operation.GetOperationById(LanguageCode, operationData.Id, false);
                var operationDataReturnDto = _mapper.Map<OperationDto>(operationReturnData);

                List<OperationDto> listOperationReturn = new List<OperationDto> { operationDataReturnDto };

                if (operationData.TypeOperation == 3) //transfer Money
                {
                    if (targetAccountId > 0)
                    {
                        var accountTargetMonthlyBalance = (AccountMonthlyBalance)null!;
                        var listAccountTargetMonthlyBalanceAfterOperationMonth = (IEnumerable<AccountMonthlyBalance>)null!;
                        //accountTarget = await _repository.Account.GetAccount(targetAccountId);

                        transferOperationData = new Operation
                        {
                            UserId = userId,
                            TypeOperation = operationData.TypeOperation,
                            AccountId = targetAccountId,
                            TransferOperationId = operationData.Id,
                            OperationCurrencyId = operationData.OperationCurrencyId,
                            OperationDate = operationData.OperationDate,
                            OperationItems = operationData.OperationItems.Select(item => new OperationItem
                            {
                                CategoryId = -1,
                                CurrencyAmount = Math.Abs(item.OperationAmount),
                                OperationAmount = Math.Abs(item.OperationAmount),
                            }).ToList()
                        };

                        accountTargetMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(transferOperationData, false);
                        listAccountTargetMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(transferOperationData, false);


                        if (accountTargetMonthlyBalance == null)
                        {
                            await AddAccountMonthlyBalance(accountTarget, transferOperationData);
                        }
                        else
                        {
                            List<AccountMonthlyBalance> listAccountMonthly = new List<AccountMonthlyBalance>() { accountTargetMonthlyBalance };
                            ChangeAccountMonthlyBalance(transferOperationData, (IEnumerable<AccountMonthlyBalance>)listAccountMonthly, false);
                        }

                        ChangeAccountMonthlyBalance(transferOperationData, listAccountTargetMonthlyBalanceAfterOperationMonth, false);
                        accountTarget.AccountBalance.Balance += transferOperationData.OperationItems.Sum(item => item.CurrencyAmount);
                        transferOperationData.Description = transferWithdrawDescription;
                        await _repository.Account.UpdateAccount(accountTarget);
                        await _repository.Operation.CreateOperation(transferOperationData);
                        await _repository.SaveAsync();

                        transferOperationData = await _repository.Operation.GetOperationById(LanguageCode, transferOperationData.Id, false);
                        var operationTransferDataReturnDto = _mapper.Map<OperationDto>(transferOperationData);

                        listOperationReturn.Add(operationTransferDataReturnDto);
                    }
                }

                var operationReturn = _mapper.Map<IEnumerable<OperationDto>>(listOperationReturn);

                return ServiceResult<IEnumerable<OperationDto>>.Success(operationReturn);
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
                    return ServiceResult<IEnumerable<OperationDto>>.CustomError(errorObject);
                }
                else return ServiceResult<IEnumerable<OperationDto>>.Error(e.Message);
            }
        }
        public async Task<ServiceResult<OperationDto>> UpdateOperation(OperationDto operationDto)
        {
            try
            {
                var operation = _mapper.Map<Operation>(operationDto);

                var existingOperation = await _repository.Operation.GetOperationById(LanguageCode, operation.Id, trackChanges: true);

                if (existingOperation == null)
                {
                    return ServiceResult<OperationDto>.NotFound($"Operation with Id = {operation.Id} not found");
                }

                _mapper.Map(operation, existingOperation);

                await _repository.Operation.UpdateOperation(existingOperation);
                await _repository.SaveAsync();
                var updatedOperation = await _repository.Operation.GetOperationById(LanguageCode, operation.Id, trackChanges: true);
                var updatedOperationDto = _mapper.Map<OperationDto>(updatedOperation);
                return ServiceResult<OperationDto>.Success(updatedOperationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateOperation)} action {ex}");
                return ServiceResult<OperationDto>.Error(ex.Message);
            }
        }
        public async Task<ServiceResult<IEnumerable<OperationDto>>> RemoveOperation(int operationId)
        {
            try 
            {
                var operationData = await _repository.Operation.GetOperationByIdWithoutCategory(operationId, false);

                if (operationData == null)
                {
                    return ServiceResult<IEnumerable<OperationDto>>.NotFound($"Operation with Id = {operationId} not found");
                }

                Account account = operationData.Account;

                var accountCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationData, false);
                if (accountCurrentMonthlyBalance == null)
                {
                    if (accountCurrentMonthlyBalance == null) 
                        return ServiceResult<IEnumerable<OperationDto>>.Error("AccountMonthlyBalance is empty");
                }
                    
                var updatedAccountCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;

                var listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationData, false);
                var updatedAccountMonthlyBalancesAfter = (IEnumerable<AccountMonthlyBalance>)null;

                account.AccountBalance.Balance -= operationData.TotalCurrencyAmount;
                if (listAccountMonthlyBalanceAfterOperationMonth.Count() > 0)
                    updatedAccountMonthlyBalancesAfter = ChangeAccountMonthlyBalance(operationData, listAccountMonthlyBalanceAfterOperationMonth, true);

                List<AccountMonthlyBalance> listAccountMonthlyBalance = new List<AccountMonthlyBalance>();
                listAccountMonthlyBalance.Add(accountCurrentMonthlyBalance);
                updatedAccountCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationData, listAccountMonthlyBalance, true);

                if (updatedAccountCurrentMonthlyBalance.Count() > 0)
                {
                    foreach (AccountMonthlyBalance amb in updatedAccountCurrentMonthlyBalance)
                    {
                        await _repository.AccountMonthlyBalance.UpdateAccountMonthlyBalance(amb);
                    }
                }

                await _repository.Operation.RemoveOperation(operationData);
                await _repository.AccountBalance.UpdateAccountBalance(account.AccountBalance);
                await _repository.SaveAsync();

                List<Operation> operationReturn = new List<Operation>();
                operationReturn.Add(operationData);

                var operationReturnDto = _mapper.Map<IEnumerable<OperationDto>>(operationReturn);

                return ServiceResult<IEnumerable<OperationDto>>.Success(operationReturnDto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(RemoveOperation)} action {ex}");
                return ServiceResult<IEnumerable<OperationDto>>.Error(ex.Message);
            }
        }
        public async Task<ServiceResult<IEnumerable<OperationDto>>> RemoveTransferOperation(int operationId)
        {
            try
            {
                var operationData = await _repository.Operation.GetOperationByIdWithoutCategory(operationId, false);

                if (operationData == null)
                {
                    return ServiceResult<IEnumerable<OperationDto>>.NotFound($"Operation with Id = {operationId} not found");
                }
                Account accountWithdraw = null!;
                Account accountTarget = operationData.Account;
                var operationWithdrawData = (Operation)null!;
                var operationIncomeData = (Operation)null!;
                List<Operation> listOperationReturn = new List<Operation>();
                AccountMonthlyBalance accountWithdrawCurrentMonthlyBalance = null;
                AccountMonthlyBalance accountTargetCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationData, false);
                var updatedAccountTargetCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null!;
                var updatedAccountWithdrawCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null!;
                var listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationData, false);
                var listAccountWithdrawMonthlyBalanceAfterOperationMonth = (IEnumerable<AccountMonthlyBalance>)null!;
                var updatedAccountTargetMonthlyBalancesAfter = (IEnumerable<AccountMonthlyBalance>)null!;
                var updatedAccountWithdrawMonthlyBalancesAfter = (IEnumerable<AccountMonthlyBalance>)null!;

                var transferItem = operationData.OperationItems.FirstOrDefault(item => item.CategoryId == -1);

                if (transferItem != null)
                {
                    if (operationData.TransferOperationId > 0)
                    {
                        operationIncomeData = operationData;
                        operationWithdrawData = await _repository.Operation.GetOperationById(LanguageCode, operationIncomeData.TransferOperationId, false);

                        if (operationWithdrawData != null)
                        {
                            accountWithdraw = operationWithdrawData.Account;
                            accountTarget = operationIncomeData.Account;
                        }
                        else return ServiceResult<IEnumerable<OperationDto>>.NotFound($"Operation with Id = {operationIncomeData.TransferOperationId} not found");
                    }
                    else
                    {
                        operationWithdrawData = operationData;
                        operationIncomeData = await _repository.Operation.GetTransferOperationById(LanguageCode, operationWithdrawData.Id);

                        if (operationIncomeData != null)
                        {
                            accountWithdraw = operationWithdrawData.Account;
                            accountTarget = operationIncomeData.Account;
                        }
                        else return ServiceResult<IEnumerable<OperationDto>>.NotFound($"Operation with Id = {operationData.TransferOperationId} not found");
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

                    foreach (var item in operationWithdrawData.OperationItems)
                    {
                        await ChangeAccountBalanceNew(operationWithdrawData, item.CurrencyAmount);
                    }

                    foreach (var item in operationIncomeData.OperationItems)
                    {
                        await ChangeAccountBalanceNew(operationIncomeData, item.CurrencyAmount);
                    }

                    await ChangeAccountMonthlyBalances(operationIncomeData, listAccountMonthlyBalanceAfterOperationMonth, false);
                    await ChangeAccountMonthlyBalances(operationWithdrawData, listAccountWithdrawMonthlyBalanceAfterOperationMonth, true);

                    await _repository.Operation.RemoveOperation(operationIncomeData);
                    await _repository.Operation.RemoveOperation(operationWithdrawData);


                    listOperationReturn.Add(operationWithdrawData);
                    listOperationReturn.Add(operationIncomeData);

                    await _repository.SaveAsync();

                    var operationReturn = _mapper.Map<List<OperationDto>>(listOperationReturn);
                    return ServiceResult<IEnumerable<OperationDto>>.Success(operationReturn);
                } 
                else 
                {
                    return ServiceResult<IEnumerable<OperationDto>>.NotFound($"Not transfer operation = {operationId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(RemoveTransferOperation)} action {ex}");
                return ServiceResult<IEnumerable<OperationDto>>.Error(ex.Message);
            }
        }
        public async Task<ServiceResult<IEnumerable<OperationDto>>> GetOperationsAccountForPeriod(int accountId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var operations = await _repository.Operation.GetOperationsForAccountForPeriod(LanguageCode, accountId, startDate, endDate, trackChanges: false);
                var operationsDto = _mapper.Map<IEnumerable<OperationDto>>(operations);
                return ServiceResult<IEnumerable<OperationDto>>.Success(operationsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetOperationsAccountForPeriod)} action {ex}");
                return ServiceResult<IEnumerable<OperationDto>>.Error(ex.Message);
            }
        }
        public async Task<ServiceResult<IEnumerable<OperationDto>>> GetLast10OperationsAccount(int accountId)
        {
            try
            {
                var operations = await _repository.Operation.GetLast10OperationsForAccount(LanguageCode, accountId, trackChanges: false);
                var operationsDto = _mapper.Map<IEnumerable<OperationDto>>(operations);
                return ServiceResult<IEnumerable<OperationDto>>.Success(operationsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetLast10OperationsAccount)} action {ex}");
                return ServiceResult<IEnumerable<OperationDto>>.Error(ex.Message);
            }
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

            if (_operation.TotalCurrencyAmount > 0)
            {
                if (_removeOperation == true)
                {
                    MonthDebit = -_operation.TotalCurrencyAmount;
                }
                else { MonthDebit = _operation.TotalCurrencyAmount; }
            }
            else
            {
                if (_removeOperation == true)
                {
                    MonthCreadit = -_operation.TotalCurrencyAmount;
                }
                else
                {
                    MonthCreadit = _operation.TotalCurrencyAmount;
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

                    //_repository.AccountMonthlyBalance.UpdateAccountMonthlyBalance(amb);
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
                var amount = isDebit ? -operation.TotalCurrencyAmount : operation.TotalCurrencyAmount;

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
        private async Task<AccountMonthlyBalance> AddAccountMonthlyBalance(Account _account, Operation operation)
        {
            decimal monthCredit = 0;
            decimal monthDebit = 0;

            foreach (var item in operation.OperationItems)
            {
                if (item.CurrencyAmount > 0)
                {
                    monthDebit += item.CurrencyAmount;
                }
                else
                {
                    monthCredit += item.CurrencyAmount;
                }
            }

            decimal openingBalance = CalculateOpeningBalance(_account, operation);

            var accountMonthlyBalance = new AccountMonthlyBalance
            {
                OpeningMonthBalance = openingBalance,
                ClosingMonthBalance = openingBalance + monthDebit + monthCredit,
                StartDateOfMonth = new DateTime(operation.OperationDate.Year, operation.OperationDate.Month, 1, 0, 0, 0, DateTimeKind.Utc),
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
                openingBalance = 0;//account.AccountBalance.Balance;
            }

            return openingBalance;
        }
    }
}
