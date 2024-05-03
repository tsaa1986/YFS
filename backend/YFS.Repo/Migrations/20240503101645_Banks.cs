using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YFS.Repo.Migrations
{
    public partial class Banks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 3, 10, 16, 43, 676, DateTimeKind.Utc).AddTicks(2514));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 3, 10, 16, 43, 676, DateTimeKind.Utc).AddTicks(2524));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 3, 10, 16, 43, 676, DateTimeKind.Utc).AddTicks(2526));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 3, 10, 16, 43, 676, DateTimeKind.Utc).AddTicks(2526));

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "GLMFO", "ADRESS", "D_CLOSE", "KOD_EDRPOU", "D_STAN", "FULLNAME", "DT_GRAND_LIC", "GR_SP", "D_GR_SP", "IDNBU", "KU", "DT_LIC", "NUM_LIC", "PR_LIC", "N_PR_LIC", "NKB", "N_OBL", "N_OBL_UR", "NP", "NAME_E", "OBL_UR", "D_OPEN", "P_IND", "SHORTNAME", "SHORTNAME_EN", "KSTAN", "N_STAN", "TNP", "TELEFON", "TYP" },
                values: new object[,]
                {
                    { 300335, "вулиця Генерала Алмазова, буд. 4а", null, "14305909", null, "Акціонерне товариство \"Райффайзен Банк\"", new DateTime(2021, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "300335", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 10, 1, "чинна банківська ліцензія", "036", "м.Київ", "м.Київ", "Київ", "Raiffeisen Bank Joint Stock Company", 26, new DateTime(1992, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), "01011", "АТ \"Райффайзен Банк\"", "Raiffeisen Bank JSC", 1, "Нормальний", "м.", null, 0 },
                    { 300528, "вул. Жилянська, 43", null, "21685166", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"ОТП БАНК\"", new DateTime(2011, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "300528", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 191, 1, "чинна банківська ліцензія", "296", "м.Київ", "м.Київ", "Київ", "JOINT-STOCK COMPANY OTP BANK", 26, new DateTime(1998, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "01033", "АТ \"ОТП БАНК\"", "OTP BANK JSC", 1, "Нормальний", "м.", null, 0 },
                    { 300614, "Район: Шевченківський, Місто: Київ, Вулиця: вул. Євгена Чикаленка, Будинок: 42/4", null, "14361575", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"КРЕДІ АГРІКОЛЬ БАНК\"", new DateTime(2011, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "300614", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 99, 1, "чинна банківська ліцензія", "171", "м.Київ", "м.Київ", "Київ", "JOINT-STOCK COMPANY \"CREDIT AGRICOLE BANK\"", 26, new DateTime(1993, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "01004", "АТ \"КРЕДІ АГРІКОЛЬ БАНК\"", "JSC \"CREDIT AGRICOLE BANK\"", 1, "Нормальний", "м.", "0445810700", 0 },
                    { 305299, "вул. Грушевського, 1Д", null, "14360570", new DateTime(2016, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "акціонерне товариство комерційний банк \"ПриватБанк\"", new DateTime(2011, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "305299", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 22, 1, "чинна банківська ліцензія", "046", "м.Київ", "м.Київ", "Київ", "Joint-Stock Company Commercial Bank \"PrivatBank\"", 26, new DateTime(1992, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "01001", "АТ КБ \"ПриватБанк\"", "JSC CB \"PrivatBank\"", 1, "Нормальний", "м.", null, 0 },
                    { 322001, "вул. Автозаводська,54/19", null, "21133352", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"УНІВЕРСАЛ БАНК\"", new DateTime(2011, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "322001", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 92, 1, "чинна банківська ліцензія", "242", "м.Київ", "м.Київ", "Київ", "JOINT STOCK COMPANY \"UNIVERSAL BANK\"", 26, new DateTime(1994, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "04082", "АТ \"УНІВЕРСАЛ БАНК\"", "JSC \"UNIVERSAL BANK\"", 1, "Нормальний", "м.", null, 0 },
                    { 351005, "вулиця Андріївська, 2/12", null, "09807750", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"УКРСИББАНК\"", new DateTime(2011, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), "SV", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "351005", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 75, 1, "чинна банківська ліцензія", "136", "м.Київ", "м.Київ", "Київ", "JOINT STOCK COMPANY \"UKRSIBBANK\"", 26, new DateTime(1991, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), "04070", "АТ \"УКРСИББАНК\"", "JSС \"UKRSIBBANK\"", 1, "Нормальний", "м.", null, 0 },
                    { 351254, "вул. Гончара Олеся, буд. 76/2", null, "09620081", null, "АКЦІОНЕРНЕ ТОВАРИСТВО \"СКАЙ БАНК\"", new DateTime(2018, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), "B", null, "351254", 26, new DateTime(2021, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), 32, 1, "чинна банківська ліцензія", "128", "м.Київ", "м.Київ", "Київ", "JOINT STOCK COMPANY \"SKY BANK\"", 26, new DateTime(1991, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), "01054", "АТ \"СКАЙ БАНК\"", "JSC \"SKY BANK\"", 1, "Нормальний", "м.", null, 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "GLMFO",
                keyValue: 300335);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "GLMFO",
                keyValue: 300528);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "GLMFO",
                keyValue: 300614);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "GLMFO",
                keyValue: 305299);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "GLMFO",
                keyValue: 322001);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "GLMFO",
                keyValue: 351005);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "GLMFO",
                keyValue: 351254);

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 3, 9, 32, 13, 366, DateTimeKind.Utc).AddTicks(7298));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 3, 9, 32, 13, 366, DateTimeKind.Utc).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 3, 9, 32, 13, 366, DateTimeKind.Utc).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "AccountTypeId",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 3, 9, 32, 13, 366, DateTimeKind.Utc).AddTicks(7308));
        }
    }
}
