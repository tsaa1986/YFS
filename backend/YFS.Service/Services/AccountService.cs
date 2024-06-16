using AutoMapper;
using Microsoft.Extensions.Logging;
using YFS.Core.Dtos;
using YFS.Core.Enums;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger, LanguageScopedService languageService) 
            : base(repository, mapper, logger, languageService)
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
                    var operation = new Operation
                    {
                        AccountId = account.Id,
                        UserId = userId,
                        OperationDate = account.OpeningDate,
                        Description = "Opening account",
                        TypeOperation = account.Balance > 0 ? 2 : 1,
                        OperationCurrencyId = account.CurrencyId, // Ensure this is correct
                    };
                    operation.OperationItems = new List<OperationItem>
                    {
                        new OperationItem
                        {
                            CategoryId = -2,
                            CurrencyAmount = account.Balance,
                            OperationAmount = account.Balance,
                        }
                    };

                    accountData.Operations = new List<Operation> { operation };
                }

                await _repository.SaveAsync();
                var accountReturnData = await _repository.Account.GetAccountWithCurrency(accountData.Id);
                var accountReturnDto = _mapper.Map<AccountDto>(accountReturnData);
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
