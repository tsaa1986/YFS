using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.AccessControl;
using YFS.Core.Models;

namespace YFS.Repo.Data
{
    public class CategoryData : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = -2, RootId = 0, Note = "" },
                new Category { Id = -1, RootId = 0, Note = "" },
                new Category { Id = 1, RootId = 0, Note = "" },
                new Category { Id = 2, RootId = 0, Note = "" },
                new Category { Id = 3, RootId = 0, Note = "" },
                new Category { Id = 4, RootId = 0, Note = "" },
                new Category { Id = 5, RootId = 0, Note = "" },
                new Category { Id = 6, RootId = 0, Note = "" },
                new Category { Id = 8, RootId = 0, Note = "" },
                new Category { Id = 9, RootId = 0, Note = "" },
                new Category { Id = 10, RootId = 0, Note = "" },
                new Category { Id = 11, RootId = 0, Note = "" },
                new Category { Id = 12, RootId = 0, Note = "" },
                new Category { Id = 13, RootId = 0, Note = "" },
                new Category { Id = 14, RootId = 13, Note = "" },
                new Category { Id = 15, RootId = 13, Note = "" },
                new Category { Id = 16, RootId = 13, Note = "" });

            builder.OwnsMany(c => c.Translations, t =>
            {
                t.HasData(
                    new CategoryTranslation { Id = 1, CategoryId = -2, LanguageCode = "ua", Name = "Корегування балансу рахунка" },
                    new CategoryTranslation { Id = 2, CategoryId = -2, LanguageCode = "en", Name = "Account Balance adjustment" },
                    new CategoryTranslation { Id = 3, CategoryId = -2, LanguageCode = "ru", Name = "Корректировка баланса счета" },
                    new CategoryTranslation { Id = 4, CategoryId = -1, LanguageCode = "ua", Name = "Переказ" },
                    new CategoryTranslation { Id = 5, CategoryId = -1, LanguageCode = "en", Name = "Money Transfer" },
                    new CategoryTranslation { Id = 6, CategoryId = -1, LanguageCode = "ru", Name = "Перевод" },
                    new CategoryTranslation { Id = 7, CategoryId = 1, LanguageCode = "ua", Name = "Халтура" },
                    new CategoryTranslation { Id = 8, CategoryId = 1, LanguageCode = "en", Name = "Wages" },
                    new CategoryTranslation { Id = 9, CategoryId = 1, LanguageCode = "ru", Name = "Халтура" },
                    new CategoryTranslation { Id = 10, CategoryId = 2, LanguageCode = "ua", Name = "Зарплата" },
                    new CategoryTranslation { Id = 11, CategoryId = 2, LanguageCode = "en", Name = "Salary" },
                    new CategoryTranslation { Id = 12, CategoryId = 2, LanguageCode = "ru", Name = "Зарплата" },
                    new CategoryTranslation { Id = 13, CategoryId = 3, LanguageCode = "ua", Name = "Відпочинок" },
                    new CategoryTranslation { Id = 14, CategoryId = 3, LanguageCode = "en", Name = "Vacation" },
                    new CategoryTranslation { Id = 15, CategoryId = 3, LanguageCode = "ru", Name = "Отдых" },
                    new CategoryTranslation { Id = 16, CategoryId = 4, LanguageCode = "ua", Name = "Борги" },
                    new CategoryTranslation { Id = 17, CategoryId = 4, LanguageCode = "en", Name = "Loans" },
                    new CategoryTranslation { Id = 18, CategoryId = 4, LanguageCode = "ru", Name = "Долги" },
                    new CategoryTranslation { Id = 19, CategoryId = 5, LanguageCode = "ua", Name = "Продукти харчування" },
                    new CategoryTranslation { Id = 20, CategoryId = 5, LanguageCode = "en", Name = "Food" },
                    new CategoryTranslation { Id = 21, CategoryId = 5, LanguageCode = "ru", Name = "Продукти питания" },
                    new CategoryTranslation { Id = 22, CategoryId = 6, LanguageCode = "ua", Name = "Медичні витрати" },
                    new CategoryTranslation { Id = 23, CategoryId = 6, LanguageCode = "en", Name = "Healthcare" },
                    new CategoryTranslation { Id = 24, CategoryId = 6, LanguageCode = "ru", Name = "Медицинские расходы" },
                    new CategoryTranslation { Id = 25, CategoryId = 8, LanguageCode = "ua", Name = "Освіта" },
                    new CategoryTranslation { Id = 26, CategoryId = 8, LanguageCode = "en", Name = "Education" },
                    new CategoryTranslation { Id = 27, CategoryId = 8, LanguageCode = "ru", Name = "Образование" },
                    new CategoryTranslation { Id = 28, CategoryId = 9, LanguageCode = "ua", Name = "Інші прибутки" },
                    new CategoryTranslation { Id = 29, CategoryId = 9, LanguageCode = "en", Name = "Other Income" },
                    new CategoryTranslation { Id = 30, CategoryId = 9, LanguageCode = "ru", Name = "Другие доходы" },
                    new CategoryTranslation { Id = 31, CategoryId = 10, LanguageCode = "ua", Name = "Комунальні платежі" },
                    new CategoryTranslation { Id = 32, CategoryId = 10, LanguageCode = "en", Name = "Communal payments" },
                    new CategoryTranslation { Id = 33, CategoryId = 10, LanguageCode = "ru", Name = "Коммунальные платежи" },
                    new CategoryTranslation { Id = 34, CategoryId = 11, LanguageCode = "ua", Name = "Одяг" },
                    new CategoryTranslation { Id = 35, CategoryId = 11, LanguageCode = "en", Name = "Clothing" },
                    new CategoryTranslation { Id = 36, CategoryId = 11, LanguageCode = "ru", Name = "Одежда" },
                    new CategoryTranslation { Id = 37, CategoryId = 12, LanguageCode = "ua", Name = "Особиста гігієна" },
                    new CategoryTranslation { Id = 38, CategoryId = 12, LanguageCode = "en", Name = "Personal Care" },
                    new CategoryTranslation { Id = 39, CategoryId = 12, LanguageCode = "ru", Name = "Личная гигиена" },
                    new CategoryTranslation { Id = 40, CategoryId = 13, LanguageCode = "ua", Name = "Побутові видатки" },
                    new CategoryTranslation { Id = 41, CategoryId = 13, LanguageCode = "en", Name = "Household" },
                    new CategoryTranslation { Id = 42, CategoryId = 13, LanguageCode = "ru", Name = "Хозяйственные расходы" },
                    new CategoryTranslation { Id = 43, CategoryId = 14, LanguageCode = "ua", Name = "Покращення" },
                    new CategoryTranslation { Id = 44, CategoryId = 14, LanguageCode = "en", Name = "Improvements" },
                    new CategoryTranslation { Id = 45, CategoryId = 14, LanguageCode = "ru", Name = "Улучшения" },
                    new CategoryTranslation { Id = 46, CategoryId = 15, LanguageCode = "ua", Name = "Меблі" },
                    new CategoryTranslation { Id = 47, CategoryId = 15, LanguageCode = "en", Name = "Furnishings" },
                    new CategoryTranslation { Id = 48, CategoryId = 15, LanguageCode = "ru", Name = "Мебель" },
                    new CategoryTranslation { Id = 49, CategoryId = 16, LanguageCode = "ua", Name = "Електроніка" },
                    new CategoryTranslation { Id = 50, CategoryId = 16, LanguageCode = "en", Name = "Electronics" },
                    new CategoryTranslation { Id = 51, CategoryId = 16, LanguageCode = "ru", Name = "Електроника" }
                );
            });
        }
    }
}
