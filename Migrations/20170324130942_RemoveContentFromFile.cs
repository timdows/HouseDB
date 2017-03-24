using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseDB.Migrations
{
    public partial class RemoveContentFromFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "ExportFile");

            migrationBuilder.RenameColumn(
                name: "Filename",
                table: "ExportFile",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "ExportFile",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "ExportFile",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "ExportFile");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "ExportFile");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "ExportFile",
                newName: "Filename");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "ExportFile",
                nullable: true);
        }
    }
}
