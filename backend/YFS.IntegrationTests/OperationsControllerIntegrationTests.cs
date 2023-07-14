using Azure;
using Azure.Core;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit.Priority;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class OperationsControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;

        public OperationsControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        //create 3 operation. per 100 000
        private async Task CreateOperation3monthAutomatically(int _accountId)
        {
            var createOperation1Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation1Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            // Create operation 1
            var createOperation1Body = new
            {
                transferOperationId = 0,
                categoryId = 2,
                typeOperation = 2,
                accountId = _accountId,
                operationCurrencyId = 980,
                currencyAmount = 100000,
                operationAmount = 100000,
                operationDate = DateTime.Now,
                description = "description operation 1",
                tag = "tag operation 1"
            };

            var createOperation1RequestBody = JsonConvert.SerializeObject(createOperation1Body);
            createOperation1Request.Content = new StringContent(createOperation1RequestBody, Encoding.UTF8, "application/json");
            var createOperation1Response = await _client.SendAsync(createOperation1Request);
            createOperation1Response.EnsureSuccessStatusCode();

            // Create operation 2
            var createOperation2Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation2Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var createOperation2Body = new
            {
                transferOperationId = 0,
                categoryId = 2,
                typeOperation = 2,
                accountId = _accountId,
                operationCurrencyId = 980,
                currencyAmount = 100000,
                operationAmount = 100000,
                operationDate = DateTime.Now.AddMonths(-1),
                description = "description operation 2",
                tag = "tag operation 2"
            };

            var createOperation2RequestBody = JsonConvert.SerializeObject(createOperation2Body);
            createOperation2Request.Content = new StringContent(createOperation2RequestBody, Encoding.UTF8, "application/json");
            var createOperation2Response = await _client.SendAsync(createOperation2Request);
            createOperation2Response.EnsureSuccessStatusCode();

            // Create operation 3
            var createOperation3Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation3Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var createOperation3Body = new
            {
                transferOperationId = 0,
                categoryId = 2,
                typeOperation = 2,
                accountId = _accountId,
                operationCurrencyId = 980,
                currencyAmount = 100000,
                operationAmount = 100000,
                operationDate = DateTime.Now.AddMonths(-2),
                description = "description operation 3",
                tag = "tag operation 3"
            };

            var createOperation3RequestBody = JsonConvert.SerializeObject(createOperation3Body);
            createOperation3Request.Content = new StringContent(createOperation3RequestBody, Encoding.UTF8, "application/json");
            var createOperation3Response = await _client.SendAsync(createOperation3Request);
            createOperation3Response.EnsureSuccessStatusCode();
        }

        [Fact, Priority(1)]
        public async Task Get_Last10OperationsAccount_Returns_Success()
        {
            //Arrange
            int _accountId = 1;
            var requestOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/operations/last10/{_accountId}");
            requestOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            await CreateOperation3monthAutomatically(_accountId);

            //Act
            var responseOperation = await _client.SendAsync(requestOperation);
            var contentOperation = await responseOperation.Content.ReadAsStringAsync();
            var operations = JsonConvert.DeserializeObject<OperationDto[]>(contentOperation);

            //Assert
            Assert.Equal(HttpStatusCode.OK, responseOperation.StatusCode);
            Assert.True(operations.Length == 3);
            Assert.True(operations[0].Balance == 300000);
        }

        [Fact, Priority(2)]
        public async Task Get_LastOperationForPeriod_Returns_Success()
        {
            //Arrange
            int _accountId = 1;
            String startDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd") + " 00:00:00";
            String endDate = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            //2023 - 04 - 01 / 2023 - 04 - 28
            var requestOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Operations/period/{_accountId}/{startDate}/{endDate}");
            requestOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            await CreateOperation3monthAutomatically(_accountId);

            //Act
            var responseOperation = await _client.SendAsync(requestOperation);
            var contentOperation = await responseOperation.Content.ReadAsStringAsync();
            var operations = JsonConvert.DeserializeObject<OperationDto[]>(contentOperation);

            //Assert
            Assert.Equal(HttpStatusCode.OK, responseOperation.StatusCode);
            Assert.True(operations.Length == 6);
            Assert.True(operations[0].Balance == 600000);
        }
    }
}
