using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainlineUK.Data.Migrations
{
    public partial class StocklistDefaultDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateImported",
                table: "StocklistImport",
                nullable: false,
                defaultValue: new DateTime(2018, 10, 19, 20, 48, 36, 242, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateImported",
                table: "StocklistImport");
        }
    }
}
