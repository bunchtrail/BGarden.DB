using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Specimens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Specimens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Specimens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Specimens",
                type: "numeric(9,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Specimens",
                type: "numeric(9,6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Specimens",
                type: "integer",
                nullable: true);

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
                    Radius = table.Column<decimal>(type: "numeric(9,2)", nullable: true),
                    BoundaryWkt = table.Column<string>(type: "text", nullable: true),
                    SectorType = table.Column<int>(type: "integer", nullable: false)
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
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specimen_Coordinates",
                table: "Specimens",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_CreatedByUserId",
                table: "Specimens",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_RegionId",
                table: "Specimens",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_Coordinates",
                table: "Regions",
                columns: new[] { "Latitude", "Longitude" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Specimens_Regions_RegionId",
                table: "Specimens",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Specimens_Users_CreatedByUserId",
                table: "Specimens",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specimens_Regions_RegionId",
                table: "Specimens");

            migrationBuilder.DropForeignKey(
                name: "FK_Specimens_Users_CreatedByUserId",
                table: "Specimens");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Specimen_Coordinates",
                table: "Specimens");

            migrationBuilder.DropIndex(
                name: "IX_Specimens_CreatedByUserId",
                table: "Specimens");

            migrationBuilder.DropIndex(
                name: "IX_Specimens_RegionId",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Specimens");
        }
    }
}
