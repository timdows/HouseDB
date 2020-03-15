using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HouseDB.Api.Migrations
{
    public partial class AddClientDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FitbitClientDetailID",
                table: "FitbitActivitySteps",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FitbitActivitySteps_FitbitClientDetailID",
                table: "FitbitActivitySteps",
                column: "FitbitClientDetailID");

            migrationBuilder.AddForeignKey(
                name: "FK_FitbitActivitySteps_FitbitClientDetails_FitbitClientDetailID",
                table: "FitbitActivitySteps",
                column: "FitbitClientDetailID",
                principalTable: "FitbitClientDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FitbitActivitySteps_FitbitClientDetails_FitbitClientDetailID",
                table: "FitbitActivitySteps");

            migrationBuilder.DropIndex(
                name: "IX_FitbitActivitySteps_FitbitClientDetailID",
                table: "FitbitActivitySteps");

            migrationBuilder.DropColumn(
                name: "FitbitClientDetailID",
                table: "FitbitActivitySteps");
        }
    }
}
