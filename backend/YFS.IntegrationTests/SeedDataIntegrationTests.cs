using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YFS.Controllers;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Service.Interfaces;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class SeedDataIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private static readonly SeedDataIntegrationTests _instance = new SeedDataIntegrationTests(new TestingWebAppFactory<Program>());
        private readonly IServiceProvider _serviceProvider;
        private static bool _isDatabaseInitialized = false;

        /*public SeedDataIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }*/
        private SeedDataIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _serviceProvider = _factory.Services;
        }
        public static SeedDataIntegrationTests Instance => _instance;
        public async Task<int> GetCurrencyIdByCodeAndCountry(int code, string country)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var serviceCurrency = scopedServiceProvider.GetRequiredService<ICurrencyService>();
                var currency = await serviceCurrency.GetCurrencyByCodeAndCountry(code, country);
                return currency.Data.CurrencyId;
            }
        }
        public async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            if (_isDatabaseInitialized)
                return;  // Skip initialization if already done

            // Perform database initialization logic here
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var context = scopedServiceProvider.GetRequiredService<RepositoryContext>();
                await context.Database.EnsureCreatedAsync();
            }

            _isDatabaseInitialized = true;
        }
        public async Task<int> CreateAccountUAH()
        {
            var createAccountRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Accounts");
            createAccountRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            string accountName = $"AccountTest_{Guid.NewGuid()}";

            var requestAccountGroup = new HttpRequestMessage(HttpMethod.Get, "/api/AccountGroups");
            requestAccountGroup.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var responseAccountGroup = await _client.SendAsync(requestAccountGroup);
            var contentAccountGroup = await responseAccountGroup.Content.ReadAsStringAsync();
            var accountGroupsForDemoUser = JsonConvert.DeserializeObject<AccountGroupDto[]>(contentAccountGroup);

            int accountGroupId = 0;
            if (accountGroupsForDemoUser != null)
            {
                if (accountGroupsForDemoUser != null)
                {
                    // Assuming you want to find the first account group with a specific condition
                    var desiredAccountGroup = accountGroupsForDemoUser.FirstOrDefault(ag => ag.Translations.Any(t => t.AccountGroupName.Equals("Bank")));

                    if (desiredAccountGroup != null)
                    {
                        accountGroupId = desiredAccountGroup.AccountGroupId;
                    }
                }
            }

            int currencyId = await GetCurrencyIdByCodeAndCountry(980, "Ukraine");
            // Create account
            var createAccountBody = new
             {
                    Id = 0,
                    ExternalId = "",  // This value should be appropriately set
                    AccountStatus = 0,
                    Favorites = 0,
                    AccountGroupId = accountGroupId,
                    AccountTypeId = 1,
                    AccountIsEnabled = 1,
                    CurrencyId = currencyId,
                    Bank_GLMFO = 322001,
                    Name = accountName,    // Assuming accountName is defined elsewhere in your code
                    OpeningDate = DateTime.UtcNow,
                    Note = "test note for " + accountName,
                    Balance = 0m          // Ensure the decimal is correctly formatted with 'm'
             };

            var createAccountRequestBody = JsonConvert.SerializeObject(createAccountBody);
            createAccountRequest.Content = new StringContent(createAccountRequestBody, Encoding.UTF8, "application/json");

            var createAccountResponse = await _client.SendAsync(createAccountRequest);
            createAccountResponse.EnsureSuccessStatusCode();

            var responseAccountContent = await createAccountResponse.Content.ReadAsStringAsync();
            var newAccounts = JsonConvert.DeserializeObject<AccountDto>(responseAccountContent);

            if (newAccounts == null)
              {
                 throw new InvalidOperationException("Account creation failed, no account returned.");
              }

            return newAccounts.Id;
        }
        public async Task<IEnumerable<OperationDto>> CreateOperationUAH(int accountId, DateTime operationDate, 
            OperationDto.OperationType operationType,int categoryId, decimal operationAmount)
        {
            // Create operation 1
            if (OperationDto.OperationType.Transfer == operationType)
                throw new Exception("Transfer operation is restricted! Use only income/expense");

            var createOperationRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperationRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            int currencyId = await GetCurrencyIdByCodeAndCountry(980, "Ukraine");

            var operationRequestBody = new
            {
                AccountId = accountId,
                TypeOperation = operationType,
                OperationCurrencyId = currencyId,
                OperationDate = operationDate.ToUniversalTime(), // Ensure it's UTC
                Description = "description operation",
                OperationItems = new List<object>
                {
                    new
                    {
                        CategoryId = categoryId,
                        CurrencyAmount = operationAmount,
                        OperationAmount = operationAmount,
                        Description = "description operation item"
                    }
                }
            };

            var requestBody = JsonConvert.SerializeObject(operationRequestBody);
            createOperationRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Send the request to create the operation
            var createOperationResponse = await _client.SendAsync(createOperationRequest);
            createOperationResponse.EnsureSuccessStatusCode();

            // Deserialize the response to get the created operation
            var content = await createOperationResponse.Content.ReadAsStringAsync();
            var createdOperation = JsonConvert.DeserializeObject<OperationDto[]>(content);

            return createdOperation;
        }
        public async Task<IEnumerable<OperationDto>> CreateTransferOperation(int _accountWithdrawId, int _accountTargetId,
            DateTime _dateOperation, decimal _operationAmount)
        {
            // Create operation 1
            var createOperation1Request = new HttpRequestMessage(HttpMethod.Post, $"/api/Operations/{_accountTargetId}");
            createOperation1Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            int currencyId = await GetCurrencyIdByCodeAndCountry(980, "Ukraine");

            var createOperation1Body = new
            {
                transferOperationId = 0,
                categoryId = -1,
                typeOperation = OperationDto.OperationType.Transfer, // Transfer
                accountId = _accountWithdrawId,
                operationCurrencyId = currencyId, // Using the currencyId obtained earlier
                currencyAmount = _operationAmount,
                operationAmount = _operationAmount,
                operationDate = _dateOperation.ToUniversalTime(), // Ensure operationDate is in UTC format
                description = "description transfer operation 1",
                operationItems = new[]
                {
                    new
                    {
                        categoryId = -1, // Assuming category ID for transfer
                        currencyAmount = _operationAmount,
                        operationAmount = _operationAmount,
                        description = "Description for operation item" // Modify as needed
                    }
                }
            };

            var createOperation1RequestBody = JsonConvert.SerializeObject(createOperation1Body);
            createOperation1Request.Content = new StringContent(createOperation1RequestBody, Encoding.UTF8, "application/json");
            var createOperation1Response = await _client.SendAsync(createOperation1Request);
            createOperation1Response.EnsureSuccessStatusCode();
            var contentTransferOperation = await createOperation1Response.Content.ReadAsStringAsync();
            var operations = JsonConvert.DeserializeObject<OperationDto[]>(contentTransferOperation);

            return operations;          
        }
        public async Task CreateOperationIncome3monthAutomatically(int accountId)
        {
            var operationType = OperationDto.OperationType.Income;
            var incomeAmount = 100000M;

            for (int i = 0; i < 3; i++)
            {
                var monthOffset = -i;
                var operationDate = DateTime.UtcNow.AddMonths(monthOffset);

                var createdOperations = await CreateOperationUAH(accountId, operationDate, operationType, 2, incomeAmount);

                if (createdOperations.Count() == 0)
                {
                    throw new Exception($"Failed to create income operation for {operationDate}");
                }
            }
        }
    }
}
