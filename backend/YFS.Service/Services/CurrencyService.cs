using AutoMapper;
using Microsoft.Extensions.Logging;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        public CurrencyService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<ServiceResult<IEnumerable<CurrencyDto>>> GetCurrencies()
        {
            try
            {
                var currencies = await _repository.Currency.GetCurrencies(trackChanges: false);
                var currenciesDto = _mapper.Map<IEnumerable<CurrencyDto>>(currencies);
                return ServiceResult<IEnumerable<CurrencyDto>>.Success(currenciesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCurrencies)} action {ex}");
                return ServiceResult<IEnumerable<CurrencyDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<CurrencyDto>> GetCurrencyByCountry(int number, string country)
        {
            try
            {
                var currency = await _repository.Currency.GetCurrencyByCountry(number, country);
                var currencyDto = _mapper.Map<CurrencyDto>(currency);
                return ServiceResult<CurrencyDto>.Success(currencyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCurrencyByCountry)} action {ex}");
                return ServiceResult<CurrencyDto>.Error(ex.Message);
            }
        }
    }
}
