using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using YFS.Core.Models.MonoIntegration;
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
        private readonly MonoClientInfoResponse _monoClientInfoResponse;
        private readonly List<MonoTransaction> _transactionsFromWhiteUAH;
        private readonly List<MonoTransaction> _transactionsFromBlackUAH;
        private readonly List<MonoTransaction> _transactionsFromBlackEURO;
        private readonly List<MonoTransaction> _transactionsFromBlackUSD;

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

            _monoClientInfoResponse = GetClientInfoFromJsonAsync("../../../MonoIntegrationTestJson/expectedClientInfo.json").GetAwaiter().GetResult();
            // Initialize statement
            _transactionsFromWhiteUAH = InitializeTransactionsAsync("../../../MonoIntegrationTestJson/expectedStatementWhiteUAH.json").GetAwaiter().GetResult();
            _transactionsFromBlackUAH = InitializeTransactionsAsync("../../../MonoIntegrationTestJson/expectedStatementBlackUAH.json").GetAwaiter().GetResult();
            _transactionsFromBlackEURO = InitializeTransactionsAsync("../../../MonoIntegrationTestJson/expectedStatementBlackEuro.json").GetAwaiter().GetResult();
            _transactionsFromBlackUSD = InitializeTransactionsAsync("../../../MonoIntegrationTestJson/expectedStatementBlackUSD.json").GetAwaiter().GetResult();
        }
        public static SeedDataIntegrationTests Instance => _instance;

        public MonoClientInfoResponse MonoClientInfoResponse => _monoClientInfoResponse;
        public List<MonoTransaction> MonoStatementWhiteUAH => _transactionsFromWhiteUAH;
        public List<MonoTransaction> MonoStatementBlackUAH => _transactionsFromBlackUAH;
        public List<MonoTransaction> MonoStatementBlackUSD => _transactionsFromBlackUSD;
        public List<MonoTransaction> MonoStatementBlackEURO => _transactionsFromBlackEURO;

        public string GetJwtTokenForUser(string userName)
        {
            string _jwtTokenForUser = GetJwtTokenForUserAsync(userName).GetAwaiter().GetResult();
            return _jwtTokenForUser;
        }
        private async Task<string> GetJwtTokenForUserAsync(string userName)
        {
            string password = "demo123$qweR";

            // Prepare the request payload (if required)
            var requestContent = new StringContent(
                $"{{\"username\":\"{userName}\",\"password\":\"{password}\"}}",
                Encoding.UTF8,
                "application/json"
            );

            // Make the request to the authentication endpoint
            var response = await _client.PostAsync("/api/Authentication/sign-in", requestContent);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Extract the JWT token from the response
            var content = await response.Content.ReadAsStringAsync();
            var token = JObject.Parse(content)["token"].ToString();

            return token;
        }
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
        public async Task<UserAccountDto> CreateUserSignUpAsync(HttpClient client)
        {
            string uniqueUserName = "UniqueUser" + Guid.NewGuid().ToString().Substring(0, 8);
            string uniqueEmail = "uniqueuser" + Guid.NewGuid().ToString().Substring(0, 8) + "@example.com";
            string uniqueFirstName = "username" + Guid.NewGuid().ToString().Substring(0, 8);
            string uniqueLastName = "lastname-" + DateTime.UtcNow.ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Authentication/sign-up");

            var signUpDto = new
            {
                firstName = uniqueFirstName,
                lastName = uniqueLastName,
                userName = uniqueUserName, // Use the unique username
                password = "demo123$qweR",
                email = uniqueEmail,
                phoneNumber = "12345678"
            };
            var jsonContent = JsonConvert.SerializeObject(signUpDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var createdUser = JsonConvert.DeserializeObject<UserAccountDto>(responseContent);

            return createdUser;
        }
        public ApiTokenDto CreateApiTokenMonobank(string userId)
        {
            ApiTokenDto token = new ApiTokenDto
            {
                    UserId = userId,
                    Name = "apiMonoBank",
                    TokenType = "X-Token",
                    TokenValue = Guid.NewGuid().ToString(), // Generate a unique token value
                    Url = "https://api.monobank.ua/personal/client-info",
                    Note = "This token is used for accessing Monobank API"
             };

            return token;
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
        public async Task<MonoClientInfoResponse> GetClientInfoFromJsonAsync(string filePath)
        {
            // Log the current directory to help with debugging the file path
            //string path = Directory.GetCurrentDirectory();
            //Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            string jsonData = await File.ReadAllTextAsync(filePath);

            MonoClientInfoResponse clientInfo = JsonConvert.DeserializeObject<MonoClientInfoResponse>(jsonData);

            if (clientInfo == null)
            {
                throw new InvalidOperationException("Deserialization resulted in a null object.");
            }

            foreach(MonoAccount account in clientInfo.accounts)
            {
                account.id = Guid.NewGuid().ToString();
            }

            return clientInfo;
        }
        private async Task<List<MonoTransaction>> InitializeTransactionsAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            string jsonData = await File.ReadAllTextAsync(filePath);
            List<MonoTransaction> transactions = JsonConvert.DeserializeObject<List<MonoTransaction>>(jsonData);

            if (transactions == null)
            {
                throw new InvalidOperationException("Deserialization resulted in a null object.");
            }

            return transactions;
        }
    }
}
