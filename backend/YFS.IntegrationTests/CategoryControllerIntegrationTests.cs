using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class CategoryControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;

        public CategoryControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task Get_Returns_CategoriesForUser_Success()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Category");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var categoryForDemoUser = JsonConvert.DeserializeObject<CategoryDto[]>(content);
            var categoryTransfer = categoryForDemoUser.Where(c => c.Id == -1).FirstOrDefault();
            var categoryAccountBalaneAdjustment = categoryForDemoUser.Where(c => c.Id == -2).FirstOrDefault();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(categoryAccountBalaneAdjustment);
            Assert.NotNull(categoryTransfer);
            Assert.NotNull(categoryForDemoUser);
            Assert.True(categoryForDemoUser.Length > 0);
        }

    }
}
