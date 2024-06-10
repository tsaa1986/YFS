using YFS.Core.Models;

namespace YFS.Core.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string ExternalId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int Favorites { get; set; }
        public int AccountIsEnabled { get; set; }
        public int AccountGroupId { get; set; }
        public int AccountTypeId { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; } = null!;
        public int Bank_GLMFO { get; set; }
        public string? Name { get; set; }
        public DateTime OpeningDate { get; set; }
        public string? Note { get; set; }
        public decimal Balance { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
