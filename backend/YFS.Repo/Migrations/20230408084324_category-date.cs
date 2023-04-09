using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YFS.Repo.Migrations
{
    public partial class categorydate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name_ENG", "Name_RU", "Name_UA", "Note", "RootId", "UserId" },
                values: new object[,]
                {
                    { 13, "Household", "Хозяйственные расходы", "Побутові видатки", "", 0, null },
                    { 14, "Improvements", "Улучшения", "Покращення", "", 13, null },
                    { 15, "Furnishings", "Мебель", "Меблі", "", 13, null },
                    { 16, "Electronics", "Електроника", "Електроніка", "", 13, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 16);
        }
    }
}
