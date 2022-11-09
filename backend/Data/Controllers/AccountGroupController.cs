using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YFS.Data.Models;
using YFS.Data.Repository;

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
