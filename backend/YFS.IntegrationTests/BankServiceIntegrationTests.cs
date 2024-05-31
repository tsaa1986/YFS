using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Service.Interfaces;
using YFS.Service.Services;

namespace YFS.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class BankServiceIntegrationTests
    {
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly SeedDataIntegrationTests _seedDataIntegrationTests;

        public BankServiceIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _factory = factory;
            _serviceProvider = _factory.Services;
            _seedDataIntegrationTests = SeedDataIntegrationTests.Instance;

            using (var scope = _serviceProvider.CreateScope())
            {
                _serviceProvider = scope.ServiceProvider;

                _seedDataIntegrationTests.InitializeDatabaseAsync(_serviceProvider).Wait();
            }
        }

        [Fact]
        public async Task GetBankByGLMFO_Returns_BankUkrSib()
        {
            // Arrange
            int glmfo = 351005; // Assume this GLMFO exists in the seeded data

            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var bankService = serviceProvider.GetRequiredService<IBankService>();

                // Act
                var result = await bankService.GetBankByGLMFO(glmfo);

                // Assert
                Assert.NotNull(result.Data);
                Assert.Equal(glmfo, result.Data.GLMFO);
            }
        }
        [Fact]
        public async Task GetBankByGLMFO_Returns_BankMono()
        {
            // Arrange
            int glmfo = 322001; 

            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var bankService = serviceProvider.GetRequiredService<IBankService>();

                // Act
                var result = await bankService.GetBankByGLMFO(glmfo);

                // Assert
                Assert.NotNull(result.Data);
                Assert.Equal(glmfo, result.Data.GLMFO);
            }
        }
        [Fact]
        public async Task GetBankByGLMFO_Returns_BankPrivat()
        {
            // Arrange
            int glmfo = 305299;

            using (var scope = _factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var bankService = serviceProvider.GetRequiredService<IBankService>();

                // Act
                var result = await bankService.GetBankByGLMFO(glmfo);

                // Assert
                Assert.NotNull(result.Data);
                Assert.Equal(glmfo, result.Data.GLMFO);
            }
        }
    }
}