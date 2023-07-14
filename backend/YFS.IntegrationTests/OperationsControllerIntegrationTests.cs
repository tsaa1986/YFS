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

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class OperationsControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;

        public OperationsControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Last10OperationsAccount_Returns_Success()
        {
            //Arrange
            int accountId = 1;
            var requestOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/operations/last10/{accountId}");
            requestOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var createOperation1Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation1Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var createOperation2Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation2Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var createOperation3Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation3Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //add 3 operations automatically with other date month
            var createOperation1Body = new
            {
                transferOperationId = 0,
                categoryId = 2,
                typeOperation = 2,
                accountId = accountId,
                operationCurrencyId = 980,
                currencyAmount = 100000,
                operationAmount = 100000,
                operationDate = DateTime.Now,
                description = "description operation 1",
                tag = "tag operation 1"
            };
            var createOperation2Body = new
            {
                transferOperationId = 0,
                categoryId = 2,
                typeOperation = 2,
                accountId = accountId,
                operationCurrencyId = 980,
                currencyAmount = 100000,
                operationAmount = 100000,
                operationDate = DateTime.Now.AddMonths(-1),
                description = "description operation 2",
                tag = "tag operation 2"
            };
            var createOperation3Body = new
            {
                transferOperationId = 0,
                categoryId = 2,
                typeOperation = 2,
                accountId = accountId,
                operationCurrencyId = 980,
                currencyAmount = 100000,
                operationAmount = 100000,
                operationDate = DateTime.Now.AddMonths(-2),
                description = "description operation 3",
                tag = "tag operation 3"
            };

            var createOperation1RequestBody = JsonConvert.SerializeObject(createOperation1Body);
            createOperation1Request.Content = new StringContent(createOperation1RequestBody, Encoding.UTF8, "application/json");
            var createOperation1Response = await _client.SendAsync(createOperation1Request);
            createOperation1Response.EnsureSuccessStatusCode();

            var createOperation2RequestBody = JsonConvert.SerializeObject(createOperation2Body);
            createOperation2Request.Content = new StringContent(createOperation2RequestBody, Encoding.UTF8, "application/json");
            var createOperation2Response = await _client.SendAsync(createOperation2Request);
            createOperation2Response.EnsureSuccessStatusCode();

            var createOperation3RequestBody = JsonConvert.SerializeObject(createOperation3Body);
            createOperation3Request.Content = new StringContent(createOperation3RequestBody, Encoding.UTF8, "application/json");
            var createOperation3Response = await _client.SendAsync(createOperation3Request);
            createOperation3Response.EnsureSuccessStatusCode();

            //Act
            var responseOperation = await _client.SendAsync(requestOperation);
            var contentOperation = await responseOperation.Content.ReadAsStringAsync();
            var operations = JsonConvert.DeserializeObject<OperationDto[]>(contentOperation);

            //Assert
            Assert.Equal(HttpStatusCode.OK, responseOperation.StatusCode);
            Assert.True(operations.Length == 3);
        }
    }
}
