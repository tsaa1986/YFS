using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountTypesService : BaseService, IAccountTypesService
    {
        public AccountTypesService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<ServiceResult<IEnumerable<AccountTypeDto>>> GetAccountTypes()
        {
            try
            {
                var accountTypes = await _repository.AccountType.GetAllAccountTypes(trackChanges: false);
                var accountTypesDto = _mapper.Map<IEnumerable<AccountTypeDto>>(accountTypes);
                return ServiceResult<IEnumerable<AccountTypeDto>>.Success(accountTypesDto);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong in the {nameof(GetAccountTypes)} action {ex}");
                return ServiceResult<IEnumerable<AccountTypeDto>>.Error(ex.Message);
            }
        }
    }
}
