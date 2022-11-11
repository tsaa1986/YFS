using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    }

}