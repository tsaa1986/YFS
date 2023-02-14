using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using YFS.Service.Interfaces;

namespace Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected readonly IRepositoryManager _repository;
        protected readonly IMapper _mapper;

        public BaseApiController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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