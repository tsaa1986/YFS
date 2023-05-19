﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YFS.Repo.Data;

#nullable disable

namespace YFS.Repo.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    partial class RepositoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("YFS.Core.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AccountId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountGroupId")
                        .HasColumnType("int");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("int");

                    b.Property<int>("AccountTypeId")
                        .HasColumnType("int");

                    b.Property<int>("BalanceId")
                        .HasColumnType("int");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("Favorites")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("0");

                    b.Property<string>("IBAN")
                        .HasMaxLength(40)
                        .HasColumnType("VARCHAR(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("Note")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR(255)");

                    b.Property<DateTime>("OpeningDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AccountGroupId");

                    b.HasIndex("AccountTypeId");

                    b.HasIndex("BankId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValueSql("0.0");

                    b.Property<DateTime>("LastUpdateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("AccountsBalance");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountGroup", b =>
                {
                    b.Property<int>("AccountGroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountGroupId"), 1L, 1);

                    b.Property<string>("AccountGroupNameEn")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("AccountGroupNameRu")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("AccountGroupNameUa")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<int>("GroupOrderBy")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AccountGroupId");

                    b.HasIndex("UserId", "AccountGroupNameEn")
                        .IsUnique();

                    b.HasIndex("UserId", "AccountGroupNameRu")
                        .IsUnique();

                    b.HasIndex("UserId", "AccountGroupNameUa")
                        .IsUnique();

                    b.ToTable("AccountGroups");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountMonthlyBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("ClosingMonthBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MonthCredit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MonthDebit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("OpeningMonthBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("YearNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountsMonthlyBalance");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountType", b =>
                {
                    b.Property<int>("AccountTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountTypeId"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("VARCHAR(30)");

                    b.Property<string>("NameRu")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("VARCHAR(30)");

                    b.Property<string>("NameUa")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("VARCHAR(30)");

                    b.Property<string>("NoteEn")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("NoteRu")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("NoteUa")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR(255)");

                    b.Property<int>("TypeOrederBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("0");

                    b.HasKey("AccountTypeId");

                    b.ToTable("AccountTypes");

                    b.HasData(
                        new
                        {
                            AccountTypeId = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NameEn = "Cash",
                            NameRu = "Наличные деньги",
                            NameUa = "Готівкові гроші",
                            NoteRu = "Учет наличных средств",
                            TypeOrederBy = 0
                        },
                        new
                        {
                            AccountTypeId = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NameEn = "Internet-money",
                            NameRu = "Интернет-деньги",
                            NameUa = "Інтернет-гроші",
                            NoteRu = "Интернет счета",
                            TypeOrederBy = 0
                        },
                        new
                        {
                            AccountTypeId = 3,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NameEn = "Deposit",
                            NameRu = "Депозит",
                            NameUa = "Депозит",
                            NoteRu = "Учет реальных депозитов",
                            TypeOrederBy = 0
                        },
                        new
                        {
                            AccountTypeId = 4,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            NameEn = "Bank account",
                            NameRu = "Банковский счет",
                            NameUa = "Банківський рахунок",
                            NoteRu = "Банковский счет",
                            TypeOrederBy = 0
                        });
                });

            modelBuilder.Entity("YFS.Core.Models.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BankId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR(255)");

                    b.HasKey("Id");

                    b.ToTable("Banks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Demo Bank"
                        });
                });

            modelBuilder.Entity("YFS.Core.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CategoryId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name_ENG")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Name_RU")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Name_UA")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Note")
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<int>("RootId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name_ENG = "Money Transfer",
                            Name_RU = "Перевод",
                            Name_UA = "Переказ",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 1,
                            Name_ENG = "Wages",
                            Name_RU = "Халтура",
                            Name_UA = "Халтура",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 2,
                            Name_ENG = "Salary",
                            Name_RU = "Зарплата",
                            Name_UA = "Зарплата",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 3,
                            Name_ENG = "Vacation",
                            Name_RU = "Отдых",
                            Name_UA = "Відпочинок",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 4,
                            Name_ENG = "Loans",
                            Name_RU = "Долги",
                            Name_UA = "Борги",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 5,
                            Name_ENG = "Food",
                            Name_RU = "Продукти питания",
                            Name_UA = "Продукти харчування",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 6,
                            Name_ENG = "Healthcare",
                            Name_RU = "Медицинские расходы",
                            Name_UA = "Медичні витрати",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 7,
                            Name_ENG = "Food",
                            Name_RU = "Продукти питания",
                            Name_UA = "Продукти харчування",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 8,
                            Name_ENG = "Education",
                            Name_RU = "Образование",
                            Name_UA = "Освіта",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 9,
                            Name_ENG = "Other Income",
                            Name_RU = "Другие доходы",
                            Name_UA = "Інші прибутки",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 10,
                            Name_ENG = "Communal payments",
                            Name_RU = "Коммунальные платежи",
                            Name_UA = "Комунальні платежі",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 11,
                            Name_ENG = "Clothing",
                            Name_RU = "Одежда",
                            Name_UA = "Одяг",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 12,
                            Name_ENG = "Personal Care",
                            Name_RU = "Личная гигиена",
                            Name_UA = "Особиста гігієна",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 13,
                            Name_ENG = "Household",
                            Name_RU = "Хозяйственные расходы",
                            Name_UA = "Побутові видатки",
                            Note = "",
                            RootId = 0
                        },
                        new
                        {
                            Id = 14,
                            Name_ENG = "Improvements",
                            Name_RU = "Улучшения",
                            Name_UA = "Покращення",
                            Note = "",
                            RootId = 13
                        },
                        new
                        {
                            Id = 15,
                            Name_ENG = "Furnishings",
                            Name_RU = "Мебель",
                            Name_UA = "Меблі",
                            Note = "",
                            RootId = 13
                        },
                        new
                        {
                            Id = 16,
                            Name_ENG = "Electronics",
                            Name_RU = "Електроника",
                            Name_UA = "Електроніка",
                            Note = "",
                            RootId = 13
                        });
                });

            modelBuilder.Entity("YFS.Core.Models.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Name_en")
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Name_ru")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Name_ua")
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("ShortNameUs")
                        .HasMaxLength(10)
                        .HasColumnType("VARCHAR(10)");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            CurrencyId = 36,
                            Name_en = "Australian dollar",
                            Name_ru = "Австралийский доллар",
                            Name_ua = "Австралійський Долар",
                            ShortNameUs = "AUD"
                        },
                        new
                        {
                            CurrencyId = 980,
                            Name_en = "Hryvnia",
                            Name_ru = "Гривня",
                            Name_ua = "Гривня",
                            ShortNameUs = "UAH"
                        },
                        new
                        {
                            CurrencyId = 840,
                            Name_en = "US Dollar",
                            Name_ru = "Доллар США",
                            Name_ua = "Долар США",
                            ShortNameUs = "USD"
                        },
                        new
                        {
                            CurrencyId = 978,
                            Name_en = "Euro",
                            Name_ru = "Евро",
                            Name_ua = "Євро",
                            ShortNameUs = "EUR"
                        });
                });

            modelBuilder.Entity("YFS.Core.Models.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("CashbackAmount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal>("CurrencyAmount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int?>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR(200)");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("MCC")
                        .HasColumnType("int");

                    b.Property<decimal>("OperationAmount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("OperationCurrencyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OperationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Tag")
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR(200)");

                    b.Property<int>("TransferOperationId")
                        .HasColumnType("int");

                    b.Property<int>("TypeOperation")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("YFS.Core.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("YFS.Core.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("YFS.Core.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YFS.Core.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("YFS.Core.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YFS.Core.Models.Account", b =>
                {
                    b.HasOne("YFS.Core.Models.AccountGroup", null)
                        .WithMany("Accounts")
                        .HasForeignKey("AccountGroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("YFS.Core.Models.AccountType", null)
                        .WithMany("Accounts")
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("YFS.Core.Models.Bank", "Bank")
                        .WithMany("Accounts")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("YFS.Core.Models.Currency", "Currency")
                        .WithMany("Accounts")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("YFS.Core.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("Currency");

                    b.Navigation("User");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountBalance", b =>
                {
                    b.HasOne("YFS.Core.Models.Account", "Account")
                        .WithOne("AccountBalance")
                        .HasForeignKey("YFS.Core.Models.AccountBalance", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountGroup", b =>
                {
                    b.HasOne("YFS.Core.Models.User", "User")
                        .WithMany("AccountsGroup")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountMonthlyBalance", b =>
                {
                    b.HasOne("YFS.Core.Models.Account", null)
                        .WithMany("AccountsMonthlyBalance")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YFS.Core.Models.Operation", b =>
                {
                    b.HasOne("YFS.Core.Models.Account", "Account")
                        .WithMany("Operations")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YFS.Core.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YFS.Core.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.Navigation("Account");

                    b.Navigation("Category");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("YFS.Core.Models.Account", b =>
                {
                    b.Navigation("AccountBalance")
                        .IsRequired();

                    b.Navigation("AccountsMonthlyBalance");

                    b.Navigation("Operations");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountGroup", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("YFS.Core.Models.AccountType", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("YFS.Core.Models.Bank", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("YFS.Core.Models.Currency", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("YFS.Core.Models.User", b =>
                {
                    b.Navigation("AccountsGroup");
                });
#pragma warning restore 612, 618
        }
    }
}
