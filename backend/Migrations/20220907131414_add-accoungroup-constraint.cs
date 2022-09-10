using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YFS.Migrations
{
    public partial class addaccoungroupconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
