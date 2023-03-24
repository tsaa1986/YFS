using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class CurrencyMappingProfile : Profile
    {
        public CurrencyMappingProfile() {
            CreateMap<Currency, CurrencyDto>();
            CreateMap<CurrencyDto, Currency>();
        }
    }
}
