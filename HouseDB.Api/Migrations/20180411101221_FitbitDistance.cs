using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HouseDB.Api.Migrations
{
    public partial class FitbitDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FitbitActivityDistances",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    FitbitClientDetailID = table.Column<long>(nullable: true),
                    KiloMeters = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitbitActivityDistances", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FitbitActivityDistances_FitbitClientDetails_FitbitClientDetailID",
                        column: x => x.FitbitClientDetailID,
                        principalTable: "FitbitClientDetails",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FitbitActivityDistances_FitbitClientDetailID",
                table: "FitbitActivityDistances",
                column: "FitbitClientDetailID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FitbitActivityDistances");
        }
    }
}
