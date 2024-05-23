using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public static partial class DatabaseInitializer
    {
        private static void InitializeAccountTypes(RepositoryContext context)
        {
            if (!context.AccountTypes.Any())
            {
                var accountTypes = new List<AccountType>()
                {
                    new AccountType { AccountTypeId = 1, TypeOrderBy = 1, CreatedOn = DateTime.UtcNow, IsEnabled = true },
                    new AccountType { AccountTypeId = 2, TypeOrderBy = 2, CreatedOn = DateTime.UtcNow, IsEnabled = true },
                    new AccountType { AccountTypeId = 3, TypeOrderBy = 3, CreatedOn = DateTime.UtcNow, IsEnabled = true },
                    new AccountType { AccountTypeId = 4, TypeOrderBy = 4, CreatedOn = DateTime.UtcNow, IsEnabled = true },
                    new AccountType { AccountTypeId = 5, TypeOrderBy = 5, CreatedOn = DateTime.UtcNow, IsEnabled = true },
                    new AccountType { AccountTypeId = 6, TypeOrderBy = 6, CreatedOn = DateTime.UtcNow, IsEnabled = false },
                    new AccountType { AccountTypeId = 7, TypeOrderBy = 7, CreatedOn = DateTime.UtcNow, IsEnabled = false },
                    new AccountType { AccountTypeId = 8, TypeOrderBy = 8, CreatedOn = DateTime.UtcNow, IsEnabled = true },
                    new AccountType { AccountTypeId = 9, TypeOrderBy = 9, CreatedOn = DateTime.UtcNow, IsEnabled = true },
                    new AccountType { AccountTypeId = 10, TypeOrderBy = 10, CreatedOn = DateTime.UtcNow, IsEnabled = true },
                    new AccountType { AccountTypeId = 11, TypeOrderBy = 11, CreatedOn = DateTime.UtcNow, IsEnabled = false },
                    new AccountType { AccountTypeId = 12, TypeOrderBy = 12, CreatedOn = DateTime.UtcNow, IsEnabled = false },
                    new AccountType { AccountTypeId = 13, TypeOrderBy = 13, CreatedOn = DateTime.UtcNow, IsEnabled = true }
                };
                var accountTypeTranslations = new List<AccountTypeTranslation>()
                {
                    // Savings Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 1, AccountTypeId = 1, Language = "en", Name = "Savings", Description = "Savings Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 2, AccountTypeId = 1, Language = "uk", Name = "Зберігання", Description = "Зберігання рахунку" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 3, AccountTypeId = 1, Language = "ru", Name = "Сбережения", Description = "Счет для сбережений" },

                    // Checking Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 4, AccountTypeId = 2, Language = "en", Name = "Checking", Description = "Checking Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 5, AccountTypeId = 2, Language = "uk", Name = "Поточний рахунок", Description = "Поточний рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 6, AccountTypeId = 2, Language = "ru", Name = "Текущий счет", Description = "Текущий счет" },

                    // Credit Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 7, AccountTypeId = 3, Language = "en", Name = "Credit", Description = "Credit Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 8, AccountTypeId = 3, Language = "uk", Name = "Кредитний", Description = "Кредитний рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 9, AccountTypeId = 3, Language = "ru", Name = "Кредитный", Description = "Кредитный счет" },

                    // Investment Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 10, AccountTypeId = 4, Language = "en", Name = "Investment", Description = "Investment Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 11, AccountTypeId = 4, Language = "uk", Name = "Інвестиції", Description = "Інвестиційний рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 12, AccountTypeId = 4, Language = "ru", Name = "Инвестиции", Description = "Инвестиционный счет" },

                    // Loan Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 13, AccountTypeId = 5, Language = "en", Name = "Loan", Description = "Loan Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 14, AccountTypeId = 5, Language = "uk", Name = "Кредит", Description = "Кредитний рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 15, AccountTypeId = 5, Language = "ru", Name = "Кредит", Description = "Кредитный счет" },

                    // Joint Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 16, AccountTypeId = 6, Language = "en", Name = "Joint", Description = "Joint Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 17, AccountTypeId = 6, Language = "uk", Name = "Спільний", Description = "Спільний рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 18, AccountTypeId = 6, Language = "ru", Name = "Совместный", Description = "Совместный счет" },

                    // Business Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 19, AccountTypeId = 7, Language = "en", Name = "Business", Description = "Business Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 20, AccountTypeId = 7, Language = "uk", Name = "Бізнес", Description = "Бізнес рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 21, AccountTypeId = 7, Language = "ru", Name = "Бизнес", Description = "Бизнес счет" },

                    // Fixed Deposit Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 22, AccountTypeId = 8, Language = "en", Name = "FixedDeposit", Description = "Fixed Deposit Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 23, AccountTypeId = 8, Language = "uk", Name = "Терміновий вклад", Description = "Рахунок строкового вкладу" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 24, AccountTypeId = 8, Language = "ru", Name = "Срочный вклад", Description = "Счет срочного вклада" },

                    // Retirement Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 25, AccountTypeId = 9, Language = "en", Name = "Retirement", Description = "Retirement Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 26, AccountTypeId = 9, Language = "uk", Name = "Пенсійний", Description = "Пенсійний рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 27, AccountTypeId = 9, Language = "ru", Name = "Пенсионный", Description = "Пенсионный счет" },

                    // Custodial Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 28, AccountTypeId = 10, Language = "en", Name = "Custodial", Description = "Custodial Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 29, AccountTypeId = 10, Language = "uk", Name = "Зберігання", Description = "Рахунок зберігання" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 30, AccountTypeId = 10, Language = "ru", Name = "Хранение", Description = "Счет хранения" },

                    // Trust Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 31, AccountTypeId = 11, Language = "en", Name = "Trust", Description = "Trust Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 32, AccountTypeId = 11, Language = "uk", Name = "Трастовий", Description = "Трастовий рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 33, AccountTypeId = 11, Language = "ru", Name = "Трастовый", Description = "Трастовый счет" },

                    // Forex Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 34, AccountTypeId = 12, Language = "en", Name = "Forex", Description = "Forex Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 35, AccountTypeId = 12, Language = "uk", Name = "Форекс", Description = "Рахунок Форекс" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 36, AccountTypeId = 12, Language = "ru", Name = "Форекс", Description = "Счет Форекс" },

                    // Virtual Account Translations
                    new AccountTypeTranslation { AccountTypeTranslationId = 37, AccountTypeId = 13, Language = "en", Name = "Virtual", Description = "Virtual Account" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 38, AccountTypeId = 13, Language = "uk", Name = "Віртуальний", Description = "Віртуальний рахунок" },
                    new AccountTypeTranslation { AccountTypeTranslationId = 39, AccountTypeId = 13, Language = "ru", Name = "Виртуальный", Description = "Виртуальный счет" }

                };

                context.AccountTypes.AddRange(accountTypes);
                context.AccountTypeTranslations.AddRange(accountTypeTranslations);

                context.SaveChanges();
            } 
        }
    }
}
