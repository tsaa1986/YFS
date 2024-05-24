using YFS.Core.Models;

namespace YFS.Service.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetCurrencies(bool trackChanges);
        Task<Currency?> GetCurrencyByCodeAndCountry(int number, string country);
    }
}
