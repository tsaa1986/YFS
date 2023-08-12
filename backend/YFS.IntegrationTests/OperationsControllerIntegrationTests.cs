using Azure;
using Azure.Core;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using NuGet.Frameworks;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit.Priority;
using YFS.Controllers;
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
                OperationDto.OperationType.Income, 2, 100000M);
            // Create operation 1
            if (operationIncome1.Count() == 0) { throw new Exception(); }

            // Create operation 2
            var operationIncome2 = await _seedData.CreateOperation(_accountId, DateTime.Now.AddMonths(-1),
                OperationDto.OperationType.Income, 2, 100000M);
            if (operationIncome2.Count() == 0) { throw new Exception(); }

            // Create operation 3
            var operationIncome3 = await _seedData.CreateOperation(_accountId, DateTime.Now.AddMonths(-2),
                OperationDto.OperationType.Income, 2, 100000M);
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
                typeOperation = OperationDto.OperationType.Income, //income
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
                typeOperation = OperationDto.OperationType.Expense, //expense
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
                categoryId = -1,
                typeOperation = OperationDto.OperationType.Transfer, 
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

        [Fact]
        public async Task Delete_RemoveOperationIncomeCurrentMonth_Return_Success()
        {
            //Arrange
            AuthenticationHeaderValue headerJwtKey = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncomeCurrentMonth = await _seedData.CreateOperation(_accountId, DateTime.Now,
                OperationDto.OperationType.Income, 2, 100000.99M);
            int operationId = operationIncomeCurrentMonth.First().Id;
            var requestOperation = new HttpRequestMessage(HttpMethod.Delete, $"/api/Operations/{operationId}");
            requestOperation.Headers.Authorization = headerJwtKey;
            var requestAccount = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountId}");
            requestAccount.Headers.Authorization = headerJwtKey;
            var requestAccountMonthlyBalance = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            requestAccountMonthlyBalance.Headers.Authorization = headerJwtKey;

            //Act
            var responseOperation = await _client.SendAsync(requestOperation);
            var responseAccount = await _client.SendAsync(requestAccount);
            var contentAccoount = await responseAccount.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountDto>(contentAccoount);
            var responseAccountMonthlyBalance = await _client.SendAsync(requestAccountMonthlyBalance);
            var contentAccoountMonthlyBalance = await responseAccountMonthlyBalance.Content.ReadAsStringAsync();
            var accountMonthlyBalance = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(contentAccoountMonthlyBalance);


            //Assert
            Assert.Equal(HttpStatusCode.OK, responseOperation.StatusCode);
            Assert.True(account.Balance == 0);
            Assert.True(accountMonthlyBalance.Count().Equals(1));
            Assert.True(accountMonthlyBalance[0].OpeningMonthBalance == 0 &&
                accountMonthlyBalance[0].ClosingMonthBalance == 0);
        }

        [Fact]
        public async Task Delete_RemoveOperationExpenseCurrentMonth_Return_Success()
        {
            //Arrange
            AuthenticationHeaderValue headerJwtKey = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            int _accountId = await _seedData.CreateAccountUAH();
            var operationExpenseCurrentMonth = await _seedData.CreateOperation(_accountId, DateTime.Now,
                OperationDto.OperationType.Expense, 5, 1000.39M);
            int operationId = operationExpenseCurrentMonth.First().Id;
            var requestOperation = new HttpRequestMessage(HttpMethod.Delete, $"/api/Operations/{operationId}");
            requestOperation.Headers.Authorization = headerJwtKey;
            var requestAccount = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountId}");
            requestAccount.Headers.Authorization = headerJwtKey;
            var requestAccountAfterDeleteOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountId}");
            requestAccountAfterDeleteOperation.Headers.Authorization = headerJwtKey;
            var requestAccountMonthlyBalance = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            requestAccountMonthlyBalance.Headers.Authorization = headerJwtKey;
            var requestAccountMonthlyBalanceAfterDeleteOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            requestAccountMonthlyBalanceAfterDeleteOperation.Headers.Authorization = headerJwtKey;

            //Act
            var responseAccount = await _client.SendAsync(requestAccount);
            Assert.Equal(HttpStatusCode.OK, responseAccount.StatusCode);
            var contentAccoount = await responseAccount.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountDto>(contentAccoount);
            Assert.NotNull(account);
            Assert.True(account.Balance == -1000.39M);
            var responseAccountMonthlyBalance = await _client.SendAsync(requestAccountMonthlyBalance);
            Assert.Equal(HttpStatusCode.OK, responseAccountMonthlyBalance.StatusCode);
            var contentAccoountMonthlyBalance = await responseAccountMonthlyBalance.Content.ReadAsStringAsync();
            var accountMonthlyBalance = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(contentAccoountMonthlyBalance);
            var accountCurrentMonthlyBalance = accountMonthlyBalance.Where(amb => (amb.StartDateOfMonth.Month == DateTime.Now.Month
            && amb.StartDateOfMonth.Year == DateTime.Now.Year));
            Assert.True(accountCurrentMonthlyBalance.Count() == 1);
            Assert.True(accountCurrentMonthlyBalance.First().OpeningMonthBalance == 0);
            Assert.True(accountCurrentMonthlyBalance.First().MonthDebit == 0);
            Assert.True(accountCurrentMonthlyBalance.First().MonthCredit == -1000.39M);
            Assert.True(accountCurrentMonthlyBalance.First().ClosingMonthBalance == -1000.39M);
            Assert.False(accountCurrentMonthlyBalance.First().ClosingMonthBalance == 1000M);

            var responseOperation = await _client.SendAsync(requestOperation);
            var responseAccountAfterDeleteOperation = await _client.SendAsync(requestAccountAfterDeleteOperation);
            var contentResponseAccountAfterDeleteOperation = await responseAccountAfterDeleteOperation.Content.ReadAsStringAsync();
            account = JsonConvert.DeserializeObject<AccountDto>(contentResponseAccountAfterDeleteOperation);
            var responseAccountMonthlyBalanceAfterDeleteOperation = await _client.SendAsync(requestAccountMonthlyBalanceAfterDeleteOperation);
            var contentAccoountMonthlyBalanceAfterDeleteOperation = await responseAccountMonthlyBalanceAfterDeleteOperation.Content.ReadAsStringAsync();           
            var accountMonthlyBalanceAfterDeleteOperation = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(contentAccoountMonthlyBalanceAfterDeleteOperation);
            var accountCurrentMonthlyBalanceAfterDeleteOperation = accountMonthlyBalanceAfterDeleteOperation.Where(amb => (amb.StartDateOfMonth.Month == DateTime.Now.Month
            && amb.StartDateOfMonth.Year == DateTime.Now.Year));

            //Assert
            Assert.Equal(HttpStatusCode.OK, responseOperation.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseAccountMonthlyBalanceAfterDeleteOperation.StatusCode);
            Assert.True(account.Balance == 0);
            Assert.True(accountCurrentMonthlyBalanceAfterDeleteOperation.First().MonthDebit == 0);
            Assert.True(accountCurrentMonthlyBalanceAfterDeleteOperation.First().MonthCredit == 0);
            Assert.True(accountCurrentMonthlyBalanceAfterDeleteOperation.First().ClosingMonthBalance == 0);
            Assert.False(accountCurrentMonthlyBalanceAfterDeleteOperation.First().ClosingMonthBalance == 1000M);
        }    
        
        [Fact]
        public async Task Delete_RemoveOperationIncomeTransferCurrentMonth_Return_Success()
        {
            //Arrange
            AuthenticationHeaderValue headerJwtKey = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            int _accountTargetId = await _seedData.CreateAccountUAH();
            int _accountWithdrawId = await _seedData.CreateAccountUAH();
            await _seedData.CreateOperation(_accountWithdrawId, DateTime.Now, OperationDto.OperationType.Income, 2, 100000.39M);
            var operations = await _seedData.CreateTransferOperation(_accountWithdrawId, _accountTargetId, DateTime.Now, 10000.70M);
            var operationTransferIncome = operations.Where(o => o.TransferOperationId > 0);
            int operationTransferIncomeId = operationTransferIncome.First().Id;
            var operationTransferExpense = operations.Where(o => o.TransferOperationId == 0);
            var requestRemoveTransferOperation = new HttpRequestMessage(HttpMethod.Delete, $"/api/Operations/transfer/{operationTransferIncomeId}");
            requestRemoveTransferOperation.Headers.Authorization = headerJwtKey;
            var requestAccountBeforeRemoveOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountTargetId}");
            requestAccountBeforeRemoveOperation.Headers.Authorization = headerJwtKey;
            var requestAccountAfterRemoveOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountTargetId}");
            requestAccountAfterRemoveOperation.Headers.Authorization = headerJwtKey;

            var requestCheckRemoveTransferIncomeOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Operations/last10/{_accountTargetId}");
            requestCheckRemoveTransferIncomeOperation.Headers.Authorization = headerJwtKey;

            var responseAccountTargetBeforeRemoveOperation = await _client.SendAsync(requestAccountBeforeRemoveOperation);
            Assert.Equal(HttpStatusCode.OK, responseAccountTargetBeforeRemoveOperation.StatusCode);
            var contentAccoountTargetBeforeRemove = await responseAccountTargetBeforeRemoveOperation.Content.ReadAsStringAsync();
            var accountTargetBeforeRemoveOperation = JsonConvert.DeserializeObject<AccountDto>(contentAccoountTargetBeforeRemove);
            Assert.True(accountTargetBeforeRemoveOperation.Balance == 10000.70M);


            //Act
            var responseAccountRemoveOperation = await _client.SendAsync(requestRemoveTransferOperation);
            var responseAccountTargetAfterRemoveOperation = await _client.SendAsync(requestAccountAfterRemoveOperation);
            var contentAccoountTargetAfterRemove = await responseAccountTargetAfterRemoveOperation.Content.ReadAsStringAsync();
            var accountTargetAfterRemoveOperation = JsonConvert.DeserializeObject<AccountDto>(contentAccoountTargetAfterRemove);

            var responseCheckTransferIncomeOperation = await _client.SendAsync(requestCheckRemoveTransferIncomeOperation);
            var contentCheckTransferIncomeOperation = await responseCheckTransferIncomeOperation.Content.ReadAsStringAsync();
            var checkTransferIncomeOperation = JsonConvert.DeserializeObject<OperationDto[]>(contentCheckTransferIncomeOperation);


            //Assert
            Assert.Equal(HttpStatusCode.OK, responseAccountRemoveOperation.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseAccountTargetBeforeRemoveOperation.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseCheckTransferIncomeOperation.StatusCode);
            Assert.True(accountTargetAfterRemoveOperation.Balance == 0);
            Assert.True(checkTransferIncomeOperation.Where(tr => tr.Id == operationTransferIncomeId).Count() == 0);
        }      
        
        [Fact]
        public async Task Delete_RemoveOperationWithdrawTransferCurrentMonth_Return_Success()
        {
            //Arrange
            AuthenticationHeaderValue headerJwtKey = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            int _accountTargetId = await _seedData.CreateAccountUAH();
            int _accountWithdrawId = await _seedData.CreateAccountUAH();
            await _seedData.CreateOperation(_accountWithdrawId, DateTime.Now, OperationDto.OperationType.Income, 2, 100000.39M);
            var operations = await _seedData.CreateTransferOperation(_accountWithdrawId, _accountTargetId, DateTime.Now, 10000.70M);
            var operationTransferIncome = operations.Where(o => o.TransferOperationId > 0);
            int operationTransferIncomeId = operationTransferIncome.First().Id;
            var operationTransferExpense = operations.Where(o => o.TransferOperationId == 0);
            int operationTransferExpenseId = operationTransferExpense.First().Id;
            var requestRemoveTransferOperation = new HttpRequestMessage(HttpMethod.Delete, $"/api/Operations/transfer/{operationTransferExpenseId}");
            requestRemoveTransferOperation.Headers.Authorization = headerJwtKey;
            var requestAccountBeforeRemoveOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountWithdrawId}");
            requestAccountBeforeRemoveOperation.Headers.Authorization = headerJwtKey;
            var requestAccountAfterRemoveOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountWithdrawId}");
            requestAccountAfterRemoveOperation.Headers.Authorization = headerJwtKey;

            var requestCheckRemoveTransferExpenseOperation = new HttpRequestMessage(HttpMethod.Get, $"/api/Operations/last10/{_accountWithdrawId}");
            requestCheckRemoveTransferExpenseOperation.Headers.Authorization = headerJwtKey;

            var responseAccountWithdrawBeforeRemoveOperation = await _client.SendAsync(requestAccountBeforeRemoveOperation);
            Assert.Equal(HttpStatusCode.OK, responseAccountWithdrawBeforeRemoveOperation.StatusCode);
            var contentAccoountWithdrawBeforeRemove = await responseAccountWithdrawBeforeRemoveOperation.Content.ReadAsStringAsync();
            var accountWithdrawBeforeRemoveOperation = JsonConvert.DeserializeObject<AccountDto>(contentAccoountWithdrawBeforeRemove);
            Assert.True(accountWithdrawBeforeRemoveOperation.Balance == 89999.69M);


            //Act
            var responseAccountRemoveOperation = await _client.SendAsync(requestRemoveTransferOperation);
            var responseAccountWithdrawAfterRemoveOperation = await _client.SendAsync(requestAccountAfterRemoveOperation);
            var contentAccoountWithdrawAfterRemove = await responseAccountWithdrawAfterRemoveOperation.Content.ReadAsStringAsync();
            var accountWithdrawAfterRemoveOperation = JsonConvert.DeserializeObject<AccountDto>(contentAccoountWithdrawAfterRemove);

            var responseCheckTransferExpenseOperation = await _client.SendAsync(requestCheckRemoveTransferExpenseOperation);
            var contentCheckTransferExpenseOperation = await responseCheckTransferExpenseOperation.Content.ReadAsStringAsync();
            var checkTransferExpenseOperation = JsonConvert.DeserializeObject<OperationDto[]>(contentCheckTransferExpenseOperation);


            //Assert
            Assert.Equal(HttpStatusCode.OK, responseAccountRemoveOperation.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseAccountWithdrawBeforeRemoveOperation.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseCheckTransferExpenseOperation.StatusCode);
            Assert.True(accountWithdrawAfterRemoveOperation.Balance == 100000.39M);
            Assert.True(checkTransferExpenseOperation.Where(tr => tr.Id == operationTransferExpenseId).Count() == 0);
        }

        [Fact]
        public async Task Put_UpdateOperation_Return_Success()
        {
            //Arrange

            //Act


            //Assert
            Assert.True(false);
        }
    }
}
