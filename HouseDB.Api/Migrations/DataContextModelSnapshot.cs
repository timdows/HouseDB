﻿// <auto-generated />
using HouseDB.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace HouseDB.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("HouseDB.Api.Data.Models.ConfigurationValue", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("Setting");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("ConfigurationValues");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.Device", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DataMineChannel");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<int>("DomoticzKwhIdx");

                    b.Property<int>("DomoticzMotionDetectionIdx");

                    b.Property<int>("DomoticzWattIdx");

                    b.Property<bool>("IsForKwhImport");

                    b.Property<bool>("IsForTemperatureImport");

                    b.Property<string>("Name");

                    b.Property<int>("VeraChannel");

                    b.HasKey("ID");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.ExportFile", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("FileName");

                    b.Property<long>("Length");

                    b.Property<string>("OriginalFileName");

                    b.HasKey("ID");

                    b.ToTable("ExportFiles");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.FitbitAccessToken", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<DateTime>("DateTimeAdded");

                    b.Property<long?>("FitbitAuthCodeID");

                    b.HasKey("ID");

                    b.HasIndex("FitbitAuthCodeID");

                    b.ToTable("FitbitAccessTokens");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.FitbitAuthCode", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthCode");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<DateTime>("DateTimeAdded");

                    b.Property<long?>("FitbitClientDetailID");

                    b.HasKey("ID");

                    b.HasIndex("FitbitClientDetailID");

                    b.ToTable("FitbitAuthCodes");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.FitbitClientDetail", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientId");

                    b.Property<string>("ClientSecret");

                    b.Property<DateTime?>("DateDeleted");

                    b.HasKey("ID");

                    b.ToTable("FitbitClientDetails");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.HeaterMeter", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("GNumber");

                    b.Property<long?>("HeaterMeterGroupID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("HeaterMeterGroupID");

                    b.ToTable("HeaterMeters");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.HeaterMeterGroup", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("HeaterMeterGroups");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.HeaterValue", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<long?>("HeaterMeterID");

                    b.Property<int>("Value");

                    b.HasKey("ID");

                    b.HasIndex("HeaterMeterID");

                    b.ToTable("HeaterValues");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.KwhDateUsage", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<long>("DeviceID");

                    b.Property<decimal>("Usage");

                    b.HasKey("ID");

                    b.HasIndex("DeviceID");

                    b.ToTable("KwhDateUsages");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.KwhDeviceValue", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<DateTime>("DateTime");

                    b.Property<long>("DeviceID");

                    b.Property<string>("RawDataLine");

                    b.Property<long>("UnixTimestamp");

                    b.Property<decimal>("Value");

                    b.HasKey("ID");

                    b.HasIndex("DeviceID");

                    b.ToTable("KwhDeviceValues");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.MotionDetection", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<DateTime>("DateTimeDetection");

                    b.Property<long>("DeviceID");

                    b.Property<bool>("Status");

                    b.HasKey("ID");

                    b.HasIndex("DeviceID");

                    b.ToTable("MotionDetections");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.P1Consumption", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<double>("DayUsage");

                    b.HasKey("ID");

                    b.ToTable("P1Consumptions");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.FitbitAccessToken", b =>
                {
                    b.HasOne("HouseDB.Api.Data.Models.FitbitAuthCode", "FitbitAuthCode")
                        .WithMany()
                        .HasForeignKey("FitbitAuthCodeID");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.FitbitAuthCode", b =>
                {
                    b.HasOne("HouseDB.Api.Data.Models.FitbitClientDetail", "FitbitClientDetail")
                        .WithMany()
                        .HasForeignKey("FitbitClientDetailID");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.HeaterMeter", b =>
                {
                    b.HasOne("HouseDB.Api.Data.Models.HeaterMeterGroup", "HeaterMeterGroup")
                        .WithMany("HeaterMeters")
                        .HasForeignKey("HeaterMeterGroupID");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.HeaterValue", b =>
                {
                    b.HasOne("HouseDB.Api.Data.Models.HeaterMeter", "HeaterMeter")
                        .WithMany("HeaterValues")
                        .HasForeignKey("HeaterMeterID");
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.KwhDateUsage", b =>
                {
                    b.HasOne("HouseDB.Api.Data.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.KwhDeviceValue", b =>
                {
                    b.HasOne("HouseDB.Api.Data.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HouseDB.Api.Data.Models.MotionDetection", b =>
                {
                    b.HasOne("HouseDB.Api.Data.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
