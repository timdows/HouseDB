using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HouseDB.Api.Migrations
{
    public partial class FitbitClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FitbitClientDetailID",
                table: "FitbitAuthCodes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FitbitClientDetails",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    DateDeleted = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitbitClientDetails", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FitbitAuthCodes_FitbitClientDetailID",
                table: "FitbitAuthCodes",
                column: "FitbitClientDetailID");

            migrationBuilder.AddForeignKey(
                name: "FK_FitbitAuthCodes_FitbitClientDetails_FitbitClientDetailID",
                table: "FitbitAuthCodes",
                column: "FitbitClientDetailID",
                principalTable: "FitbitClientDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FitbitAuthCodes_FitbitClientDetails_FitbitClientDetailID",
                table: "FitbitAuthCodes");

            migrationBuilder.DropTable(
                name: "FitbitClientDetails");

            migrationBuilder.DropIndex(
                name: "IX_FitbitAuthCodes_FitbitClientDetailID",
                table: "FitbitAuthCodes");

            migrationBuilder.DropColumn(
                name: "FitbitClientDetailID",
                table: "FitbitAuthCodes");
        }
    }
}
