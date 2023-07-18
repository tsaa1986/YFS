using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class SeedDataIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        public SeedDataIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
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
        public async Task CreateOperationIncome(int _accountId)
        {

        }
    }
}
