using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Migrations
{
    public partial class RenameKwh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DomoticzIdx",
                table: "Device",
                newName: "DomoticzKwhIdx");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DomoticzKwhIdx",
                table: "Device",
                newName: "DomoticzIdx");
        }
    }
}
