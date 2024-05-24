using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface ICurrencyService
    {
        Task<ServiceResult<IEnumerable<CurrencyDto>>> GetCurrencies();
        Task<ServiceResult<CurrencyDto>> GetCurrencyByCodeAndCountry(int number, string country);//840, United States of America (the)
    }
}
