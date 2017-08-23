using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HouseDB.Migrations
{
    public partial class Initial20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationValues");

            migrationBuilder.DropTable(
                name: "ExpenseRecords");

            migrationBuilder.DropTable(
                name: "ExportFiles");

            migrationBuilder.DropTable(
                name: "HeaterValues");

            migrationBuilder.DropTable(
                name: "KwhDateUsages");

            migrationBuilder.DropTable(
                name: "KwhDeviceValues");

            migrationBuilder.DropTable(
                name: "MotionDetections");

            migrationBuilder.DropTable(
                name: "ExpenseTypes");

            migrationBuilder.DropTable(
                name: "HeaterMeters");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "HeaterMeterGroups");
        }
    }
}
