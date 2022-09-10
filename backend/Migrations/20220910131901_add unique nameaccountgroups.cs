using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YFS.Migrations
{
    public partial class adduniquenameaccountgroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AccountGroups_AccountGroupNameRu",
                table: "AccountGroups");

            migrationBuilder.AlterColumn<string>(
                name: "AccountGroupNameRu",
                table: "AccountGroups",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameEn",
                table: "AccountGroups",
                columns: new[] { "UserId", "AccountGroupNameEn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameRu",
                table: "AccountGroups",
                columns: new[] { "UserId", "AccountGroupNameRu" },
                unique: true,
                filter: "[AccountGroupNameRu] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameUa",
                table: "AccountGroups",
                columns: new[] { "UserId", "AccountGroupNameUa" },
                unique: true,
                filter: "[AccountGroupNameUa] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameEn",
                table: "AccountGroups");

            migrationBuilder.DropIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameRu",
                table: "AccountGroups");

            migrationBuilder.DropIndex(
                name: "IX_AccountGroups_UserId_AccountGroupNameUa",
                table: "AccountGroups");

            migrationBuilder.AlterColumn<string>(
                name: "AccountGroupNameRu",
                table: "AccountGroups",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AccountGroups_AccountGroupNameRu",
                table: "AccountGroups",
                column: "AccountGroupNameRu");
        }
    }
}
