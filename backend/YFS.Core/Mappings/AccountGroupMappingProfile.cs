using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class AccountGroupMappingProfile : Profile
    {
        public AccountGroupMappingProfile()
        {
            CreateMap<AccountGroupDto, AccountGroup>();
            CreateMap<AccountGroup, AccountGroupDto>();
        }
    }
}
