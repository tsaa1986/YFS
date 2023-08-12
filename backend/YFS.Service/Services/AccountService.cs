﻿using AutoMapper;
using Azure.Core;
using Microsoft.Identity.Client;
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
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<ServiceResult<AccountDto>> CreateAccountForUser(AccountDto account, string userId)
        {
            try
            {
                var accountData = _mapper.Map<Account>(account);

                accountData.UserId = userId;
                accountData.AccountBalance = new AccountBalance { Balance = account.Balance };
                await _repository.Account.CreateAccount(accountData);

                if (account.Balance != 0)
                {
                    accountData.Operations = new List<Operation>() { {
                    new Operation {
                        AccountId = account.Id,
                        UserId= userId,
                        OperationAmount = account.Balance,
                        OperationCurrencyId = account.CurrencyId,
                        CurrencyAmount = account.Balance,
                        Description = "openning account",
                        TypeOperation = account.Balance > 0 ? 2 : 1,
                        CategoryId = -2,
                        OperationDate = account.OpeningDate,
                    } } };
                }
                await _repository.SaveAsync();

                var accountReturn = _mapper.Map<AccountDto>(accountData);
                return ServiceResult<AccountDto>.Success(accountReturn);
            }
            catch (Exception ex)
            {
                return ServiceResult<AccountDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<AccountDto>> GetAccountById(int accountId)
        {
            try
            {
                var account = await _repository.Account.GetAccount(accountId);
                var accountDto = _mapper.Map<AccountDto>(account);
                return ServiceResult<AccountDto>.Success(accountDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<AccountDto>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<AccountDto>>> GetAccountsByGroup(int accountGroupId, string userId, bool trackChanges)
        {
            try
            {
                var accounts = await _repository.Account.GetAccountsByGroup(accountGroupId, userId, false);
                var accountsDto = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return ServiceResult<IEnumerable<AccountDto>>.Success(accountsDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<AccountDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<IEnumerable<AccountDto>>> GetOpenAccountsByUserId(string userId, bool trackChanges)
        {
            try
            {
                var accounts = await _repository.Account.GetOpenAccountsByUserId(userId, trackChanges);
                var accountsDto = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return ServiceResult<IEnumerable<AccountDto>>.Success(accountsDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<AccountDto>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResult<AccountDto>> UpdateAccount(AccountDto account)
        {
            try
            {
                var accountData = _mapper.Map<Account>(account);
                await _repository.Account.UpdateAccount(accountData);
                await _repository.SaveAsync();

                Account updatedAccount = await _repository.Account.GetAccount(accountData.Id);
                var accountDto = _mapper.Map<AccountDto>(updatedAccount);
                return ServiceResult<AccountDto>.Success(accountDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<AccountDto>.Error(ex.Message);
            }
        }
    }
}
