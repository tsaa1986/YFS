using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<AccountDto, Account>();
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Balance, conf => conf.MapFrom(opt => opt.AccountBalance.Balance));
        }
    }
}
