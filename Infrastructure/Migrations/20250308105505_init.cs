using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
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
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

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
                name: "MapLayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BaseDirectory = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MinZoom = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    MaxZoom = table.Column<int>(type: "integer", nullable: false, defaultValue: 18),
                    TileFormat = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "png"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    MinX = table.Column<double>(type: "double precision", nullable: true),
                    MinY = table.Column<double>(type: "double precision", nullable: true),
                    MaxX = table.Column<double>(type: "double precision", nullable: true),
                    MaxY = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapLayers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CenterLatitude = table.Column<double>(type: "double precision", nullable: false),
                    CenterLongitude = table.Column<double>(type: "double precision", nullable: false),
                    DefaultZoom = table.Column<int>(type: "integer", nullable: false),
                    Zoom = table.Column<int>(type: "integer", nullable: false),
                    MinZoom = table.Column<int>(type: "integer", nullable: false),
                    MaxZoom = table.Column<int>(type: "integer", nullable: false),
                    MapSchemaUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SouthBound = table.Column<double>(type: "double precision", nullable: true),
                    WestBound = table.Column<double>(type: "double precision", nullable: true),
                    NorthBound = table.Column<double>(type: "double precision", nullable: true),
                    EastBound = table.Column<double>(type: "double precision", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    Location = table.Column<Point>(type: "geometry(Point,4326)", nullable: true),
                    Radius = table.Column<decimal>(type: "numeric", nullable: true),
                    BoundaryWkt = table.Column<string>(type: "text", nullable: true),
                    Boundary = table.Column<Polygon>(type: "geometry(Polygon,4326)", nullable: true),
                    PolygonCoordinates = table.Column<string>(type: "text", nullable: true),
                    StrokeColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FillColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FillOpacity = table.Column<decimal>(type: "numeric(3,2)", nullable: true),
                    SectorType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PasswordSalt = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastActiveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TwoFactorKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapTileMetadata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MapLayerId = table.Column<int>(type: "integer", nullable: false),
                    ZoomLevel = table.Column<int>(type: "integer", nullable: false),
                    TileColumn = table.Column<int>(type: "integer", nullable: false),
                    TileRow = table.Column<int>(type: "integer", nullable: false),
                    FileSize = table.Column<int>(type: "integer", nullable: false),
                    Checksum = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    RelativePath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapTileMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapTileMetadata_MapLayers_MapLayerId",
                        column: x => x.MapLayerId,
                        principalTable: "MapLayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Details = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RevokedByIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specimens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SectorType = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    Location = table.Column<Point>(type: "geometry(Point,4326)", nullable: true),
                    RegionId = table.Column<int>(type: "integer", nullable: true),
                    FamilyId = table.Column<int>(type: "integer", nullable: false),
                    RussianName = table.Column<string>(type: "text", nullable: true),
                    LatinName = table.Column<string>(type: "text", nullable: true),
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
                    FilledBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Specimens_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Specimens_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                name: "MapMarkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Location = table.Column<Point>(type: "geometry(Point,4326)", nullable: true),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PopupContent = table.Column<string>(type: "text", nullable: true),
                    SpecimenId = table.Column<int>(type: "integer", nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: true),
                    RegionId1 = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapMarkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapMarkers_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MapMarkers_Regions_RegionId1",
                        column: x => x.RegionId1,
                        principalTable: "Regions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MapMarkers_Specimens_SpecimenId",
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
                name: "IX_AuthLogs_EventType",
                table: "AuthLogs",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_AuthLogs_Timestamp",
                table: "AuthLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuthLogs_UserId",
                table: "AuthLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthLogs_Username",
                table: "AuthLogs",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Biometries_SpecimenId",
                table: "Biometries",
                column: "SpecimenId");

            migrationBuilder.CreateIndex(
                name: "IX_MapLayers_IsActive",
                table: "MapLayers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MapLayers_Name",
                table: "MapLayers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapMarker_Location_Spatial",
                table: "MapMarkers",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_MapMarkers_RegionId",
                table: "MapMarkers",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_MapMarkers_RegionId1",
                table: "MapMarkers",
                column: "RegionId1");

            migrationBuilder.CreateIndex(
                name: "IX_MapMarkers_SpecimenId",
                table: "MapMarkers",
                column: "SpecimenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapOptions_IsDefault",
                table: "MapOptions",
                column: "IsDefault",
                unique: true,
                filter: "\"IsDefault\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_MapTileMetadata_MapLayerId_ZoomLevel_TileColumn_TileRow",
                table: "MapTileMetadata",
                columns: new[] { "MapLayerId", "ZoomLevel", "TileColumn", "TileRow" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phenologies_SpecimenId",
                table: "Phenologies",
                column: "SpecimenId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_IsRevoked",
                table: "RefreshTokens",
                columns: new[] { "UserId", "IsRevoked" });

            migrationBuilder.CreateIndex(
                name: "IX_Region_Coordinates",
                table: "Regions",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_Coordinates",
                table: "Specimens",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_Location_Spatial",
                table: "Specimens",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_CreatedByUserId",
                table: "Specimens",
                column: "CreatedByUserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_RegionId",
                table: "Specimens",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthLogs");

            migrationBuilder.DropTable(
                name: "Biometries");

            migrationBuilder.DropTable(
                name: "MapMarkers");

            migrationBuilder.DropTable(
                name: "MapOptions");

            migrationBuilder.DropTable(
                name: "MapTileMetadata");

            migrationBuilder.DropTable(
                name: "Phenologies");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "MapLayers");

            migrationBuilder.DropTable(
                name: "Specimens");

            migrationBuilder.DropTable(
                name: "Expositions");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
