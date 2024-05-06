using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using YFS.Service.Interfaces;

namespace YFS.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService, IRepositoryManager repository, 
            IMapper mapper, ILogger<BaseApiController> logger) : base(repository, mapper, logger)
        {
            _categoryService = categoryService;
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetCategoriesForUser()
        {
             string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
             var result = await _categoryService.GetCategoriesForUser(userId);

             if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                else if (result.IsNotFound)
                {
                    return NotFound(result.ErrorMessage);
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }
        /*
        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateCategoryForUser([FromBody] CategoryDto category)
        {
            category.UserId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var categoryData = _mapper.Map<Category>(category);

            //await _repository.AccountGroup.CreateAccountGroupForUser(accountGroupData);
            await _repository.SaveAsync();

            var categoryReturn = _mapper.Map<CategoryDto>(categoryData);
            return Ok(category);
        }

        [HttpPut()]
        [Authorize]
        public async Task<IActionResult> UpdateCategoryForUser([FromBody] CategoryDto category)
        {
            category.UserId = GetUserIdFromJwt(Request.Headers["Authorization"]);

            var categoryData = _mapper.Map<Category>(category);

            _mapper.Map(category, categoryData);

            //await _repository.Category.UpdateCategoryForUser(category);
            //await _repository.SaveAsync();
            return NoContent();
        }*/
    }
}
