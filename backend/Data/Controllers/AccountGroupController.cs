using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YFS.Data.Models;

namespace YFS.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountGroupController : ControllerBase
    {
        private readonly IAccountGroupRepository _accgrpRepository;
        public AccountGroupController(IAccountGroupRepository accgrpRepository)
        {
            _accgrpRepository = accgrpRepository;
        }

        [HttpGet("{userid}")]
        public IEnumerable<AccountGroup> GetAccountGroup(int userid)
        {
            var accountgroup = _accgrpRepository.GetAllByUser(userid);
            return accountgroup;
        }

        [HttpGet]
        [Route("GetAll")]
        public string GetAll()
        {
            return "api service";
        }


    }
}
