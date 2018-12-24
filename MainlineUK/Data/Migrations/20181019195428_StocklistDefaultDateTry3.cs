using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainlineUK.Data.Migrations
{
    public partial class StocklistDefaultDateTry3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateImported",
                table: "StocklistImport",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 10, 19, 20, 48, 36, 242, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateImported",
                table: "StocklistImport",
                nullable: false,
                defaultValue: new DateTime(2018, 10, 19, 20, 48, 36, 242, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
