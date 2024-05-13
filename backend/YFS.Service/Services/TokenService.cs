using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class TokenService : BaseService, ITokenService
    {
        public TokenService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger) : base(repository, mapper, logger)
        {
        }

        public Task<ServiceResult<ApiTokenDto>> CreateToken(ApiTokenDto accountGroup, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ApiTokenDto>> GetTokenByNameForUser(string tokenName, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<ApiTokenDto>>> GetTokensForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ApiTokenDto>> UpdateToken(ApiTokenDto accountGroup, string userId)
        {
            throw new NotImplementedException();
        }

        /*
        public async Task<ServiceResult<AccountGroupDto>> CreateAccountGroupForUser(AccountGroupDto accountGroup, string userId)
        {
            AccountGroup accountGroupData = _mapper.Map<AccountGroup>(accountGroup);
            accountGroupData.UserId = userId;
            try
            {
                await _repository.AccountGroup.CreateAccountGroupForUser(accountGroupData);
                await _repository.SaveAsync();
                AccountGroup accountGroupDataReturn = await _repository.AccountGroup.GetAccountGroup(accountGroupData.AccountGroupId, false);

                var accountGroupReturn = _mapper.Map<AccountGroupDto>(accountGroupDataReturn);
                return ServiceResult<AccountGroupDto>.Success(accountGroupReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while CreateAccountGroup: {UserId}", userId);
                return ServiceResult<AccountGroupDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<AccountGroupDto>>> GetAccountGroupsForUser(string userId)
        {
            try
            {
                var accountGroups = await _repository.AccountGroup.GetAccountGroupsForUser(userId, false);
                var accountGroupsDto = _mapper.Map<IEnumerable<AccountGroupDto>>(accountGroups);
                return ServiceResult<IEnumerable<AccountGroupDto>>.Success(accountGroupsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while GetAccountGroupsForUser: {UserId}", userId);
                return ServiceResult<IEnumerable<AccountGroupDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<AccountGroupDto>> UpdateGroupForUser(AccountGroupDto accountGroup, string userId)
        {
            try
            {
                accountGroup.UserId = userId;
                var accountGroupData = _mapper.Map<AccountGroup>(accountGroup);

                await _repository.AccountGroup.UpdateAccountGroupForUser(accountGroupData);
                await _repository.SaveAsync();
                AccountGroup accountGroupDataReturn = await _repository.AccountGroup.GetAccountGroup(accountGroupData.AccountGroupId, false);
                var accountGroupsDtoUpdated = _mapper.Map<AccountGroupDto>(accountGroupDataReturn);
                return ServiceResult<AccountGroupDto>.Success(accountGroupsDtoUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(UpdateGroupForUser)} action {ex}");
                return ServiceResult<AccountGroupDto>.Error(ex.Message);
            }

        }*/
    }
}
