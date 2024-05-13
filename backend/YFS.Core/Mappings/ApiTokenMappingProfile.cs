using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class ApiTokenMappingProfile : Profile
    {
        public ApiTokenMappingProfile()
        {
            CreateMap<ApiTokenDto, ApiToken>();
            CreateMap<ApiToken, ApiTokenDto>();
        }
    }
}
