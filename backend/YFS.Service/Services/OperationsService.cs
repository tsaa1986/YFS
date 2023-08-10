using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        public enum OperationType
        {
            Expense = 1,
            Income = 2,
            Transfer = 3
        }
        public OperationsService(IRepositoryManager repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<ServiceResult<IEnumerable<OperationDto>>> CreateOperation(OperationDto operation, int targetAccountId, string userId)
        {
            var operationData = _mapper.Map<Operation>(operation);
            try
            {
                Account account = null;
                Account accountTarget = null;
                var accountMonthlyBalance = (AccountMonthlyBalance)null;
                Operation transferOperaitonData = null;
                operationData.UserId = userId;
                string transferWithdrawDescription = "";

                if ((operationData.AccountId == targetAccountId) && (operationData.CategoryId == -1))
                    return ServiceResult<IEnumerable<OperationDto>>.Error("Target Account must be not equal Withdraw Account");

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
                            UserId = userId,
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

                var existingOperation = await _repository.Operation.GetOperationById(operation.Id, trackChanges: true);

                if (existingOperation == null)
                {
                    return ServiceResult<OperationDto>.NotFound($"Operation with Id = {operation.Id} not found");
                }

                _mapper.Map(operation, existingOperation);

                await _repository.Operation.UpdateOperation(existingOperation);
                await _repository.SaveAsync();
                var updatedOperation = await _repository.Operation.GetOperationById(operation.Id, trackChanges: true);
                var updatedOperationDto = _mapper.Map<OperationDto>(updatedOperation);
                return ServiceResult<OperationDto>.Success(updatedOperationDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<OperationDto>.Error(ex.Message);
            }
        }
        public async Task<ServiceResult<OperationDto>> RemoveOperation(int operationId)
        {
            try 
            {
                var operationData = await _repository.Operation.GetOperationByIdWithoutCategory(operationId, false);

                if (operationData == null)
                {
                    return ServiceResult<OperationDto>.NotFound($"Operation with Id = {operationId} not found");
                }

                Account account = operationData.Account;

                AccountMonthlyBalance accountCurrentMonthlyBalance = await _repository.AccountMonthlyBalance.CheckAccountMonthlyBalance(operationData, false);
                var updatedAccountCurrentMonthlyBalance = (IEnumerable<AccountMonthlyBalance>)null;

                var listAccountMonthlyBalanceAfterOperationMonth = await _repository.AccountMonthlyBalance.GetAccountMonthlyBalanceAfterOperation(operationData, false);
                var updatedAccountMonthlyBalancesAfter = (IEnumerable<AccountMonthlyBalance>)null;

                Operation operationReturn = new Operation();
                account.AccountBalance.Balance -= operationData.CurrencyAmount;
                if (listAccountMonthlyBalanceAfterOperationMonth.Count() > 0)
                    updatedAccountMonthlyBalancesAfter = ChangeAccountMonthlyBalance(operationData, listAccountMonthlyBalanceAfterOperationMonth, true);

                List<AccountMonthlyBalance> listAccountMonthlyBalance = new List<AccountMonthlyBalance>();
                listAccountMonthlyBalance.Add(accountCurrentMonthlyBalance);
                updatedAccountCurrentMonthlyBalance = ChangeAccountMonthlyBalance(operationData, listAccountMonthlyBalance, true);

                await _repository.Operation.RemoveOperation(operationData);
                await _repository.AccountBalance.UpdateAccountBalance(account.AccountBalance);

                operationReturn = operationData;

                await _repository.SaveAsync();

                var operationReturnDto = _mapper.Map<OperationDto>(operationReturn);

                return ServiceResult<OperationDto>.Success(operationReturnDto);

            }
            catch (Exception ex)
            {
                return ServiceResult<OperationDto>.Error(ex.Message);
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
                        else return ServiceResult<IEnumerable<OperationDto>>.NotFound($"Operation with Id = {operationIncomeData.TransferOperationId} not found");
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
                    return ServiceResult<IEnumerable<OperationDto>>.Success(operationReturn);
                } 
                else 
                {
                    return ServiceResult<IEnumerable<OperationDto>>.NotFound($"Not transfer operation = {operationId}");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<OperationDto>>.Error(ex.Message);
            }
        }
        public async Task<ServiceResult<IEnumerable<OperationDto>>> GetOperationsAccountForPeriod(int accountId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var operations = await _repository.Operation.GetOperationsForAccountForPeriod(accountId, startDate, endDate, trackChanges: false);
                var operationsDto = _mapper.Map<IEnumerable<OperationDto>>(operations);
                return ServiceResult<IEnumerable<OperationDto>>.Success(operationsDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<OperationDto>>.Error(ex.Message);
            }
        }
        public async Task<ServiceResult<IEnumerable<OperationDto>>> GetLast10OperationsAccount(int accountId)
        {
            try
            {
                var operations = await _repository.Operation.GetLast10OperationsForAccount(accountId, trackChanges: false);
                var operationsDto = _mapper.Map<IEnumerable<OperationDto>>(operations);
                return ServiceResult<IEnumerable<OperationDto>>.Success(operationsDto);
            }
            catch (Exception ex)
            {
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
                openingBalance = 0;//account.AccountBalance.Balance;
            }

            return openingBalance;
        }
    }
}
