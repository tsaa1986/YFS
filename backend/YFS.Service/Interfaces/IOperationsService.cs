using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;

namespace YFS.Service.Interfaces
{
    public interface IOperationsService
    {
        Task<IActionResult> UpdateOperation(OperationDto operationDto);
        Task<ActionResult> RemoveTransferOperation(int operationId);
    }
}
