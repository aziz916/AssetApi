﻿// <auto-generated />
using System;
using AssetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AssetAPI.Migrations
{
    [DbContext(typeof(AssetDbContext))]
    [Migration("20210214202100_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("AssetAPI.Models.Asset", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("id");

                    b.HasAlternateKey("name")
                        .HasName("AlternateKey_AssetName");

                    b.ToTable("asset");
                });

            modelBuilder.Entity("AssetAPI.Models.AssetProperty", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<long>("assetid")
                        .HasColumnType("bigint");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("time_stamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

                    b.Property<bool>("value")
                        .HasColumnType("bit");

                    b.HasKey("id");

                    b.HasAlternateKey("name")
                        .HasName("AlternateKey_AssetPropertyName");

                    b.HasIndex("assetid");

                    b.ToTable("asset_property");
                });

            modelBuilder.Entity("AssetAPI.Models.AssetProperty", b =>
                {
                    b.HasOne("AssetAPI.Models.Asset", "asset")
                        .WithMany("properties")
                        .HasForeignKey("assetid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("asset");
                });

            modelBuilder.Entity("AssetAPI.Models.Asset", b =>
                {
                    b.Navigation("properties");
                });
#pragma warning restore 612, 618
        }
    }
}
