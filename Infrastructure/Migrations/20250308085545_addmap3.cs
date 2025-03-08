using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addmap3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapMarkers_Specimens_SpecimenId",
                table: "MapMarkers");

            migrationBuilder.DropTable(
                name: "MapAreaCoordinates");

            migrationBuilder.DropTable(
                name: "MapLayers");

            migrationBuilder.DropTable(
                name: "MapAreas");

            migrationBuilder.DropIndex(
                name: "IX_MapOptions_IsDefault",
                table: "MapOptions");

            migrationBuilder.DropIndex(
                name: "IX_MapMarkers_SpecimenId",
                table: "MapMarkers");

            migrationBuilder.DropColumn(
                name: "EastBound",
                table: "MapOptions");

            migrationBuilder.DropColumn(
                name: "NorthBound",
                table: "MapOptions");

            migrationBuilder.DropColumn(
                name: "SouthBound",
                table: "MapOptions");

            migrationBuilder.DropColumn(
                name: "WestBound",
                table: "MapOptions");

            migrationBuilder.DropColumn(
                name: "PopupContent",
                table: "MapMarkers");

            migrationBuilder.RenameColumn(
                name: "Zoom",
                table: "MapOptions",
                newName: "DefaultZoom");

            migrationBuilder.AlterColumn<decimal>(
                name: "Radius",
                table: "Regions",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(9,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Regions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FillColor",
                table: "Regions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FillOpacity",
                table: "Regions",
                type: "numeric(3,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PolygonCoordinates",
                table: "Regions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StrokeColor",
                table: "Regions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Regions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "MinZoom",
                table: "MapOptions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxZoom",
                table: "MapOptions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapSchemaUrl",
                table: "MapOptions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "SpecimenId",
                table: "MapMarkers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MapMarkers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "MapMarkers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId1",
                table: "MapMarkers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapOptions_IsDefault",
                table: "MapOptions",
                column: "IsDefault",
                unique: true,
                filter: "\"IsDefault\" = TRUE");

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

            migrationBuilder.AddForeignKey(
                name: "FK_MapMarkers_Regions_RegionId",
                table: "MapMarkers",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MapMarkers_Regions_RegionId1",
                table: "MapMarkers",
                column: "RegionId1",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapMarkers_Specimens_SpecimenId",
                table: "MapMarkers",
                column: "SpecimenId",
                principalTable: "Specimens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapMarkers_Regions_RegionId",
                table: "MapMarkers");

            migrationBuilder.DropForeignKey(
                name: "FK_MapMarkers_Regions_RegionId1",
                table: "MapMarkers");

            migrationBuilder.DropForeignKey(
                name: "FK_MapMarkers_Specimens_SpecimenId",
                table: "MapMarkers");

            migrationBuilder.DropIndex(
                name: "IX_MapOptions_IsDefault",
                table: "MapOptions");

            migrationBuilder.DropIndex(
                name: "IX_MapMarkers_RegionId",
                table: "MapMarkers");

            migrationBuilder.DropIndex(
                name: "IX_MapMarkers_RegionId1",
                table: "MapMarkers");

            migrationBuilder.DropIndex(
                name: "IX_MapMarkers_SpecimenId",
                table: "MapMarkers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "FillColor",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "FillOpacity",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PolygonCoordinates",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "StrokeColor",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "MapSchemaUrl",
                table: "MapOptions");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "MapMarkers");

            migrationBuilder.DropColumn(
                name: "RegionId1",
                table: "MapMarkers");

            migrationBuilder.RenameColumn(
                name: "DefaultZoom",
                table: "MapOptions",
                newName: "Zoom");

            migrationBuilder.AlterColumn<decimal>(
                name: "Radius",
                table: "Regions",
                type: "numeric(9,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinZoom",
                table: "MapOptions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MaxZoom",
                table: "MapOptions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<double>(
                name: "EastBound",
                table: "MapOptions",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NorthBound",
                table: "MapOptions",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SouthBound",
                table: "MapOptions",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WestBound",
                table: "MapOptions",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SpecimenId",
                table: "MapMarkers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MapMarkers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PopupContent",
                table: "MapMarkers",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MapAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpositionId = table.Column<int>(type: "integer", nullable: true),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FillColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapAreas_Expositions_ExpositionId",
                        column: x => x.ExpositionId,
                        principalTable: "Expositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MapLayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Attribution = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LayerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapLayers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapAreaCoordinates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MapAreaId = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapAreaCoordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapAreaCoordinates_MapAreas_MapAreaId",
                        column: x => x.MapAreaId,
                        principalTable: "MapAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapOptions_IsDefault",
                table: "MapOptions",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_MapMarkers_SpecimenId",
                table: "MapMarkers",
                column: "SpecimenId");

            migrationBuilder.CreateIndex(
                name: "IX_MapAreaCoordinates_MapAreaId_Order",
                table: "MapAreaCoordinates",
                columns: new[] { "MapAreaId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_MapAreas_ExpositionId",
                table: "MapAreas",
                column: "ExpositionId");

            migrationBuilder.CreateIndex(
                name: "IX_MapAreas_Type",
                table: "MapAreas",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_MapLayers_DisplayOrder",
                table: "MapLayers",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_MapLayers_IsDefault",
                table: "MapLayers",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_MapLayers_LayerId",
                table: "MapLayers",
                column: "LayerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MapMarkers_Specimens_SpecimenId",
                table: "MapMarkers",
                column: "SpecimenId",
                principalTable: "Specimens",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
