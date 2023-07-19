using Azure.Core;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class AccountMonthlyBalanceControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly SeedDataIntegrationTests _seedData;

        public AccountMonthlyBalanceControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _seedData = new SeedDataIntegrationTests(factory);
        }

        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_CurrentMonth_ByAccountId_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncome = await _seedData.CreateOperationIncome(_accountId, DateTime.Now, 100000.23M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);
            //var openAccounts = accounts.Where(a => a.AccountStatus.Equals(1));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances.Length == 1);
            Assert.NotNull(accountMonthlyBalances);
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == 0
                && accountMonthlyBalances[0].ClosingMonthBalance == operationIncome.First().OperationAmount);
            Assert.True(accountMonthlyBalances[0].MonthCredit == 0
                && accountMonthlyBalances[0].MonthDebit == operationIncome.First().OperationAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncome.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncome.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_Last2Month_ByAccountId_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncomeCurrentMonth = await _seedData.CreateOperationIncome(_accountId, DateTime.Now, 100000.23M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationIncome(_accountId, DateTime.Now.AddMonths(-1), 100000.21M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances.Length == 2);
            Assert.NotNull(accountMonthlyBalances);
            decimal closingMonthBalance = operationIncomeCurrentMonth.First().OperationAmount 
                + operationIncomePreviousMonth.First().OperationAmount;
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == operationIncomePreviousMonth.First().OperationAmount);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == closingMonthBalance);
            Assert.True(accountMonthlyBalances[0].MonthCredit == 0
                && accountMonthlyBalances[0].MonthDebit == operationIncomeCurrentMonth.First().OperationAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncomeCurrentMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[1].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[1].ClosingMonthBalance == operationIncomePreviousMonth.First().OperationAmount);
            Assert.True(accountMonthlyBalances[1].MonthCredit == 0
                && accountMonthlyBalances[1].MonthDebit == operationIncomePreviousMonth.First().OperationAmount);
            Assert.True(accountMonthlyBalances[1].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month-1
                && accountMonthlyBalances[1].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_Last3Month_ByAccountId_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncomeCurrentMonth = await _seedData.CreateOperationIncome(_accountId, DateTime.Now, 100000.23M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationIncome(_accountId, DateTime.Now.AddMonths(-1), 100000.21M);
            var operationIncome3MonthAgo = await _seedData.CreateOperationIncome(_accountId, DateTime.Now.AddMonths(-2), 100000.11M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances.Length == 3);
            Assert.NotNull(accountMonthlyBalances);
            decimal closingMonthBalance = operationIncomeCurrentMonth.First().OperationAmount
                + operationIncomePreviousMonth.First().OperationAmount
                + operationIncome3MonthAgo.First().OperationAmount;
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == operationIncomePreviousMonth.First().OperationAmount
                + operationIncome3MonthAgo.First().OperationAmount);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == closingMonthBalance);
            Assert.True(accountMonthlyBalances[0].MonthCredit == 0
                && accountMonthlyBalances[0].MonthDebit == operationIncomeCurrentMonth.First().OperationAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncomeCurrentMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[1].OpeningMonthBalance == operationIncome3MonthAgo.First().OperationAmount);
            Assert.True(accountMonthlyBalances[1].ClosingMonthBalance == (closingMonthBalance - operationIncomeCurrentMonth.First().OperationAmount));
            Assert.True(accountMonthlyBalances[1].MonthCredit == 0
                && accountMonthlyBalances[1].MonthDebit == operationIncomePreviousMonth.First().OperationAmount);
            Assert.True(accountMonthlyBalances[1].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month - 1
                && accountMonthlyBalances[1].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[2].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[2].ClosingMonthBalance == operationIncome3MonthAgo.First().OperationAmount);
            Assert.True(accountMonthlyBalances[2].MonthCredit == 0
                && accountMonthlyBalances[2].MonthDebit == operationIncome3MonthAgo.First().OperationAmount);
            Assert.True(accountMonthlyBalances[2].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month - 2
                && accountMonthlyBalances[2].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_FutureMonth_ByAccountId_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncome = await _seedData.CreateOperationIncome(_accountId, DateTime.Now, 100000.23M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationIncome(_accountId, DateTime.Now.AddMonths(-1), 100000.21M);
            var operationIncome3MonthAgo = await _seedData.CreateOperationIncome(_accountId, DateTime.Now.AddMonths(-2), 100000.11M);
            var operationIncome2MonthAfterCurrent = await _seedData.CreateOperationIncome(_accountId, DateTime.Now.AddMonths(2), 100000.36M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);
            //var openAccounts = accounts.Where(a => a.AccountStatus.Equals(1));

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances.Length == 4);
            Assert.NotNull(accountMonthlyBalances);
            decimal openningBalance = operationIncome.First().OperationAmount
                + operationIncome3MonthAgo.First().OperationAmount
                + operationIncomePreviousMonth.First().OperationAmount; 
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == openningBalance
                && accountMonthlyBalances[0].ClosingMonthBalance == openningBalance + operationIncome2MonthAfterCurrent.First().OperationAmount);
            Assert.True(accountMonthlyBalances[0].MonthCredit == 0
                && accountMonthlyBalances[0].MonthDebit == operationIncome2MonthAfterCurrent.First().OperationAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncome2MonthAfterCurrent.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncome2MonthAfterCurrent.First().OperationDate.Year);
        }
    }
}
