using Azure;
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
        public async Task GetAccountGroupsForDemoUser()
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


        }

    }
}
