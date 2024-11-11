﻿// <auto-generated />
using System;
using AssetSentry.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AssetSentry.Migrations
{
    [DbContext(typeof(AssetSentryContext))]
    partial class AssetSentryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AssetSentry.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("Devices");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "The old laptop found in a corner",
                            Name = "TestDevice",
                            StatusId = "overdue"
                        });
                });

            modelBuilder.Entity("AssetSentry.Models.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Student")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId")
                        .IsUnique();

                    b.ToTable("Loans");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DeviceId = 1,
                            EndDate = new DateTime(2019, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsActive = true,
                            StartDate = new DateTime(2019, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Student = "David Miller"
                        });
                });

            modelBuilder.Entity("AssetSentry.Models.Status", b =>
                {
                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StatusId");

                    b.ToTable("Statuses");

                    b.HasData(
                        new
                        {
                            StatusId = "available",
                            Name = "Available"
                        },
                        new
                        {
                            StatusId = "rented",
                            Name = "Rented"
                        },
                        new
                        {
                            StatusId = "overdue",
                            Name = "Overdue"
                        });
                });

            modelBuilder.Entity("AssetSentry.Models.Device", b =>
                {
                    b.HasOne("AssetSentry.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("AssetSentry.Models.Loan", b =>
                {
                    b.HasOne("AssetSentry.Models.Device", "Device")
                        .WithOne("Loan")
                        .HasForeignKey("AssetSentry.Models.Loan", "DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("AssetSentry.Models.Device", b =>
                {
                    b.Navigation("Loan");
                });
#pragma warning restore 612, 618
        }
    }
}
