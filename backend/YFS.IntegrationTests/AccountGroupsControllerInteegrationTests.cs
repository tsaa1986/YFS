using Azure;
using Azure.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class AccountGroupsControllerInteegrationTests
    {

        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;

        public AccountGroupsControllerInteegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task GetAccountGroupsForDemoUser_Return_Success_ListOfAccountGroups()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/AccountGroups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var accountGroupsForDemoUser = JsonConvert.DeserializeObject<AccountGroupDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(accountGroupsForDemoUser.FirstOrDefault());
            Assert.True(accountGroupsForDemoUser.Length > 0);
        }
        [Fact]
        public async Task Post_Create_AccountGroupsForDemoUser_Return_Success()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/AccountGroups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var requestBody = new
            {
              accountGroupId = 0,
              userId = "",
              accountGroupNameRu = "accountGroupDemoUserRu",
              accountGroupNameEn = "accountGroupDemoUserEn",
              accountGroupNameUa = "accountGroupDemoUserUa",
              groupOrderBy = 5
            };
            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            request.Content= content;
            //Act
            var response = await _client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var newAccountGroup = JsonConvert.DeserializeObject<AccountGroupDto>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(newAccountGroup);
            Assert.True(newAccountGroup.AccountGroupNameEn?.Equals("accountGroupDemoUserEn"));
        }
        [Fact]
        public async Task Put_Update_AccountGroup_Return_Success()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Put, "/api/AccountGroups");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var requestUpdatedAccount = new HttpRequestMessage(HttpMethod.Get, "/api/AccountGroups");
            requestUpdatedAccount.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var responseUpdatedAccount = await _client.SendAsync(requestUpdatedAccount);
            var responseUpdatedAcountContent = await responseUpdatedAccount.Content.ReadAsStringAsync();
            var accountGroupsForDemoUser = JsonConvert.DeserializeObject<AccountGroupDto[]>(responseUpdatedAcountContent);
            var accountGroupUpdated = accountGroupsForDemoUser.FirstOrDefault();

            var requestBody = new
            {
                accountGroupId = accountGroupUpdated.AccountGroupId,
                accountGroupNameRu = "accountGroupUpdatedDemoUserRu",
                accountGroupNameEn = "accountGroupUpdatedDemoUserEn",
                accountGroupNameUa = "accountGroupUpdatedDemoUserUa",
                groupOrderBy = 5
            };
            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            request.Content = content;

            //Act
            var response = await _client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var newAccountGroup = JsonConvert.DeserializeObject<AccountGroupDto>(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(newAccountGroup);
            Assert.True(newAccountGroup.AccountGroupNameEn?.Equals("accountGroupUpdatedDemoUserEn"));
        }
    }
}
