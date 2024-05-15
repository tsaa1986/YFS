using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models.MonoIntegration
{
    public class MonoJar
    {
        public string id { get; set; }
        public string sendId { get; set; }  //	Ідентифікатор для сервісу https://send.monobank.ua/{sendId}
        public string title { get; set; }   //  Назва банки
        public string description { get; set; } //
        public int currencyCode { get; set; }   //  Код валюти банки відповідно ISO 4217
        public long balance { get; set; }   //Баланс банки в мінімальних одиницях валюти (копійках, центах)
        public long goal { get; set; }  //Цільова сума для накопичення в банці в мінімальних одиницях валюти (копійках, центах)
    }
}
