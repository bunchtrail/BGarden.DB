﻿// <auto-generated />
using System;
using BGarden.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(BotanicalContext))]
    [Migration("20250326074500_AddLocationType")]
    partial class AddLocationType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

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

                    b.Property<Polygon>("Boundary")
                        .HasColumnType("geometry(Polygon,4326)");

                    b.Property<string>("BoundaryWkt")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("FillColor")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal?>("FillOpacity")
                        .HasColumnType("decimal(3,2)");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<Point>("Location")
                        .HasColumnType("geometry(Point,4326)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("PolygonCoordinates")
                        .HasColumnType("text");

                    b.Property<decimal?>("Radius")
                        .HasColumnType("numeric");

                    b.Property<int>("SectorType")
                        .HasColumnType("integer");

                    b.Property<string>("StrokeColor")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

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

                    b.Property<Point>("Location")
                        .HasColumnType("geometry(Point,4326)");

                    b.Property<int>("LocationType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<int?>("MapId")
                        .HasColumnType("integer");

                    b.Property<int?>("MapX")
                        .HasColumnType("integer");

                    b.Property<int?>("MapY")
                        .HasColumnType("integer");

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

                    b.HasIndex("Location")
                        .HasDatabaseName("IX_Specimen_Location_Spatial");

                    b.HasIndex("MapId");

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

            modelBuilder.Entity("Domain.Entities.Map", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("IsActive");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Maps", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.SpecimenImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<bool>("IsMain")
                        .HasColumnType("boolean");

                    b.Property<int>("SpecimenId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UploadedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.HasIndex("SpecimenId", "IsMain");

                    b.ToTable("SpecimenImages", (string)null);
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

                    b.HasOne("Domain.Entities.Map", "Map")
                        .WithMany("Specimens")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BGarden.Domain.Entities.Region", "Region")
                        .WithMany("Specimens")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CreatedByUser");

                    b.Navigation("Exposition");

                    b.Navigation("Family");

                    b.Navigation("Map");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Domain.Entities.SpecimenImage", b =>
                {
                    b.HasOne("BGarden.Domain.Entities.Specimen", "Specimen")
                        .WithMany("SpecimenImages")
                        .HasForeignKey("SpecimenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specimen");
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

                    b.Navigation("SpecimenImages");
                });

            modelBuilder.Entity("BGarden.Domain.Entities.User", b =>
                {
                    b.Navigation("ManagedSpecimens");
                });

            modelBuilder.Entity("Domain.Entities.Map", b =>
                {
                    b.Navigation("Specimens");
                });
#pragma warning restore 612, 618
        }
    }
}
