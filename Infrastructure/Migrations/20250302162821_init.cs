using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specimens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FamilyId = table.Column<int>(type: "integer", nullable: false),
                    Genus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Species = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Cultivar = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Form = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Synonyms = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DeterminedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PlantingYear = table.Column<int>(type: "integer", nullable: true),
                    SampleOrigin = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NaturalRange = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    EcologyAndBiology = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    EconomicUse = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    ConservationStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ExpositionId = table.Column<int>(type: "integer", nullable: true),
                    HasHerbarium = table.Column<bool>(type: "boolean", nullable: false),
                    DuplicatesInfo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OriginalBreeder = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    OriginalYear = table.Column<int>(type: "integer", nullable: true),
                    Country = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Illustration = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Notes = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    FilledBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specimens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specimens_Expositions_ExpositionId",
                        column: x => x.ExpositionId,
                        principalTable: "Expositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Specimens_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Biometries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpecimenId = table.Column<int>(type: "integer", nullable: false),
                    MeasurementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: true),
                    FlowerDiameter = table.Column<float>(type: "real", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biometries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Biometries_Specimens_SpecimenId",
                        column: x => x.SpecimenId,
                        principalTable: "Specimens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phenologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpecimenId = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    FloweringStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FloweringEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FruitingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phenologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phenologies_Specimens_SpecimenId",
                        column: x => x.SpecimenId,
                        principalTable: "Specimens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biometries_SpecimenId",
                table: "Biometries",
                column: "SpecimenId");

            migrationBuilder.CreateIndex(
                name: "IX_Phenologies_SpecimenId",
                table: "Phenologies",
                column: "SpecimenId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_ExpositionId",
                table: "Specimens",
                column: "ExpositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_FamilyId",
                table: "Specimens",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_InventoryNumber",
                table: "Specimens",
                column: "InventoryNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biometries");

            migrationBuilder.DropTable(
                name: "Phenologies");

            migrationBuilder.DropTable(
                name: "Specimens");

            migrationBuilder.DropTable(
                name: "Expositions");

            migrationBuilder.DropTable(
                name: "Families");
        }
    }
}
