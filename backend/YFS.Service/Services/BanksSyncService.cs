using AutoMapper;
using Microsoft.Extensions.Logging;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class BanksSyncService : BaseService, IBanksSyncService
    {
        public BanksSyncService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger) : base(repository, mapper, logger)
        {
        }
        public async Task<string> SyncBanksAsync(string country)
        {
            // Example logic: fetch data from external API based on country
            string apiUrl = GetApiUrl(country);
            var client = new HttpClient();
            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            // Simulate data processing and database operations
            //Logger.LogInformation("Processing bank data for country: {Country}", country);
            //Repository.Banks.Update(data); // Assuming such a method exists

            return "Data synchronized for country: " + country;
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
