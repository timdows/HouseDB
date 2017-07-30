using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Migrations
{
    public partial class MotionStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotionDetection_Device_DeviceID",
                table: "MotionDetection");

            migrationBuilder.AlterColumn<long>(
                name: "DeviceID",
                table: "MotionDetection",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "MotionDetection",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_MotionDetection_Device_DeviceID",
                table: "MotionDetection",
                column: "DeviceID",
                principalTable: "Device",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotionDetection_Device_DeviceID",
                table: "MotionDetection");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MotionDetection");

            migrationBuilder.AlterColumn<long>(
                name: "DeviceID",
                table: "MotionDetection",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_MotionDetection_Device_DeviceID",
                table: "MotionDetection",
                column: "DeviceID",
                principalTable: "Device",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
