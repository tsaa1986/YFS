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
                name: "AccountSyncSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    FromSyncDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSuccessSyncDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AdditionalSettings = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSyncSettings", x => x.Id);
                });

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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RootId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
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
                name: "MccCategoryMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MccCode = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MccCategoryMappings", x => x.Id);
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
                name: "MonoSyncRules",
                columns: table => new
                {
                    RuleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RuleName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Condition = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApiTokenId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonoSyncRules", x => x.RuleId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
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
                name: "CategoryTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    LanguageCode = table.Column<string>(type: "VARCHAR", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountGroupTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountGroupId = table.Column<int>(type: "integer", nullable: false),
                    AccountGroupName = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    LanguageCode = table.Column<string>(type: "VARCHAR", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroupTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountGroupTranslation_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "AccountGroupId",
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
                    TypeOperation = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalCurrencyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    OperationCurrencyId = table.Column<int>(type: "integer", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CashbackAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    MCC = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR", maxLength: 200, nullable: true)
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
                        name: "FK_Operations_Currencies_OperationCurrencyId",
                        column: x => x.OperationCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonoSyncedTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MonoTransactionId = table.Column<string>(type: "text", nullable: false),
                    OperationId = table.Column<int>(type: "integer", nullable: false),
                    TransferOperationId = table.Column<int>(type: "integer", nullable: false),
                    SyncedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonoSyncedTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonoSyncedTransaction_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OperationId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    OperationAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationItem_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationItem_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationTags",
                columns: table => new
                {
                    OperationId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTags", x => new { x.OperationId, x.TagId });
                    table.ForeignKey(
                        name: "FK_OperationTags_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "GLMFO", "ADRESS", "D_CLOSE", "KOD_EDRPOU", "D_STAN", "FULLNAME", "DT_GRAND_LIC", "GR_SP", "D_GR_SP", "IDNBU", "KU", "DT_LIC", "NUM_LIC", "PR_LIC", "N_PR_LIC", "NKB", "N_OBL", "N_OBL_UR", "NP", "NAME_E", "OBL_UR", "D_OPEN", "P_IND", "SHORTNAME", "SHORTNAME_EN", "KSTAN", "N_STAN", "TNP", "TELEFON", "TYP" },
                values: new object[,]
                {
                    { 300335, "вулиця Генерала Алмазова, буд. 4а", null, "14305909", null, "Акціонерне товариство \"Райффайзен Банк\"", new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "300335", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 10, 1, "чинна банківська ліцензія", "036", "м.Київ", "м.Київ", "Київ", "Raiffeisen Bank Joint Stock Company", 26, new DateTime(1992, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), "01011", "АТ \"Райффайзен Банк\"", "Raiffeisen Bank JSC", 1, "Нормальний", "м.", null, 0 },
                    { 300465, "вул. Госпітальна, 12г", null, "00032129", null, "акціонерне товариство \"Державний ощадний банк України\"", new DateTime(2011, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "300465", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 148, 1, "чинна банківська ліцензія", "006", "м.Київ", "м.Київ", "Київ", "Joint Stock Company \"State Savings Bank of Ukraine\"", 26, new DateTime(1991, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "01023", "АТ \"Ощадбанк\"", "JSC \"Oschadbank\"", 1, "Нормальний", "м.", null, 0 },
                    { 300528, "вул. Жилянська, 43", null, "21685166", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"ОТП БАНК\"", new DateTime(2011, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "300528", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 191, 1, "чинна банківська ліцензія", "296", "м.Київ", "м.Київ", "Київ", "JOINT-STOCK COMPANY OTP BANK", 26, new DateTime(1998, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "01033", "АТ \"ОТП БАНК\"", "OTP BANK JSC", 1, "Нормальний", "м.", null, 0 },
                    { 300614, "Район: Шевченківський, Місто: Київ, Вулиця: вул. Євгена Чикаленка, Будинок: 42/4", null, "14361575", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"КРЕДІ АГРІКОЛЬ БАНК\"", new DateTime(2011, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "300614", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 99, 1, "чинна банківська ліцензія", "171", "м.Київ", "м.Київ", null, "JOINT-STOCK COMPANY \"CREDIT AGRICOLE BANK\"", 26, new DateTime(1993, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "01004", "АТ \"КРЕДІ АГРІКОЛЬ БАНК\"", "JSC \"CREDIT AGRICOLE BANK\"", 1, "Нормальний", null, "0445810700", 0 },
                    { 305299, "вул. Грушевського, 1Д", null, "14360570", new DateTime(2016, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "акціонерне товариство комерційний банк \"ПриватБанк\"", new DateTime(2011, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "305299", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 22, 1, "чинна банківська ліцензія", "046", "м.Київ", "м.Київ", "Київ", "Joint-Stock Company Commercial Bank \"PrivatBank\"", 26, new DateTime(1992, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "01001", "АТ КБ \"ПриватБанк\"", "JSC CB \"PrivatBank\"", 1, "Нормальний", "м.", null, 0 },
                    { 322001, "вул. Автозаводська,54/19", null, "21133352", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"УНІВЕРСАЛ БАНК\"", new DateTime(2011, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "322001", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 92, 1, "чинна банківська ліцензія", "242", "м.Київ", "м.Київ", "Київ", "JOINT STOCK COMPANY \"UNIVERSAL BANK\"", 26, new DateTime(1994, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "04082", "АТ \"УНІВЕРСАЛ БАНК\"", "JSC \"UNIVERSAL BANK\"", 1, "Нормальний", "м.", null, 0 },
                    { 351005, "вулиця Андріївська, 2/12", null, "09807750", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"УКРСИББАНК\"", new DateTime(2011, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "351005", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 75, 1, "чинна банківська ліцензія", "136", "м.Київ", "м.Київ", "Київ", "JOINT STOCK COMPANY \"UKRSIBBANK\"", 26, new DateTime(1991, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), "04070", "АТ \"УКРСИББАНК\"", "JSС \"UKRSIBBANK\"", 1, "Нормальний", "м.", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Note", "RootId", "UserId" },
                values: new object[,]
                {
                    { -2, "", 0, null },
                    { -1, "", 0, null },
                    { 1, "", 0, null },
                    { 2, "", 0, null },
                    { 3, "", 0, null },
                    { 4, "", 0, null },
                    { 5, "", 0, null },
                    { 6, "", 0, null },
                    { 8, "", 0, null },
                    { 9, "", 0, null },
                    { 10, "", 0, null },
                    { 11, "", 0, null },
                    { 12, "", 0, null },
                    { 13, "", 0, null },
                    { 14, "", 13, null },
                    { 15, "", 13, null },
                    { 16, "", 13, null },
                    { 17, "Mobile Expenses", 0, null }
                });

            migrationBuilder.InsertData(
                table: "CategoryTranslations",
                columns: new[] { "Id", "CategoryId", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { 1, -2, "ua", "Корегування балансу рахунка" },
                    { 2, -2, "en", "Account Balance adjustment" },
                    { 3, -2, "ru", "Корректировка баланса счета" },
                    { 4, -1, "ua", "Переказ" },
                    { 5, -1, "en", "Money Transfer" },
                    { 6, -1, "ru", "Перевод" },
                    { 7, 1, "ua", "Халтура" },
                    { 8, 1, "en", "Wages" },
                    { 9, 1, "ru", "Халтура" },
                    { 10, 2, "ua", "Зарплата" },
                    { 11, 2, "en", "Salary" },
                    { 12, 2, "ru", "Зарплата" },
                    { 13, 3, "ua", "Відпочинок" },
                    { 14, 3, "en", "Vacation" },
                    { 15, 3, "ru", "Отдых" },
                    { 16, 4, "ua", "Борги" },
                    { 17, 4, "en", "Loans" },
                    { 18, 4, "ru", "Долги" },
                    { 19, 5, "ua", "Продукти харчування" },
                    { 20, 5, "en", "Food" },
                    { 21, 5, "ru", "Продукти питания" },
                    { 22, 6, "ua", "Медичні витрати" },
                    { 23, 6, "en", "Healthcare" },
                    { 24, 6, "ru", "Медицинские расходы" },
                    { 25, 8, "ua", "Освіта" },
                    { 26, 8, "en", "Education" },
                    { 27, 8, "ru", "Образование" },
                    { 28, 9, "ua", "Інші прибутки" },
                    { 29, 9, "en", "Other Income" },
                    { 30, 9, "ru", "Другие доходы" },
                    { 31, 10, "ua", "Комунальні платежі" },
                    { 32, 10, "en", "Communal payments" },
                    { 33, 10, "ru", "Коммунальные платежи" },
                    { 34, 11, "ua", "Одяг" },
                    { 35, 11, "en", "Clothing" },
                    { 36, 11, "ru", "Одежда" },
                    { 37, 12, "ua", "Особиста гігієна" },
                    { 38, 12, "en", "Personal Care" },
                    { 39, 12, "ru", "Личная гигиена" },
                    { 40, 13, "ua", "Побутові видатки" },
                    { 41, 13, "en", "Household" },
                    { 42, 13, "ru", "Хозяйственные расходы" },
                    { 43, 14, "ua", "Покращення" },
                    { 44, 14, "en", "Improvements" },
                    { 45, 14, "ru", "Улучшения" },
                    { 46, 15, "ua", "Меблі" },
                    { 47, 15, "en", "Furnishings" },
                    { 48, 15, "ru", "Мебель" },
                    { 49, 16, "ua", "Електроніка" },
                    { 50, 16, "en", "Electronics" },
                    { 51, 16, "ru", "Електроника" },
                    { 52, 17, "ua", "Мобільні витрати" },
                    { 53, 17, "en", "Mobile Expenses" },
                    { 54, 17, "ru", "Мобильне затраты" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_UserId",
                table: "AccountGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroupTranslation_AccountGroupId_LanguageCode",
                table: "AccountGroupTranslation",
                columns: new[] { "AccountGroupId", "LanguageCode" },
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
                name: "IX_CategoryTranslations_CategoryId",
                table: "CategoryTranslations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MccCategoryMappings_MccCode_CategoryId_Description",
                table: "MccCategoryMappings",
                columns: new[] { "MccCode", "CategoryId", "Description" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonoSyncedTransaction_MonoTransactionId_OperationId",
                table: "MonoSyncedTransaction",
                columns: new[] { "MonoTransactionId", "OperationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonoSyncedTransaction_OperationId",
                table: "MonoSyncedTransaction",
                column: "OperationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationItem_CategoryId",
                table: "OperationItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationItem_OperationId",
                table: "OperationItem",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AccountId",
                table: "Operations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_OperationCurrencyId",
                table: "Operations",
                column: "OperationCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationTags_TagId",
                table: "OperationTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountGroupTranslation");

            migrationBuilder.DropTable(
                name: "AccountsBalance");

            migrationBuilder.DropTable(
                name: "AccountsMonthlyBalance");

            migrationBuilder.DropTable(
                name: "AccountSyncSettings");

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
                name: "CategoryTranslations");

            migrationBuilder.DropTable(
                name: "MccCategoryMappings");

            migrationBuilder.DropTable(
                name: "Mccs");

            migrationBuilder.DropTable(
                name: "MonoSyncedTransaction");

            migrationBuilder.DropTable(
                name: "MonoSyncRules");

            migrationBuilder.DropTable(
                name: "OperationItem");

            migrationBuilder.DropTable(
                name: "OperationTags");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Accounts");

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
