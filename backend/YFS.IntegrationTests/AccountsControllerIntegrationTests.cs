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
    public class AccountsControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;

        public AccountsControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
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
            var openAccounts = accounts.Where(a => a.AccountStatus.Equals(1));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(accounts);
            Assert.True(openAccounts.Count() > 0);
        }
        [Fact]
        public async Task Post_Create_AccountsForDemoUser_Return_Success()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Accounts");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            string accountName = $"AccountTest_{Guid.NewGuid()}";

            var requestAccountBody = new
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
            var jsonRequestBody = JsonConvert.SerializeObject(requestAccountBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            request.Content = content;

            //Act
            var response = await _client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var newAccount = JsonConvert.DeserializeObject<AccountDto>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(newAccount);
            Assert.True(newAccount.Name?.Equals(accountName));
        }
        [Fact]
        public async Task Put_Update_Account_Returns_Success()
        {
            //Arrange
            var createAccountRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Accounts");
            createAccountRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            string accountCreateName = $"AccountTest_{Guid.NewGuid()}";

            var createAccountRequestBody = new
            {
                id = 0,
                accountStatus = 0,
                favorites = 0,
                accountGroupId = 0,
                accountTypeId = 0,
                currencyId = 840,
                bankId = 1,
                name = accountCreateName,
                openingDate = DateTime.Now,
                note = "test note",
                balance = 0
            };
            var createRequestBody = JsonConvert.SerializeObject(createAccountRequestBody);
            createAccountRequest.Content = new StringContent(createRequestBody, Encoding.UTF8, "application/json");

            var createResponse = await _client.SendAsync(createAccountRequest);
            createResponse.EnsureSuccessStatusCode();
            var createResponseContent = await createResponse.Content.ReadAsStringAsync();
            var createdAccount = JsonConvert.DeserializeObject<AccountDto>(createResponseContent);

            var updateAccountRequest = new HttpRequestMessage(HttpMethod.Put, "/api/Accounts");
            updateAccountRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var updateAccountRequestBody = new
            {
                id = createdAccount.Id,
                accountStatus = 1, // Example: Update the account status
                favorites = 1,
                accountGroupId = 0,
                accountTypeId = 0,
                currencyId = 840,
                bankId = 1,
                name = "updatedAccountName",
                //openingDate = DateTime.Now,
                note = "updated note",
                balance = 0 
            };

            var updateRequestBody = JsonConvert.SerializeObject(updateAccountRequestBody);
            updateAccountRequest.Content = new StringContent(updateRequestBody, Encoding.UTF8, "application/json");


            // Act
            var updateResponse = await _client.SendAsync(updateAccountRequest);
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedAccount = JsonConvert.DeserializeObject<AccountDto>(updateResponseContent);


            // Assert
            updateResponse.EnsureSuccessStatusCode();
            Assert.Equal(updatedAccount.AccountStatus, 1); 
            Assert.Equal(updatedAccount.Note, "updated note"); 
            Assert.Equal(updatedAccount.Name, "updatedAccountName");
        }
        [Fact]
        public async Task Get_AccountById_Return_Success()
        {
            //Arrange
            var requestCreateAccount = new HttpRequestMessage(HttpMethod.Post, $"/api/Accounts/");
            requestCreateAccount.Headers.Authorization = 
                new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());   
            string accountName = $"AccountTest_{Guid.NewGuid()}";
            var requestAccountBody = new
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
            var jsonRequestBody = JsonConvert.SerializeObject(requestAccountBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            requestCreateAccount.Content = content;
            var response = await _client.SendAsync(requestCreateAccount);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var newAccount = JsonConvert.DeserializeObject<AccountDto>(responseContent);
            var requestGetAccount = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{newAccount.Id}");
            requestGetAccount.Headers.Authorization = 
                new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());            

            //Act
            var responseAccount = await _client.SendAsync(requestGetAccount);
            var responseAccountContent = await responseAccount.Content.ReadAsStringAsync();
            var newGetAccount = JsonConvert.DeserializeObject<AccountDto>(responseAccountContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, responseAccount.StatusCode);
            Assert.NotNull(newGetAccount);
            Assert.True(newGetAccount.Name?.Equals(accountName));
            Assert.True(newGetAccount.Balance == 0);
            Assert.True(newGetAccount.CurrencyId == 840);
        }
    }
}
