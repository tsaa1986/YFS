using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;
using YFS.Core.Dtos;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AuthControllerIntegrationTests //: IClassFixture<TestingWebAppFactory<Program>>
    {
        private static string? _jwtTokenDemoUser;
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact, Priority(1)]
        public async Task SignIn_Post_Check_Exists_DemoUser()
        {
            //Arrange
            var requestUri = "/api/Authentication/sign-in";
            var requestBody = new
            {
                userName = "demo",
                password = "123$qweR"
            };
            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            //Act          
            var response = await _client.PostAsync(requestUri, content);


            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JObject.Parse(responseContent)["token"].ToString();
            Assert.NotNull(token);
        }

        [Fact, Priority(2)]
        public async Task Me_Get_Returns_DemoUserInfo()
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Authentication/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());


            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var demoUserInfo = JsonConvert.DeserializeObject<UserAccountDto>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(demoUserInfo);
            Assert.Equal("Demo", demoUserInfo.UserName);
        }
        [Fact, Priority(3)]
        public async Task SignUp_Post_Returns_Success_CreateNewUser()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Authentication/sign-up");

            var signUpDto = new
            {
                firstName = "Anton",
                lastName = "Test",
                userName = "AntTest",
                password = "123$qweR",
                email = "anton.test@gmail.com",
                phoneNumber = "12345678"
            };
            var jsonContent = JsonConvert.SerializeObject(signUpDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
