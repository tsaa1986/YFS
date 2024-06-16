using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using YFS.Service.Interfaces;
using YFS.Core.Models.MonoIntegration;
using YFS.Core.Dtos;
using Newtonsoft.Json;
using Microsoft.Extensions.Http;
using Newtonsoft.Json.Linq;
using YFS.Core.Models;

namespace YFS.Service.Services
{
    public class MonoIntegrationApiService : BaseService, IMonoIntegrationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccountService _accountService;
        private readonly IAccountGroupsService _accountGroupService;
        private readonly IBankService _bankService; 
        private readonly ICurrencyService _currencyService;
        private readonly IOperationsService _operationsService;
        private readonly ITokenService _tokenService;
        public MonoIntegrationApiService(IHttpClientFactory httpClientFactory, IRepositoryManager repository, 
            IMapper mapper, 
            ILogger<BaseService> logger, 
            HttpClient httpClient,
            IAccountService accountService,
            IAccountGroupsService accountGroupService,
            IBankService bankService,
            ICurrencyService currencyService,
            IOperationsService operationsService,
            ITokenService tokenService,
            LanguageScopedService languageService
            ) : base(repository, mapper, logger, languageService)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.monobank.ua/personal/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _accountService = accountService;
            _accountGroupService = accountGroupService;
            _bankService = bankService;
            _currencyService = currencyService;
            _operationsService = operationsService;
            _tokenService = tokenService;
        }
        public async Task<ServiceResult<MonoClientInfoResponse>> GetClientInfo(string xToken)
        {
            try
            {               
                _httpClient.DefaultRequestHeaders.Add("X-Token", xToken);
                HttpResponseMessage response = await _httpClient.GetAsync("client-info");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    MonoClientInfoResponse clientInfo = JsonConvert.DeserializeObject<MonoClientInfoResponse>(responseBody);

                    if (clientInfo != null)
                    {
                        return ServiceResult<MonoClientInfoResponse>.Success(clientInfo);
                    }
                    else
                    {
                        _logger.LogError("MonoCleint Info Deserialization result is null");

                        return ServiceResult<MonoClientInfoResponse>.Error("Failed to deserialize client info");
                    }
                }
                else {
                    _logger.LogError($"Failed to retrieve mono client info: {response.StatusCode}");

                    return ServiceResult<MonoClientInfoResponse>.NotFound($"Failed to retrieve client info: {response.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetClientInfo)} action {ex}");

                return ServiceResult<MonoClientInfoResponse>.Error(ex.Message);
            }
        }
        public async Task<ServiceResult<IEnumerable<MonoTransaction>>> GetTransactions(string xToken, string account, DateTime fromDate, DateTime toDate)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("X-Token", xToken);
                long fromUnixTime = (long)(fromDate.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                long toUnixTime = (long)(toDate.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                /*
                var dateTimeOffsetFrom = new DateTimeOffset(fromDate);
                var dateTimeOffsetTo = new DateTimeOffset(toDate);
                var unixDateTimeFrom = dateTimeOffsetFrom.ToUnixTimeSeconds();
                var unixDateTimeTo = dateTimeOffsetTo.ToUnixTimeSeconds();
                */

                string requestUrl = $"{_httpClient.BaseAddress}statement/{account}/{fromUnixTime}/{toUnixTime}";

                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    IEnumerable<MonoTransaction>? transactions = JsonConvert.DeserializeObject<IEnumerable<MonoTransaction>>(responseBody);
                    if (transactions != null)
                    {
                        return ServiceResult<IEnumerable<MonoTransaction>>.Success(transactions);
                    }
                    else
                    {
                        return ServiceResult<IEnumerable<MonoTransaction>>.Error("Failed to deserialize statements");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to retrieve statements from Monobank API: {response.StatusCode}");
                    return ServiceResult<IEnumerable<MonoTransaction>>.Error($"Failed to retrieve statements: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetStatementsBetweenDates method: {ex.Message}");
                return ServiceResult<IEnumerable<MonoTransaction>>.Error($"Error: {ex.Message}");
            }
        }        
        public async Task<ServiceResult<IEnumerable<AccountDto>>> SyncAccounts(string xToken, string userId, MonoClientInfoResponse clientInfoResponse)
        {
            try
            {
                if ((clientInfoResponse == null) || (clientInfoResponse.accounts == null) 
                    || clientInfoResponse.accounts.Count == 0) {
                    return ServiceResult<IEnumerable<AccountDto>>.NotFound("Accounts from monobank not found");
                }

                var monoAccounts = clientInfoResponse.accounts;
                var accountData = new List<AccountDto>();

                int accountGroupId = await GetOrCreateAccountGroupId(userId, LanguageCode);

                if (accountGroupId == 0)
                {
                    return ServiceResult<IEnumerable<AccountDto>>.Error("Failed to get or create account group");
                }

                var monobank = await _bankService.GetBankByGLMFO(322001);
                if (monobank.Data == null || !monobank.IsSuccess)
                {
                    return ServiceResult<IEnumerable<AccountDto>>.NotFound("Monobank from UniversalBank not found");
                }
                int monoBankId = monobank.Data.GLMFO;

                foreach (var monoAccount in monoAccounts)
                {
                    if (await AccountExists(monoAccount.id, userId))
                    {
                        continue;
                    }

                    var currency = await GetCurrency(monoAccount.currencyCode);
                    if (currency.IsSuccess != true)
                    {
                        return ServiceResult<IEnumerable<AccountDto>>.NotFound($"Currency {monoAccount.currencyCode} not found");
                    }

                    var accountDto = CreateAccountDto(monoAccount, accountGroupId, monoBankId, currency.Data);

                    var createdAccount = await _accountService.CreateAccountForUser(accountDto, userId);
                    if (!createdAccount.IsSuccess)
                    {
                        return ServiceResult<IEnumerable<AccountDto>>.Error("Failed to add accounts from monobank");
                    }

                    accountData.Add(createdAccount.Data);
                }

                if (accountData.Count == 0)
                {
                    return ServiceResult<IEnumerable<AccountDto>>.NotFound("Accounts for synchronization not found");
                }

                return ServiceResult<IEnumerable<AccountDto>>.Success(accountData);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<AccountDto>>.Error("Failed to fetch accounts: " + ex.Message);
            }
        }
        private async Task<int> GetOrCreateAccountGroupId(string userId, string languageCode)
        {
            var accountsGroup = await _accountGroupService.GetAccountGroupsForUser(userId, languageCode);
            if (accountsGroup?.Data != null)
            {
                var accountGroup = accountsGroup.Data.FirstOrDefault(ag => ag.Translations.Any(t => t.AccountGroupName.Equals("Bank")));
                if (accountGroup != null)
                {
                    return accountGroup.AccountGroupId;
                }
            }

            var newAccountGroup = await _accountGroupService.CreateAccountGroupForUser(new AccountGroupDto
            {
                UserId = userId,
                Translations = new[]
                    {
                        new AccountGroupTranslationDto { LanguageCode = "ru", AccountGroupName = "Банковские" },
                        new AccountGroupTranslationDto { LanguageCode = "en", AccountGroupName = "Bank" },
                        new AccountGroupTranslationDto { LanguageCode = "ua", AccountGroupName = "Банківські" }
                    },
                GroupOrderBy = 0
            }, userId);

            return newAccountGroup.IsSuccess ? newAccountGroup.Data.AccountGroupId : 0;
        }

        private async Task<bool> AccountExists(string externalId, string userId)
        {
            var accountFromDB = await _accountService.GetExternalAccountById(externalId, userId, false);
            return accountFromDB.Data != null;
        }
        private async Task<int> GetAccountId(string externalId, string userId)
        {
            var accountFromDB = await _accountService.GetExternalAccountById(externalId, userId, false);
            return accountFromDB.Data == null ? 0 : accountFromDB.Data.Id;
        }
        private async Task<ServiceResult<CurrencyDto>> GetCurrency(int currencyCode)
        {
            string? currencyCountry = currencyCode switch
            {
                840 => "United States of America (the)",
                980 => "Ukraine",
                978 => "European Union",
                _ => null
            };

            if (currencyCountry == null)
            {
                return ServiceResult<CurrencyDto>.NotFound($"Currency + {currencyCode} not found");
            }

            var currency = await _currencyService.GetCurrencyByCodeAndCountry(currencyCode, currencyCountry);
            return currency.IsSuccess ? ServiceResult<CurrencyDto>.Success(currency.Data) : ServiceResult<CurrencyDto>.NotFound("Monobank from UniversalBank not found"); ;
        }

        private AccountDto CreateAccountDto(MonoAccount monoAccount, int accountGroupId, int monoBankId, CurrencyDto currency)
        {
            return new AccountDto
            {
                ExternalId = monoAccount.id,
                AccountIsEnabled = 1,
                Favorites = 0,
                AccountGroupId = accountGroupId,
                AccountTypeId = 2, // banks account
                Name = $"Mono | {monoAccount.type} card | [{currency.Code}]",
                Bank_GLMFO = monoBankId,
                CurrencyId = currency.CurrencyId,
                CreditLimit = monoAccount.calculatedCreditLimit,
                IBAN = monoAccount.iban,
                OpeningDate = DateTime.UtcNow,
                Note = monoAccount.type
            };
        }

        public async Task<ServiceResult<bool>> SyncTransactionFromStatements(string xToken, string userId, string externalIdAccount,
            IEnumerable<MonoTransaction> transactions)
        {
            if (transactions == null)
            {
                return ServiceResult<bool>.Error("No transactions to sync.");
            }

            //find account? if not exist return error
            var account = await _repository.Account.GetExternalAccountById(externalIdAccount, userId, false);

            if (account == null)
            {
                return ServiceResult<bool>.NotFound("account not found for import statement");
            }

            // Retrieve the API token for the user
            var apiToken = await _tokenService.GetTokenByNameForUser("apiMonoBank", userId);
            if (apiToken == null)
            {
                return ServiceResult<bool>.Error("Invalid token.");
            }

            foreach (var transaction in transactions)
            {
                if (transaction == null)
                {
                    continue; // Skip null transactions
                }

                // Check if the transaction already exists in the MonoSyncedTransaction table
                var existingTransaction = await _repository.MonoSyncedTransaction.ExistsAsync(transaction.Id);

                if (existingTransaction == true)
                {
                    continue; // Skip already synced transactions
                }

                // Apply sync rules and create operations
                var operations = ApplySyncRules(transaction, apiToken.Data.Id);
                var operationsDto = _mapper.Map<List<OperationDto>>(operations);

                int? operationId = null;
                int? transferFromOperationId = null;
                int? transferToOperationId = null;

                foreach (var operationDto in operationsDto)
                {
                    var result = await _operationsService.CreateOperation(operationDto, operationDto.AccountId, userId);

                    if (!result.IsSuccess)
                    {
                        // Log the error and continue with the next transaction
                        continue;
                    }

                    var createdOperations = result.Data.ToList();

                    // Determine the type of operation
                    foreach (var createdOperation in createdOperations)
                    {
                        if (createdOperation.TypeOperation == 1 || createdOperation.TypeOperation == 2)
                        {
                            operationId = createdOperation.Id;
                        }
                        else if (createdOperation.TypeOperation == 3)
                        {
                            if (transferFromOperationId == null)
                            {
                                transferFromOperationId = createdOperation.Id;
                            }
                            else
                            {
                                transferToOperationId = createdOperation.Id;
                            }
                        }
                    }
                }


                // Save the successful sync status in the MonoSyncedTransaction table
                var syncedTransaction = new MonoSyncedTransaction
                {
                    MonoTransactionId = transaction.Id,
                    //UserId = userId,
                    //OperationId = operationId,
                    //TransferFromOperationId = transferFromOperationId,
                    //TransferToOperationId = transferToOperationId,
                    //IsSynced = true,
                    SyncedOn = DateTime.UtcNow
                };

                await _repository.MonoSyncedTransaction.AddAsync(syncedTransaction);
                await _repository.SaveAsync();
            }

            return ServiceResult<bool>.Success(true);
        }

        #region analyze transaction
        private List<Operation> ApplySyncRules(MonoTransaction transaction, int apiTokenId)
        {
            var activeRules = _repository.MonoSyncRule.GetActiveRulesByApiTokenIdAsync(apiTokenId).Result;
            var operations = new List<Operation>();

            foreach (var rule in activeRules)
            {
                if (EvaluateCondition(transaction, rule.Condition))
                {
                    var operation = ApplyAction(transaction, rule.Action);
                    operations.Add(operation);
                    // Assuming one rule per transaction
                    break;
                }
            }

            return operations;
        }

        private bool EvaluateCondition(MonoTransaction transaction, string condition)
        {
            if (transaction == null || string.IsNullOrEmpty(condition))
            {
                throw new ArgumentException("Transaction and condition cannot be null or empty.");
            }

            Dictionary<string, object> conditionDict;
            try
            {
                conditionDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(condition);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize condition string.", ex);
            }

            if (conditionDict == null)
            {
                throw new InvalidOperationException("Deserialized condition dictionary is null.");
            }

            if (conditionDict.TryGetValue("Mcc", out var mccValue) && (int)mccValue == transaction.Mcc)
            {
                return true;
            }
            if (conditionDict.TryGetValue("DescriptionEquals", out var descriptionEqualsValue) && (string)descriptionEqualsValue == transaction.Description)
            {
                return true;
            }
            if (conditionDict.TryGetValue("DescriptionContains", out var descriptionContainsValue) && transaction.Description != null && transaction.Description.Contains((string)descriptionContainsValue))
            {
                return true;
            }
            return false;
        }

        private Operation ApplyAction(MonoTransaction transaction, string action)
        {
            if (transaction == null || string.IsNullOrEmpty(action))
            {
                throw new ArgumentException("Transaction and action cannot be null or empty.");
            }

            var actionDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(action);
            var operation = new Operation
            {
                //UserId = transaction.UserId,
                TypeOperation = actionDict.TryGetValue("TypeOperation", out var typeOperationValue) ? (int)typeOperationValue : 0,
                AccountId = actionDict.TryGetValue("AccountId", out var accountIdValue) ? (int)accountIdValue : 0,
                OperationDate = DateTime.UtcNow,
                TotalCurrencyAmount = transaction.AmountCalculated,
                OperationCurrencyId = actionDict.TryGetValue("OperationCurrencyId", out var operationCurrencyIdValue) ? (int)operationCurrencyIdValue : 0,
                ExchangeRate = 1, // Assuming a default exchange rate of 1
                CashbackAmount = transaction.CashbackAmountCalculated,
                MCC = transaction.Mcc,
                Description = transaction.Description
            };

            if (actionDict.TryGetValue("CategoryId", out var categoryIdValue))
            {
                var item = new OperationItem
                {
                    CategoryId = (int)categoryIdValue
                };
                operation.OperationItems.Add(item);
            }

            return operation;
        }
        #endregion

        #region Action For Rules
        public async Task<ServiceResult<IEnumerable<MonoSyncRule>>> GetActiveRulesByApiTokenIdAsync(int apiTokenId)
        {
            try
            {
                var rules = await _repository.MonoSyncRule.GetActiveRulesByApiTokenIdAsync(apiTokenId);
                return ServiceResult<IEnumerable<MonoSyncRule>>.Success(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting active rules by ApiTokenId: {ApiTokenId}", apiTokenId);
                return ServiceResult<IEnumerable<MonoSyncRule>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<MonoSyncRule>> AddRuleAsync(MonoSyncRule newRule)
        {
            try
            {
                await _repository.MonoSyncRule.AddRule(newRule);
                return ServiceResult<MonoSyncRule>.Success(newRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding new rule");
                return ServiceResult<MonoSyncRule>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<MonoSyncRule>>> AddRulesAsync(IEnumerable<MonoSyncRule> rules)
        {
            try
            {
                await _repository.MonoSyncRule.AddRange(rules);
                return ServiceResult<IEnumerable<MonoSyncRule>>.Success(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding multiple rules");
                return ServiceResult<IEnumerable<MonoSyncRule>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<MonoSyncRule>> UpdateRuleAsync(MonoSyncRule updatedRule)
        {
            try
            {
                await _repository.MonoSyncRule.UpdateRule(updatedRule);
                return ServiceResult<MonoSyncRule>.Success(updatedRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating rule");
                return ServiceResult<MonoSyncRule>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<MonoSyncRule>> GetRuleAsync(int ruleId)
        {
            try
            {
                var rule = await _repository.MonoSyncRule.GetRule(ruleId);
                return ServiceResult<MonoSyncRule>.Success(rule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting rule by ID: {RuleId}", ruleId);
                return ServiceResult<MonoSyncRule>.Error(ex.Message);
            }
        }
        #endregion
    }
}
