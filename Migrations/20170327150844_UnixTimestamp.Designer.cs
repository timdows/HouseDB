using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HouseDB.Data;

namespace HouseDB.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20170327150844_UnixTimestamp")]
    partial class UnixTimestamp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("HouseDB.Data.Models.ConfigurationValue", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("Setting");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("ConfigurationValue");
                });

            modelBuilder.Entity("HouseDB.Data.Models.Device", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DataMineChannel");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<bool>("IsForKwhImport");

                    b.Property<bool>("IsForTemperatureImport");

                    b.Property<string>("Name");

                    b.Property<int>("VeraChannel");

                    b.HasKey("ID");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("HouseDB.Data.Models.ExportFile", b =>
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

                    b.ToTable("ExportFile");
                });

            modelBuilder.Entity("HouseDB.Data.Models.HeaterMeter", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("GNumber");

                    b.Property<long?>("HeaterMeterGroupID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("HeaterMeterGroupID");

                    b.ToTable("HeaterMeter");
                });

            modelBuilder.Entity("HouseDB.Data.Models.HeaterMeterGroup", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("HeaterMeterGroup");
                });

            modelBuilder.Entity("HouseDB.Data.Models.HeaterValue", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<long?>("HeaterMeterID");

                    b.Property<int>("Value");

                    b.HasKey("ID");

                    b.HasIndex("HeaterMeterID");

                    b.ToTable("HeaterValue");
                });

            modelBuilder.Entity("HouseDB.Data.Models.KwhDeviceValue", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<DateTime>("DateTime");

                    b.Property<long?>("DeviceID");

                    b.Property<string>("RawDataLine");

                    b.Property<long>("UnixTimestamp");

                    b.Property<decimal>("Value");

                    b.HasKey("ID");

                    b.HasIndex("DeviceID");

                    b.ToTable("KwhDeviceValue");
                });

            modelBuilder.Entity("HouseDB.Data.Models.HeaterMeter", b =>
                {
                    b.HasOne("HouseDB.Data.Models.HeaterMeterGroup", "HeaterMeterGroup")
                        .WithMany("HeaterMeters")
                        .HasForeignKey("HeaterMeterGroupID");
                });

            modelBuilder.Entity("HouseDB.Data.Models.HeaterValue", b =>
                {
                    b.HasOne("HouseDB.Data.Models.HeaterMeter", "HeaterMeter")
                        .WithMany("HeaterValues")
                        .HasForeignKey("HeaterMeterID");
                });

            modelBuilder.Entity("HouseDB.Data.Models.KwhDeviceValue", b =>
                {
                    b.HasOne("HouseDB.Data.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceID");
                });
        }
    }
}
