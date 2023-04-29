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
               .ForMember(dest => dest.AccountName, conf => conf.MapFrom(opt => opt.Account.Name)); 
               
        }
    }
}
