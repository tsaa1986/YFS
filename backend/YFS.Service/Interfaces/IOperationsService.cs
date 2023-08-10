using Microsoft.AspNetCore.Mvc;
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
    public interface IOperationsService
    {
        Task<ServiceResult<IEnumerable<OperationDto>>> CreateOperation(OperationDto operation, int targetAccountId, string userId);
        Task<ServiceResult<OperationDto>> UpdateOperation(OperationDto operationDto);
        Task<ServiceResult<IEnumerable<OperationDto>>> RemoveTransferOperation(int operationId);
        Task<ServiceResult<OperationDto>> RemoveOperation(int operationId);
        Task<ServiceResult<IEnumerable<OperationDto>>> GetOperationsAccountForPeriod(int accountId, DateTime startDate, DateTime endDate);
        Task<ServiceResult<IEnumerable<OperationDto>>> GetLast10OperationsAccount(int accountId);
    }
}
