﻿// <auto-generated />
using System;
using BGarden.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(BotanicalContext))]
    partial class BotanicalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("ExpositionId")
                        .HasColumnType("integer");

                    b.Property<string>("FillColor")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ExpositionId");

                    b.HasIndex("Type");

                    b.ToTable("MapAreas");
                });

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapAreaCoordinate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<int>("MapAreaId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MapAreaId", "Order");

                    b.ToTable("MapAreaCoordinates");
                });

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapLayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Attribution")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("DisplayOrder")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<bool>("IsDefault")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("LayerId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("DisplayOrder");

                    b.HasIndex("IsDefault");

                    b.HasIndex("LayerId")
                        .IsUnique();

                    b.ToTable("MapLayers");
                });

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapMarker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("PopupContent")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<int?>("SpecimenId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("SpecimenId");

                    b.ToTable("MapMarkers");
                });

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("CenterLatitude")
                        .HasColumnType("double precision");

                    b.Property<double>("CenterLongitude")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("EastBound")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsDefault")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int?>("MaxZoom")
                        .HasColumnType("integer");

                    b.Property<int?>("MinZoom")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<double?>("NorthBound")
                        .HasColumnType("double precision");

                    b.Property<double?>("SouthBound")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("WestBound")
                        .HasColumnType("double precision");

                    b.Property<int>("Zoom")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IsDefault");

                    b.ToTable("MapOptions");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.AuthLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Details")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<int>("EventType")
                        .HasColumnType("integer");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("Success")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserAgent")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("EventType");

                    b.HasIndex("Timestamp");

                    b.HasIndex("UserId");

                    b.HasIndex("Username");

                    b.ToTable("AuthLogs");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Biometry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float?>("FlowerDiameter")
                        .HasColumnType("real");

                    b.Property<float?>("Height")
                        .HasColumnType("real");

                    b.Property<DateTime>("MeasurementDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("SpecimenId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SpecimenId");

                    b.ToTable("Biometries");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Exposition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("Expositions");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Family", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Families");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Phenology", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("FloweringEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FloweringStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FruitingDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("SpecimenId")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SpecimenId");

                    b.ToTable("Phenologies");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByIp")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<string>("ReasonRevoked")
                        .HasColumnType("text");

                    b.Property<string>("ReplacedByToken")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RevokedByIp")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId", "IsRevoked");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BoundaryWkt")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<decimal?>("Radius")
                        .HasColumnType("decimal(9,2)");

                    b.Property<int>("SectorType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Latitude", "Longitude")
                        .HasDatabaseName("IX_Region_Coordinates");

                    b.ToTable("Regions", (string)null);
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Specimen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ConservationStatus")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Country")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("integer");

                    b.Property<string>("Cultivar")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("DeterminedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("DuplicatesInfo")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("EcologyAndBiology")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("EconomicUse")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int?>("ExpositionId")
                        .HasColumnType("integer");

                    b.Property<int>("FamilyId")
                        .HasColumnType("integer");

                    b.Property<string>("FilledBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Form")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Genus")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("HasHerbarium")
                        .HasColumnType("boolean");

                    b.Property<string>("Illustration")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("InventoryNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LatinName")
                        .HasColumnType("text");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("NaturalRange")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Notes")
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<string>("OriginalBreeder")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int?>("OriginalYear")
                        .HasColumnType("integer");

                    b.Property<int?>("PlantingYear")
                        .HasColumnType("integer");

                    b.Property<int?>("RegionId")
                        .HasColumnType("integer");

                    b.Property<string>("RussianName")
                        .HasColumnType("text");

                    b.Property<string>("SampleOrigin")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("SectorType")
                        .HasColumnType("integer");

                    b.Property<string>("Species")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Synonyms")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ExpositionId");

                    b.HasIndex("FamilyId");

                    b.HasIndex("InventoryNumber")
                        .IsUnique();

                    b.HasIndex("RegionId");

                    b.HasIndex("Latitude", "Longitude")
                        .HasDatabaseName("IX_Specimen_Coordinates");

                    b.ToTable("Specimens");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("FailedLoginAttempts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastActiveAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Position")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<bool>("TwoFactorEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("TwoFactorKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapArea", b =>
                {
                    b.HasOne("BGarden.Domain.Entities.Exposition", "Exposition")
                        .WithMany()
                        .HasForeignKey("ExpositionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Exposition");
                });

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapAreaCoordinate", b =>
                {
                    b.HasOne("BGarden.DB.Domain.Entities.MapArea", "MapArea")
                        .WithMany("Coordinates")
                        .HasForeignKey("MapAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MapArea");
                });

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapMarker", b =>
                {
                    b.HasOne("BGarden.Domain.Entities.Specimen", "Specimen")
                        .WithMany()
                        .HasForeignKey("SpecimenId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Specimen");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.AuthLog", b =>
                {
                    b.HasOne("BGarden.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Biometry", b =>
                {
                    b.HasOne("BGarden.Domain.Entities.Specimen", "Specimen")
                        .WithMany("Biometries")
                        .HasForeignKey("SpecimenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specimen");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Phenology", b =>
                {
                    b.HasOne("BGarden.Domain.Entities.Specimen", "Specimen")
                        .WithMany("Phenologies")
                        .HasForeignKey("SpecimenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specimen");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("BGarden.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Specimen", b =>
                {
                    b.HasOne("BGarden.Domain.Entities.User", "CreatedByUser")
                        .WithMany("ManagedSpecimens")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BGarden.Domain.Entities.Exposition", "Exposition")
                        .WithMany("Specimens")
                        .HasForeignKey("ExpositionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BGarden.Domain.Entities.Family", "Family")
                        .WithMany("Specimens")
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BGarden.Domain.Entities.Region", "Region")
                        .WithMany("Specimens")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CreatedByUser");

                    b.Navigation("Exposition");

                    b.Navigation("Family");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("BGarden.DB.Domain.Entities.MapArea", b =>
                {
                    b.Navigation("Coordinates");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Exposition", b =>
                {
                    b.Navigation("Specimens");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Family", b =>
                {
                    b.Navigation("Specimens");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Region", b =>
                {
                    b.Navigation("Specimens");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.Specimen", b =>
                {
                    b.Navigation("Biometries");

                    b.Navigation("Phenologies");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.User", b =>
                {
                    b.Navigation("ManagedSpecimens");
                });
#pragma warning restore 612, 618
        }
    }
}
