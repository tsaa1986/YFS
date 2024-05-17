using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.Core.Models.MonoIntegration
{
    public class MonoAccount
    {
        public string id { get; set; } = null!;
        public string sendId { get; set; } = null!;  // Ідентифікатор для сервісу https://send.monobank.ua/{sendId}
        public long balance { get; set; }   //  Баланс рахунку в мінімальних одиницях валюти (копійках, центах)
        public long creditLimit { get; set; }
        public string type { get; set; } = null!;   //	Enum: "black" "white" "platinum" "iron" "fop" "yellow" "eAid"
        public int currencyCode { get; set; }   //  Код валюти рахунку відповідно ISO 4217
        public string cashbackType { get; set; } = null!;    //Enum: "None" "UAH" "Miles"   Тип кешбеку який нараховується на рахунок
        public List<string>? maskedPan { get; set; }
        public string iban { get; set; } = null!;
    }
}
