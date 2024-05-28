using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using YFS.Controllers;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Service.Interfaces;
using YFS.Service.Services;
using static YFS.Controllers.OperationsController;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class AccountsControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly SeedDataIntegrationTests _seedDataIntegrationTests;
        private readonly IServiceProvider _serviceProvider;

        public AccountsControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _seedDataIntegrationTests = SeedDataIntegrationTests.Instance;

            // Ensure database is initialized before running tests
            //_seedDataIntegrationTests.InitializeDatabaseAsync().Wait();
            using (var scope = _factory.Services.CreateScope())
            {
                // Get the scoped service provider
                _serviceProvider = scope.ServiceProvider;

                // Ensure database is initialized before running tests
                _seedDataIntegrationTests.InitializeDatabaseAsync(_serviceProvider).Wait();
            }
        }

        [Fact]
        public async Task Get_Returns_OpenAccountByUserId_Success()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Accounts/openAccountsByUserId");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accounts = JsonConvert.DeserializeObject<AccountDto[]>(content);
            var openAccounts = accounts.Where(a => a.AccountIsEnabled.Equals(1));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(accounts);
            Assert.True(openAccounts.Count() > 0);
        }
        [Fact]
        public async Task Post_Create_AccountsForDemoUser_Return_Success()
        {
            //Arrange
            int newAccountId = await _seedDataIntegrationTests.CreateAccountUAH();

            //Act
            var getAccountRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{newAccountId}");
            getAccountRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var getAccountResponse = await _client.SendAsync(getAccountRequest);
            var responseContent = await getAccountResponse.Content.ReadAsStringAsync();
            var newAccount = JsonConvert.DeserializeObject<AccountDto>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, getAccountResponse.StatusCode);
            Assert.NotNull(newAccount);
            Assert.Equal(newAccountId, newAccount.Id);
        }
        [Fact]
        public async Task Put_Update_Account_Returns_Success()
        {
            //Arrange
            int cerateAccountId = await _seedDataIntegrationTests.CreateAccountUAH();
            var requestGetAccount = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{cerateAccountId}");
            requestGetAccount.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var responseAccount = await _client.SendAsync(requestGetAccount);
            var responseAccountContent = await responseAccount.Content.ReadAsStringAsync();
            var accountToUpdate = JsonConvert.DeserializeObject<AccountDto>(responseAccountContent);

            accountToUpdate.Name = "updatedAccountName";
            accountToUpdate.Note = "updated note";
            accountToUpdate.AccountIsEnabled = 1;

            var updateAccountRequest = new HttpRequestMessage(HttpMethod.Put, "/api/Accounts");
            updateAccountRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var updateRequestBody = JsonConvert.SerializeObject(accountToUpdate);
            updateAccountRequest.Content = new StringContent(updateRequestBody, Encoding.UTF8, "application/json");

            //Act
            var updateResponse = await _client.SendAsync(updateAccountRequest);
            updateResponse.EnsureSuccessStatusCode();
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedAccount = JsonConvert.DeserializeObject<AccountDto>(updateResponseContent);

            // Assert
            Assert.NotNull(updatedAccount);
            Assert.Equal(1, updatedAccount.AccountIsEnabled);
            Assert.Equal("updated note", updatedAccount.Note);
            Assert.Equal("updatedAccountName", updatedAccount.Name);
        }
        [Fact]
        public async Task Get_AccountById_Return_Success()
        {
            //Arrange
            int cerateAccountId = await _seedDataIntegrationTests.CreateAccountUAH();

            var requestGetAccount = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{cerateAccountId}");
            requestGetAccount.Headers.Authorization = 
            new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());            

            //Act
            var responseAccount = await _client.SendAsync(requestGetAccount);
            var responseAccountContent = await responseAccount.Content.ReadAsStringAsync();
            var newGetAccount = JsonConvert.DeserializeObject<AccountDto>(responseAccountContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, responseAccount.StatusCode);
            Assert.NotNull(newGetAccount);
            Assert.True(newGetAccount.Id == cerateAccountId);
        }
        [Fact]
        public async Task Get_CheckAccountBalanceAfterIncomeOperation_Return_Success()
        {
            //Arrange
            int _accountId = await _seedDataIntegrationTests.CreateAccountUAH();
            var operationIncome = await _seedDataIntegrationTests.CreateOperationUAH(_accountId, DateTime.UtcNow, 
                OperationDto.OperationType.Income, 2,100000.23M);
            var requestAccount = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountId}");
            requestAccount.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());


            //Act
            var responseAccount = await _client.SendAsync(requestAccount);
            var contentAccoount = await responseAccount.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountDto>(contentAccoount);

            //Assert
            Assert.Equal(HttpStatusCode.OK, responseAccount.StatusCode);
            Assert.True(account.Balance == 100000.23M);
        }

    }
}
