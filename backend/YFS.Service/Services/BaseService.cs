using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IRepositoryManager _repository;
        protected readonly ILogger<BaseService> _logger;
        public BaseService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }
    }
}
