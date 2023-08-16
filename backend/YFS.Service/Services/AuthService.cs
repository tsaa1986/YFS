using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RTools_NTS.Util;
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
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(IRepositoryManager repository, IMapper mapper, ILogger<BaseService> logger) : base(repository, mapper, logger)
        {
        }

        public async Task<ServiceResult<string>> Authenticate(UserLoginDto user)
        {
            /*
            return !await _repository.UserAuthentication.ValidateUserAsync(user)
                ? Unauthorized()
                : Ok(new { Token = await _repository.UserAuthentication.CreateTokenAsync() });*/
            try
            {
                var isAuth = await _repository.UserAuthentication.ValidateUserAsync(user);
                if (isAuth == false)
                {
                    return ServiceResult<string>.Error(null);
                }
                else 
                {
                    string token = await _repository.UserAuthentication.CreateTokenAsync();
                    return ServiceResult<string>.Success(token);                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(Authenticate)} action {ex}");
                return ServiceResult<string>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<UserAccountDto>> GetMe(string userId)
        {
            try
            {
                var userAccount = await _repository.UserAuthentication.GetUserAccountById(userId);
                var userAccountDto = _mapper.Map<UserAccountDto>(userAccount);
                return ServiceResult<UserAccountDto>.Success(userAccountDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetMe)} action {ex}");
                return ServiceResult<UserAccountDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<UserAccountDto>> RegisterUser(UserRegistrationDto userRegistration)
        {
            try
            {
                var userResult = await _repository.UserAuthentication.RegisterUserAsync(userRegistration);

                if (userResult.Succeeded)
                {
                    UserLoginDto user = new UserLoginDto { UserName = userRegistration.UserName, Password = userRegistration.Password };

                    var Token = await _repository.UserAuthentication.ValidateUserAsync(user);
                    string user_id = _repository.UserAuthentication.GetUserId(user).Result;


                    await _repository.AccountGroup.CreateAccountGroupsDefaultForUser(user_id);
                    await _repository.SaveAsync();
                    var returnUser = await GetMe(user_id);
                    return userResult.Succeeded ? ServiceResult<UserAccountDto>.Success(returnUser.Data) : ServiceResult<UserAccountDto>.CustomError(userResult);
                }
                else
                {                    
                    return ServiceResult<UserAccountDto>.CustomError(userResult);
                }
                //return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);
                //return userResult.Succeeded ? ServiceResult<UserRegistrationDto>.Success(returnUser) : ServiceResult<UserRegistrationDto>.CustomError(userResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(RegisterUser)} action {RegisterUser}");
                return ServiceResult<UserAccountDto>.Error(ex.Message);
            }
        }
    }
}
