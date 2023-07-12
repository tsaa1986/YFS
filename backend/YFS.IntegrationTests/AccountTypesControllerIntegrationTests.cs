using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class AccountTypesControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;

        public AccountTypesControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Returns_AccountTypes()
        {
            //Arrange
            //contain in AccountTypeData

            //Act
            var response = await _client.GetAsync("/api/AccountTypes");
            var content = await response.Content.ReadAsStringAsync();
            var accountTypes = JsonConvert.DeserializeObject<AccountTypeDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(accountTypes.FirstOrDefault());
            Assert.True(accountTypes.Length > 0);
        }
    }
}
