using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;
using YFS.Repo.Data;
using YFS.Service.Interfaces;
using YFS.Service.Services;
using YFS.Data.Controllers;
using Microsoft.AspNetCore.Mvc;
using Controllers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Xunit.Sdk;
using System.Text.Json;
using FluentAssertions;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class MonoIntegrationApiServiceIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly SeedDataIntegrationTests _seedDataIntegrationTests;

        public MonoIntegrationApiServiceIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _serviceProvider = _factory.Services;
            _seedDataIntegrationTests = SeedDataIntegrationTests.Instance;
            _client = factory.CreateClient();

            using (var scope = _serviceProvider.CreateScope())
            {
                _serviceProvider = scope.ServiceProvider;
                _seedDataIntegrationTests.InitializeDatabaseAsync(_serviceProvider).Wait();
            }

        }
        [Fact]
        public async Task SyncAllAccountsFromMono_ShouldReturnOk_Return4Accounts()
        {
            //Arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);

            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);
            ApiTokenDto savedToken;

            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();
                var result = await tokenService.CreateToken(apiTokenMono);
                savedToken = result.Data;
                Assert.NotNull(result.Data);
                var getResultToken = await tokenService.GetTokenByNameForUser(apiTokenMono.Name, user.Id);

                // Act
                Assert.True(getResultToken.IsSuccess);
                Assert.NotNull(getResultToken.Data);

                var monoIntegrationApiService = serviceProvider.GetRequiredService<IMonoIntegrationApiService>();
                MonoClientInfoResponse monoClientResponse = _seedDataIntegrationTests.MonoClientInfoResponse;
                Assert.NotNull(monoClientResponse);
                if (monoClientResponse == null)
                {
                    throw new Exception("monClientResponse is empty! Check SeedData json files");
                }
                var syncAccountsResult = monoIntegrationApiService.SyncAccounts(getResultToken.Data.TokenValue, user.Id, monoClientResponse);
                if (syncAccountsResult == null)
                {
                    throw new Exception("moon account is not found! Check SeedData json files");
                }

                Assert.NotNull(syncAccountsResult);
                Assert.True(syncAccountsResult?.Result.IsSuccess);

                //check account data
                Assert.Equal(syncAccountsResult.Result.Data.Count(), 4);
            }


            //GetActiveRulesByApiTokenIdAsync should 1 rule

            //AddRuleAsync(MonoSyncRule newRule)

            //<IEnumerable<MonoSyncRule>>> AddRulesAsync
        }
        [Fact]
        public async Task Post_Create_Rules_ShouldReturnOk_ReturnRule()
        {
            //Arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            ApiTokenDto savedToken = null;
            Assert.NotNull(user);
            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);
            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();
                var result = await tokenService.CreateToken(apiTokenMono);
                Assert.NotNull(result.Data);
                savedToken = result.Data;          
            }

            var newRule = new MonoSyncRule
            {
                RuleName = "Test Rule",
                Description = "This is a test rule",
                Condition = "{\"Mcc\": 4378}",
                Action = "{\"CategoryId\": 5}",
                Priority = 1,
                IsActive = true,
                ApiTokenId = savedToken.Id
            };

            var requestRule = new HttpRequestMessage(HttpMethod.Post, "/api/MonobankIntegrationApi/rules")
            {
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newRule), Encoding.UTF8, "application/json")
            };
            requestRule.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _seedDataIntegrationTests.GetJwtTokenForUser(user.UserName));

            // Act
            var responseRule = await _client.SendAsync(requestRule);


            // Assert
            responseRule.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await responseRule.Content.ReadAsStringAsync();

            // Ensure responseContent is not null or empty before deserialization
            responseContent.Should().NotBeNullOrEmpty();

            var createdRule = System.Text.Json.JsonSerializer.Deserialize<MonoSyncRule>(responseContent, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            createdRule.Should().NotBeNull();
            createdRule.RuleName.Should().Be(newRule.RuleName);
            createdRule.Description.Should().Be(newRule.Description);
            createdRule.Condition.Should().Be(newRule.Condition);
            createdRule.Action.Should().Be(newRule.Action);
            createdRule.Priority.Should().Be(newRule.Priority);
            createdRule.IsActive.Should().Be(newRule.IsActive);
            createdRule.ApiTokenId.Should().Be(newRule.ApiTokenId);
        }

        //GetActiveRulesByApiTokenIdAsync should 1 rule

        //<IEnumerable<MonoSyncRule>>> AddRulesAsync

        [Fact]
        public async Task SyncTransactionFromStatementsMonoBlackUAH_ShouldReturnSumCategoriesCalculatedOk() {
           //arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);
            List<MonoTransaction> monoTransactionsUAH = _seedDataIntegrationTests.MonoStatementBlackUAH;
            MonoClientInfoResponse monoClientResponse = _seedDataIntegrationTests.MonoClientInfoResponse;
            Assert.NotNull(monoClientResponse);
            if (monoClientResponse == null)
            {
                throw new Exception("monClientResponse is empty! Check SeedData json files");
            }            

            //act
            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var monoIntegrationApiService = serviceProvider.GetRequiredService<IMonoIntegrationApiService>();
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();
                var resultToken = await tokenService.CreateToken(apiTokenMono);

                var syncAccountsResult = await monoIntegrationApiService.SyncAccounts(resultToken.Data.TokenValue, user.Id, monoClientResponse);
                if (syncAccountsResult.Data == null)
                {
                    throw new Exception("moon account is not found! Check SeedData json files");
                }
                var accountsBlackUAH = syncAccountsResult.Data
                    .Where(account => account.Name.Equals("Mono | black card | [UAH]"))
                        .SingleOrDefault();


            var result = await monoIntegrationApiService.SyncTransactionFromStatements(resultToken.Data.TokenValue,user.Id, accountsBlackUAH.ExternalId, monoTransactionsUAH);
            
             //Assert
             Assert.NotNull(result);

                //mobile expense = 6
                var mobileExpense = result.Data
                    .SelectMany(d => d.OperationList)
                    .SelectMany(ol => ol.OperationItems)
                    .Where(oi => oi.CategoryId == 17).ToList();
                Assert.NotNull(mobileExpense);
                Assert.True(mobileExpense.Count == 6);
                decimal sumMobileExpense = mobileExpense.Sum(m => m.OperationAmount);
                Assert.True(sumMobileExpense == -765M);

                //food expense = 
                var foodExpense = result.Data
                    .SelectMany(d => d.OperationList)
                    .SelectMany(ol => ol.OperationItems)
                    .Where(oi => oi.CategoryId == 5).ToList();
                Assert.NotNull(foodExpense);
                Assert.True(foodExpense.Count == 18);
                decimal sumFoodExpense = foodExpense.Sum(m => m.OperationAmount);
                Assert.True(sumFoodExpense == -10198.25M);

                //clothes
                var clothesExpense = result.Data
                    .SelectMany(d => d.OperationList)
                    .SelectMany(ol => ol.OperationItems)
                    .Where(oi => oi.CategoryId == 11).ToList();
                Assert.NotNull(clothesExpense);
                Assert.True(clothesExpense.Count == 2);
                decimal sumclothesExpense = clothesExpense.Sum(m => m.OperationAmount);
                Assert.True(sumclothesExpense == -728M);

            }
        }
        [Fact]
        public async Task SyncTransactionFromStatementsMonoBlackUAH_ShouldReturnRules_SumCategoriesCalculatedOk()
        {
            //arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);
            List<MonoTransaction> monoTransactionsUAH = _seedDataIntegrationTests.MonoStatementBlackUAH;
            MonoClientInfoResponse monoClientResponse = _seedDataIntegrationTests.MonoClientInfoResponse;
            Assert.NotNull(monoClientResponse);
            Assert.NotNull(monoTransactionsUAH);
            if (monoClientResponse == null)
            {
                throw new Exception("monClientResponse is empty! Check SeedData json files");
            }
            var requestRule = new HttpRequestMessage(HttpMethod.Post, $"/api/MonobankIntegrationApi/rules");
            requestRule.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestingWebAppFactory<Program>.GetJwtTokenForDemoUser());

            /*
             {
  "ruleId": 0,
  "ruleName": "string",
  "description": "string",
  "condition": "string",
  "action": "string",
  "priority": 0,
  "isActive": true,
  "createdOn": "2024-06-27T05:27:16.898Z",
  "apiTokenId": 0
}
             */


            //act
            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var monoIntegrationApiService = serviceProvider.GetRequiredService<IMonoIntegrationApiService>();
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();
                var resultToken = await tokenService.CreateToken(apiTokenMono);

                var syncAccountsResult = await monoIntegrationApiService.SyncAccounts(resultToken.Data.TokenValue, user.Id, monoClientResponse);
                if (syncAccountsResult.Data == null)
                {
                    throw new Exception("moon account is not found! Check SeedData json files");
                }
                var accountsBlackUAH = syncAccountsResult.Data
                    .Where(account => account.Name.Equals("Mono | black card | [UAH]"))
                        .SingleOrDefault();


                var result = await monoIntegrationApiService.SyncTransactionFromStatements(resultToken.Data.TokenValue, user.Id, accountsBlackUAH.ExternalId, monoTransactionsUAH);

                //Assert
                Assert.NotNull(result);

                //mobile expense = 6
                var mobileExpense = result.Data
                    .SelectMany(d => d.OperationList)
                    .SelectMany(ol => ol.OperationItems)
                    .Where(oi => oi.CategoryId == 17).ToList();
                Assert.NotNull(mobileExpense);
                Assert.True(mobileExpense.Count == 6);
                decimal sumMobileExpense = mobileExpense.Sum(m => m.OperationAmount);
                Assert.True(sumMobileExpense == -765M);

                //food expense = 
                var foodExpense = result.Data
                    .SelectMany(d => d.OperationList)
                    .SelectMany(ol => ol.OperationItems)
                    .Where(oi => oi.CategoryId == 5).ToList();
                Assert.NotNull(foodExpense);
                Assert.True(foodExpense.Count == 18);
                decimal sumFoodExpense = foodExpense.Sum(m => m.OperationAmount);
                Assert.True(sumFoodExpense == -10198.25M);

                //clothes
                var clothesExpense = result.Data
                    .SelectMany(d => d.OperationList)
                    .SelectMany(ol => ol.OperationItems)
                    .Where(oi => oi.CategoryId == 11).ToList();
                Assert.NotNull(clothesExpense);
                Assert.True(clothesExpense.Count == 2);
                decimal sumclothesExpense = clothesExpense.Sum(m => m.OperationAmount);
                Assert.True(sumclothesExpense == -728M);

            }
        }
    }
}