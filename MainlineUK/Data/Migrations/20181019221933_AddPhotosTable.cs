using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainlineUK.Data.Migrations
{
    public partial class AddPhotosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    PhotoID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PhotoPath = table.Column<string>(maxLength: 255, nullable: false),
                    DateImported = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    StocklistImportID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.PhotoID);
                    table.ForeignKey(
                        name: "FK_Photo_StocklistImport_StocklistImportID",
                        column: x => x.StocklistImportID,
                        principalTable: "StocklistImport",
                        principalColumn: "StocklistImportID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photo_StocklistImportID",
                table: "Photo",
                column: "StocklistImportID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photo");
        }
    }
}
