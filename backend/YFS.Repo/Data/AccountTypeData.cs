using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public class AccountTypeData : IEntityTypeConfiguration<AccountType>
    {
        public void Configure(EntityTypeBuilder<AccountType> builder)
        {
            builder.HasData(
                new AccountType { TypeId = 1, NameUa = "Готівкові гроші", NameRu = "Наличные деньги", NameEn = "Cash",NoteRu = "Учет наличных средств", TypeOrederBy = 0 },
                new AccountType { TypeId = 2, NameUa = "Інтернет-гроші", NameRu = "Интернет-деньги", NameEn = "Internet-money", NoteRu = "Интернет счета",TypeOrederBy = 0 },
                new AccountType { TypeId = 3, NameUa = "Депозит", NameRu = "Депозит", NameEn = "Deposit", NoteRu = "Учет реальных депозитов", TypeOrederBy = 0 },
                new AccountType { TypeId = 4, NameUa = "Кредитна картка", NameRu = "Кредитная карточка", NameEn = "Credit card", NoteRu = "Кредитные карточки Visa, Mastercard и др.", TypeOrederBy = 0 },
                new AccountType { TypeId = 5, NameUa = "Дебетна картка", NameRu = "Дебетная карта", NameEn = "Debit card", NoteRu = "Дебетовые карты Visa, Mastercard и др.", TypeOrederBy = 0 }
                );
        }
    }
}
