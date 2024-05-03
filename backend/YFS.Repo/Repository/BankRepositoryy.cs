using Microsoft.EntityFrameworkCore;
using YFS.Core.Models;
using YFS.Repo.Data;
using YFS.Repo.GenericRepository.Services;
using YFS.Service.Interfaces;

namespace YFS.Service.Services
{
    public class BankRepository : RepositoryBase<Bank>, IBankRepository
    {
        public BankRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
           
        }
    }
}
