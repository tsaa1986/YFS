using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Enums;
using YFS.Core.Models;
using YFS.Core.Models.MonoIntegration;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class TokenService : BaseService, ITokenService
    {
        public TokenService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger, LanguageScopedService languageService) 
            : base(repository, mapper, logger, languageService)
        {
        }
        public async Task<ServiceResult<ApiTokenDto>> CreateToken(ApiTokenDto token)
        {
            try
            {
                var tokenData = _mapper.Map<ApiToken>(token);
                await _repository.ApiToken.AddToken(tokenData);
                await _repository.SaveAsync();

                // Initialize default rules
                //await InitializeDefaultRules(tokenData.Id);
                var resultToken = _mapper.Map<ApiTokenDto>(tokenData);

                return ServiceResult<ApiTokenDto>.Success(resultToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while  CreateToken: userid: {UserId}, token: {token}", token.UserId, token);
                return ServiceResult<ApiTokenDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<ApiTokenDto>> GetTokenByNameForUser(string tokenName, string userId)
        {
            try
            {
                var apiToken = await _repository.ApiToken.GetApiToken(tokenName, userId);

                if (apiToken != null)
                {
                    var apiTokenDto = _mapper.Map<ApiTokenDto>(apiToken);
                    return ServiceResult<ApiTokenDto>.Success(apiTokenDto);
                }
                else
                {
                    return ServiceResult<ApiTokenDto>.Error("ApiToken not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetTokenByNameForUser)} action {ex}");
                return ServiceResult<ApiTokenDto>.Error(ex.Message);
            }
        }

        public Task<ServiceResult<IEnumerable<ApiTokenDto>>> GetTokensForUser(string userId)
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceResult<ApiTokenDto>> UpdateToken(ApiTokenDto token)
        {
            try
            {
                var tokenData = _mapper.Map<ApiToken>(token);
                await _repository.ApiToken.UpdateToken(tokenData);
                await _repository.SaveAsync();

                return ServiceResult<ApiTokenDto>.Success(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while  UpdateToken: userid: {UserId}, token: {token}", token.UserId, token);
                return ServiceResult<ApiTokenDto>.Error(ex.Message);
            }
        }

        private async Task<ServiceResult<bool>> InitializeDefaultRules(int apiTokenId)
        {
            try
            {
                var defaultRules = new List<MonoSyncRule>
                    {
                        new MonoSyncRule
                            {
                                ApiTokenId = apiTokenId,
                                RuleName = "Set Category for MCC 4829",
                                Description = "Set CategoryId to -1 if MCC is 4378",
                                Condition = "{\"Mcc\": 4378}",
                                Action = "{\"CategoryId\": -1}",
                                Priority = 100,
                                IsActive = true
                            }
                       /* new MonoSyncRule
                        {
                            ApiTokenId = apiTokenId,
                            RuleName = "Set Category for MCC 4829",
                            Description = "Set CategoryId to -1 if MCC is 4378",
                            Condition = "{\"Mcc\": 4378}",
                            Action = "{\"CategoryId\": -1}",
                            Priority = 1,
                            IsActive = true
                        } */,


                    };
                await _repository.MonoSyncRule.AddRange(defaultRules);
                await _repository.SaveAsync();

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while  CreateRules for Token");
                return ServiceResult<bool>.Error(ex.Message);

            }
        }
    }
}
