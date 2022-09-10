using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YFS.Migrations
{
    public partial class addaccountgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.CreateTable(
                name: "AccountGroups",
                columns: table => new
                {
                    AccountGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccountGroupNameRu = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    AccountGroupNameEn = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    AccountGroupNameUa = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    GroupOrederBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroups", x => x.AccountGroupId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountGroups");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AspNetUsers");
        }
    }
}
