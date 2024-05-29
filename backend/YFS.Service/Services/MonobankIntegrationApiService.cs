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
using System.Data;
using YFS.Core.Enums;
using YFS.Core.Utilities;

namespace YFS.Service.Services
{
    public class MonobankIntegrationApiService : BaseService, IMonobankIntegrationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccountService _accountService;
        private readonly IAccountGroupsService _accountGroupService;
        private readonly IBankService _bankService; 
        private readonly ICurrencyService _currencyService;
        public MonobankIntegrationApiService(IHttpClientFactory httpClientFactory, IRepositoryManager repository, 
            IMapper mapper, 
            ILogger<BaseService> logger, 
            HttpClient httpClient,
            IAccountService accountService,
            IAccountGroupsService accountGroupService,
            IBankService bankService,
            ICurrencyService currencyService, LanguageScopedService languageService
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
        public async Task<ServiceResult<IEnumerable<MonoStatement>>> GetStatementsBetweenDates(string xToken, string account, DateTime fromDate, DateTime toDate)
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
                    IEnumerable<MonoStatement>? statements = JsonConvert.DeserializeObject<IEnumerable<MonoStatement>>(responseBody);
                    if (statements != null)
                    {
                        return ServiceResult<IEnumerable<MonoStatement>>.Success(statements);
                    }
                    else
                    {
                        return ServiceResult<IEnumerable<MonoStatement>>.Error("Failed to deserialize statements");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to retrieve statements from Monobank API: {response.StatusCode}");
                    return ServiceResult<IEnumerable<MonoStatement>>.Error($"Failed to retrieve statements: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetStatementsBetweenDates method: {ex.Message}");
                return ServiceResult<IEnumerable<MonoStatement>>.Error($"Error: {ex.Message}");
            }
        }
        public async Task<ServiceResult<IEnumerable<AccountDto>>> SynchronizeAccounts(string xToken, string userId)
        {
            try
            {
                var clientInfoResult = await GetClientInfo(xToken);
                if (!clientInfoResult.IsSuccess || clientInfoResult.Data.accounts.Count == 0)
                {
                    return ServiceResult<IEnumerable<AccountDto>>.NotFound("Accounts from monobank not found");
                }

                var monoAccounts = clientInfoResult.Data.accounts;
                var accountData = new List<AccountDto>();

                int accountGroupId = await GetOrCreateAccountGroupId(userId, LanguageCode);

                if (accountGroupId == 0)
                {
                    return ServiceResult<IEnumerable<AccountDto>>.Error("Failed to get or create account group");
                }

                var monobank = await _bankService.GetBankByGLMFO(322001);
                if (monobank == null || !monobank.IsSuccess)
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
                OpeningDate = DateTime.UtcNow,
                Note = monoAccount.type
            };
        }

        public Task<ServiceResult<IEnumerable<MonoStatement>>> GetStatements(string xToken, string accountId, DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }



