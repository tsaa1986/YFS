using AutoMapper;
using YFS.Data.Dtos;
using YFS.Data.Models;

namespace YFS.Data.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegistrationDto, User>();
        }
    }
}
