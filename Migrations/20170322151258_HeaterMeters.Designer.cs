using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HouseDB.Data;

namespace HouseDB.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20170322151258_HeaterMeters")]
    partial class HeaterMeters
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

            modelBuilder.Entity("HouseDB.Data.Models.HeaterMeter", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateDeleted");

                    b.Property<string>("GNumber");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("HeaterMeter");
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

            modelBuilder.Entity("HouseDB.Data.Models.HeaterValue", b =>
                {
                    b.HasOne("HouseDB.Data.Models.HeaterMeter", "HeaterMeter")
                        .WithMany("HeaterValues")
                        .HasForeignKey("HeaterMeterID");
                });
        }
    }
}
