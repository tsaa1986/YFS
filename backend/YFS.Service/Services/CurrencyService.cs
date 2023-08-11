using AutoMapper;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        public CurrencyService(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
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
                return ServiceResult<IEnumerable<CurrencyDto>>.Error(ex.Message);
            }
        }
    }
}
