using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YFS.Controllers;
using YFS.Core.Dtos;
using static YFS.Controllers.OperationsController;

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
            _seedData = SeedDataIntegrationTests.Instance;
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_CurrentMonth_ByAccountId_Income_Expense_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncome = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Income, 2, 50000.23M);
            var operationExpense = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Expense, 5, 1000.13M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());
            var requestAccount = new HttpRequestMessage(HttpMethod.Get, $"/api/Accounts/byId/{_accountId}");
            requestAccount.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var responseAccount = await _client.SendAsync(requestAccount);
            var content = await response.Content.ReadAsStringAsync();
            var contentAccount = await responseAccount.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);
            var accountBalance = JsonConvert.DeserializeObject<AccountDto>(contentAccount);


            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseAccount.StatusCode);
            Assert.NotNull(accountMonthlyBalances);
            Assert.True(accountMonthlyBalances.Length == 1);
            Assert.NotNull(accountBalance);
            decimal balance = accountBalance.Balance;
            decimal balanceCalculateOperation = operationIncome.First().TotalCurrencyAmount + operationExpense.First().TotalCurrencyAmount;
            Assert.True(balance == balanceCalculateOperation);
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == operationIncome.First().TotalCurrencyAmount 
                + operationExpense.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].MonthCredit == operationExpense.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].MonthDebit == operationIncome.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncome.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncome.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_CurrentMonth_ByAccountId_Income_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncome = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow, 
                OperationDto.OperationType.Income, 2, 100000.23M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances.Length == 1);
            Assert.NotNull(accountMonthlyBalances);
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == 0
                && accountMonthlyBalances[0].ClosingMonthBalance == operationIncome.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].MonthCredit == 0
                && accountMonthlyBalances[0].MonthDebit == operationIncome.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncome.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncome.First().OperationDate.Year);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == operationIncome.First().TotalCurrencyAmount);    
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_LastMonth_ByAccountId_Income_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncomeCurrentMonth = await _seedData.CreateOperationUAH(_accountId,DateTime.UtcNow,
                OperationDto.OperationType.Income, 2, 100000.23M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Income, 2, 100000.21M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances?.Length == 2);
            Assert.NotNull(accountMonthlyBalances);
            decimal closingMonthBalance = operationIncomeCurrentMonth.First().TotalCurrencyAmount
                + operationIncomePreviousMonth.First().TotalCurrencyAmount;
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == operationIncomePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == closingMonthBalance);
            Assert.True(accountMonthlyBalances[0].MonthCredit == 0
                && accountMonthlyBalances[0].MonthDebit == operationIncomeCurrentMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncomeCurrentMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[1].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[1].ClosingMonthBalance == operationIncomePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].MonthCredit == 0
                && accountMonthlyBalances[1].MonthDebit == operationIncomePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month-1
                && accountMonthlyBalances[1].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_LastMonth_ByAccountId_Income_Expense_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncomeCurrentMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Income, 2, 100000.23M);
            var operationExpenseCurrentMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Expense, 5, 1000.03M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Income, 2, 100000.21M);
            var operationExpensePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Expense, 5, 100.20M);
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
            decimal closingMonthBalance = operationIncomeCurrentMonth.First().TotalCurrencyAmount
                + operationIncomePreviousMonth.First().TotalCurrencyAmount
                + operationExpenseCurrentMonth.First().TotalCurrencyAmount
                + operationExpensePreviousMonth.First().TotalCurrencyAmount;
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == operationIncomePreviousMonth.First().TotalCurrencyAmount
                + operationExpensePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == closingMonthBalance);
            Assert.True(accountMonthlyBalances[0].MonthCredit == operationExpenseCurrentMonth.First().TotalCurrencyAmount
                && accountMonthlyBalances[0].MonthDebit == operationIncomeCurrentMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncomeCurrentMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[1].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[1].ClosingMonthBalance == operationIncomePreviousMonth.First().TotalCurrencyAmount 
                + operationExpensePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].MonthCredit == operationExpensePreviousMonth.First().TotalCurrencyAmount
                && accountMonthlyBalances[1].MonthDebit == operationIncomePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month - 1
                && accountMonthlyBalances[1].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_Last2Month_ByAccountId_Income_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncomeCurrentMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow, 
                OperationDto.OperationType.Income, 2, 100000.23M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Income, 2, 100000.21M);
            var operationIncome2MonthAgo = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-2), 
                OperationDto.OperationType.Income, 2, 100000.11M);
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
            decimal closingMonthBalance = operationIncomeCurrentMonth.First().TotalCurrencyAmount
                + operationIncomePreviousMonth.First().TotalCurrencyAmount
                + operationIncome2MonthAgo.First().TotalCurrencyAmount;
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == operationIncomePreviousMonth.First().TotalCurrencyAmount
                + operationIncome2MonthAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == closingMonthBalance);
            Assert.True(accountMonthlyBalances[0].MonthCredit == 0
                && accountMonthlyBalances[0].MonthDebit == operationIncomeCurrentMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncomeCurrentMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[1].OpeningMonthBalance == operationIncome2MonthAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].ClosingMonthBalance == (closingMonthBalance - operationIncomeCurrentMonth.First().TotalCurrencyAmount));
            Assert.True(accountMonthlyBalances[1].MonthCredit == 0
                && accountMonthlyBalances[1].MonthDebit == operationIncomePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month - 1
                && accountMonthlyBalances[1].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[2].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[2].ClosingMonthBalance == operationIncome2MonthAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[2].MonthCredit == 0
                && accountMonthlyBalances[2].MonthDebit == operationIncome2MonthAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[2].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month - 2
                && accountMonthlyBalances[2].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_Last2Month_ByAccountId_Income_Expense_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncomeCurrentMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Income, 2, 100000.23M);
            var operationExpenseCurrentMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Expense, 5, 100.13M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Income, 2, 100000.21M);
            var operationExpensePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Expense, 5, 1000.21M);
            var operationIncome2MonthAgo = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-2),
                OperationDto.OperationType.Income, 2, 100000.11M);
            var operationExpense2MonthAgo = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-2),
                OperationDto.OperationType.Expense, 5, 500.11M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances?.Length == 3);
            Assert.NotNull(accountMonthlyBalances);
            decimal closingMonthBalance = 
                operationIncomeCurrentMonth.First().TotalCurrencyAmount
                + operationExpenseCurrentMonth.First().TotalCurrencyAmount
                + operationIncomePreviousMonth.First().TotalCurrencyAmount
                + operationExpensePreviousMonth.First().TotalCurrencyAmount
                + operationIncome2MonthAgo.First().TotalCurrencyAmount
                + operationExpense2MonthAgo.First().TotalCurrencyAmount;
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == 
                operationIncomePreviousMonth.First().TotalCurrencyAmount
                + operationExpensePreviousMonth.First().TotalCurrencyAmount
                + operationIncome2MonthAgo.First().TotalCurrencyAmount
                + operationExpense2MonthAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == closingMonthBalance);
            Assert.True(accountMonthlyBalances[0].MonthCredit == operationExpenseCurrentMonth.First().TotalCurrencyAmount
                && accountMonthlyBalances[0].MonthDebit == operationIncomeCurrentMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationIncomeCurrentMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[1].OpeningMonthBalance == operationIncome2MonthAgo.First().TotalCurrencyAmount
                + operationExpense2MonthAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].ClosingMonthBalance == (closingMonthBalance - (operationIncomeCurrentMonth.First().TotalCurrencyAmount 
                + operationExpenseCurrentMonth.First().TotalCurrencyAmount)));
            Assert.True(accountMonthlyBalances[1].MonthCredit == operationExpensePreviousMonth.First().TotalCurrencyAmount
                && accountMonthlyBalances[1].MonthDebit == operationIncomePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month - 1
                && accountMonthlyBalances[1].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);

            Assert.True(accountMonthlyBalances[2].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[2].ClosingMonthBalance == operationIncome2MonthAgo.First().TotalCurrencyAmount + operationExpense2MonthAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[2].MonthCredit == operationExpense2MonthAgo.First().TotalCurrencyAmount
                && accountMonthlyBalances[2].MonthDebit == operationIncome2MonthAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[2].StartDateOfMonth.Month == operationIncomeCurrentMonth.First().OperationDate.Month - 2
                && accountMonthlyBalances[2].StartDateOfMonth.Year == operationIncomePreviousMonth.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_FutureMonth_ByAccountId_Income_Success()
        {
            //Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            int currentMonth = DateTime.UtcNow.Month;
            int currentYear = DateTime.UtcNow.Year;
            var operationIncomeCurrent = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Income, 2, 100000.23M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Income, 2, 100000.21M);
            var operationIncome2MonthAgo = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-2),
                OperationDto.OperationType.Income, 2, 100000.11M);
            var operationIncome2MonthAfterCurrent = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(2),
                OperationDto.OperationType.Income, 2, 100000.36M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances?.Length == 4);
            Assert.NotNull(accountMonthlyBalances);
            AccountMonthlyBalanceDto ambCurrent = accountMonthlyBalances.Where(a => a.MonthNumber == currentMonth && a.YearNumber == currentYear).FirstOrDefault();
            Assert.NotNull(ambCurrent);
            decimal openningBalance = //operationIncome.First().TotalCurrencyAmount
                + operationIncome2MonthAgo.First().TotalCurrencyAmount
                + operationIncomePreviousMonth.First().TotalCurrencyAmount;
            Assert.True(ambCurrent.OpeningMonthBalance == openningBalance);
            Assert.True(ambCurrent.ClosingMonthBalance == openningBalance 
                + operationIncomeCurrent.First().TotalCurrencyAmount);
            Assert.True(ambCurrent.MonthCredit == 0
                && ambCurrent.MonthDebit == operationIncomeCurrent.First().TotalCurrencyAmount);
            Assert.True(ambCurrent.StartDateOfMonth.Month == operationIncomeCurrent.First().OperationDate.Month
                && ambCurrent.StartDateOfMonth.Year == operationIncomeCurrent.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_FutureMonth_ByAccountId_Income_Expense_Success()
        {
            //Arrange
            int currentMonth = DateTime.UtcNow.Month;
            int currentYear = DateTime.UtcNow.Year;
            int _accountId = await _seedData.CreateAccountUAH();
            var operationIncomeCurrent = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Income, 2, 100000.23M);
            var operationExpenseCurrent = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Expense, 5, 100.53M);
            var operationIncomePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Income, 2, 100000.21M);
            var operationExpensePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Expense, 5, 1000.21M);
            var operationIncome2MonthAgo = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-2),
                OperationDto.OperationType.Income, 2, 100000.11M);
            var operationExpense2MonthAgo = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-2),
                OperationDto.OperationType.Expense, 5, 1050.11M);
            var operationIncome2MonthAfterCurrent = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(2),
                OperationDto.OperationType.Income, 2, 100000.36M);
            var operationExpense2MonthAfterCurrent = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(2),
                OperationDto.OperationType.Expense, 5, 1659.36M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            //Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(accountMonthlyBalances);
            Assert.True(accountMonthlyBalances.Length == 4);            
            AccountMonthlyBalanceDto ambCurrent = accountMonthlyBalances.Where(a => a.MonthNumber == currentMonth && a.YearNumber == currentYear).FirstOrDefault();
            Assert.NotNull(ambCurrent);
            decimal openningBalanceCurrent =
                + operationIncome2MonthAgo.First().TotalCurrencyAmount
                + operationExpense2MonthAgo.First().TotalCurrencyAmount
                + operationIncomePreviousMonth.First().TotalCurrencyAmount
                + operationExpensePreviousMonth.First().TotalCurrencyAmount;
            Assert.True(ambCurrent.OpeningMonthBalance == openningBalanceCurrent);
            Assert.True(ambCurrent.ClosingMonthBalance == openningBalanceCurrent
                + operationIncomeCurrent.First().TotalCurrencyAmount
                + operationExpenseCurrent.First().TotalCurrencyAmount);
            Assert.True(ambCurrent.MonthCredit == operationExpenseCurrent.First().TotalCurrencyAmount
                && ambCurrent.MonthDebit == operationIncomeCurrent.First().TotalCurrencyAmount);
            Assert.True(ambCurrent.StartDateOfMonth.Month == operationIncomeCurrent.First().OperationDate.Month
                && ambCurrent.StartDateOfMonth.Year == operationIncomeCurrent.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_CurrentMonth_ByAccountId_Expense_Success()
        {
            // Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationExpense = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Expense, 5, 1000.46M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances.Length == 1);
            Assert.NotNull(accountMonthlyBalances);
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == 0
                && accountMonthlyBalances[0].ClosingMonthBalance == operationExpense.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].MonthDebit == 0
                && accountMonthlyBalances[0].MonthCredit == operationExpense.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationExpense.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationExpense.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_Last2Month_ByAccountId_Expense_Success()
        {
            // Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationExpenseCurrentMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Expense, 5, 100.63M);
            var operationExpensePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Expense, 5, 1000.21M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances.Length == 2);
            Assert.NotNull(accountMonthlyBalances);

            decimal closingMonthBalanceCurrentMonth = operationExpenseCurrentMonth.First().TotalCurrencyAmount;
            decimal closingMonthBalancePreviousMonth = operationExpensePreviousMonth.First().TotalCurrencyAmount;

            // Current month balance assertions
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == closingMonthBalancePreviousMonth);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == closingMonthBalanceCurrentMonth + closingMonthBalancePreviousMonth);
            Assert.True(accountMonthlyBalances[0].MonthDebit == 0
                && accountMonthlyBalances[0].MonthCredit == operationExpenseCurrentMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationExpenseCurrentMonth.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationExpenseCurrentMonth.First().OperationDate.Year);

            // Previous month balance assertions
            Assert.True(accountMonthlyBalances[1].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[1].ClosingMonthBalance == closingMonthBalancePreviousMonth);
            Assert.True(accountMonthlyBalances[1].MonthDebit == 0
                && accountMonthlyBalances[1].MonthCredit == operationExpensePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].StartDateOfMonth.Month == operationExpensePreviousMonth.First().OperationDate.Month
                && accountMonthlyBalances[1].StartDateOfMonth.Year == operationExpensePreviousMonth.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_Last3Month_ByAccountId_Expense_Success()
        {
            // Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            var operationExpenseCurrentMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Expense, 5, 100.23M);
            var operationExpensePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Expense, 5, 1000.21M);
            var operationExpense3MonthsAgo = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-2),
                OperationDto.OperationType.Expense, 5, 500.11M);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(accountMonthlyBalances.Length == 3);
            Assert.NotNull(accountMonthlyBalances);

            decimal closingMonthBalanceCurrentMonth = operationExpenseCurrentMonth.First().TotalCurrencyAmount;
            decimal closingMonthBalancePreviousMonth = operationExpensePreviousMonth.First().TotalCurrencyAmount;
            decimal closingMonthBalance3MonthsAgo = operationExpense3MonthsAgo.First().TotalCurrencyAmount;

            decimal closingMonthBalance = closingMonthBalanceCurrentMonth + closingMonthBalancePreviousMonth + closingMonthBalance3MonthsAgo;

            // Current month balance assertions
            Assert.True(accountMonthlyBalances[0].OpeningMonthBalance == closingMonthBalancePreviousMonth + closingMonthBalance3MonthsAgo);
            Assert.True(accountMonthlyBalances[0].ClosingMonthBalance == closingMonthBalance);
            Assert.True(accountMonthlyBalances[0].MonthDebit == 0
                && accountMonthlyBalances[0].MonthCredit == operationExpenseCurrentMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[0].StartDateOfMonth.Month == operationExpenseCurrentMonth.First().OperationDate.Month
                && accountMonthlyBalances[0].StartDateOfMonth.Year == operationExpenseCurrentMonth.First().OperationDate.Year);

            // Previous month balance assertions
            Assert.True(accountMonthlyBalances[1].OpeningMonthBalance == closingMonthBalance3MonthsAgo);
            Assert.True(accountMonthlyBalances[1].ClosingMonthBalance == (closingMonthBalance - closingMonthBalanceCurrentMonth));
            Assert.True(accountMonthlyBalances[1].MonthDebit == 0
                && accountMonthlyBalances[1].MonthCredit == operationExpensePreviousMonth.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[1].StartDateOfMonth.Month == operationExpensePreviousMonth.First().OperationDate.Month
                && accountMonthlyBalances[1].StartDateOfMonth.Year == operationExpensePreviousMonth.First().OperationDate.Year);

            // Three months ago balance assertions
            Assert.True(accountMonthlyBalances[2].OpeningMonthBalance == 0);
            Assert.True(accountMonthlyBalances[2].ClosingMonthBalance == closingMonthBalance3MonthsAgo);
            Assert.True(accountMonthlyBalances[2].MonthDebit == 0
                && accountMonthlyBalances[2].MonthCredit == operationExpense3MonthsAgo.First().TotalCurrencyAmount);
            Assert.True(accountMonthlyBalances[2].StartDateOfMonth.Month == operationExpense3MonthsAgo.First().OperationDate.Month
                && accountMonthlyBalances[2].StartDateOfMonth.Year == operationExpense3MonthsAgo.First().OperationDate.Year);
        }
        [Fact]
        public async Task Get_Returns_AccountMonthlyBalance_FutureMonth_ByAccountId_Expense_Success()
        {
            // Arrange
            int _accountId = await _seedData.CreateAccountUAH();
            int currentMonth = DateTime.UtcNow.Month;
            int currentYear = DateTime.UtcNow.Year;

            var operationExpenseCurrentMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow,
                OperationDto.OperationType.Expense, 5, 100.23M);
            var operationExpensePreviousMonth = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-1),
                OperationDto.OperationType.Expense, 5, 1000.21M);
            var operationExpense2MonthsAgo = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(-2),
                OperationDto.OperationType.Expense, 5, 500.11M);
            var operationExpense2MonthsAfterCurrent = await _seedData.CreateOperationUAH(_accountId, DateTime.UtcNow.AddMonths(2),
                OperationDto.OperationType.Expense, 5, 600.36M);

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/AccountMonthlyBalance/{_accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            // Act
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Ensure HTTP 200 OK

            var content = await response.Content.ReadAsStringAsync();
            var accountMonthlyBalances = JsonConvert.DeserializeObject<AccountMonthlyBalanceDto[]>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(accountMonthlyBalances);
            Assert.True(accountMonthlyBalances.Length == 4);

            // Assert for each month
            // Month 0 (Current Month)
            decimal expectedOpeningBalance = operationExpensePreviousMonth.First().TotalCurrencyAmount
                    + operationExpense2MonthsAgo.First().TotalCurrencyAmount;
            AccountMonthlyBalanceDto ambCurrent = accountMonthlyBalances.Where(a => a.MonthNumber == currentMonth && a.YearNumber == currentYear).FirstOrDefault();
            Assert.NotNull(ambCurrent);
            Assert.Equal(expectedOpeningBalance, ambCurrent.OpeningMonthBalance);
            Assert.Equal(expectedOpeningBalance + ambCurrent.MonthCredit, ambCurrent.ClosingMonthBalance);
            Assert.Equal(0, ambCurrent.MonthDebit);
            Assert.Equal(ambCurrent.MonthNumber, currentMonth);
            Assert.Equal(ambCurrent.YearNumber, currentYear);

            // Month -1 (Previous Month)
            int previousMounth = DateTime.UtcNow.AddMonths(-1).Month;
            int previousYear = DateTime.UtcNow.AddMonths(-1).Year;
            decimal expectedPreviousOpeningBalance = operationExpense2MonthsAgo.First().TotalCurrencyAmount;
            AccountMonthlyBalanceDto ambPrevious = accountMonthlyBalances.Where(a => a.MonthNumber == previousMounth 
                && a.YearNumber == previousYear).FirstOrDefault();
            Assert.NotNull(ambPrevious);
            Assert.Equal(expectedPreviousOpeningBalance, ambPrevious.OpeningMonthBalance);
            Assert.Equal(ambPrevious.MonthCredit + operationExpense2MonthsAgo.First().TotalCurrencyAmount, ambPrevious.ClosingMonthBalance);
            Assert.Equal(0, ambPrevious.MonthDebit);
            Assert.Equal(previousMounth, ambPrevious.StartDateOfMonth.Month);
            Assert.Equal(previousYear, ambPrevious.StartDateOfMonth.Year);

            // Month -2 (Two Months Ago)
            int twoMounthAgo = DateTime.UtcNow.AddMonths(-2).Month;
            int twoYearAgo = DateTime.UtcNow.AddMonths(-2).Year;
            decimal expectedTwoMonthAgoOpeningBalance = 0;
            AccountMonthlyBalanceDto ambTwoMonthAgo = accountMonthlyBalances.Where(a => a.MonthNumber == twoMounthAgo
                    && a.YearNumber == twoYearAgo).FirstOrDefault();
            Assert.NotNull(ambTwoMonthAgo);
            Assert.Equal(expectedTwoMonthAgoOpeningBalance, ambTwoMonthAgo.OpeningMonthBalance);
            Assert.Equal(expectedTwoMonthAgoOpeningBalance + operationExpense2MonthsAgo.First().TotalCurrencyAmount, ambTwoMonthAgo.ClosingMonthBalance);
            Assert.Equal(operationExpense2MonthsAgo.First().TotalCurrencyAmount, ambTwoMonthAgo.MonthCredit);
            Assert.Equal(0, ambTwoMonthAgo.MonthDebit);
            Assert.Equal(operationExpense2MonthsAgo.First().OperationDate.Month, twoMounthAgo);
            Assert.Equal(operationExpense2MonthsAgo.First().OperationDate.Year, twoYearAgo);

            // Month +2 (Two Months After)
            int twoMounthAfter = DateTime.UtcNow.AddMonths(+2).Month;
            int twoYearAfter = DateTime.UtcNow.AddMonths(+2).Year;
            decimal expectedTwoMonthAfterOpeningBalance = 
                operationExpenseCurrentMonth.First().TotalCurrencyAmount +
                operationExpensePreviousMonth.First().TotalCurrencyAmount +
                operationExpense2MonthsAgo.First().TotalCurrencyAmount;
            AccountMonthlyBalanceDto ambTwoMonthAfter = accountMonthlyBalances.Where(a => a.MonthNumber == twoMounthAfter
                    && a.YearNumber == twoYearAfter).FirstOrDefault();
            Assert.NotNull(ambTwoMonthAfter);
            Assert.Equal(expectedTwoMonthAfterOpeningBalance, ambTwoMonthAfter.OpeningMonthBalance);
            Assert.Equal(operationExpense2MonthsAfterCurrent.First().TotalCurrencyAmount 
                + operationExpenseCurrentMonth.First().TotalCurrencyAmount 
                + operationExpense2MonthsAgo.First().TotalCurrencyAmount
                + operationExpensePreviousMonth.First().TotalCurrencyAmount,
                ambTwoMonthAfter.ClosingMonthBalance);
            Assert.Equal(0, ambTwoMonthAfter.MonthDebit);
            Assert.Equal(operationExpense2MonthsAfterCurrent.First().OperationDate.Month, twoMounthAfter);
            Assert.Equal(operationExpense2MonthsAfterCurrent.First().OperationDate.Year, twoYearAfter);
        }

    }
}
