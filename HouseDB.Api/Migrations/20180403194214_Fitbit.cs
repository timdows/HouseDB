using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HouseDB.Api.Migrations
{
    public partial class Fitbit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FitbitAuthCodes",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthCode = table.Column<string>(nullable: true),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    DateTimeAdded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitbitAuthCodes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FitbitAccessTokens",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccessToken = table.Column<string>(nullable: true),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    DateTimeAdded = table.Column<DateTime>(nullable: false),
                    FitbitAuthCodeID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitbitAccessTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FitbitAccessTokens_FitbitAuthCodes_FitbitAuthCodeID",
                        column: x => x.FitbitAuthCodeID,
                        principalTable: "FitbitAuthCodes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FitbitAccessTokens_FitbitAuthCodeID",
                table: "FitbitAccessTokens",
                column: "FitbitAuthCodeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FitbitAccessTokens");

            migrationBuilder.DropTable(
                name: "FitbitAuthCodes");
        }
    }
}
