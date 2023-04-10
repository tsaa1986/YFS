using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        public CategoryController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {            
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetCategoriesForUser()
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var categories = await _repository.Category.GetCategoryForUser(userid, trackChanges: false);
                var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return Ok(categoriesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
