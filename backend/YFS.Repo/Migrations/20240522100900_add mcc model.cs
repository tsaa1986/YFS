using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YFS.Repo.Migrations
{
    public partial class addmccmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 22, 10, 8, 59, 348, DateTimeKind.Utc).AddTicks(626));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 22, 10, 8, 59, 348, DateTimeKind.Utc).AddTicks(637));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 22, 10, 8, 59, 348, DateTimeKind.Utc).AddTicks(639));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 22, 10, 8, 59, 348, DateTimeKind.Utc).AddTicks(640));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mccs");

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 20, 5, 49, 59, 581, DateTimeKind.Utc).AddTicks(3158));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 20, 5, 49, 59, 581, DateTimeKind.Utc).AddTicks(3168));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 20, 5, 49, 59, 581, DateTimeKind.Utc).AddTicks(3170));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 20, 5, 49, 59, 581, DateTimeKind.Utc).AddTicks(3171));
        }
    }
}
