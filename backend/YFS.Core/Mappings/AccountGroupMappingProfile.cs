using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class AccountGroupMappingProfile : Profile
    {
        public AccountGroupMappingProfile()
        {
            // Mapping from AccountGroup to AccountGroupDto
            CreateMap<AccountGroup, AccountGroupDto>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<AccountGroupDto, AccountGroup>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

            CreateMap<AccountGroupTranslationDto, AccountGroupTranslation>();
            CreateMap<AccountGroupTranslation, AccountGroupTranslationDto>();
        }
    }
}
