using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainlineUK.Data.Migrations
{
    public partial class SetStockListFieldTypesForCompatibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StockItemID",
                table: "StocklistImport",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "FranchiseApproved",
                table: "StocklistImport",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FourWheelDrive",
                table: "StocklistImport",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StockItemID",
                table: "StocklistImport",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "FranchiseApproved",
                table: "StocklistImport",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "FourWheelDrive",
                table: "StocklistImport",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true);
        }
    }
}
