using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YFS.Core.Dtos;
using YFS.Core.Enums;
using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class BankService : BaseService, IBankService
    {
        public BankService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger, LanguageScopedService languageService) 
            : base(repository, mapper, logger, languageService)
        {
        }

        public async Task<ServiceResult<Bank>> GetBankByGLMFO(int glmfo)
        {
            try
            {
                var bank = await _repository.Bank.GetBankByGLMFO(glmfo);
                if (bank == null) { new Exception($"Bank {glmfo} not found"); }
                   return ServiceResult<Bank>.Success(bank);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetBankByGLMFO)} action {ex}");
                return ServiceResult<Bank>.Error(ex.Message);
            }
        }
    }
}
