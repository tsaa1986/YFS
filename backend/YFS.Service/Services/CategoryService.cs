using AutoMapper;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<ServiceResult<IEnumerable<CategoryDto>>> GetCategoriesForUser(string userId)
        {
            try
            {
                var categories = await _repository.Category.GetCategoryForUser(userId, false);
                var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return ServiceResult<IEnumerable<CategoryDto>>.Success(categoriesDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<CategoryDto>>.Error(ex.Message);
            }
        }
    }
}
