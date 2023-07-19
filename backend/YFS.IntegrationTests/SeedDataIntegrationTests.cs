using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YFS.Controllers;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class SeedDataIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private static readonly SeedDataIntegrationTests _instance = new SeedDataIntegrationTests();

        /*public SeedDataIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }*/
        private SeedDataIntegrationTests()
        {
            _factory = new TestingWebAppFactory<Program>();
            _client = _factory.CreateClient();
        }
        public static SeedDataIntegrationTests Instance => _instance;
        public async Task<int> CreateAccountUAH()
        {
            var createAccountRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Accounts");
            createAccountRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            string accountName = $"AccountTest_{Guid.NewGuid()}";

            // Create account
            var createAccountBody = new
            {
                id = 0,
                accountStatus = 0,
                favorites = 0,
                accountGroupId = 0,
                accountTypeId = 0,
                currencyId = 840,
                bankId = 1,
                name = accountName,
                openingDate = DateTime.Now,
                note = "test note",
                balance = 0
            };

            var createAccountRequestBody = JsonConvert.SerializeObject(createAccountBody);
            createAccountRequest.Content = new StringContent(createAccountRequestBody, Encoding.UTF8, "application/json");
            var createAccountResponse = await _client.SendAsync(createAccountRequest);
            createAccountResponse.EnsureSuccessStatusCode();
            var responseAccountContent = await createAccountResponse.Content.ReadAsStringAsync();
            var newAccount = JsonConvert.DeserializeObject<AccountDto>(responseAccountContent);

            return newAccount.Id;
        }
        public async Task<IEnumerable<OperationDto>> CreateOperationIncome(int accountId, DateTime operationDate, decimal operationAmount)
        {
            // Create operation 1
            var createOperation1Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation1Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var createOperation1Body = new
            {
                transferOperationId = 0,
                categoryId = 2,
                typeOperation = OperationsController.OperationType.Income, //income
                accountId = accountId,
                operationCurrencyId = 980,
                currencyAmount = operationAmount,
                operationAmount = operationAmount,
                operationDate = operationDate,
                description = "description operation 1",
                tag = "tag operation 1"
            };

            var createOperation1RequestBody = JsonConvert.SerializeObject(createOperation1Body);
            createOperation1Request.Content = new StringContent(createOperation1RequestBody, Encoding.UTF8, "application/json");
            var createOperation1Response = await _client.SendAsync(createOperation1Request);
            createOperation1Response.EnsureSuccessStatusCode();

            var contentOperation = await createOperation1Response.Content.ReadAsStringAsync();
            var operations = JsonConvert.DeserializeObject<OperationDto[]>(contentOperation);

            return operations;
        }
       
    }
}
