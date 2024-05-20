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
            ICurrencyService currencyService
            ) : base(repository, mapper, logger)
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
                if (clientInfoResult.Data.accounts.Count == 0)
                    ServiceResult<IEnumerable<AccountDto>>.NotFound("Accounts from monobank not found");


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
                            ServiceResult<AccountGroupDto>.Error("Failed to create accountGroup");
                        }
                    }

                    //get monobank id glmfo=322001
                    var monobank = await _bankService.GetBankByGLMFO(322001);
                    int monoBankId = 0;
                    if (accountsGroup != null && accountsGroup.Data != null)
                        monoBankId = monobank.Data.GLMFO;
                    else
                        ServiceResult<Bank>.NotFound("Monobank from UnivarsalBank not found");


                    foreach (var monoAccount in monoAccounts)
                        {
                            var accountFromDB = await _accountService.GetExternalAccountById(monoAccount.id, userId, false);

                            if (accountFromDB.Data != null)
                                break;

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
                                ServiceResult<Bank>.NotFound($"Currency {monoAccount.currencyCode} not found");
                                break;
                        }

                        var currency = _currencyService.GetCurrencyByCountry(monoAccount.currencyCode, currencyCountry);
                        int currencyId = 0;

                        if (currency.Result.IsSuccess)
                            currencyId = currency.Result.Data.CurrencyId;
                        else
                            ServiceResult<Bank>.NotFound($"Currency {monoAccount.currencyCode} not found");

                        var account = new AccountDto
                        {
                            ExternalId = monoAccount.id,
                            AccountStatus = 1,
                            Favorites = 0,
                            AccountGroupId = accountGroupId,
                            AccountTypeId = 2, //banks account
                                               //CurrencyId =
                                               //BankId =
                            Name = "mono" + monoAccount.type,
                            Bank_GLMFO = monoBankId,
                            CurrencyId = currencyId,
                            OpeningDate = DateTime.UtcNow,
                            Note = monoAccount.type
                            };

                            // Add the created AccountDto to the list
                            accountData.Add(account);
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
    }
}
