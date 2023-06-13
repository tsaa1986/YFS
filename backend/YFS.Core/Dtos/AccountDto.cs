using YFS.Core.Models;

namespace YFS.Core.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public int AccountStatus { get; set; }
        public int Favorites { get; set; }
        public int AccountGroupId { get; set; }
        public int AccountTypeId { get; set; }
        public int CurrencyId { get; set; }
        public int BankId { get; set; }
        public string? Name { get; set; }
        public DateTime OpeningDate { get; set; }
        public string? Note { get; set; }
        public decimal Balance { get; set; }
    }
}
