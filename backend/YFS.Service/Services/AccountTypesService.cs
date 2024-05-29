using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Enums;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class AccountTypesService : BaseService, IAccountTypesService
    {
        public AccountTypesService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger, LanguageScopedService languageService) 
            : base(repository, mapper, logger, languageService)
        {
        }

        public async Task<ServiceResult<IEnumerable<AccountTypeDto>>> GetAccountTypes(string language)
        {
            try
            {
                var accountTypes = await _repository.AccountType.GetAllAccountTypes(trackChanges: false, language: language);
                var accountTypesDto = _mapper.Map<IEnumerable<AccountTypeDto>>(accountTypes);
                return ServiceResult<IEnumerable<AccountTypeDto>>.Success(accountTypesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAccountTypes)} action {ex}");
                return ServiceResult<IEnumerable<AccountTypeDto>>.Error(ex.Message);
            }
        }
    }
}
