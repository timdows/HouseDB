using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Migrations
{
    public partial class Motion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForMotionDetectionImport",
                table: "Device");

            migrationBuilder.AddColumn<int>(
                name: "DomoticzMotionDetectionIdx",
                table: "Device",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DomoticzMotionDetectionIdx",
                table: "Device");

            migrationBuilder.AddColumn<bool>(
                name: "IsForMotionDetectionImport",
                table: "Device",
                nullable: false,
                defaultValue: false);
        }
    }
}
