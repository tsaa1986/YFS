using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public class CurrencyData : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasData(
                new Currency {
                    Id = 036,
                    ShortNameUs = "AUD",
                    Name_ua = "Австралійський Долар",     
                    Name_ru = "Австралийский доллар",
                    Name_en = "Australian dollar"
                },
                new Currency
                {
                    Id = 980,
                    ShortNameUs = "UAH",
                    Name_ua = "Гривня",
                    Name_ru = "Гривня",
                    Name_en = "Hryvnia"
                },
                new Currency
                {
                    Id = 840,
                    ShortNameUs = "USD",
                    Name_ua = "Долар США",
                    Name_ru = "Доллар США",
                    Name_en = "US Dollar"
                },
                new Currency
                {
                    Id = 978,
                    ShortNameUs = "EUR",
                    Name_ua = "Євро",
                    Name_ru = "Евро",
                    Name_en = "Euro"
                }
                );
        }
    }
}
