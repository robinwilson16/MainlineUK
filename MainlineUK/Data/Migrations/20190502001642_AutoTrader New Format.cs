using Microsoft.EntityFrameworkCore.Migrations;

namespace MainlineUK.Data.Migrations
{
    public partial class AutoTraderNewFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertDescription1",
                table: "StocklistImport");

            migrationBuilder.DropColumn(
                name: "AdvertDescription2",
                table: "StocklistImport");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdvertDescription1",
                table: "StocklistImport",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdvertDescription2",
                table: "StocklistImport",
                nullable: true);
        }
    }
}
