using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainlineUK.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StocklistImport",
                columns: table => new
                {
                    StocklistImportID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DealerID = table.Column<int>(nullable: false),
                    StockItemID = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    Registration = table.Column<string>(nullable: true),
                    RegCode = table.Column<int>(nullable: false),
                    Make = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Derivative = table.Column<string>(nullable: true),
                    AttentionGrabber = table.Column<string>(nullable: true),
                    EngineSize = table.Column<int>(nullable: false),
                    EngineSizeUnit = table.Column<int>(nullable: false),
                    FuelType = table.Column<int>(nullable: false),
                    ManufacturedYear = table.Column<int>(nullable: false),
                    Mileage = table.Column<int>(nullable: false),
                    MileageUnit = table.Column<int>(nullable: false),
                    BodyType = table.Column<int>(nullable: false),
                    Colour = table.Column<string>(nullable: true),
                    Transmission = table.Column<int>(nullable: false),
                    Doors = table.Column<int>(nullable: false),
                    PreviousOwners = table.Column<int>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceExtra = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdvertDescription1 = table.Column<string>(nullable: true),
                    AdvertDescription2 = table.Column<string>(nullable: true),
                    WheelBaseType = table.Column<string>(nullable: true),
                    CabType = table.Column<string>(nullable: true),
                    FourWheelDrive = table.Column<bool>(nullable: true),
                    FranchiseApproved = table.Column<bool>(nullable: true),
                    Style = table.Column<string>(nullable: true),
                    SubStyle = table.Column<string>(nullable: true),
                    Hours = table.Column<int>(nullable: true),
                    NumberOfBerths = table.Column<int>(nullable: true),
                    Axle = table.Column<string>(nullable: true),
                    DealerReference = table.Column<string>(nullable: true),
                    Images = table.Column<string>(nullable: true),
                    VideoURL = table.Column<string>(nullable: true),
                    DateOfRegistration = table.Column<DateTime>(nullable: false),
                    ServiceHistory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocklistImport", x => x.StocklistImportID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StocklistImport");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");
        }
    }
}
