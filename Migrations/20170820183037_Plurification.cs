using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HouseDB.Migrations
{
    public partial class Plurification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseRecord_ExpenseType_ExpenseTypeID",
                table: "ExpenseRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_HeaterMeter_HeaterMeterGroup_HeaterMeterGroupID",
                table: "HeaterMeter");

            migrationBuilder.DropForeignKey(
                name: "FK_HeaterValue_HeaterMeter_HeaterMeterID",
                table: "HeaterValue");

            migrationBuilder.DropForeignKey(
                name: "FK_KwhDateUsage_Device_DeviceID",
                table: "KwhDateUsage");

            migrationBuilder.DropForeignKey(
                name: "FK_KwhDeviceValue_Device_DeviceID",
                table: "KwhDeviceValue");

            migrationBuilder.DropForeignKey(
                name: "FK_MotionDetection_Device_DeviceID",
                table: "MotionDetection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotionDetection",
                table: "MotionDetection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KwhDeviceValue",
                table: "KwhDeviceValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KwhDateUsage",
                table: "KwhDateUsage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeaterValue",
                table: "HeaterValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeaterMeterGroup",
                table: "HeaterMeterGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeaterMeter",
                table: "HeaterMeter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExportFile",
                table: "ExportFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseType",
                table: "ExpenseType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseRecord",
                table: "ExpenseRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Device",
                table: "Device");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfigurationValue",
                table: "ConfigurationValue");

            migrationBuilder.RenameTable(
                name: "MotionDetection",
                newName: "MotionDetections");

            migrationBuilder.RenameTable(
                name: "KwhDeviceValue",
                newName: "KwhDeviceValues");

            migrationBuilder.RenameTable(
                name: "KwhDateUsage",
                newName: "KwhDateUsages");

            migrationBuilder.RenameTable(
                name: "HeaterValue",
                newName: "HeaterValues");

            migrationBuilder.RenameTable(
                name: "HeaterMeterGroup",
                newName: "HeaterMeterGroups");

            migrationBuilder.RenameTable(
                name: "HeaterMeter",
                newName: "HeaterMeters");

            migrationBuilder.RenameTable(
                name: "ExportFile",
                newName: "ExportFiles");

            migrationBuilder.RenameTable(
                name: "ExpenseType",
                newName: "ExpenseTypes");

            migrationBuilder.RenameTable(
                name: "ExpenseRecord",
                newName: "ExpenseRecords");

            migrationBuilder.RenameTable(
                name: "Device",
                newName: "Devices");

            migrationBuilder.RenameTable(
                name: "ConfigurationValue",
                newName: "ConfigurationValues");

            migrationBuilder.RenameIndex(
                name: "IX_MotionDetection_DeviceID",
                table: "MotionDetections",
                newName: "IX_MotionDetections_DeviceID");

            migrationBuilder.RenameIndex(
                name: "IX_KwhDeviceValue_DeviceID",
                table: "KwhDeviceValues",
                newName: "IX_KwhDeviceValues_DeviceID");

            migrationBuilder.RenameIndex(
                name: "IX_KwhDateUsage_DeviceID",
                table: "KwhDateUsages",
                newName: "IX_KwhDateUsages_DeviceID");

            migrationBuilder.RenameIndex(
                name: "IX_HeaterValue_HeaterMeterID",
                table: "HeaterValues",
                newName: "IX_HeaterValues_HeaterMeterID");

            migrationBuilder.RenameIndex(
                name: "IX_HeaterMeter_HeaterMeterGroupID",
                table: "HeaterMeters",
                newName: "IX_HeaterMeters_HeaterMeterGroupID");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseRecord_ExpenseTypeID",
                table: "ExpenseRecords",
                newName: "IX_ExpenseRecords_ExpenseTypeID");

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "MotionDetections",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "KwhDeviceValues",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "KwhDateUsages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "HeaterValues",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "HeaterMeterGroups",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "HeaterMeters",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "ExportFiles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "ExpenseTypes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "ExpenseRecords",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "Devices",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "ConfigurationValues",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotionDetections",
                table: "MotionDetections",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KwhDeviceValues",
                table: "KwhDeviceValues",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KwhDateUsages",
                table: "KwhDateUsages",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeaterValues",
                table: "HeaterValues",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeaterMeterGroups",
                table: "HeaterMeterGroups",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeaterMeters",
                table: "HeaterMeters",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExportFiles",
                table: "ExportFiles",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseTypes",
                table: "ExpenseTypes",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseRecords",
                table: "ExpenseRecords",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfigurationValues",
                table: "ConfigurationValues",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseRecords_ExpenseTypes_ExpenseTypeID",
                table: "ExpenseRecords",
                column: "ExpenseTypeID",
                principalTable: "ExpenseTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeaterMeters_HeaterMeterGroups_HeaterMeterGroupID",
                table: "HeaterMeters",
                column: "HeaterMeterGroupID",
                principalTable: "HeaterMeterGroups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HeaterValues_HeaterMeters_HeaterMeterID",
                table: "HeaterValues",
                column: "HeaterMeterID",
                principalTable: "HeaterMeters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KwhDateUsages_Devices_DeviceID",
                table: "KwhDateUsages",
                column: "DeviceID",
                principalTable: "Devices",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KwhDeviceValues_Devices_DeviceID",
                table: "KwhDeviceValues",
                column: "DeviceID",
                principalTable: "Devices",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MotionDetections_Devices_DeviceID",
                table: "MotionDetections",
                column: "DeviceID",
                principalTable: "Devices",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseRecords_ExpenseTypes_ExpenseTypeID",
                table: "ExpenseRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HeaterMeters_HeaterMeterGroups_HeaterMeterGroupID",
                table: "HeaterMeters");

            migrationBuilder.DropForeignKey(
                name: "FK_HeaterValues_HeaterMeters_HeaterMeterID",
                table: "HeaterValues");

            migrationBuilder.DropForeignKey(
                name: "FK_KwhDateUsages_Devices_DeviceID",
                table: "KwhDateUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_KwhDeviceValues_Devices_DeviceID",
                table: "KwhDeviceValues");

            migrationBuilder.DropForeignKey(
                name: "FK_MotionDetections_Devices_DeviceID",
                table: "MotionDetections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotionDetections",
                table: "MotionDetections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KwhDeviceValues",
                table: "KwhDeviceValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KwhDateUsages",
                table: "KwhDateUsages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeaterValues",
                table: "HeaterValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeaterMeters",
                table: "HeaterMeters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeaterMeterGroups",
                table: "HeaterMeterGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExportFiles",
                table: "ExportFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseTypes",
                table: "ExpenseTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseRecords",
                table: "ExpenseRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfigurationValues",
                table: "ConfigurationValues");

            migrationBuilder.RenameTable(
                name: "MotionDetections",
                newName: "MotionDetection");

            migrationBuilder.RenameTable(
                name: "KwhDeviceValues",
                newName: "KwhDeviceValue");

            migrationBuilder.RenameTable(
                name: "KwhDateUsages",
                newName: "KwhDateUsage");

            migrationBuilder.RenameTable(
                name: "HeaterValues",
                newName: "HeaterValue");

            migrationBuilder.RenameTable(
                name: "HeaterMeters",
                newName: "HeaterMeter");

            migrationBuilder.RenameTable(
                name: "HeaterMeterGroups",
                newName: "HeaterMeterGroup");

            migrationBuilder.RenameTable(
                name: "ExportFiles",
                newName: "ExportFile");

            migrationBuilder.RenameTable(
                name: "ExpenseTypes",
                newName: "ExpenseType");

            migrationBuilder.RenameTable(
                name: "ExpenseRecords",
                newName: "ExpenseRecord");

            migrationBuilder.RenameTable(
                name: "Devices",
                newName: "Device");

            migrationBuilder.RenameTable(
                name: "ConfigurationValues",
                newName: "ConfigurationValue");

            migrationBuilder.RenameIndex(
                name: "IX_MotionDetections_DeviceID",
                table: "MotionDetection",
                newName: "IX_MotionDetection_DeviceID");

            migrationBuilder.RenameIndex(
                name: "IX_KwhDeviceValues_DeviceID",
                table: "KwhDeviceValue",
                newName: "IX_KwhDeviceValue_DeviceID");

            migrationBuilder.RenameIndex(
                name: "IX_KwhDateUsages_DeviceID",
                table: "KwhDateUsage",
                newName: "IX_KwhDateUsage_DeviceID");

            migrationBuilder.RenameIndex(
                name: "IX_HeaterValues_HeaterMeterID",
                table: "HeaterValue",
                newName: "IX_HeaterValue_HeaterMeterID");

            migrationBuilder.RenameIndex(
                name: "IX_HeaterMeters_HeaterMeterGroupID",
                table: "HeaterMeter",
                newName: "IX_HeaterMeter_HeaterMeterGroupID");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseRecords_ExpenseTypeID",
                table: "ExpenseRecord",
                newName: "IX_ExpenseRecord_ExpenseTypeID");

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "MotionDetection",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "KwhDeviceValue",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "KwhDateUsage",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "HeaterValue",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "HeaterMeter",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "HeaterMeterGroup",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "ExportFile",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "ExpenseType",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "ExpenseRecord",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "Device",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "ConfigurationValue",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotionDetection",
                table: "MotionDetection",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KwhDeviceValue",
                table: "KwhDeviceValue",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KwhDateUsage",
                table: "KwhDateUsage",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeaterValue",
                table: "HeaterValue",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeaterMeter",
                table: "HeaterMeter",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeaterMeterGroup",
                table: "HeaterMeterGroup",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExportFile",
                table: "ExportFile",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseType",
                table: "ExpenseType",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseRecord",
                table: "ExpenseRecord",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Device",
                table: "Device",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfigurationValue",
                table: "ConfigurationValue",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseRecord_ExpenseType_ExpenseTypeID",
                table: "ExpenseRecord",
                column: "ExpenseTypeID",
                principalTable: "ExpenseType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeaterMeter_HeaterMeterGroup_HeaterMeterGroupID",
                table: "HeaterMeter",
                column: "HeaterMeterGroupID",
                principalTable: "HeaterMeterGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HeaterValue_HeaterMeter_HeaterMeterID",
                table: "HeaterValue",
                column: "HeaterMeterID",
                principalTable: "HeaterMeter",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KwhDateUsage_Device_DeviceID",
                table: "KwhDateUsage",
                column: "DeviceID",
                principalTable: "Device",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KwhDeviceValue_Device_DeviceID",
                table: "KwhDeviceValue",
                column: "DeviceID",
                principalTable: "Device",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MotionDetection_Device_DeviceID",
                table: "MotionDetection",
                column: "DeviceID",
                principalTable: "Device",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
