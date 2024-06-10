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
        public async Task GetClientInfoFromMono_ShouldReturnOk_ShouldReturn4Accounts()
        {
            //Arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);


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


        /*
        [Fact]
        public async Task GetClientInfo_ShouldReturnOk_WhenValidTokenProvided()
        {
            //Arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            var userSignIn = new UserLoginDto { UserName = user.UserName, Password = "demo123$qweR" }; // Provide valid credentials

            var authenticationRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Authentication/sign-in")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userSignIn), Encoding.UTF8, "application/json")
            };
            var authenticationResponse = await _client.SendAsync(authenticationRequest);
            authenticationResponse.EnsureSuccessStatusCode();
            var authenticationContent = await authenticationResponse.Content.ReadAsStringAsync();
            var token = JObject.Parse(authenticationContent)["token"].ToString();


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
            }

            // Include token in request headers
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        // Add any other claims if necessary
                    }, "mock")),
                    Request = { Headers = { ["Authorization"] = $"Bearer {token}" } }
                }
            };
            _controller.ControllerContext = controllerContext;

            // Load expected client info from JSON file
            var expectedClientInfoJson = await File.ReadAllTextAsync("MonoIntegrationTestJson/expectedClientInfo.json");
            var expectedClientInfo = JsonConvert.DeserializeObject<MonoClientInfoResponse>(expectedClientInfoJson);

            // Setup mock for IMonoIntegrationApiService
            _mockMonobankIntegrationApiService.Setup(s => s.GetClientInfo(savedToken.TokenValue))
                                              .ReturnsAsync(ServiceResult<MonoClientInfoResponse>.Success(expectedClientInfo));

            // Act
            var resultAction = await _controller.GetClientInfo();
            var okResult = Assert.IsType<OkObjectResult>(resultAction);
            var returnValue = Assert.IsType<MonoClientInfoResponse>(okResult.Value);

            //Assert.Equal(expectedClientInfo.Property1, returnValue.Property1);
            //Assert.Equal(expectedClientInfo.Property2, returnValue.Property2);
        }*/


    }
}