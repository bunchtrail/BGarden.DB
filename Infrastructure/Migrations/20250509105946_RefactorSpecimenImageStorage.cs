using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSpecimenImageStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "SpecimenImages");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "SpecimenImages",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "SpecimenImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "SpecimenImages",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "SpecimenImages");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "SpecimenImages");

            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "SpecimenImages");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "SpecimenImages",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
