using AutoMapper;
using YFS.Core.Dtos;
using YFS.Core.Models;

namespace YFS.Core.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
