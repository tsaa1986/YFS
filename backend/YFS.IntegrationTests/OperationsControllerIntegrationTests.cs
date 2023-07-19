using Azure;
using Azure.Core;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using NuGet.Frameworks;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit.Priority;
using YFS.Controllers;
using YFS.Core.Dtos;
using YFS.Core.Models;
using static YFS.Controllers.OperationsController;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class OperationsControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly SeedDataIntegrationTests _seedData;

        public OperationsControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _seedData = SeedDataIntegrationTests.Instance;
        }
        //create 3 operation. per 100 000
        private async Task CreateOperationIncome3monthAutomatically(int _accountId)
        {
            var operationIncome1 = await _seedData.CreateOperation(_accountId, DateTime.Now,
                OperationType.Income, 2, 100000M);
            // Create operation 1
            if (operationIncome1.Count() == 0) { throw new Exception(); }

            // Create operation 2
            var operationIncome2 = await _seedData.CreateOperation(_accountId, DateTime.Now.AddMonths(-1),
                OperationType.Income, 2, 100000M);
            if (operationIncome2.Count() == 0) { throw new Exception(); }

            // Create operation 3
            var operationIncome3 = await _seedData.CreateOperation(_accountId, DateTime.Now.AddMonths(-2),
                OperationType.Income, 2, 100000M);
            if (operationIncome3.Count() == 0) { throw new Exception(); }
        }
        private async Task<int> CreateAccountUAH()
        {
            var createAccountRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Accounts");
            createAccountRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            string accountName = $"AccountTest_{Guid.NewGuid()}";

            // Create account
            var createAccountBody = new
            {
                id = 0,
                accountStatus = 0,
                favorites = 0,
                accountGroupId = 0,
                accountTypeId = 0,
                currencyId = 840,
                bankId = 1,
                name = accountName,
                openingDate = DateTime.Now,
                note = "test note",
                balance = 0
            };

            var createAccountRequestBody = JsonConvert.SerializeObject(createAccountBody);
            createAccountRequest.Content = new StringContent(createAccountRequestBody, Encoding.UTF8, "application/json");
            var createAccountResponse = await _client.SendAsync(createAccountRequest);
            createAccountResponse.EnsureSuccessStatusCode();
            var responseAccountContent = await createAccountResponse.Content.ReadAsStringAsync();
            var newAccount = JsonConvert.DeserializeObject<AccountDto>(responseAccountContent);

            return newAccount.Id;
        }

        [Fact, Priority(1)]
        public async Task Get_Last10OperationsAccount_Returns_Success()
        {
            //Arrange
            int _accountId = 1;
            var requestOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/operations/last10/{_accountId}");
            requestOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            await CreateOperationIncome3monthAutomatically(_accountId);

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
        public async Task Get_OperationForPeriod_Returns_Success()
        {
            //Arrange
            int _accountId = 1;
            String startDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd") + " 00:00:00";
            String endDate = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            //2023 - 04 - 01 / 2023 - 04 - 28
            var requestOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Operations/period/{_accountId}/{startDate}/{endDate}");
            requestOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            await CreateOperationIncome3monthAutomatically(_accountId);

            //Act
            var responseOperation = await _client.SendAsync(requestOperation);
            var contentOperation = await responseOperation.Content.ReadAsStringAsync();
            var operations = JsonConvert.DeserializeObject<OperationDto[]>(contentOperation);

            //Assert
            Assert.Equal(HttpStatusCode.OK, responseOperation.StatusCode);
            Assert.True(operations.Length == 6);
            Assert.True(operations[0].Balance == 600000);
        }
        [Fact]
        public async Task Post_CreateIncomeOperation_Return_Success()
        {
            //Arrange
            int accountId = await CreateAccountUAH();
            // Create operation 1
            var createOperation1Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation1Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var createOperation1Body = new
            {
                transferOperationId = 0,
                categoryId = 2,
                typeOperation = OperationsController.OperationType.Income, //income
                accountId = accountId,
                operationCurrencyId = 980,
                currencyAmount = 100000.23,
                operationAmount = 100000.23,
                operationDate = DateTime.Now,
                description = "description operation 1",
                tag = "tag operation 1"
            };

            var createOperation1RequestBody = JsonConvert.SerializeObject(createOperation1Body);
            createOperation1Request.Content = new StringContent(createOperation1RequestBody, Encoding.UTF8, "application/json");
            var createOperation1Response = await _client.SendAsync(createOperation1Request);
            createOperation1Response.EnsureSuccessStatusCode();


            var requestOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/operations/last10/{accountId}");
            requestOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());


            //Act
            var responseOperation = await _client.SendAsync(requestOperation);
            var contentOperation = await responseOperation.Content.ReadAsStringAsync();
            var operations = JsonConvert.DeserializeObject<OperationDto[]>(contentOperation);

            //Asset
            Assert.Equal(HttpStatusCode.OK, responseOperation.StatusCode);
            Assert.True(operations.Length == 1);
            Assert.True(operations[0].Balance == 100000.23M);
        }
        [Fact]
        public async Task Post_CreateExpenseOperation_Return_Success()
        {
            //Arrange
            int accountId = await CreateAccountUAH();
            // Create operation 1
            var createOperation1Request = new HttpRequestMessage(HttpMethod.Post, "/api/Operations/0");
            createOperation1Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var createOperation1Body = new
            {
                transferOperationId = 0,
                categoryId = 7,
                typeOperation = OperationsController.OperationType.Expense, //expense
                accountId = accountId,
                operationCurrencyId = 980,
                currencyAmount = 1000.23,
                operationAmount = 1000.23,
                operationDate = DateTime.Now,
                description = "description operation 1",
                tag = "tag operation 1"
            };

            var createOperation1RequestBody = JsonConvert.SerializeObject(createOperation1Body);
            createOperation1Request.Content = new StringContent(createOperation1RequestBody, Encoding.UTF8, "application/json");
            var createOperation1Response = await _client.SendAsync(createOperation1Request);
            createOperation1Response.EnsureSuccessStatusCode();

            var requestOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/operations/last10/{accountId}");
            requestOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());


            //Act
            var responseOperation = await _client.SendAsync(requestOperation);
            var contentOperation = await responseOperation.Content.ReadAsStringAsync();
            var operations = JsonConvert.DeserializeObject<OperationDto[]>(contentOperation);

            //Asset
            Assert.Equal(HttpStatusCode.OK, responseOperation.StatusCode);
            Assert.True(operations.Length == 1);
            Assert.True(operations[0].Balance == -1000.23M);
        }
        [Fact]
        public async Task Post_CreateTransferOperation_Return_Success()
        {
            //Arrange
            int accountWithdrawId = await CreateAccountUAH();
            int accountTargetId = await CreateAccountUAH();
            // Create operation 1
            var createOperation1Request = new HttpRequestMessage(HttpMethod.Post, $"/api/Operations/{accountTargetId}");
            createOperation1Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            var createOperation1Body = new
            {
                transferOperationId = 0,
                categoryId = 7,
                typeOperation = OperationsController.OperationType.Transfer, //expense
                accountId = accountWithdrawId,
                operationCurrencyId = 980,
                currencyAmount = 5000.13,
                operationAmount = 5000.13,
                operationDate = DateTime.Now,
                description = "description operation 1",
                tag = "tag operation 1"
            };

            var createOperation1RequestBody = JsonConvert.SerializeObject(createOperation1Body);
            createOperation1Request.Content = new StringContent(createOperation1RequestBody, Encoding.UTF8, "application/json");
            var createOperation1Response = await _client.SendAsync(createOperation1Request);
            createOperation1Response.EnsureSuccessStatusCode();

            var requestWithdrawOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/operations/last10/{accountWithdrawId}");
            requestWithdrawOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var requestTargetOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/operations/last10/{accountTargetId}");
            requestTargetOperation.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());


            //Act
            var responseWithdrawOperation = await _client.SendAsync(requestWithdrawOperation);
            var contentWithdrawOperation = await responseWithdrawOperation.Content.ReadAsStringAsync();
            var operationsWithdraw = JsonConvert.DeserializeObject<OperationDto[]>(contentWithdrawOperation);
            var responseTargetOperation = await _client.SendAsync(requestTargetOperation);
            var contentTargetOperation = await responseTargetOperation.Content.ReadAsStringAsync();
            var operationsTarget = JsonConvert.DeserializeObject<OperationDto[]>(contentTargetOperation);

            //Asset
            Assert.Equal(HttpStatusCode.OK, responseWithdrawOperation.StatusCode);
            Assert.True(operationsWithdraw.Length == 1);
            Assert.True(operationsWithdraw[0].Balance == -5000.13M);
            Assert.Equal(HttpStatusCode.OK, responseTargetOperation.StatusCode);
            Assert.True(operationsTarget.Length == 1);
            Assert.True(operationsTarget[0].Balance == 5000.13M);
        }

    }
}
