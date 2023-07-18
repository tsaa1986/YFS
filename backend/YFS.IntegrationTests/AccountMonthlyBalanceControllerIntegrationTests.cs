using Azure.Core;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class AccountMonthlyBalanceControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly SeedDataIntegrationTests _seedData;

        public AccountMonthlyBalanceControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _seedData = new SeedDataIntegrationTests(factory);
        }

        [Fact]
        public async Task Get_Returns_AccountMonthlyBalanceByAccountId_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);
            //var openAccounts = accounts.Where(a => a.AccountStatus.Equals(1));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(accountMonthlyBalances);
            //Assert.True(openAccounts.Count() > 0);
        }
    }
}
