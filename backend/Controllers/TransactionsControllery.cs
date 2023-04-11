using AutoMapper;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace YFS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : BaseApiController
    {
        public TransactionsController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transaction)
        {
            var transactionData = _mapper.Map<Transaction>(transaction);

            string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
            transactionData.UserId = userid;
            await _repository.Transaction.CreateTransaction(transactionData);
            await _repository.SaveAsync();

            var transactionReturn = _mapper.Map<TransactionDto>(transactionData);
            return Ok(transactionReturn);
        }

        [HttpGet("{accountId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountsByGroup(int accountId)
        {
            try
            {
                string userid = GetUserIdFromJwt(Request.Headers["Authorization"]);
                var transactions = await _repository.Transaction.GetTransactionForAccount(userid, accountId, trackChanges: false);
                var transactionDto = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
                return Ok(transactionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateTransaction([FromBody] TransactionDto transaction)
        {
            //var accountData = HttpContext.Items["account"] as Account;
            var transactionData = _mapper.Map<Transaction>(transaction);
            _mapper.Map(transaction, transactionData);
            await _repository.Transaction.UpdateTransaction(transactionData);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