        /*    
            public async Task<ServiceResult<IEnumerable<AccountDto>>> SynchronizeAccounts(string xToken, string userId)
            {
                try
                {
                    var clientInfoResult = await GetClientInfo(xToken);
                    if (clientInfoResult.Data.accounts.Count == 0)
                        return ServiceResult<IEnumerable<AccountDto>>.NotFound("Accounts from monobank not found");


                    if ((clientInfoResult.IsSuccess))
                    {
                        // get accounts from mono
                        var monoAccounts = clientInfoResult.Data.accounts;
                        var accountData = new List<AccountDto>();

                        //get accountgroup by name (Bank)
                        var accountsGroup = await _accountGroupService.GetAccountGroupsForUser(userId);
                        int accountGroupId = 0;
                        if (accountsGroup != null && accountsGroup.Data != null)
                        {
                            foreach (var accountGroup in accountsGroup.Data)
                            {
                                if (accountGroup?.AccountGroupNameEn != null && accountGroup.AccountGroupNameEn.Equals("Bank"))
                                {
                                    accountGroupId = accountGroup.AccountGroupId;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            AccountGroupDto ag = new AccountGroupDto
                            {
                                AccountGroupNameEn = "Bank",
                                AccountGroupNameRu = "",
                                AccountGroupNameUa = "",
                                GroupOrderBy = 0
                            };
                            var newAccountGroup = await _accountGroupService.CreateAccountGroupForUser(ag, userId);
                            if (newAccountGroup.IsSuccess == true)
                            {
                                accountGroupId = newAccountGroup.Data.AccountGroupId;
                            }
                            else
                            {
                                //ServiceResult<AccountGroupDto>.Error("Failed to create accountGroup");
                                return ServiceResult<IEnumerable<AccountDto>>.Error("Failed to create accountGroup");
                            }
                        }

                        //get monobank id glmfo=322001
                        var monobank = await _bankService.GetBankByGLMFO(322001);
                        int monoBankId = 0;
                        if (accountsGroup != null && accountsGroup.Data != null)
                            monoBankId = monobank.Data.GLMFO;
                        else
                            return ServiceResult<IEnumerable<AccountDto>>.NotFound("Monobank from UnivarsalBank not found");


                        foreach (var monoAccount in monoAccounts)
                        {
                                var accountFromDB = await _accountService.GetExternalAccountById(monoAccount.id, userId, false);

                                if (accountFromDB.Data != null)
                                    continue;

                            //get currencyid
                            string currencyCountry = "";

                            switch (monoAccount.currencyCode)
                            {
                                case 840: currencyCountry = "United States of America (the)"; 
                                    break;
                                case 980: currencyCountry = "Ukraine";
                                    break;
                                case 978: currencyCountry = "European Union";
                                    break;
                                default:
                                    return ServiceResult<IEnumerable<AccountDto>>.NotFound($"Currency {monoAccount.currencyCode} not found");
                                    //break;
                            }

                            var currency = _currencyService.GetCurrencyByCountry(monoAccount.currencyCode, currencyCountry);
                            int currencyId = 0;
                            string currencyCode = "";

                            if (currency.Result.IsSuccess)
                            {
                                currencyId = currency.Result.Data.CurrencyId;
                                currencyCode = currency.Result.Data.Code;
                            }
                            else
                                ServiceResult<Bank>.NotFound($"Currency {monoAccount.currencyCode} not found");

                            //create account for adding
                            var accountDto = new AccountDto
                            {
                                ExternalId = monoAccount.id,
                                AccountStatus = 1,
                                Favorites = 0,
                                AccountGroupId = accountGroupId,
                                AccountTypeId = 2, //banks account
                                                   //CurrencyId =
                                                   //BankId =
                                Name = "Mono | " + monoAccount.type + " card |  " + $"[{currencyCode}]",
                                Bank_GLMFO = monoBankId,
                                CurrencyId = currencyId,
                                OpeningDate = DateTime.UtcNow,
                                Note = monoAccount.type
                                };

                                // Add the created AccountDto to the list
                                //accountData.Add(accountDto);

                            var createdAccount = await _accountService.CreateAccountForUser(accountDto , userId);
                            if (createdAccount.IsSuccess == false )
                                return ServiceResult<IEnumerable<AccountDto>>.Error("Failed to add accounts from monobank");

                            accountData.Add(createdAccount.Data);
                         }

                        if (accountData.Count == 0)
                        {
                            return ServiceResult<IEnumerable<AccountDto>>.NotFound("Accounts for synchronization not found");
                        }



                        return ServiceResult<IEnumerable<AccountDto>>.Success(accountData);
                    }   
                    else
                    {
                        return ServiceResult<IEnumerable<AccountDto>>.Error("Failed to fetch accounts from monobank");
                    }
                }
                catch(Exception ex)
                {
                    return ServiceResult<IEnumerable<AccountDto>>.Error("Failed to fetch accounts" + ex.Message);
                }
            }
        */
    }
}
