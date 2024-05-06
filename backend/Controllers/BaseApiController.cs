using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using YFS.Core.Models;
using YFS.Service.Interfaces;
using YFS.Service.Services;

namespace Controllers
{
    public class BaseApiController : ControllerBase
    {        
        protected readonly IRepositoryManager _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger<BaseApiController> _logger;

        public BaseApiController(IRepositoryManager repository, IMapper mapper, ILogger<BaseApiController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }       
        protected string GetUserIdFromJwt(string _authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            _authHeader = _authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(_authHeader);
            var tokenS = handler.ReadToken(_authHeader) as JwtSecurityToken;
            var userid = tokenS.Claims.First(claim => claim.Type == "userId").Value;

            return userid;
        }
    }

}