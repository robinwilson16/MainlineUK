using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainlineUK.Data.Migrations
{
    public partial class UpdateStocklisttoAutoTraderv7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Closer",
                table: "StocklistImport",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature",
                table: "StocklistImport",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyInformation",
                table: "StocklistImport",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherVehicleText",
                table: "StocklistImport",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VatStatus",
                table: "StocklistImport",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarRequestSell",
                columns: table => new
                {
                    CarRequestSellID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarRequestID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Telephone = table.Column<string>(maxLength: 15, nullable: false),
                    Email = table.Column<string>(maxLength: 200, nullable: false),
                    CarMake = table.Column<string>(maxLength: 20, nullable: false),
                    CarModel = table.Column<string>(maxLength: 20, nullable: false),
                    CarMileage = table.Column<int>(nullable: false),
                    CarColour = table.Column<string>(maxLength: 20, nullable: false),
                    CarFuelType = table.Column<string>(maxLength: 10, nullable: false),
                    CarTransmission = table.Column<string>(maxLength: 10, nullable: false),
                    CarCondition = table.Column<string>(nullable: true),
                    CarRegistration = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRequestSell", x => x.CarRequestSellID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarRequestSell");

            migrationBuilder.DropColumn(
                name: "Closer",
                table: "StocklistImport");

            migrationBuilder.DropColumn(
                name: "Feature",
                table: "StocklistImport");

            migrationBuilder.DropColumn(
                name: "KeyInformation",
                table: "StocklistImport");

            migrationBuilder.DropColumn(
                name: "OtherVehicleText",
                table: "StocklistImport");

            migrationBuilder.DropColumn(
                name: "VatStatus",
                table: "StocklistImport");
        }
    }
}
