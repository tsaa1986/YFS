using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YFS.Repo.Migrations
{
    public partial class Addtransactionmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountTypes",
                keyColumn: "TypeId",
                keyValue: 5);

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    TypeTransaction = table.Column<int>(type: "int", nullable: false),
                    WithdrawFromAccountId = table.Column<int>(type: "int", nullable: true),
                    TargetAccountId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true),
                    Tag = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "TypeId",
                keyValue: 4,
                columns: new[] { "NameEn", "NameRu", "NameUa", "NoteRu" },
                values: new object[] { "Bank account", "Банковский счет", "Банківський рахунок", "Банковский счет" });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "BankId", "Name" },
                values: new object[] { 1, "Demo Bank" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "TypeId",
                keyValue: 4,
                columns: new[] { "NameEn", "NameRu", "NameUa", "NoteRu" },
                values: new object[] { "Credit card", "Кредитная карточка", "Кредитна картка", "Кредитные карточки Visa, Mastercard и др." });

            migrationBuilder.InsertData(
                table: "AccountTypes",
                columns: new[] { "TypeId", "CreatedOn", "NameEn", "NameRu", "NameUa", "NoteEn", "NoteRu", "NoteUa", "TypeOrederBy" },
                values: new object[] { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Debit card", "Дебетная карта", "Дебетна картка", null, "Дебетовые карты Visa, Mastercard и др.", null, 0 });
        }
    }
}
