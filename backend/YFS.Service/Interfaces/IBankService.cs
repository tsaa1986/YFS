using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Core.Models;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
    public interface IBankService
    {
        Task<ServiceResult<Bank>> GetBankByGLMFO(int glmfo);
    }
}
