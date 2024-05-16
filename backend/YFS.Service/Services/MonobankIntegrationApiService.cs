using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using YFS.Service.Interfaces;
using YFS.Core.Models.MonoIntegration;
using YFS.Core.Dtos;
using Newtonsoft.Json;
using Microsoft.Extensions.Http;
using Newtonsoft.Json.Linq;

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
        public async Task<ServiceResult<IEnumerable<MonoStatement>>> GetStatementsBetweenDates(string xToken, string account, DateTime fromDate, DateTime toDate)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("X-Token", xToken);
                long fromUnixTime = (long)(fromDate.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                long toUnixTime = (long)(toDate.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                /*
                var dateTimeOffsetFrom = new DateTimeOffset(fromDate);
                var dateTimeOffsetTo = new DateTimeOffset(toDate);
                var unixDateTimeFrom = dateTimeOffsetFrom.ToUnixTimeSeconds();
                var unixDateTimeTo = dateTimeOffsetTo.ToUnixTimeSeconds();*/

                string requestUrl = $"{_httpClient.BaseAddress}statement/{account}/{fromUnixTime}/{toUnixTime}";

                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    IEnumerable<MonoStatement>? statements = JsonConvert.DeserializeObject<IEnumerable<MonoStatement>>(responseBody);
                    if (statements != null)
                    {
                        return ServiceResult<IEnumerable<MonoStatement>>.Success(statements);
                    }
                    else
                    {
                        return ServiceResult<IEnumerable<MonoStatement>>.Error("Failed to deserialize statements");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to retrieve statements from Monobank API: {response.StatusCode}");
                    return ServiceResult<IEnumerable<MonoStatement>>.Error($"Failed to retrieve statements: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetStatementsBetweenDates method: {ex.Message}");
                return ServiceResult<IEnumerable<MonoStatement>>.Error($"Error: {ex.Message}");
            }
        }
    }
}
