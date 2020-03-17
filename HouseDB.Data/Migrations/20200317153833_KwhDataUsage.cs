using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Data.Migrations
{
    public partial class KwhDataUsage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KwhDateUsages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateTimeDeleted = table.Column<DateTime>(nullable: true),
                    DeletedByIdentityServerUserId = table.Column<int>(nullable: true),
                    DeviceId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Usage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KwhDateUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KwhDateUsages_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KwhDateUsages_DeviceId",
                table: "KwhDateUsages",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KwhDateUsages");
        }
    }
}
