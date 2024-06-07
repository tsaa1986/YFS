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

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class MonoIntegrationApiControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly SeedDataIntegrationTests _seedDataIntegrationTests;
        private readonly Mock<IMonoIntegrationApiService> _mockMonobankIntegrationApiService;
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<BaseApiController>> _mockLogger;
        private readonly MonobankIntegrationApiController _controller;
        private readonly ITokenService _tokenService;

        public MonoIntegrationApiControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _mockMonobankIntegrationApiService = new Mock<IMonoIntegrationApiService>();
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BaseApiController>>();
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