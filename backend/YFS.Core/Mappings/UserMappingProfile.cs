using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegistrationDto, User>();
        }
    }
}
