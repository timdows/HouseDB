using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Migrations
{
    public partial class DomoticzIdx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DomoticzIdx",
                table: "Device",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KwhDeviceValue_Device_DeviceID",
                table: "KwhDeviceValue");

            migrationBuilder.DropColumn(
                name: "DomoticzIdx",
                table: "Device");

            migrationBuilder.AlterColumn<long>(
                name: "DeviceID",
                table: "KwhDeviceValue",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_KwhDeviceValue_Device_DeviceID",
                table: "KwhDeviceValue",
                column: "DeviceID",
                principalTable: "Device",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
