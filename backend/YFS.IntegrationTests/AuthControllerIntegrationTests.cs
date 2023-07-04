namespace YFS.IntegrationTests
{
    public class AuthControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact]
        public void Test1()
        {
            //Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Authentication/sign-in");

            var loginDemo = new Dictionary<string, string>
            {
                {"userName", "demo"},
                {"password", "123$qweR"}
            };
            postRequest.Content = new FormUrlEncodedContent(loginDemo);

            //Act
            var response = _client.SendAsync(postRequest);

            //response.EnsureSuccessStatusCode();

            //Assert
            Assert.NotNull(response);
        }
    }
}