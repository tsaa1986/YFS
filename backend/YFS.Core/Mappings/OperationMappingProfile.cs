using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class OperationMappingProfile : Profile
    {
        public OperationMappingProfile()
        {
            CreateMap<OperationDto, Operation>();
            CreateMap<Operation, OperationDto>()
             .ForMember(dest => dest.AccountName, conf => conf.MapFrom(opt => opt.Account.Name))
             .ForMember(dest => dest.CategoryName, conf => conf.MapFrom(opt => opt.Category.Name_ENG))
             .ForMember(dest => dest.Balance, conf => conf.MapFrom(opt => opt.Account.AccountBalance.Balance));
        }
    }
}
