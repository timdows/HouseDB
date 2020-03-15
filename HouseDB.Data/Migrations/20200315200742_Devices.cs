using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Data.Migrations
{
    public partial class Devices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateTimeDeleted = table.Column<DateTime>(nullable: true),
                    DeletedByIdentityServerUserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DataMineChannel = table.Column<int>(nullable: false),
                    VeraChannel = table.Column<int>(nullable: false),
                    DomoticzKwhIdx = table.Column<int>(nullable: false),
                    DomoticzWattIdx = table.Column<int>(nullable: false),
                    IsForKwhImport = table.Column<bool>(nullable: false),
                    IsForTemperatureImport = table.Column<bool>(nullable: false),
                    DomoticzMotionDetectionIdx = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
