using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Service.Filters.ActionFilters;
using YFS.Service.Interfaces;
using YFS.Core.Models;
using YFS.Service.Services;

namespace YFS.Data.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService ,IRepositoryManager repository, 
            IMapper mapper) 
            : base(repository, mapper)
        {
            _authService = authService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            string userId = GetUserIdFromJwt(Request.Headers["Authorization"]);
            var result = await _authService.GetMe(userId);

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

        [HttpPost("sign-up")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var serviceResult = await _authService.RegisterUser(userRegistration);

            if (serviceResult.IsSuccess)
            {
                return StatusCode(201, serviceResult.Data); //Ok(serviceResult.Data);
            }
            else
            {
                return BadRequest(serviceResult.ErrorMessage);
            }
        }

        [HttpPost("sign-in")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto user)
        {
            var result = await _authService.Authenticate(user);

            if (result.IsSuccess)
            {
                return Ok(new { Token = result.Data });
            }
            else if (result.IsNotFound)
            {
                return Unauthorized();
            }
            else
            {
                return Unauthorized();//BadRequest(result.ErrorMessage);
            }
        }
    }
}
