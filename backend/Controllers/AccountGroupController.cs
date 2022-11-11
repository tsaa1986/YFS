using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using YFS.Service.Interfaces;

namespace YFS.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountGroupController : BaseApiController
    {
        public AccountGroupController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {            
        }
    }
}
