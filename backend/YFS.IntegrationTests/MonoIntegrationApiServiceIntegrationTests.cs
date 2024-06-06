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
    public class MonoIntegrationApiServiceIntegrationTests
    {
        private readonly TestingWebAppFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly SeedDataIntegrationTests _seedDataIntegrationTests;

        public MonoIntegrationApiServiceIntegrationTests(TestingWebAppFactory<Program> factory)
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
    }
}