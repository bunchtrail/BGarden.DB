using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapId",
                table: "Specimens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapX",
                table: "Specimens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapY",
                table: "Specimens",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specimens_MapId",
                table: "Specimens",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_IsActive",
                table: "Maps",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_Name",
                table: "Maps",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Specimens_Maps_MapId",
                table: "Specimens",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specimens_Maps_MapId",
                table: "Specimens");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropIndex(
                name: "IX_Specimens_MapId",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "MapX",
                table: "Specimens");

            migrationBuilder.DropColumn(
                name: "MapY",
                table: "Specimens");
        }
    }
}
