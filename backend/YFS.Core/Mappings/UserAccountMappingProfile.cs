using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class UserAccountMappingProfile : Profile
    {
        public UserAccountMappingProfile()
        {
            CreateMap<User, UserAccountDto>();
        }
    }
}
