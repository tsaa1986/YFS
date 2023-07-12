using Newtonsoft.Json;
using System.Net;
using YFS.Core.Dtos;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class CurrencyControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;

        public CurrencyControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Returns_Currencies()
        {
            //Arrange
            //contain in CurrencyData

            //Act
            var response = await _client.GetAsync("/api/Currency");
            var content = await response.Content.ReadAsStringAsync();
            var currencies = JsonConvert.DeserializeObject<CurrencyDto[]>(content);

            var currencyUSDDto = currencies.Where(a => a.CurrencyId.Equals(840));
            var currencyUAHDto = currencies.Where(a => a.CurrencyId.Equals(980));
            var currencyEuroDto = currencies.Where(a => a.CurrencyId.Equals(978));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.NotNull(currencyUSDDto.FirstOrDefault());
            Assert.NotNull(currencyUAHDto.FirstOrDefault());
            Assert.NotNull(currencyEuroDto.FirstOrDefault());
        }        
    }
}