using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Migrations
{
    public partial class KwhDeviceValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KwhDeviceValue",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    DeviceID = table.Column<long>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KwhDeviceValue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KwhDeviceValue_Device_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "Device",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KwhDeviceValue_DeviceID",
                table: "KwhDeviceValue",
                column: "DeviceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KwhDeviceValue");
        }
    }
}
