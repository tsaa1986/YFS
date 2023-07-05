using System.Net;

namespace YFS.IntegrationTests
{
    public class CurrencyControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;

        public CurrencyControllerIntegrationTests(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient(); 

        [Fact]
        public async Task Get_ReturnsCurrencies()
        {
            //Arrange

            
            //Act
            var response = await _client.GetAsync("/api/Currency");


            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}