using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public class BankData : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.HasData(
            new Bank
            {
                GLMFO = 351005,
                NameEnglish = "JOINT STOCK COMPANY \"UKRSIBBANK\"",
                CodeEDRPOU = "09807750",
                ShortName = "АТ \"УКРСИББАНК\"",
                FullName = "АКЦІОНЕРНЕ ТОВАРИСТВО \"УКРСИББАНК\"",
                NKB = "136",
                Type = 0,
                KU = 26,
                NOBL = "м.Київ",
                OBLUR = 26,
                NOBLUR = "м.Київ",
                PostalIndex = "04070",
                TNP = "м.",
                NP = "Київ",
                Address = "вулиця Андріївська, 2/12",
                Status = 1,
                StatusName = "Нормальний",
                OpenDate = DateTime.SpecifyKind(DateTime.ParseExact("28.10.1991", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc),
                IDNBU = "351005",
                LicenseNumber = 75,
                GrantLicenseDate = DateTime.SpecifyKind(DateTime.ParseExact("05.10.2011", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc),
                LicenseStatus = 1,
                LicenseStatusDescription = "чинна банківська ліцензія",
                LicenseDate = DateTime.SpecifyKind(DateTime.ParseExact("05.08.2021", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc),
                ShortNameEnglish = "JSС \"UKRSIBBANK\"",
                GroupSpecial = "SV",
                GroupSpecialDate = DateTime.SpecifyKind(DateTime.ParseExact("01.01.2023", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc),
            },
            new Bank
            {
                GLMFO = 351254,
                NameEnglish = "JOINT STOCK COMPANY \"SKY BANK\"",
                CodeEDRPOU = "09620081",
                ShortName = "АТ \"СКАЙ БАНК\"",
                FullName = "АКЦІОНЕРНЕ ТОВАРИСТВО \"СКАЙ БАНК\"",
                NKB = "128",
                Type = 0,
                KU = 26,
                NOBL = "м.Київ",
                OBLUR = 26,
                NOBLUR = "м.Київ",
                PostalIndex = "01054",
                TNP = "м.",
                NP = "Київ",
                Address = "вул. Гончара Олеся, буд. 76/2",
                Status = 1,
                StatusName = "Нормальний",
                OpenDate = DateTime.SpecifyKind(DateTime.ParseExact("28.10.1991", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc),
                IDNBU = "351254",
                LicenseNumber = 32,
                GrantLicenseDate = DateTime.SpecifyKind(DateTime.ParseExact("19.06.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc),
                LicenseStatus = 1,
                LicenseStatusDescription = "чинна банківська ліцензія",
                LicenseDate = DateTime.SpecifyKind(DateTime.ParseExact("05.08.2021", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc),
                ShortNameEnglish = "JSC \"SKY BANK\"",
                GroupSpecial = "B",
                GroupSpecialDate = null
            }
            );
        }
    }
}
