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
    public class AccountGroupsController : BaseApiController
    {
        public AccountGroupsController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {            
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAccountGroupsForUser()
        {
            try
            {
                //var handler = new JwtSecurityTokenHandler();
                //string authHeader = Request.Headers["Authorization"];
                //authHeader = authHeader.Replace("Bearer ", "");
                //var jsonToken = handler.ReadToken(authHeader);
                //var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                //var userid = tokenS.Claims.First(claim => claim.Type == "userId").Value;

                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var accountGroups = await _repository.AccountGroup.GetAccountGroupsForUser(userid, trackChanges: false);
                var accountGroupsDto = _mapper.Map<IEnumerable<AccountGroupDto>>(accountGroups);
                return Ok(accountGroupsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{userid}")]
        public async Task<IActionResult> CreateAccountGroupForUser(string userid, [FromBody] AccountGroupDto accountGroup)
        {
            accountGroup.UserId = userid;
            var accountGroupData = _mapper.Map<AccountGroup>(accountGroup);

            await _repository.AccountGroup.CreateAccountGroupForUser(accountGroupData);
            await _repository.SaveAsync();

            var accountGroupReturn = _mapper.Map<AccountGroupDto>(accountGroupData);
            return Ok(accountGroupReturn);
        }

        [HttpPut("{userid}")]
        public async Task<IActionResult> UpdateCreateGroupForUser(string userid, [FromBody] AccountGroupDto accountGroup)
        {
            accountGroup.UserId = userid;

            var accountGroupData = _mapper.Map<AccountGroup>(accountGroup);

            _mapper.Map(accountGroup, accountGroupData);

            await _repository.AccountGroup.UpdateAccountGroupForUser(accountGroupData);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
