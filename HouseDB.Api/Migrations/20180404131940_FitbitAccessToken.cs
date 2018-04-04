using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HouseDB.Api.Migrations
{
    public partial class FitbitAccessToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpiresIn",
                table: "FitbitAccessTokens",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "FitbitAccessTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenType",
                table: "FitbitAccessTokens",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresIn",
                table: "FitbitAccessTokens");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "FitbitAccessTokens");

            migrationBuilder.DropColumn(
                name: "TokenType",
                table: "FitbitAccessTokens");
        }
    }
}
