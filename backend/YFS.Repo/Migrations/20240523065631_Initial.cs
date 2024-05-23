using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YFS.Repo.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    TypeOrderBy = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "0"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.AccountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    GLMFO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME_E = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    KOD_EDRPOU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SHORTNAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FULLNAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    NKB = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TYP = table.Column<int>(type: "integer", nullable: false),
                    KU = table.Column<int>(type: "integer", nullable: false),
                    N_OBL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OBL_UR = table.Column<int>(type: "integer", nullable: false),
                    N_OBL_UR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    P_IND = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TNP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NP = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ADRESS = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TELEFON = table.Column<string>(type: "text", nullable: true),
                    KSTAN = table.Column<int>(type: "integer", nullable: false),
                    N_STAN = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    D_STAN = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    D_OPEN = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    D_CLOSE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IDNBU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NUM_LIC = table.Column<int>(type: "integer", nullable: false),
                    DT_GRAND_LIC = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PR_LIC = table.Column<int>(type: "integer", nullable: false),
                    N_PR_LIC = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DT_LIC = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SHORTNAME_EN = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    GR_SP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    D_GR_SP = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.GLMFO);
                });

            migrationBuilder.CreateTable(
                name: "BankSyncHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankSyncHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RootId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Name_UA = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    Name_ENG = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    Name_RU = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    Note = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 3, nullable: false),
                    Country = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    Name_en = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    Name_ru = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: true),
                    Name_ua = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: true),
                    Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "Mccs",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mccs", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "AccountTypeTranslations",
                columns: table => new
                {
                    AccountTypeTranslationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypeTranslations", x => x.AccountTypeTranslationId);
                    table.ForeignKey(
                        name: "FK_AccountTypeTranslations_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "AccountTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountGroups",
                columns: table => new
                {
                    AccountGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    AccountGroupNameRu = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    AccountGroupNameEn = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    AccountGroupNameUa = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    GroupOrderBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroups", x => x.AccountGroupId);
                    table.ForeignKey(
                        name: "FK_AccountGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TokenType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TokenValue = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    AccountIsEnabled = table.Column<int>(type: "integer", nullable: false),
                    IBAN = table.Column<string>(type: "VARCHAR", maxLength: 40, nullable: true),
                    Favorites = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "0"),
                    AccountGroupId = table.Column<int>(type: "integer", nullable: false),
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    Bank_GLMFO = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "AccountGroupId");
                    table.ForeignKey(
                        name: "FK_Accounts_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "AccountTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Banks_Bank_GLMFO",
                        column: x => x.Bank_GLMFO,
                        principalTable: "Banks",
                        principalColumn: "GLMFO");
                    table.ForeignKey(
                        name: "FK_Accounts_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId");
                });

            migrationBuilder.CreateTable(
                name: "AccountsBalance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValueSql: "0.0"),
                    LastUpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsBalance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsBalance_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountsMonthlyBalance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    StartDateOfMonth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MonthNumber = table.Column<int>(type: "integer", nullable: false),
                    YearNumber = table.Column<int>(type: "integer", nullable: false),
                    MonthDebit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MonthCredit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OpeningMonthBalance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ClosingMonthBalance = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsMonthlyBalance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsMonthlyBalance_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferOperationId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    TypeOperation = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    OperationCurrencyId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: true),
                    CurrencyAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    OperationAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CashbackAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    MCC = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR", maxLength: 200, nullable: true),
                    Tag = table.Column<string>(type: "VARCHAR", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId");
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "GLMFO", "ADRESS", "D_CLOSE", "KOD_EDRPOU", "D_STAN", "FULLNAME", "DT_GRAND_LIC", "GR_SP", "D_GR_SP", "IDNBU", "KU", "DT_LIC", "NUM_LIC", "PR_LIC", "N_PR_LIC", "NKB", "N_OBL", "N_OBL_UR", "NP", "NAME_E", "OBL_UR", "D_OPEN", "P_IND", "SHORTNAME", "SHORTNAME_EN", "KSTAN", "N_STAN", "TNP", "TELEFON", "TYP" },
                values: new object[,]
                {
                    { 351005, "вулиця Андріївська, 2/12", null, "09807750", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"УКРСИББАНК\"", new DateTime(2011, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "351005", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 75, 1, "чинна банківська ліцензія", "136", "м.Київ", "м.Київ", "Київ", "JOINT STOCK COMPANY \"UKRSIBBANK\"", 26, new DateTime(1991, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), "04070", "АТ \"УКРСИББАНК\"", "JSС \"UKRSIBBANK\"", 1, "Нормальний", "м.", null, 0 },
                    { 351254, "вул. Гончара Олеся, буд. 76/2", null, "09620081", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"СКАЙ БАНК\"", new DateTime(2018, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), "B", null, "351254", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 32, 1, "чинна банківська ліцензія", "128", "м.Київ", "м.Київ", "Київ", "JOINT STOCK COMPANY \"SKY BANK\"", 26, new DateTime(1991, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), "01054", "АТ \"СКАЙ БАНК\"", "JSC \"SKY BANK\"", 1, "Нормальний", "м.", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name_ENG", "Name_RU", "Name_UA", "Note", "RootId", "UserId" },
                values: new object[,]
                {
                    { -2, "Account Balance adjustment", "Корректировка баланса счета", "Корегування балансу рахунка", "", 0, null },
                    { -1, "Money Transfer", "Перевод", "Переказ", "", 0, null },
                    { 1, "Wages", "Халтура", "Халтура", "", 0, null },
                    { 2, "Salary", "Зарплата", "Зарплата", "", 0, null },
                    { 3, "Vacation", "Отдых", "Відпочинок", "", 0, null },
                    { 4, "Loans", "Долги", "Борги", "", 0, null },
                    { 5, "Food", "Продукти питания", "Продукти харчування", "", 0, null },
                    { 6, "Healthcare", "Медицинские расходы", "Медичні витрати", "", 0, null },
                    { 8, "Education", "Образование", "Освіта", "", 0, null },
                    { 9, "Other Income", "Другие доходы", "Інші прибутки", "", 0, null },
                    { 10, "Communal payments", "Коммунальные платежи", "Комунальні платежі", "", 0, null },
                    { 11, "Clothing", "Одежда", "Одяг", "", 0, null },
                    { 12, "Personal Care", "Личная гигиена", "Особиста гігієна", "", 0, null },
                    { 13, "Household", "Хозяйственные расходы", "Побутові видатки", "", 0, null },
                    { 14, "Improvements", "Улучшения", "Покращення", "", 13, null },
                    { 15, "Furnishings", "Мебель", "Меблі", "", 13, null },
                    { 16, "Electronics", "Електроника", "Електроніка", "", 13, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameEn",
                table: "AccountGroups",
                columns: new[] { "UserId", "AccountGroupNameEn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameRu",
                table: "AccountGroups",
                columns: new[] { "UserId", "AccountGroupNameRu" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameUa",
                table: "AccountGroups",
                columns: new[] { "UserId", "AccountGroupNameUa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountGroupId",
                table: "Accounts",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Bank_GLMFO",
                table: "Accounts",
                column: "Bank_GLMFO");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CurrencyId",
                table: "Accounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsBalance_AccountId",
                table: "AccountsBalance",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountsMonthlyBalance_AccountId",
                table: "AccountsMonthlyBalance",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTypeTranslations_AccountTypeId",
                table: "AccountTypeTranslations",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiTokens_UserId",
                table: "ApiTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AccountId",
                table: "Operations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CategoryId",
                table: "Operations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CurrencyId",
                table: "Operations",
                column: "CurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsBalance");

            migrationBuilder.DropTable(
                name: "AccountsMonthlyBalance");

            migrationBuilder.DropTable(
                name: "AccountTypeTranslations");

            migrationBuilder.DropTable(
                name: "ApiTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BankSyncHistories");

            migrationBuilder.DropTable(
                name: "Mccs");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AccountGroups");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
