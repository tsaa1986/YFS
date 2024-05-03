using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Dtos;
using YFS.Service.Services;

namespace YFS.Service.Interfaces
{
   public interface IBanksSyncService
   {
     Task<string> SyncBanksAsync(string country);
   }
}
