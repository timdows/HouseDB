using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Migrations
{
    public partial class ExportFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DataMineChannel = table.Column<int>(nullable: false),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    IsForKwhImport = table.Column<bool>(nullable: false),
                    IsForTemperatureImport = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    VeraChannel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ExportFile",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Content = table.Column<byte[]>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    Filename = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportFile", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "ExportFile");
        }
    }
}
