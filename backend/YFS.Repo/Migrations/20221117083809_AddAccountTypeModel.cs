using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YFS.Repo.Migrations
{
    public partial class AddAccountTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AccountGroups_AccountGroupId",
                table: "Account");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Account_AccountGroupId",
                table: "Accounts",
                newName: "IX_Accounts_AccountGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "AccountId");

            migrationBuilder.CreateTable(
                name: "AccountsType",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameUa = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    NameRu = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    NameEn = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    NoteUa = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    NoteRu = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    NoteEn = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    TypeOrederBy = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsType", x => x.TypeId);
                });

            migrationBuilder.InsertData(
                table: "AccountsType",
                columns: new[] { "TypeId", "NameEn", "NameRu", "NameUa", "NoteEn", "NoteRu", "NoteUa" },
                values: new object[,]
                {
                    { 1, "Cash", "Наличные деньги", "Готівкові гроші", null, "Учет наличных средств", null },
                    { 2, "Internet-money", "Интернет-деньги", "Інтернет-гроші", null, "Интернет счета", null },
                    { 3, "Deposit", "Депозит", "Депозит", null, "Учет реальных депозитов", null },
                    { 4, "Credit card", "Кредитная карточка", "Кредитна картка", null, "Кредитные карточки Visa, Mastercard и др.", null },
                    { 5, "Debit card", "Дебетная карта", "Дебетна картка", null, "Дебетовые карты Visa, Mastercard и др.", null }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "CurrencyId", "Name_en", "Name_ru", "Name_ua", "ShortNameUs" },
                values: new object[,]
                {
                    { 36, "Australian dollar", "Австралийский доллар", "Австралійський Долар", "AUD" },
                    { 840, "US Dollar", "Доллар США", "Долар США", "USD" },
                    { 978, "Euro", "Евро", "Євро", "EUR" },
                    { 980, "Hryvnia", "Гривня", "Гривня", "UAH" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AccountGroups_AccountGroupId",
                table: "Accounts",
                column: "AccountGroupId",
                principalTable: "AccountGroups",
                principalColumn: "AccountGroupId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountGroups_AccountGroupId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountsType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "CurrencyId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "CurrencyId",
                keyValue: 840);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "CurrencyId",
                keyValue: 978);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "CurrencyId",
                keyValue: 980);

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_AccountGroupId",
                table: "Account",
                newName: "IX_Account_AccountGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AccountGroups_AccountGroupId",
                table: "Account",
                column: "AccountGroupId",
                principalTable: "AccountGroups",
                principalColumn: "AccountGroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
