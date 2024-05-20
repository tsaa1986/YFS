using Microsoft.AspNetCore.Mvc;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<AccountDto>> GetAccountById(int accountId);
        Task<ServiceResult<IEnumerable<AccountDto>>> GetAccountsByGroup(int accountGroupId, string userId, bool trackChanges);
        Task<ServiceResult<IEnumerable<AccountDto>>> GetOpenAccountsByUserId(string userId, bool trackChanges);
        Task<ServiceResult<IEnumerable<AccountDto>>> GetAccountsByUserId(string userId, bool trackChanges);
        Task<ServiceResult<AccountDto>> GetExternalAccountById(string externalAccountId, string userId, bool trackChanges);
        Task<ServiceResult<AccountDto>> CreateAccountForUser(AccountDto account, string userId);
        Task<ServiceResult<AccountDto>> UpdateAccount(AccountDto account);
    }
}