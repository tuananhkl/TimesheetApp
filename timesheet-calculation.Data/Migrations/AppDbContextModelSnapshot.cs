﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using timesheet_calculation.Data;

#nullable disable

namespace timesheet_calculation.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("timesheet_calculation.Data.Entities.im_TimeSheet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CheckInTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CheckOutTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("im_UserUserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("im_UserUserId");

                    b.ToTable("TimeSheets");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e688b871-464c-4015-88f9-bd1cde3ebdfe"),
                            CheckInTime = new DateTime(2022, 8, 8, 7, 59, 50, 0, DateTimeKind.Utc),
                            CheckOutTime = new DateTime(2022, 8, 8, 17, 31, 18, 0, DateTimeKind.Utc),
                            UserId = new Guid("d29a38b9-566c-4c95-a380-2351796764f1")
                        },
                        new
                        {
                            Id = new Guid("bd002b36-daf2-4208-b3fb-132985d41eae"),
                            CheckInTime = new DateTime(2022, 8, 8, 7, 58, 14, 0, DateTimeKind.Utc),
                            CheckOutTime = new DateTime(2022, 8, 8, 17, 32, 46, 0, DateTimeKind.Utc),
                            UserId = new Guid("d29a38b9-566c-4c95-a380-2351796764f1")
                        });
                });

            modelBuilder.Entity("timesheet_calculation.Data.Entities.im_TimeSheetManager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Day")
                        .HasColumnType("integer");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TimeSheetManagers");
                });

            modelBuilder.Entity("timesheet_calculation.Data.Entities.im_User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("d29a38b9-566c-4c95-a380-2351796764f1"),
                            Name = "Tuan Anh"
                        });
                });

            modelBuilder.Entity("timesheet_calculation.Data.Entities.im_TimeSheet", b =>
                {
                    b.HasOne("timesheet_calculation.Data.Entities.im_User", null)
                        .WithMany("TimeSheets")
                        .HasForeignKey("im_UserUserId");
                });

            modelBuilder.Entity("timesheet_calculation.Data.Entities.im_User", b =>
                {
                    b.Navigation("TimeSheets");
                });
#pragma warning restore 612, 618
        }
    }
}
