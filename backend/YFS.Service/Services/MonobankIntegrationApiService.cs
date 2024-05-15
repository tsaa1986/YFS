using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using YFS.Service.Interfaces;
using YFS.Core.Models.MonoIntegration;
using YFS.Core.Dtos;
using Newtonsoft.Json;
using Microsoft.Extensions.Http;

namespace YFS.Service.Services
{
    public class MonobankIntegrationApiService : BaseService, IMonobankIntegrationApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        public MonobankIntegrationApiService(IHttpClientFactory httpClientFactory, IRepositoryManager repository, 
            IMapper mapper, 
            ILogger<BaseService> logger, 
            HttpClient httpClient) : base(repository, mapper, logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.monobank.ua/personal/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ServiceResult<MonoClientInfoResponse>> GetClientInfo(string xToken)
        {
            try
            {               
                _httpClient.DefaultRequestHeaders.Add("X-Token", xToken);
                HttpResponseMessage response = await _httpClient.GetAsync("client-info");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    MonoClientInfoResponse clientInfo = JsonConvert.DeserializeObject<MonoClientInfoResponse>(responseBody);

                    if (clientInfo != null)
                    {
                        return ServiceResult<MonoClientInfoResponse>.Success(clientInfo);
                    }
                    else
                    {
                        _logger.LogError("MonoCleint Info Deserialization result is null");

                        return ServiceResult<MonoClientInfoResponse>.Error("Failed to deserialize client info");
                    }
                }
                else {
                    _logger.LogError($"Failed to retrieve mono client info: {response.StatusCode}");

                    return ServiceResult<MonoClientInfoResponse>.NotFound($"Failed to retrieve client info: {response.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetClientInfo)} action {ex}");

                return ServiceResult<MonoClientInfoResponse>.Error(ex.Message);
            }
        }
    }
}
