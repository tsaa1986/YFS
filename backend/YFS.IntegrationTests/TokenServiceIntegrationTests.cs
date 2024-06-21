using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;
using YFS.Repo.Data;
using YFS.Service.Interfaces;
using YFS.Service.Services;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class TokenServiceIntegrationTests
    {
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly SeedDataIntegrationTests _seedDataIntegrationTests;
        private readonly HttpClient _client;
        public TokenServiceIntegrationTests(TestingWebAppFactory<Program> factory)
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
        public async Task CreateToken_ShouldReturnSuccess_WhenValidTokenProvided()
        {
            // Arrange            
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);
            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();

                // Act
                var result = await tokenService.CreateToken(apiTokenMono);

                // Assert
                Assert.NotNull(user);
                Assert.NotNull(apiTokenMono);
                Assert.NotNull(result.Data);
                Assert.Equal(apiTokenMono.UserId, result.Data.UserId);
                Assert.Equal(apiTokenMono.Name, result.Data.Name);
                Assert.Equal(apiTokenMono.TokenType, result.Data.TokenType);
                Assert.Equal(apiTokenMono.TokenValue, result.Data.TokenValue);
                Assert.Equal(apiTokenMono.Url, result.Data.Url);
                Assert.Equal(apiTokenMono.Note, result.Data.Note);
            }
        }
        [Fact]
        public async Task GetTokenByNameForUser_ShouldReturnApiToken_WhenValidTokenProvided()
        {
            // Arrange            
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);
            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();
                var result = await tokenService.CreateToken(apiTokenMono);
                Assert.NotNull(result.Data);

                // Act
                var getResult = await tokenService.GetTokenByNameForUser(apiTokenMono.Name, user.Id);

                // Assert
                Assert.True(getResult.IsSuccess);
                Assert.NotNull(getResult.Data);
                Assert.Equal(apiTokenMono.UserId, getResult.Data.UserId);
                Assert.Equal(apiTokenMono.Name, getResult.Data.Name);
                Assert.Equal(apiTokenMono.TokenType, getResult.Data.TokenType);
                Assert.Equal(apiTokenMono.TokenValue, getResult.Data.TokenValue);
                Assert.Equal(apiTokenMono.Url, getResult.Data.Url);
                Assert.Equal(apiTokenMono.Note, getResult.Data.Note);
            }
        }

        [Fact]
        public async Task UpdateToken_ShouldReturnSuccess_WhenValidTokenProvided()
        {
            // Arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);
            ApiTokenDto createdToken = new ApiTokenDto();

            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();

                // Create token
                var createResult = await tokenService.CreateToken(apiTokenMono);
                createdToken = createResult.Data;

                Assert.NotNull(createResult.Data);
            }

            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();

                // Update token details
                createdToken.Note = "Updated Note";
                createdToken.Url = "https://updated-url.com";

                // Act
                var updateResult = await tokenService.UpdateToken(createdToken);

                // Assert
                Assert.True(updateResult.IsSuccess);
                Assert.NotNull(updateResult.Data);
                Assert.Equal("Updated Note", updateResult.Data.Note);
                Assert.Equal("https://updated-url.com", updateResult.Data.Url);
            }
        }

        [Fact]
        public async Task InitializeDefaultRules_ShouldGetDefaultRules_WhenApiTokenIsCreated()
        {
            // Arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);

            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();
                var monoIntegrateService = serviceProvider.GetRequiredService<IMonoIntegrationApiService>();

                // Create the token
                var createResult = await tokenService.CreateToken(apiTokenMono);
                Assert.NotNull(createResult.Data);

                // Act
                var rules = await monoIntegrateService.GetActiveRulesByApiTokenIdAsync(createResult.Data.Id);

                // Assert
                Assert.Equal(rules.Data.Count(), 0);
                //Assert.Single(rules.Data);
                /*var rule = rules.Data.First();
                Assert.Equal("Set Category for MCC 4829", rule.RuleName);
                Assert.Equal("{\"Mcc\": 4378}", rule.Condition);
                Assert.Equal("{\"CategoryId\": -1}", rule.Action);
                Assert.Equal(100, rule.Priority);
                Assert.True(rule.IsActive);*/
            }
        }


        /*
        [Fact]
        public async Task GetTokensForUser_ShouldReturnSuccess_WhenValidUserIdProvided()
        {
            // Arrange
            var user = await _seedDataIntegrationTests.CreateUserSignUpAsync(_client);
            ApiTokenDto apiTokenMono = _seedDataIntegrationTests.CreateApiTokenMonobank(user.Id);

            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tokenService = serviceProvider.GetRequiredService<ITokenService>();

                // Create tokens
                var createResult = await tokenService.CreateToken(apiTokenMono);

                Assert.NotNull(createResult.Data);

                // Act
                var getResult = await tokenService.GetTokensForUser(user.Id);

                // Assert
                Assert.True(getResult.IsSuccess);
                Assert.NotNull(getResult.Data);
                var tokens = getResult.Data.ToList();
                Assert.Equal(1, tokens.Count);

                var token = tokens.FirstOrDefault(t => t.TokenValue == apiTokenMono.TokenValue);

                Assert.NotNull(token);
                Assert.Equal(apiTokenMono.UserId, token.UserId);
                Assert.Equal(apiTokenMono.Name, token.Name);
                Assert.Equal(apiTokenMono.TokenType, token.TokenType);
                Assert.Equal(apiTokenMono.TokenValue, token.TokenValue);
                Assert.Equal(apiTokenMono.Url, token.Url);
                Assert.Equal(apiTokenMono.Note, token.Note);
            }
        }*/
    }
}