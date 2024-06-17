using static YFS.Core.Utilities.UnixTimeExtension;

namespace YFS.Core.Models.MonoIntegration
{
    public class MonoTransaction
    {
        public string Id { get; set; } = null!;
        public long Time { get; set; }  //Час транзакції в секундах в форматі Unix time
        public string? Description { get; set; } //Опис транзакцій
        public int Mcc { get; set; }    //Код типу транзакції (Merchant Category Code), відповідно ISO 18245
        public int OriginalMcc { get; set; }    //Оригінальний код типу транзакції (Merchant Category Code), відповідно ISO 18245
        public bool Hold { get; set; }  //Статус блокування суми
        public long Amount { get; set; }    //Сума у валюті рахунку в мінімальних одиницях валюти (копійках, центах)
        public long OperationAmount { get; set; }   //Сума у валюті транзакції в мінімальних одиницях валюти (копійках, центах)
        public int CurrencyCode { get; set; }   //Код валюти рахунку відповідно ISO 4217
        public long CommissionRate { get; set; }    //Розмір комісії в мінімальних одиницях валюти (копійках, центах)
        public long CashbackAmount { get; set; }    //Розмір кешбеку в мінімальних одиницях валюти (копійках, центах)
        public long Balance { get; set; }   //Баланс рахунку в мінімальних одиницях валюти (копійках, центах)
        public string? Comment { get; set; } //Коментар до переказу, уведений користувачем. Якщо не вказаний, поле буде відсутнім
        public string? ReceiptId { get; set; }   //Номер квитанції для check.gov.ua. Поле може бути відсутнім
        public string? InvoiceId { get; set; }   //Номер квитанції ФОПа, приходить у випадку якщо це операція із зарахуванням коштів
        public string? CounterEdrpou { get; set; }   //ЄДРПОУ контрагента, присутній лише для елементів виписки рахунків ФОП
        public string? CounterIban { get; set; } //IBAN контрагента, присутній лише для елементів виписки рахунків ФОП
        public string? CounterName { get; set; }     //Найменування контрагента
        public decimal AmountCalculated => Amount == 0 ? 0 : Amount / 100.0m;
        public decimal OperationAmountCalculated => OperationAmount == 0 ? 0 : OperationAmount / 100.0m;
        public decimal CashbackAmountCalculated => CashbackAmount == 0 ? 0 : CashbackAmount / 100.0m;
        public decimal BalanceCalculated => Balance == 0 ? 0 : Balance / 100.0m;
        public DateTime OperationDate => UnixTimeExtensions.FromUnixTimeSecondsToDateTimeUtc(Time);
        public bool ImportSuccessful { get; set; }  // Flag indicating if import was successful
        public List<Operation> OperationList { get; set; } = new List<Operation>();  // List to save successful operations
    }
}
