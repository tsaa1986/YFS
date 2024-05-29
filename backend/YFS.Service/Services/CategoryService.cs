using AutoMapper;
using Microsoft.Extensions.Logging;
using YFS.Core.Dtos;
using YFS.Core.Enums;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger, LanguageScopedService languageService) 
            : base(repository, mapper, logger, languageService)
        {
        }

        public async Task<ServiceResult<IEnumerable<CategoryDto>>> GetCategoriesForUser(string userId)
        {
            try
            {
                var categories = await _repository.Category.GetCategoryForUser(userId, LanguageCode, trackChanges: false);
                var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return ServiceResult<IEnumerable<CategoryDto>>.Success(categoriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCategoriesForUser)} action {ex}");
                return ServiceResult<IEnumerable<CategoryDto>>.Error(ex.Message);
            }
        }
    }
}
