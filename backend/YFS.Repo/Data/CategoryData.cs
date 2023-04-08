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
                new Category {
                    Id = 1,
                    RootId = 0,
                    Name_UA = "Халтура",     
                    Name_ENG = "Wages",
                    Name_RU = "Халтура",
                    Note = ""
                },
                new Category
                {
                    Id = 2,
                    RootId = 0,
                    Name_UA = "Зарплата",
                    Name_ENG = "Salary",
                    Name_RU = "Зарплата",
                    Note = ""
                },
                new Category
                {
                    Id = 3,
                    RootId = 0,
                    Name_UA = "Відпочинок",
                    Name_ENG = "Vacation",
                    Name_RU = "Отдых",
                    Note = ""
                },
                new Category
                {
                    Id = 4,
                    RootId = 0,
                    Name_UA = "Борги",
                    Name_ENG = "Loans",
                    Name_RU = "Долги",
                    Note = ""
                },
                new Category
                {
                    Id = 5,
                    RootId = 0,
                    Name_UA = "Продукти харчування",
                    Name_ENG = "Food",
                    Name_RU = "Продукти питания",
                    Note = ""
                },
                new Category
                {
                    Id = 6,
                    RootId = 0,
                    Name_UA = "Медичні витрати",
                    Name_ENG = "Healthcare",
                    Name_RU = "Медицинские расходы",
                    Note = ""
                },
                new Category
                {
                    Id = 7,
                    RootId = 0,
                    Name_UA = "Продукти харчування",
                    Name_ENG = "Food",
                    Name_RU = "Продукти питания",
                    Note = ""
                },
                new Category
                {
                    Id = 8,
                    RootId = 0,
                    Name_UA = "Освіта",
                    Name_ENG = "Education",
                    Name_RU = "Образование",
                    Note = ""
                },
                new Category
                {
                    Id = 9,
                    RootId = 0,
                    Name_UA = "Інші прибутки",
                    Name_ENG = "Other Income",
                    Name_RU = "Другие доходы",
                    Note = ""
                },
                new Category
                {
                    Id = 10,
                    RootId = 0,
                    Name_UA = "Комунальні платежі",
                    Name_ENG = "Communal payments",
                    Name_RU = "Коммунальные платежи",
                    Note = ""
                },
                new Category
                {
                    Id = 11,
                    RootId = 0,
                    Name_UA = "Одяг",
                    Name_ENG = "Clothing",
                    Name_RU = "Одежда",
                    Note = ""
                },
                new Category
                {
                    Id = 12,
                    RootId = 0,
                    Name_UA = "Особиста гігієна",
                    Name_ENG = "Personal Care",
                    Name_RU = "Личная гигиена",
                    Note = ""
                },
                new Category
                {
                    Id = 13,
                    RootId = 0,
                    Name_UA = "Побутові видатки",
                    Name_ENG = "Household",
                    Name_RU = "Хозяйственные расходы",
                    Note = ""
                },
                new Category
                {
                    Id = 14,
                    RootId = 13,
                    Name_UA = "Покращення",
                    Name_ENG = "Improvements",
                    Name_RU = "Улучшения",
                    Note = ""
                },
                new Category
                {
                    Id = 15,
                    RootId = 13,
                    Name_UA = "Меблі",
                    Name_ENG = "Furnishings",
                    Name_RU = "Мебель",
                    Note = ""
                },
                new Category
                {
                    Id = 16,
                    RootId = 13,
                    Name_UA = "Електроніка",
                    Name_ENG = "Electronics",
                    Name_RU = "Електроника",
                    Note = ""
                }

                );
        }
    }
}
