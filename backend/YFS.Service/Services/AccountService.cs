using AutoMapper;
using Microsoft.Extensions.Logging;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<ServiceResult<AccountDto>> CreateAccountForUser(AccountDto account, string userId)
        {
            try
            {
                var accountData = _mapper.Map<Account>(account);

                accountData.UserId = userId;
                accountData.AccountBalance = new AccountBalance { Balance = account.Balance };
                await _repository.Account.CreateAccount(accountData);

                if (account.Balance != 0)
                {
                    accountData.Operations = new List<Operation>() { {
                    new Operation {
                        AccountId = account.Id,
                        UserId= userId,
                        OperationAmount = account.Balance,
                        OperationCurrencyId = account.CurrencyId,
                        CurrencyAmount = account.Balance,
                        Description = "openning account",
                        TypeOperation = account.Balance > 0 ? 2 : 1,
                        CategoryId = -2,
                        OperationDate = account.OpeningDate,
                    } } };
                }
                await _repository.SaveAsync();
                var accountReturnData = _repository.Account.GetAccountWithCurrency(accountData.Id);
                var accountReturnDto = _mapper.Map<AccountDto>(accountReturnData.Result);
                return ServiceResult<AccountDto>.Success(accountReturnDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while  CreateAccount: userid: {UserId}, account: {account}", userId, account.Name);
                return ServiceResult<AccountDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<AccountDto>> GetAccountById(int accountId)
        {
            try
            {                
                var account = await _repository.Account.GetAccount(accountId);
                var accountDto = _mapper.Map<AccountDto>(account);
                return ServiceResult<AccountDto>.Success(accountDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting account with ID: {AccountId}", accountId);
                return ServiceResult<AccountDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<AccountDto>>> GetAccountsByGroup(int accountGroupId, string userId, bool trackChanges)
        {
            try
            {
                var accounts = await _repository.Account.GetAccountsByGroup(accountGroupId, userId, false);
                var accountsData = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return ServiceResult<IEnumerable<AccountDto>>.Success(accountsData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while GetAccountsByGroup: userid: {UserId}, accountgroupId: {accountGroupId}", userId, accountGroupId);
                return ServiceResult<IEnumerable<AccountDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<AccountDto>>> GetAccountsByUserId(string userId, bool trackChanges)
        {
            try
            {
                var accounts = await _repository.Account.GetAccountsByUserId(userId, trackChanges);
                var accountsData = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return ServiceResult<IEnumerable<AccountDto>>.Success(accountsData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while GetOpenAccountsByUserId: {UserId}", userId);
                return ServiceResult<IEnumerable<AccountDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<AccountDto>> GetExternalAccountById(string externalAccountId, string userId, bool trackChanges)
        {
            try
            {
                var account = await _repository.Account.GetExternalAccountById(externalAccountId, userId, trackChanges);
                var accountData = _mapper.Map<AccountDto>(account);
                return ServiceResult<AccountDto>.Success(accountData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while GetExternalAccountById: {Id}, user:{userId}", externalAccountId, userId);
                return ServiceResult<AccountDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<AccountDto>>> GetOpenAccountsByUserId(string userId, bool trackChanges)
        {
            try
            {
                var accounts = await _repository.Account.GetOpenAccountsByUserId(userId, trackChanges);
                var accountsDto = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return ServiceResult<IEnumerable<AccountDto>>.Success(accountsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while GetOpenAccountsByUserId: {UserId}", userId);
                return ServiceResult<IEnumerable<AccountDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<AccountDto>> UpdateAccount(AccountDto account)
        {
            try
            {
                var accountData = _mapper.Map<Account>(account);
                await _repository.Account.UpdateAccount(accountData);
                await _repository.SaveAsync();

                Account updatedAccount = await _repository.Account.GetAccount(accountData.Id);
                var accountDto = _mapper.Map<AccountDto>(updatedAccount);
                return ServiceResult<AccountDto>.Success(accountDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating account: {Account}", account);
                return ServiceResult<AccountDto>.Error(ex.Message);
            }
        }
    }
}
