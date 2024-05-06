using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class BanksSyncService : BaseService, IBanksSyncService
    {
        public BanksSyncService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger) : base(repository, mapper, logger)
        {
        }
        public async Task<ServiceResult<string>> SyncBanksAsync(string country)
        {
            try
            {
                string apiUrl = GetApiUrl(country);
                var client = new HttpClient();
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                var banks = JsonConvert.DeserializeObject<List<Bank>>(json);

                await _repository.Bank.UpdateBanksAsync(banks);
                // Log the synchronization history
                var history = new BankSyncHistory
                {
                    LastUpdated = DateTime.UtcNow,
                    Country = country
                };
                await _repository.BankSyncHistory.AddOrUpdateHistoryAsync(history);

                await _repository.SaveAsync();
                //return "Data synchronized for country: " + country;
                return ServiceResult<string>.Success($"Data synchronized for country: {country}");
            }
            catch (HttpRequestException ex)
            {
                // Handling specific network or protocol errors (e.g., connection failed)
                _logger.LogError($"Network error in {nameof(SyncBanksAsync)}: {ex}");
                return ServiceResult<string>.Error("Network error occurred while synchronizing bank data.");
            }
            catch (JsonException ex)
            {
                // Handling JSON parsing errors
                _logger.LogError($"JSON parsing error in {nameof(SyncBanksAsync)}: {ex}");
                return ServiceResult<string>.Error("Error parsing bank data from the external service.");
            }
            catch (Exception ex)
            {
                // General error handling
                _logger.LogError($"An unexpected error occurred in {nameof(SyncBanksAsync)}: {ex}");
                return ServiceResult<string>.Error("An unexpected error occurred while synchronizing bank data.");
            }
        }

        private string GetApiUrl(string country)
        {
            // Return API URL based on country
            switch (country.ToLower())
            {
                case "ukraine": return "https://bank.gov.ua/NBU_BankInfo/get_data_branch_glbank?json";
                //case "usa": return "https://api.usa.gov/banks";
                //case "netherlands": return "https://api.netherlands.gov/banks";
                default: throw new ArgumentException("Country not supported.");
            }
        }
    }
}
