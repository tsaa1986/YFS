using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class AccountTypeMappingProfile : Profile
    {
        public AccountTypeMappingProfile()
        {
            CreateMap<AccountType, AccountTypeDto>();

            CreateMap<AccountTypeDto, AccountType>();
        }
    }
}
