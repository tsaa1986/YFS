using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class OperationMappingProfile : Profile
    {
        public OperationMappingProfile()
        {
            CreateMap<OperationDto, Operation>()
                       .ForMember(dest => dest.OperationItems, opt => opt.MapFrom(src => src.OperationItems));
            CreateMap<Operation, OperationDto>()
                .ForMember(dest => dest.AccountName, conf => conf.MapFrom(opt => opt.Account.Name))
                .ForMember(dest => dest.Balance, conf => conf.MapFrom(opt => opt.Account.AccountBalance.Balance))
                .ForMember(dest => dest.OperationItems, opt => opt.MapFrom(src => src.OperationItems));

            CreateMap<OperationItemDto, OperationItem>();
            CreateMap<OperationItem, OperationItemDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Category.Translations.FirstOrDefault(t => t.LanguageCode == context.Options.Items["LanguageCode"].ToString())?.Name ?? string.Empty));

        }
    }
}
