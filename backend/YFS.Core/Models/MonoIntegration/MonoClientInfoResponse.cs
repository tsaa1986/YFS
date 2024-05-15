using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models.MonoIntegration
{
    public class MonoClientInfoResponse
    {
        public string? clientId { get; set; }
        public string? name { get; set; }
        public string? webHookUrl { get; set; } //URL для надсиляння подій по зміні балансу рахунку
        public string permissions { get; set; } = null!; //Перелік прав, які які надає сервіс (1 літера на 1 permission).
        public List<MonoAccount> accounts { get; set; } = new List<MonoAccount>();
        public List<MonoJar> jars { get; set; } = new List<MonoJar>();  //Перелік банок
    }
}
