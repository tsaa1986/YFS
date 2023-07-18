using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class AccountMonthlyBalanceMappingProfile : Profile
    {
        public AccountMonthlyBalanceMappingProfile()
        {
            CreateMap<AccountMonthlyBalanceDto, AccountMonthlyBalance>();
            CreateMap<AccountMonthlyBalance, AccountMonthlyBalanceDto>();
                //.ForMember(dest => dest.Balance, conf => conf.MapFrom(opt => opt.AccountBalance.Balance))
                //.ForMember(dest => dest.CurrencyName, conf => conf.MapFrom(opt => opt.Currency.ShortNameUs));
        }
    }
}
