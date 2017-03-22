using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Migrations
{
    public partial class HeaterMeterGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HeaterMeterGroupID",
                table: "HeaterMeter",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HeaterMeterGroup",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaterMeterGroup", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeaterMeter_HeaterMeterGroupID",
                table: "HeaterMeter",
                column: "HeaterMeterGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_HeaterMeter_HeaterMeterGroup_HeaterMeterGroupID",
                table: "HeaterMeter",
                column: "HeaterMeterGroupID",
                principalTable: "HeaterMeterGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeaterMeter_HeaterMeterGroup_HeaterMeterGroupID",
                table: "HeaterMeter");

            migrationBuilder.DropTable(
                name: "HeaterMeterGroup");

            migrationBuilder.DropIndex(
                name: "IX_HeaterMeter_HeaterMeterGroupID",
                table: "HeaterMeter");

            migrationBuilder.DropColumn(
                name: "HeaterMeterGroupID",
                table: "HeaterMeter");
        }
    }
}
